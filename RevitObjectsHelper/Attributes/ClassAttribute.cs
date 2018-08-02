using System;

namespace RevitObjectsHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassAttribute : Attribute
    {
        public Type Type { get; set; }

        public ClassAttribute(Type type)
        {
            Type = type;
        }
    }
}