using System;

namespace RevitObjectsHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ElementIdAttribute : Attribute
    {
    }
}