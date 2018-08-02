using RevitObjectsHelper.Core;

namespace RevitObjectHelper.Tests.Context
{
    public class TestContext : DbContext
    {
        public DbObjectSet<DbWall> Walls { get; set; }
        public DbObjectSet<DbFloor> Floors { get; set; }
        public DbObjectSet<DbPipe> Pipes { get; set; }
    }
}