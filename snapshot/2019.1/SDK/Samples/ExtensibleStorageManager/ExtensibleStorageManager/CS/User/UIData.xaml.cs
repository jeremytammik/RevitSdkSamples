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

namespace ExtensibleStorageManager
{
   /// <summary>
   /// A class for a Window with a TextBox for displaying many lines of text data.
   /// </summary>
   public partial class UIData : Window
   {
      public UIData()
      {
         InitializeComponent();
      }

      public void SetData(string data)
      {
         this.m_tb_Data.Text = data;
      }



   }
}
