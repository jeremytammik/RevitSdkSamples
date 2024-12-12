//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// From http://www.timvw.be/presenting-the-sortablebindinglistt/
    /// This class inherits BindingList and adds sort functionality.
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        // Property descriptor that is used for sorting the list
        private PropertyDescriptor propertyDescriptor;
        // System.ComponentModel.ListSortDirection
        private ListSortDirection listSortDirection;
        // Flag indicates whether the list is sorted
        private bool isSorted;

        /// <summary>
        /// Initializes a new instance of the SortableBindingList class using default values
        /// </summary>
        public SortableBindingList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SortableBindingList class
        /// with the specified list.
        /// </summary>
        /// <param name="enumerable">
        /// A System.Collections.Generic.IEnumerator item to be contained in the SortableBindingList
        /// </param>
        public SortableBindingList(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            foreach (T t in enumerable)
            {
                this.Add(t);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get
            { 
                return true; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the list is sorted
        /// </summary>
        protected override bool IsSortedCore
        {
            get
            {
                return this.isSorted; 
            }
        }

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                return this.propertyDescriptor;
            }
        }

        /// <summary>
        /// Gets the sort direction
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get 
            {
                return this.listSortDirection; 
            }
        }

        /// <summary>
        /// Sorts the items
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> itemsList = this.Items as List<T>;
            itemsList.Sort(delegate(T t1, T t2)
            {
                this.propertyDescriptor = prop;
                this.listSortDirection = direction;
                this.isSorted = true;

                int reverse = (direction == ListSortDirection.Ascending) ? 1 : -1;

                object value1 = prop.GetValue(t1);
                object value2 = prop.GetValue(t2);

                return reverse * Comparer.Default.Compare(value1, value2);
            });

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Removes any sort 
        /// </summary>
        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.propertyDescriptor = base.SortPropertyCore;
            this.listSortDirection = base.SortDirectionCore;
        }
    }

}
