using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitObjectsHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BuiltInParameterAttribute : Attribute
    {
        public BuiltInParameter Parameter { get; set; }

        public BuiltInParameterAttribute(BuiltInParameter parameter)
        {
            Parameter = parameter;
        }
    }
}