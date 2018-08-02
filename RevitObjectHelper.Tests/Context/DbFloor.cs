using Autodesk.Revit.DB;
using RevitObjectsHelper.Attributes;
using RevitObjectsHelper.Core;

namespace RevitObjectHelper.Tests.Context
{
    [Instance]
    public class DbFloor : DbObject
    {
        [BuiltInParameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)]
        public string Comment { get; set; }
    }
}