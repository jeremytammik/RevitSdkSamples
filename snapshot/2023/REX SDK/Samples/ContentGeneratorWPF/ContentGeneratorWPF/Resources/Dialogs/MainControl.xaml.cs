using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.REX.Framework;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : REX.Common.REXExtensionMainControl
    {
        public MainControl()
        {
            InitializeComponent();
        }
        public MainControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private void Toolbox_SelectedItemChanged(object sender, REX.Controls.WPF.REXToolboxItem item)
        {
            base.Toolbox_SelectItem(sender, item);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command is REX.Common.REXRoutedUICommand)
                RunCommand((e.Command as REX.Common.REXRoutedUICommand).Name);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command is REX.Common.REXRoutedUICommand)
                e.CanExecute = CanRunCommand((e.Command as REX.Common.REXRoutedUICommand).Name);
            else
                e.CanExecute = true;
        }

        private bool CanRunCommand(string Name)
        {
            return true;
        }

        private void RunCommand(string Name)
        {
            REXCommand Cmd = null;
            switch (Name)
            {
                case "Open":
                    ThisExtension.System.LoadFromFile(true);
                    return;
                case "Save":
                    ThisExtension.System.SaveToFile(false, true);
                    return;
                case "SaveAs":
                    ThisExtension.System.SaveToFile(true);
                    return;
                case "Calculate":
                    Cmd = new REXCommand(REXCommandType.Run);
                    break;
                case "Help":
                    Cmd = new REXCommand(REXCommandType.Help);
                    break;
                case "Close":
                    Cmd = new REXCommand(REXCommandType.Close);
                    break;
                case "About":
                    Cmd = new REXCommand(REXCommandType.About);
                    break;
                case "Preferences":
                    ThisExtension.ShowPreferencesDialog();
                    break;
            }
            if (Cmd != null)
                ThisExtension.Command(ref Cmd);
        }
    }
}
