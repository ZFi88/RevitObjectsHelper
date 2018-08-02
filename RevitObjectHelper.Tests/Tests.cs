using System.Linq;
using Autodesk.Revit.DB;
using NUnit.Framework;
using RTF.Framework;
using TestContext = RevitObjectHelper.Tests.Context.TestContext;

namespace RevitObjectHelper.Tests
{
    
    [TestFixture]
    public class Tests
    {
        private DbContextFixture _fixture;
        public TestContext Context => _fixture.DbContext;
        public Document Document => _fixture.Document;

        [SetUp]
        public void Setup()
        {
            _fixture = new DbContextFixture();
        }
        
        [Test]
        [TestModel("./testProject.rvt")]
        public void WallsCount()
        {
            Assert.AreEqual(1, Context.Walls.Count());
        }
    }
}