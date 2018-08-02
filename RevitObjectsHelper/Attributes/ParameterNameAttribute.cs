using System;

namespace RevitObjectsHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ParameterNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ParameterNameAttribute(string name)
        {
            Name = name;
        }
    }
}