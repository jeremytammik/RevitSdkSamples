using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Labs
{
  /// <summary>
  /// Ask user to type in an element id or click a button to pick an element on screen.
  /// Use ShowDialog() and check the dialogue result, which will be either
  /// OK or Cancel. In the former case, check the ElementId property.
  /// If it is empty, pick an element on screen, else use the typed-in element id.
  /// </summary>
  public partial class ElementIdForm : Form
  {
    string _element_id;

    public ElementIdForm()
    {
      _element_id = string.Empty;
      InitializeComponent();
    }

    private void textBox1_TextChanged( object sender, EventArgs e )
    {
      _element_id = textBox1.Text;
    }

    private void btnCancel_Click( object sender, EventArgs e )
    {
      _element_id = string.Empty;
      Hide();
      DialogResult = DialogResult.Cancel;
    }

    private void btnOk_Click( object sender, EventArgs e )
    {
      Hide();
      DialogResult = DialogResult.OK;
    }

    private void btnPick_Click( object sender, EventArgs e )
    {
      _element_id = string.Empty;
      Hide();
      DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// Typed-in element id, or empty string to pick element on screen.
    /// </summary>
    public string ElementId
    {
      get
      {
        return _element_id;
      }
    }
  }
}
