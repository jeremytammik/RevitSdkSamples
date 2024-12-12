using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using REX.Common;

namespace REX.FrameGenerator.Resources.Dialogs
{
    public partial class SubControl : REXExtensionControl
    {
        public SubControl(REXExtension Ext)
            : base(Ext)
        {
            InitializeComponent();

          //  double result = this.ThisExtension.Units.Interface(value, Autodesk.REX.Framework.EUnitType.Dimensions_Length, this.ThisContext.Product.Type);
           

            ThisExtension.DFM.AddUnitObject(this.rexNbX , "NbX", Autodesk.REX.Framework.EUnitType.Other_Number, 1);
            ThisExtension.DFM.AddUnitObject(this.rexNbY, "NbY", Autodesk.REX.Framework.EUnitType.Other_Number, 1);
            ThisExtension.DFM.AddUnitObject(this.rexSpacingX, "SpacingX", Autodesk.REX.Framework.EUnitType.Dimensions_Length , 1);
            ThisExtension.DFM.AddUnitObject(this.rexSpacingY, "SpacingY", Autodesk.REX.Framework.EUnitType.Dimensions_Length, 1);
           
            ThisExtension.DFM.SetBaseMinValue("NbX", true, 2);
            ThisExtension.DFM.SetBaseMinValue("NbY", true, 2);
            ThisExtension.DFM.SetBaseMinValue("SpacingX", true, 1);
            ThisExtension.DFM.SetBaseMinValue("SpacingY", true, 1);

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

        public void SetDialog()
        {
            Data d = ((Extension)ThisExtension).Data;
            ThisExtension.DFM.SetDataBase("NbX", d.NbX);
            ThisExtension.DFM.SetDataBase("NbY", d.NbY);
            ThisExtension.DFM.SetDataBase("SpacingX", d.SpacingX);
            ThisExtension.DFM.SetDataBase("SpacingY", d.SpacingY);
        }

        public void SetData()
        {
            Data d = ((Extension)ThisExtension).Data;
            d.NbX = ThisExtension.DFM.GetDataBase("NbX");
            d.NbY = ThisExtension.DFM.GetDataBase("NbY");
            d.SpacingX = ThisExtension.DFM.GetDataBase("SpacingX");
            d.SpacingY = ThisExtension.DFM.GetDataBase("SpacingY");
    
        }
    }
}
