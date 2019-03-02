using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMapper
{
    internal static class Mapper
    {
        public static object MapToObject(object source, object result)
        {
            var resultProperties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var resultProperty in resultProperties)
            {
                var attributesForProperty = resultProperty.GetCustomAttributes(typeof(MatchPropertyToAttribute), true);
                var isOfTypeFieldAttribute = false;

                MatchPropertyToAttribute currentAttribute = null;
                string currentPropertyName = resultProperty.Name;

                foreach (var attribute in attributesForProperty)
                {
                    if (attribute.GetType() == typeof(MatchPropertyToAttribute))
                    {
                        isOfTypeFieldAttribute = true;
                        currentAttribute = (MatchPropertyToAttribute)attribute;
                        break;
                    }
                }

                var sourceProperty = source.GetType().GetProperty(isOfTypeFieldAttribute ? currentAttribute.Name : currentPropertyName);

                if (isOfTypeFieldAttribute && sourceProperty is null)
                    throw new MemberAccessException($"The property '{currentAttribute.Name}' not found in the '{source.GetType().Name} class.'");

                object sourcePropertyValue = null;

                if (sourceProperty?.PropertyType == resultProperty.PropertyType)
                {
                    sourcePropertyValue = sourceProperty.GetValue(source);
                }
                else if (sourceProperty?.PropertyType != null)
                {
                    var sourcePropertyObject = sourceProperty.GetValue(source, null);

                    if (sourcePropertyObject is null) continue;

                    object resultPropertyObject = null;

                    if (sourcePropertyObject.IsEnumerable())
                    {
                        var resultPropertyGenericItemType = resultProperty.PropertyType.GetGenericArguments()[0];
                        var items = Activator.CreateInstance(typeof(List<>).MakeGenericType(resultPropertyGenericItemType));

                        foreach (var sourcePropertyEnumerableTypeObject in (IEnumerable<object>)sourcePropertyObject)
                        {
                            var resultPropertyGenericItemTypeObject = Activator.CreateInstance(resultPropertyGenericItemType);
                            ((IList)items).Add(MapToObject(sourcePropertyEnumerableTypeObject, resultPropertyGenericItemTypeObject));
                            sourcePropertyValue = items;
                        }
                    }
                    else
                    {
                        resultPropertyObject = Activator.CreateInstance(resultProperty.PropertyType);
                        sourcePropertyValue = MapToObject(sourcePropertyObject, resultPropertyObject);
                    }
                }

                resultProperty.SetValue(result, sourcePropertyValue);
            }

            return result;
        }

        private static bool IsEnumerable(this object property)
        {
            return property.GetType().GetInterfaces()
                     .Any(t => t.IsGenericType
                            && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}
