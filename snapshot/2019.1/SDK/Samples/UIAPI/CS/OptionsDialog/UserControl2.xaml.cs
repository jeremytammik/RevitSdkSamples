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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RApplication = Autodesk.Revit.ApplicationServices.Application;

namespace Revit.SDK.Samples.UIAPI.CS
{
   /// <summary>
   /// Interaction logic for UserControl2.xaml
   /// </summary>
   public partial class UserControl2 : UserControl
   {
      public UserControl2(String name)
      {
         InitializeComponent();
         _name = name;
      }

      private void onbtn_click(object sender, RoutedEventArgs e)
      {
         TaskDialog.Show("Hello", _name);
      }

      public void OnOK()
      {
         TaskDialog.Show("OK", _name);
      }

      public void OnCancel()
      {
         TaskDialog.Show("OnCancel", _name);
      }

      public void OnRestoreDefaults()
      {
         TaskDialog.Show("OnRestoreDefaults", _name);
      }

      String _name;
   }
}
