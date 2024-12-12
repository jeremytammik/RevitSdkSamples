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
using System.Windows.Interop;


namespace Revit.SDK.Samples.UIAPI.CS
{
   /// <summary>
   /// Interaction logic for UserControl3.xaml
   /// </summary>
   public partial class UserControl3 : UserControl
   {
      public UserControl3(String name)
      {
         InitializeComponent();

         _name = name;

         this.image1.Source = getBitmapAsImageSource(Revit.SDK.Samples.UIAPI.CS.Properties.Resources.autodesk);
      }

      public static ImageSource getBitmapAsImageSource(System.Drawing.Bitmap bitmap)
      {
         IntPtr hBitmap = bitmap.GetHbitmap();
         return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
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
