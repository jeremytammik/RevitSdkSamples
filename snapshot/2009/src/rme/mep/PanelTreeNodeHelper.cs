using System;
using System.Collections.Generic;
using System.Text;

namespace mep
{
  class PanelTreeNodeHelper
  {
    System.Windows.Forms.TreeNode _tn;
    Autodesk.Revit.Element _element;

    public PanelTreeNodeHelper( Autodesk.Revit.Element e )
    {
      _element = e;
    }

    public PanelTreeNodeHelper( Autodesk.Revit.Element e, System.Windows.Forms.TreeNode tn )
    {
      _element = e;
      _tn = tn;
    }

    public System.Windows.Forms.TreeNode TreeNode
    {
      get { return _tn; }
      //set { _tn = value; }
    }


    public Autodesk.Revit.Element Element
    {
      get { return _element; }
      //set { _element = value; }
    }

    public override string ToString()
    {
      return _element.Name;
    }


    public static int CompareByName( PanelTreeNodeHelper x, PanelTreeNodeHelper y )
    {
      return string.Compare( x.ToString(), y.ToString() );
    }
  }
}
