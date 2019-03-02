using System;

namespace AutoMapper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MatchPropertyToAttribute : Attribute
    {
        public readonly string Name;
        public MatchPropertyToAttribute(string name) => Name = name;

    }
}
