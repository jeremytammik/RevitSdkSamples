using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events; 

namespace GetTimeElapsed_CSharp
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.UI.Macros.AddInId("B8A5B512-EBA0-4A38-8F6C-E2254840C978")]
    public partial class ThisApplication
    {
        Dictionary<Document, DateTime> m_dicLastSaved = new Dictionary<Document, DateTime>();

        private void Module_Startup(object sender, EventArgs e)
        {
            InitializeDicLastSaved();
            this.Application.DocumentSaved += new EventHandler<DocumentSavedEventArgs>(ThisApplication_DocumentSaved);
            this.Application.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(ThisApplication_DocumentOpened);
            this.Application.DocumentCreated += new EventHandler<DocumentCreatedEventArgs>(ThisApplication_DocumentCreated);
            this.Application.DocumentClosing += new EventHandler<DocumentClosingEventArgs>(ThisApplication_DocumentClosing);
        }

        private void Module_Shutdown(object sender, EventArgs e)
        {
            this.Application.DocumentSaved -= new EventHandler<DocumentSavedEventArgs>(ThisApplication_DocumentSaved);
            this.Application.DocumentOpened -= new EventHandler<DocumentOpenedEventArgs>(ThisApplication_DocumentOpened);
            this.Application.DocumentCreated -= new EventHandler<DocumentCreatedEventArgs>(ThisApplication_DocumentCreated);
            this.Application.DocumentClosing -= new EventHandler<DocumentClosingEventArgs>(ThisApplication_DocumentClosing);
        }

        #region Revit Macros generated code
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Module_Startup);
            this.Shutdown += new System.EventHandler(Module_Shutdown);
        }
        #endregion

        /// <summary>
        /// Initialize
        /// </summary>
        private void InitializeDicLastSaved()
        {
            foreach (Document document in this.Application.Documents)
            {
                m_dicLastSaved.Add(document, DateTime.MaxValue);
            }
        }

        /// <summary>
        /// DocumentCreated event, add the created document into m_dicLastSaved
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">DocumentCreatedEventArgs</param>
        void ThisApplication_DocumentCreated(object sender, DocumentCreatedEventArgs args)
        {
            m_dicLastSaved.Add(args.Document, DateTime.MaxValue);
        }

        /// <summary>
        /// DocumentClosing event, remove the closing document from m_dicLastSaved
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">DocumentClosingEventArgs</param>
        void ThisApplication_DocumentClosing(object sender, DocumentClosingEventArgs args)
        {
            m_dicLastSaved.Remove(args.Document);
        }

        /// <summary>
        /// DocumentOpened event, add the opened document into m_dicLastSaved
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">DocumentOpenedEventArgs</param>
        void ThisApplication_DocumentOpened(object sender, DocumentOpenedEventArgs args)
        {
            m_dicLastSaved.Add(args.Document, DateTime.MaxValue);
        }

        /// <summary>
        /// DocumentSaved event, record the current DataTime for the saved document
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">DocumentSavedEventArgs</param>
        void ThisApplication_DocumentSaved(object sender , DocumentSavedEventArgs args)
        {
            this.m_dicLastSaved[args.Document] = DateTime.Now;
        }

        /// <summary>
        /// FormatTimeSpan 
        /// </summary>
        /// <param name="elapse">TimeSpan</param>
        /// <returns>the string of TimeSpan</returns>
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
