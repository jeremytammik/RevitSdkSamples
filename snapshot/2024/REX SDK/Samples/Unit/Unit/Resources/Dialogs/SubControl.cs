using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using REX.Common;

namespace REX.Unit.Resources.Dialogs
{
    public partial class SubControl : REXExtensionControl
    {
        public SubControl(REXExtension Ext)
            : base(Ext)
        {
            InitializeComponent();
          
            ThisExtension.DFM.AddUnitObject(this.rexEditBoxA, "A", Autodesk.REX.Framework.EUnitType.Dimensions_Length, 1);
            ThisExtension.DFM.AddUnitObject(this.rexEditBoxB, "B", Autodesk.REX.Framework.EUnitType.Dimensions_Length  , 1);
            ThisExtension.DFM.AddUnitObject(this.rexEditBoxC, "C", Autodesk.REX.Framework.EUnitType.Dimensions_Length, 1);

     
            ThisExtension.DFM.SetBaseMinValue("A", true, 0.01);
            ThisExtension.DFM.SetBaseMinValue("B", true, 0.01);
            ThisExtension.DFM.SetBaseMinValue("C", true, 0.01);
            }
        

        public void SetDialog()
            {
            Data d = ((Extension)ThisExtension).Data;

            ThisExtension.DFM.SetDataBase("A", d.A);
            ThisExtension.DFM.SetDataBase("B", d.B);
            ThisExtension.DFM.SetDataBase("C", d.C);
            }

        public void SetData()
            {
            Data d = ((Extension)ThisExtension).Data;
            d.A = ThisExtension.DFM.GetDataBase("A");
            d.B = ThisExtension.DFM.GetDataBase("B");
            d.C = ThisExtension.DFM.GetDataBase("C");
            }

        private void rexEditBoxA_TextChanged(object sender, EventArgs e)
            {
            textBoxA.Text = ThisExtension.DFM.GetDataBase("A").ToString();
            textBoxIA.Text = ThisExtension.DFM.GetDataInterface("A").ToString();
            }

        private void rexEditBoxA_Leave(object sender, EventArgs e)
            {
            textBoxA.Text = ThisExtension.DFM.GetDataBase("A").ToString();
            textBoxIA.Text = ThisExtension.DFM.GetDataInterface("A").ToString();
            }

        private void rexEditBoxB_TextChanged(object sender, EventArgs e)
            {
            textBoxB.Text = ThisExtension.DFM.GetDataBase("B").ToString();
            textBoxIB.Text = ThisExtension.DFM.GetDataInterface("B").ToString();
            }

        private void rexEditBoxB_Leave(object sender, EventArgs e)
            {
            textBoxB.Text = ThisExtension.DFM.GetDataBase("B").ToString();
            textBoxIB.Text = ThisExtension.DFM.GetDataInterface("B").ToString();
            }

        private void rexEditBoxC_TextChanged(object sender, EventArgs e)
            {
            textBoxC.Text = ThisExtension.DFM.GetDataBase("C").ToString();
            textBoxIC.Text = ThisExtension.DFM.GetDataInterface("C").ToString();
            }

        private void rexEditBoxC_Leave(object sender, EventArgs e)
            {
            textBoxC.Text = ThisExtension.DFM.GetDataBase("C").ToString();
            textBoxIC.Text = ThisExtension.DFM.GetDataInterface("C").ToString();
            }
        }
    }
