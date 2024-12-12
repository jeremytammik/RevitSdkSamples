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
  /// </summary>
  public class SortableBindingList<T> : BindingList<T>
  {
    private PropertyDescriptor propertyDescriptor;
    private ListSortDirection listSortDirection;
    private bool isSorted;

    public SortableBindingList()
    {
    }

    public SortableBindingList( IEnumerable<T> enumerable )
    {
      if( enumerable == null )
      {
        throw new ArgumentNullException( "enumerable" );
      }

      foreach( T t in enumerable )
      {
        this.Add( t );
      }
    }

    protected override bool SupportsSortingCore
    {
      get { return true; }
    }

    protected override bool IsSortedCore
    {
      get { return this.isSorted; }
    }

    protected override PropertyDescriptor SortPropertyCore
    {
      get { return this.propertyDescriptor; }
    }

    protected override ListSortDirection SortDirectionCore
    {
      get { return this.listSortDirection; }
    }

    protected override void ApplySortCore( PropertyDescriptor prop, ListSortDirection direction )
    {
      List<T> itemsList = this.Items as List<T>;
      itemsList.Sort( delegate( T t1, T t2 )
      {
        this.propertyDescriptor = prop;
        this.listSortDirection = direction;
        this.isSorted = true;

        int reverse = (direction == ListSortDirection.Ascending) ? 1 : -1;

        object value1 = prop.GetValue( t1 );
        object value2 = prop.GetValue( t2 );

        return reverse * Comparer.Default.Compare( value1, value2 );
      } );

      this.OnListChanged( new ListChangedEventArgs( ListChangedType.Reset, -1 ) );
    }

    protected override void RemoveSortCore()
    {
      this.isSorted = false;
      this.propertyDescriptor = base.SortPropertyCore;
      this.listSortDirection = base.SortDirectionCore;
    }
  }

}
