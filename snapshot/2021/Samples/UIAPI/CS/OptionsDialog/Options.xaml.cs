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

namespace Revit.SDK.Samples.UIAPI.CS.OptionsDialog
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : UserControl
    {
        public Options()
        {
            InitializeComponent();

            Revit.SDK.Samples.UIAPI.CS.ApplicationOptions options = ApplicationOptions.Get();


            ButtonAccessibility.SelectedIndex = (int)options.Availability;
        }

        public void OnOK()
        {
            Revit.SDK.Samples.UIAPI.CS.ApplicationOptions options = ApplicationOptions.Get();

            options.Availability = (ApplicationAvailablity)ButtonAccessibility.SelectedIndex;

        }

        public void OnRestoreDefaults()
        {
            ButtonAccessibility.SelectedIndex = 0;
        }

        private bool GetCheckbuttonChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked.HasValue)
                return checkBox.IsChecked.Value;
            return false;
        }

    }
}
