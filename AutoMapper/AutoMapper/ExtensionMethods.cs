using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    public static class ExtensionMethods
    {
        public static TModel ConvertTo<TModel>(this IEntity source)
            where TModel : IModel, new()
        {
            return (TModel)Mapper.MapToObject(source, new TModel());
        }

        public static TEntity ConvertTo<TEntity>(this IModel source)
            where TEntity : IEntity, new()
        {
            return (TEntity)Mapper.MapToObject(source, new TEntity());
        }
    }
}
