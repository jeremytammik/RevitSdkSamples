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
   public partial class AlignmentSetting : Form
   {
      /// <summary>
      /// 
      /// </summary>
      public AlignmentSetting()
      {
         InitializeComponent();
         foreach (string name in DatumAlignment.datumDic.Keys)
         {
            this.datumList.Items.Add(name);
         }
      }
   }
}
