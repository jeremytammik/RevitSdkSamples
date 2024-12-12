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
   public partial class DatumStyleSetting : Form
   {
      /// <summary>
      /// 
      /// </summary>
      public DatumStyleSetting()
      {
         InitializeComponent();
         this.datumLeftStyleListBox.SetItemChecked(0,DatumStyleModification.showLeftBubble);
         this.datumRightStyleListBox.SetItemChecked(0, DatumStyleModification.showRightBubble);
         this.datumLeftStyleListBox.SetItemChecked(1, DatumStyleModification.addLeftElbow);
         this.datumRightStyleListBox.SetItemChecked(1, DatumStyleModification.addRightElbow);
         this.datumLeftStyleListBox.SetItemChecked(2, DatumStyleModification.changeLeftEnd2D);
         this.datumRightStyleListBox.SetItemChecked(2, DatumStyleModification.changeRightEnd2D);

      }

      private void okButtonClick(object sender, EventArgs e)
      {
         DatumStyleModification.showLeftBubble = this.datumLeftStyleListBox.GetItemChecked(0);
         DatumStyleModification.showRightBubble = this.datumRightStyleListBox.GetItemChecked(0);
         DatumStyleModification.addLeftElbow = this.datumLeftStyleListBox.GetItemChecked(1);
         DatumStyleModification.addRightElbow = this.datumRightStyleListBox.GetItemChecked(1);
         DatumStyleModification.changeLeftEnd2D = this.datumLeftStyleListBox.GetItemChecked(2);
         DatumStyleModification.changeRightEnd2D = this.datumRightStyleListBox.GetItemChecked(2);
         this.Close();
      }
   }
}
