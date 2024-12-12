using Autodesk.Revit.DB;

namespace AdnRme
{
  class PanelTreeNodeHelper
  {
    System.Windows.Forms.TreeNode _tn;
    Autodesk.Revit.DB.Element _element;

    public PanelTreeNodeHelper( Element e )
    {
      _element = e;
    }

    public PanelTreeNodeHelper( Element e, System.Windows.Forms.TreeNode tn )
    {
      _element = e;
      _tn = tn;
    }

    public System.Windows.Forms.TreeNode TreeNode
    {
      get { return _tn; }
      //set { _tn = value; }
    }

    public Element Element
    {
      get { return _element; }
      //set { _element = value; }
    }

    public override string ToString()
    {
      return Element.Name;
    }


    public static int CompareByName( PanelTreeNodeHelper x, PanelTreeNodeHelper y )
    {
      return string.Compare( x.ToString(), y.ToString() );
    }
  }
}
