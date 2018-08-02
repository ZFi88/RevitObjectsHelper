using System;
using Autodesk.Revit.DB;
using RevitObjectHelper.Tests.Context;
using RTF.Applications;

namespace RevitObjectHelper.Tests
{
    public class DbContextFixture : IDisposable
    {
        public TestContext DbContext { get; set; }
        public Document Document { get; set; }

        public DbContextFixture()
        {
            var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
            Document = doc;
            DbContext = RevitObjectsHelper.Core.DbContext.Create<TestContext>(doc);
        }

        public void Dispose()
        {
        }
    }
}