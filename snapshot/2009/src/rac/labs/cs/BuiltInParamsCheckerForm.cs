using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Labs
{
  public partial class BuiltInParamsCheckerForm : Form
  {
    SortableBindingList<BuiltInParamsChecker.ParameterData> _data;

    public BuiltInParamsCheckerForm(
      string description,
      SortableBindingList<BuiltInParamsChecker.ParameterData> data )
    {
      _data = data;
      InitializeComponent();
      Text = description + " " + Text;
    }

    private void BuilItParamsCheckerForm_Load( object sender, EventArgs e )
    {
      dataGridView1.DataSource = _data;
      dataGridView1.Columns[0].HeaderText = "BuiltInParameter";
      dataGridView1.Columns[1].HeaderText = "Parameter Name";
      dataGridView1.Columns[2].HeaderText = "Type";
      dataGridView1.Columns[3].HeaderText = "Read/Write";
      dataGridView1.Columns[4].HeaderText = "String Value";
      dataGridView1.Columns[5].HeaderText = "Database Value";
      int w = dataGridView1.Width / 6;
      foreach( DataGridViewColumn c in dataGridView1.Columns )
      {
        c.Width = w;
      }
    }

    private void copyToClipboardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string s = Text + "\r\n";
      foreach( BuiltInParamsChecker.ParameterData a in _data )
      {
        s += "\r\n" + a.Enum + "\t" + a.Name + "\t" + a.Type + "\t" + a.ReadWrite + "\t" + a.ValueString + "\t" + a.Value;
      }
      if( 0 < s.Length )
      {
        Clipboard.SetDataObject( s );
      }
    }
  }
}