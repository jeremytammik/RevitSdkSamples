using System.Windows.Forms;

namespace RstLink
{
  public class Util
  {
    #region Message handlers
    /// <summary>
    /// MessageBox wrapper for informational message.
    /// </summary>
    public static void InfoMsg( string msg )
    {
      MessageBox.Show( msg, "Revit Structure Link Sample",
        MessageBoxButtons.OK, MessageBoxIcon.Information );
    }
    #endregion // Message handlers
  }
}
