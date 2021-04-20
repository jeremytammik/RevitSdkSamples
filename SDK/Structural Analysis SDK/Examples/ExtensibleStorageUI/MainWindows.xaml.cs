//
// (C) Copyright 2003-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System.Windows;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.WPF;


namespace ExtensibleStorageUI
{
    /// <summary>
    /// Interaction logic for MainWindows.xaml
    /// </summary>
    public partial class MainWindows : WindowBase
    {
        // Windows with simple event management 
        // Close, don't save schemas
        // Cancel, don't save schemas
        // Ok, save schemas in selected elements 
        public bool storeSchema; 
        public MainWindows()
        {
            InitializeComponent();
            this.storeSchema = false; 
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.storeSchema = true; 
            this.Close(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.storeSchema = false; 
            this.Close();  
        }

     
    }
}