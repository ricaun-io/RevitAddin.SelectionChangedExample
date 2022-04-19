using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.SelectionChangedExample.Views;
using System;

namespace RevitAddin.SelectionChangedExample.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        static MainView mainView;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            if (mainView == null)
            {
                mainView = new MainView(uiapp);
                mainView.Show();
                mainView.Closed += (s, e) => { mainView = null; };
            }
            mainView.Activate();

            return Result.Succeeded;
        }
    }
}
