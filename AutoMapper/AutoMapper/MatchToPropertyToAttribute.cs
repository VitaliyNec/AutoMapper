using System;

namespace AutoMapper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MatchToPropertyToAttribute : Attribute
    {
        public readonly string Name;
        public MatchToPropertyToAttribute(string name) => Name = name;

    }
}
