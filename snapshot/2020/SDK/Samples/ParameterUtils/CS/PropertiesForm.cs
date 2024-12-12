//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.ParameterUtils.CS
{
    public partial class PropertiesForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Default constructor, initialize all controls
        /// </summary>
        private PropertiesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This Form is used to display the properties that exist upon an element. 
        /// It consists of a list view and the ok, cancel buttons.
        /// </summary>
        /// <param name="information">A string array that will be loaded into the list view</param>
        public PropertiesForm(string[] information)
            : this()
        {
            // we need to add each string in to each row of the list view, and split the string
            // into substrings delimited by '\t' then put them into the columns of the row.

            // create three columns with "Name", "Type" and "Value"
            propertyListView.Columns.Add("Name");
            propertyListView.Columns.Add("Type");
            propertyListView.Columns.Add("Value");

            // loop all the strings, split them, and add them to rows of the list view
            foreach (string row in information)
            {
                if (row == null) continue;
                ListViewItem lvi = new ListViewItem(row.Split('\t'));
                propertyListView.Items.Add(lvi);
            }

            // The following code is used to sort and resize the columns within the list view 
            // so that the data can be viewed better.

            // sort the items in the list view ordered by ascending.
            propertyListView.Sorting = SortOrder.Ascending;

            // make the column width fit the content
            propertyListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // increase the width of columns by 40, make them a litter wider
            int span = 40;
            foreach (ColumnHeader ch in propertyListView.Columns)
            {
                ch.Width += span;
            }

            // the last column fit the rest of the list view
            propertyListView.Columns[propertyListView.Columns.Count - 1].Width = -2;
        }
    }
}