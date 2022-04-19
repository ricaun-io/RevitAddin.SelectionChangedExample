using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.SelectionChangedExample.Views;
using ricaun.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitAddin.SelectionChangedExample.Revit
{
    [Console]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel("RevitAddin");
            ribbonPanel.AddPushButton<Commands.Command>("Selection\rChanged")
                .SetLargeImage("/UIFrameworkRes;component/ribbon/images/revit.ico");

            application.SelectionChanged += Application_SelectionChanged;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            application.SelectionChanged -= Application_SelectionChanged;

            ribbonPanel?.Remove();
            return Result.Succeeded;
        }

        private void Application_SelectionChanged(object sender, Autodesk.Revit.UI.Events.SelectionChangedEventArgs e)
        {
            var document = e.GetDocument();
            var elementIds = e.GetSelectedElements();
            var elements = elementIds.Select(id => document.GetElement(id));
            Console.WriteLine($"SelectionChanged: {elementIds.Count} [{string.Join(",", elements.Select(e => e.Id))}]");

            //MainView.Items.Clear();
            //foreach (var element in elements)
            //{
            //    MainView.Items.Add(element.Name);
            //}
        }

    }

}