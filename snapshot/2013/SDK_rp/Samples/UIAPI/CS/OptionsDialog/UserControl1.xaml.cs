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
using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.UIAPI.CS
{
   /// <summary>
   /// Interaction logic for UserControl1.xaml
   /// </summary>
   public partial class UserControl1 : UserControl
   {

      public UserControl1()
      {
         InitializeComponent();


         ObservableCollection<Member> memberData = new ObservableCollection<Member>();
         memberData.Add(new Member(){Name = "Joe", Age = "23", Pass = true, Email = new Uri("mailto:Joe@school.com")});
         memberData.Add(new Member(){Name = "Mike", Age = "20",Pass = false,Email = new Uri("mailto:Mike@school.com")});
         memberData.Add(new Member(){Name = "Lucy", Age = "25",Pass = true,Email = new Uri("mailto:Lucy@school.com")});      
         dataGrid1.DataContext = memberData;
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

   public enum SexOpt { Male, Female };

   public class Member 
   { 
      public string Name { get; set; } 
      public string Age { get; set; } 
      public bool Pass { get; set; } 
      public Uri Email { get; set; } } 
}
