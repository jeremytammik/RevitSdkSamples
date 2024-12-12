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
