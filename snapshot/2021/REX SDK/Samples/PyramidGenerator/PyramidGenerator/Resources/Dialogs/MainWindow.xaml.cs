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
using System.Windows.Shapes;
using Autodesk.REX.Framework;

namespace REX.PyramidGenerator.Resources.Dialogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : REX.Common.REXExtensionWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.OK);
            ThisExtension.Command(ref Cmd);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.Close);
            Cmd.CommandObject = false;
            ThisExtension.Command(ref Cmd);
        }

        private void REXExtensionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.Close);
            Cmd.CommandObject = true;
            if (((bool)ThisExtension.Command(ref Cmd)) == true)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void HyperlinkHelp_Click(object sender, RoutedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.Help);
            ThisExtension.Command(ref Cmd);
        }

        private void HyperlinkAbout_Click(object sender, RoutedEventArgs e)
        {
            REXCommand Cmd = new REXCommand(REXCommandType.About);
            ThisExtension.Command(ref Cmd);
        }
    }
}
