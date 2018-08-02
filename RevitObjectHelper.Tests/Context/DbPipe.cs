using RevitObjectsHelper.Attributes;
using RevitObjectsHelper.Core;

namespace RevitObjectHelper.Tests.Context
{
    [Instance]
    public class DbPipe : DbObject
    {
        [ParameterName("Диаметр")]
        public double Diameter { get; set; }
    }
}