using Autodesk.Revit.DB;
using RevitObjectsHelper.Attributes;
using RevitObjectsHelper.Core;

namespace RevitObjectHelper.Tests.Context
{
    [Instance]
    public class DbWall : DbObject
    {
        [BuiltInParameter(BuiltInParameter.INSTANCE_LENGTH_PARAM)]
        public double Lenght { get; set; }
    }
}