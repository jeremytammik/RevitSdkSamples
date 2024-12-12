//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
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

namespace REX.PyramidGenerator.Resources.Dialogs
{
    /// <summary>
    /// Interaction logic for SubControl.xaml
    /// </summary>
    public partial class PyramidParametersControl : REX.Common.REXExtensionControl
    {
        //Step 3.1.: Preparing layouts
        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidParametersControl"/> class.
        /// </summary>
        public PyramidParametersControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidParametersControl"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public PyramidParametersControl(REX.Common.REXExtension extension)
            : base(extension)
        {
            InitializeComponent();

            //Controls registration
            ThisMainExtension.DFM.AddUnitObject(editH, Autodesk.REX.Framework.EUnitType.Dimensions_StructureDim, 1);
            editB.UnitEngine = ThisExtension.Units.UnitsBase;
            ThisMainExtension.DFM.SetBaseMinValue(editH, true, 1);
            editB.RangeMinCheck = true;
            editB.SetBaseMinValue(1);
        }

        /// <summary>
        /// Fills the controls with data
        /// </summary>
        public void SetDialog()
        {            
            ThisMainExtension.DFM.SetDataBase(editH, ThisMainExtension.Data.H);
            editB.SetDataBaseValue(ThisMainExtension.Data.B);

            comboFamilySymbol.Items.Clear();
            foreach (string familyString in ThisMainExtension.Data.AvailableFamilySymbols)
            {
                comboFamilySymbol.Items.Add(familyString);
            }

            comboFamilySymbol.SelectedItem = ThisMainExtension.Data.FamilySymbol;
        }
        /// <summary>
        /// Updates the Data with parameters from the controls
        /// </summary>
        public void SetData()
        {
            ThisMainExtension.Data.H = ThisMainExtension.DFM.GetDataBase(editH);
            ThisMainExtension.Data.B = editB.GetDataBaseValue();

            if (comboFamilySymbol.SelectedItem != null)
                ThisMainExtension.Data.FamilySymbol = comboFamilySymbol.SelectedItem.ToString();
            else
                ThisMainExtension.Data.FamilySymbol = "";
        }

    }
}
