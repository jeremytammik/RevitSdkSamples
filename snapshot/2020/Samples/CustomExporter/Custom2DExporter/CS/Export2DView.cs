using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revit.SDK.Samples.Custom2DExporter.CS
{
   public partial class Export2DView : System.Windows.Forms.Form
   {
      public Export2DView()
      {
         InitializeComponent();
         m_exportOptions = new ExportOptions();
         m_exportOptions.ExportAnnotationObjects = false;
         m_exportOptions.ExportPatternLines = false;
      }

      ExportOptions m_exportOptions = null;

      public ExportOptions ViewExportOptions
      {
         get
         {
            return m_exportOptions;
         }
      }

      /// <summary>
      /// The option for creating Path of Travel.
      /// </summary>
      public class ExportOptions
      {
         bool m_exportAnnotationObjects;
         bool m_exportPatternLines;


         public bool ExportAnnotationObjects
         {
            get
            {
               return m_exportAnnotationObjects;
            }
            set
            {
               m_exportAnnotationObjects = value;
            }
         }

         public bool ExportPatternLines
         {
            get
            {
               return m_exportPatternLines;
            }
            set
            {
               m_exportPatternLines = value;
            }
         }
      }

      private void checkBox2_CheckedChanged(object sender, EventArgs e)
      {
         m_exportOptions.ExportAnnotationObjects = checkBox2.Checked;
      }

      private void checkBox3_CheckedChanged(object sender, EventArgs e)
      {
         m_exportOptions.ExportPatternLines = checkBox3.Checked;
      }
   }
}
