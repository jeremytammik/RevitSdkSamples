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
using REX.ContentGeneratorWPF.Main;

namespace REX.ContentGeneratorWPF.Resources.Dialogs
{
    /// <summary>
    /// Represents the control which allows the user to present the list of properties.
    /// </summary>
    public partial class PropertiesControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesControl"/> class.
        /// </summary>
        public PropertiesControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Adds the property to the list.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddProperty(PropertyItem item)
        {
            stackPanel1.Children.Add(new PropertyControl(item));
        }
        /// <summary>
        /// Adds the property to the list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddProperty(string name, string value)
        {
            stackPanel1.Children.Add(new PropertyControl(name, value));
        }
        /// <summary>
        /// Adds the category to the list.
        /// </summary>
        /// <param name="name">The name.</param>
        public void AddCategory(string name)
        {
            TextBlock block = new TextBlock();
            block.FontWeight = FontWeights.Bold;
            block.Text = name;
            block.Margin = new Thickness(3,6,3,3);
            stackPanel1.Children.Add(block);

            Separator separator = new Separator();
            stackPanel1.Children.Add(separator);           
        }
        /// <summary>
        /// Clears the property list.
        /// </summary>
        public void ClearProperties()
        {
            stackPanel1.Children.Clear();
        }
    }
}
