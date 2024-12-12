#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
#endregion // Namespaces

namespace XtraCs
{
  /// <summary>
  /// From http://www.timvw.be/presenting-the-sortablebindinglistt-take-two
  /// </summary>
  public class PropertyComparer<T> : IComparer<T>
  {
    private readonly IComparer comparer;
    private PropertyDescriptor propertyDescriptor;
    private int reverse;

    public PropertyComparer( PropertyDescriptor property, ListSortDirection direction )
    {
      this.propertyDescriptor = property;
      Type comparerForPropertyType = typeof( Comparer<> ).MakeGenericType( property.PropertyType );
      this.comparer = (IComparer) comparerForPropertyType.InvokeMember( "Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null );
      this.SetListSortDirection( direction );
    }

    #region IComparer<T> Members

    public int Compare( T x, T y )
    {
      return this.reverse * this.comparer.Compare( this.propertyDescriptor.GetValue( x ), this.propertyDescriptor.GetValue( y ) );
    }

    #endregion

    private void SetPropertyDescriptor( PropertyDescriptor descriptor )
    {
      this.propertyDescriptor = descriptor;
    }

    private void SetListSortDirection( ListSortDirection direction )
    {
      this.reverse = direction == ListSortDirection.Ascending ? 1 : -1;
    }

    public void SetPropertyAndDirection( PropertyDescriptor descriptor, ListSortDirection direction )
    {
      this.SetPropertyDescriptor( descriptor );
      this.SetListSortDirection( direction );
    }
  }
}
