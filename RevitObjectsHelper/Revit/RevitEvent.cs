using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitObjectsHelper.Revit
{
    public class RevitEvent : IExternalEventHandler
    {
        private Action doAction;
        private Document doc;
        private ExternalEvent exEvent;
        private string transactionName;

        public RevitEvent()
        {
            exEvent = ExternalEvent.Create(this);
        }

        public void Run(Action doAction, Document doc = null, string transactionName = null)
        {
            this.doAction = doAction;
            this.doc = doc;
            this.transactionName = transactionName;
            exEvent.Raise();
        }

        public void Execute(UIApplication app)
        {
            if (doAction != null)
            {
                if (doc == null) doc = app.ActiveUIDocument.Document;
                using (Transaction t = new Transaction(doc, transactionName ?? "RevitEvent"))
                {
                    t.Start();
                    doAction();
                    t.Commit();
                }
            }
        }

        public string GetName()
        {
            return "RevitEvent";
        }
    }
}