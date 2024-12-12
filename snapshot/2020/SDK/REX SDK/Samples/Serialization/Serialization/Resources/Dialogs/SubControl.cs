using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using REX.Common;

namespace REX.Serialization.Resources.Dialogs
{
    public partial class SubControl : REXExtensionControl
    {
        public SubControl(REXExtension Ext)
            : base(Ext)
        {
            InitializeComponent();
            ThisExtension.DFM.AddUnitObject(this.rexEditBox1, "A", Autodesk.REX.Framework.EUnitType.Dimensions_SectionDim, 1);
            ThisExtension.DFM.AddUnitObject(this.rexEditBox2, "B", Autodesk.REX.Framework.EUnitType.Dimensions_Length, 1);
            }

        public void SetDialog()
            {
            Data d = ((Extension)ThisExtension).Data;


            ThisExtension.DFM.SetDataBase("A", d.A);
            ThisExtension.DFM.SetDataBase("B", d.B);
            }

        public void SetData()
            {
            Data d = ((Extension)ThisExtension).Data;

            d.A = ThisExtension.DFM.GetDataBase("A");
            d.B = ThisExtension.DFM.GetDataBase("B");
            
            }

        /// <summary>
        /// Gets the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }

      
    }
}
