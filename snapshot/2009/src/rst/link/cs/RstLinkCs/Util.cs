using WinForms = System.Windows.Forms;

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
      WinForms.MessageBox.Show( msg, "Revit Structure Link Sample",
        WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information );
    }
    #endregion // Message handlers
  }
}
