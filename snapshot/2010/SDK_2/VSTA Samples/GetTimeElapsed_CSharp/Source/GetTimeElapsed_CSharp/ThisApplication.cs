using System;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace GetTimeElapsed_CSharp
{
    [System.AddIn.AddIn("ThisApplication", Version = "1.0", Publisher = "", Description = "")]
    public partial class ThisApplication
    {
        Dictionary<Document, DateTime> m_dicLastSaved = new Dictionary<Document, DateTime>();

        private void Module_Startup(object sender, EventArgs e)
        {
            InitializeDicLastSaved();
            this.OnDocumentSaved += new Autodesk.Revit.Events.DocumentSavedEventHandler(ThisApplication_OnDocumentSaved);
            this.OnDocumentOpened += new Autodesk.Revit.Events.DocumentOpenedEventHandler(ThisApplication_OnDocumentOpened);
            this.OnDocumentNewed += new Autodesk.Revit.Events.DocumentNewedEventHandler(ThisApplication_OnDocumentNewed);
            this.OnDocumentClosed += new Autodesk.Revit.Events.DocumentClosedEventHandler(ThisApplication_OnDocumentClosed);
        }

        private void Module_Shutdown(object sender, EventArgs e)
        {
            this.OnDocumentSaved -= new Autodesk.Revit.Events.DocumentSavedEventHandler(ThisApplication_OnDocumentSaved);
            this.OnDocumentOpened -= new Autodesk.Revit.Events.DocumentOpenedEventHandler(ThisApplication_OnDocumentOpened);
            this.OnDocumentNewed -= new Autodesk.Revit.Events.DocumentNewedEventHandler(ThisApplication_OnDocumentNewed);
            this.OnDocumentClosed -= new Autodesk.Revit.Events.DocumentClosedEventHandler(ThisApplication_OnDocumentClosed);
        }

        #region VSTA generated code
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Module_Startup);
            this.Shutdown += new System.EventHandler(Module_Shutdown);
        }
        #endregion

        private void InitializeDicLastSaved()
        {
            foreach (Document document in this.Documents)
            {
                m_dicLastSaved.Add(document, DateTime.MaxValue);
            }
        }

        void ThisApplication_OnDocumentNewed(Document document)
        {
            m_dicLastSaved.Add(document, DateTime.MaxValue);
        }

        void ThisApplication_OnDocumentClosed(Document document)
        {
            m_dicLastSaved.Remove(document);
        }

        void ThisApplication_OnDocumentOpened(Document document)
        {
            m_dicLastSaved.Add(document, DateTime.MaxValue);
        }

        void ThisApplication_OnDocumentSaved(Document document)
        {
            this.m_dicLastSaved[document] = DateTime.Now;
        }

        private String FormatTimeSpan(TimeSpan elapse)
        {
            String elapseStr = elapse.ToString();
            int lastIndexOfDot = elapseStr.LastIndexOf('.');
            return elapseStr.Substring(0, lastIndexOfDot);
        }

        /// <summary>
        /// GetTimeElapsedSinceLastSave
        /// </summary>
        public void GetTimeElapsedSinceLastSave()
        {
            String text = String.Format("{0,-30}{1,-30}",
                "Document Full Name",
                "Elapse Time Since Last Save(day.hh:mm:ss)")
                + "\n";
            foreach (KeyValuePair<Document, DateTime> pair in m_dicLastSaved)
            {
                String strElapsed = String.Empty;
                if (pair.Value == DateTime.MaxValue)
                {
                    strElapsed = "Never";
                }
                else
                {
                    strElapsed = FormatTimeSpan(DateTime.Now - pair.Value);
                }

                String fileName = System.IO.Path.GetFileName(pair.Key.PathName);
                if (String.IsNullOrEmpty(fileName))
                {
                    fileName = "*New*";
                }

                text += String.Format("{0,-30}{1,-30}", fileName, strElapsed) + "\n";
            }

            System.Windows.Forms.MessageBox.Show(text, "Macro GetTimeElapsedSinceLastSave");
        }
    }
}
