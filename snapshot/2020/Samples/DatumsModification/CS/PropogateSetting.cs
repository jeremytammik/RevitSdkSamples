using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revit.SDK.Samples.DatumsModification.CS
{
   /// <summary>
   /// 
   /// </summary>
   public partial class PropogateSetting : Form
   {
      /// <summary>
      /// 
      /// </summary>
      public PropogateSetting()
      {
         InitializeComponent();
          foreach (string name in DatumPropagation.viewDic.Keys)
          {
             this.propagationViewList.Items.Add(name);
          }
      }


   }
}
