using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RevitAddin.SelectionChangedExample.Views
{
    public partial class MainView : Window
    {
        public static ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();
        public bool IsClosed { get; private set; }
        public MainView(UIApplication uiapp)
        {
            InitializeComponent();
            InitializeWindow();
            this.Title = "SelectionChanged";
            InitializeUIApplication(uiapp);
        }
        private void InitializeUIApplication(UIApplication uiapp)
        {
            uiapp.Idling += Uiapp_Idling;
            uiapp.SelectionChanged += Uiapp_SelectionChanged;
            Closed += (s, e) => { IsClosed = true; };
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document;
            View view = uidoc.ActiveView;
            Selection selection = uidoc.Selection;
            UpdateItems(document, selection.GetElementIds());
        }

        private void Uiapp_SelectionChanged(object sender, Autodesk.Revit.UI.Events.SelectionChangedEventArgs e)
        {
            var document = e.GetDocument();
            var elementIds = e.GetSelectedElements();
            UpdateItems(document, elementIds);
        }

        private void Uiapp_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            if (IsClosed == false) return;
            var uiapp = sender as UIApplication;
            uiapp.Idling -= Uiapp_Idling;
            uiapp.SelectionChanged -= Uiapp_SelectionChanged;
        }

        private void UpdateItems(Document document, IEnumerable<ElementId> elementIds)
        {
            Items.Clear();
            var elements = elementIds.Select(id => document.GetElement(id));
            foreach (var element in elements)
            {
                Items.Add(element.Name);
            }
        }

        #region InitializeWindow
        private void InitializeWindow()
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            new System.Windows.Interop.WindowInteropHelper(this) { Owner = Autodesk.Windows.ComponentManager.ApplicationWindow };
            this.KeyDown += (s, e) => { if (e.Key == System.Windows.Input.Key.Escape) Close(); };
        }
        #endregion
    }
}