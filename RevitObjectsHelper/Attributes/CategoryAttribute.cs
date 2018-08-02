using System;
using Autodesk.Revit.DB;

namespace RevitObjectsHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CategoryAttribute : Attribute
    {
        public BuiltInCategory Category { get; set; }

        public CategoryAttribute(BuiltInCategory cat)
        {
            this.Category = cat;
        }
    }
}