using System.Windows.Forms;

namespace AdnRme
{
  class WaitCursor
  {
    Cursor _oldCursor;

    public WaitCursor()
    {
      _oldCursor = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
    }

    ~WaitCursor()
    {
      Cursor.Current = _oldCursor;
    }
  }
}
