//
// (C) Copyright 2003-2012 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Revit.SDK.Samples.ProgressNotifier.CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application"></param>
        public MainWindow(Autodesk.Revit.ApplicationServices.Application application)
        {
            InitializeComponent();
            m_application = application;
            m_progressStack = new ProgressStack();
            m_application.ProgressChanged += new EventHandler<Autodesk.Revit.DB.Events.ProgressChangedEventArgs>(RevitApp_ProgressChanged);
            m_application.DocumentOpened += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(RevitApp_DocumentOpened);
            m_cancel = false;
            m_receivedCancelEvent = false;
            m_previousEvent = null;
        }

        #region 'Event handlers for document opened and progress changed'
        void RevitApp_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            textBox_log.Text += "RevitApp_DocumentOpened: Cancellable:" + e.Cancellable.ToString() + " , IsCancelled: " + e.IsCancelled().ToString() + " , Status:" + e.Status.ToString() + Environment.NewLine;
            m_receivedCancelEvent = (e.Status == Autodesk.Revit.DB.Events.RevitAPIEventStatus.Cancelled);
        }
        void RevitApp_ProgressChanged(object sender, Autodesk.Revit.DB.Events.ProgressChangedEventArgs e)
        {

            this.button_Cancel.IsEnabled = e.Cancellable;
            System.Windows.Forms.Application.DoEvents();

            if (m_cancel)
            {
                bool shouldCancel = e.Cancellable;
                if (e.IsCancelled())
                    textBox_log.Text += "!! We are already canceled!" + Environment.NewLine;

                try
                {
                    e.Cancel();
                    m_cancel = false;
                }
                catch (Exception ex)
                {
                    textBox_log.Text += ("Exception: " + ex.ToString() + Environment.NewLine + "'  Cancelable' value: " + shouldCancel + Environment.NewLine);
                }

            }



            ProgressItem itemReturn = m_progressStack.AddEventData(e);

            this.stackPanel_ProgressData.Children.Clear();
            List<string> progressItems = m_progressStack.ToStringList(6);
            foreach (string progressItem in progressItems)
            {
                TextBox tbProgressItem = new TextBox();
                tbProgressItem.Text = progressItem;
                this.stackPanel_ProgressData.Children.Add(tbProgressItem);
            }


            if ((itemReturn.Stage == Autodesk.Revit.DB.Events.ProgressStage.RangeChanged) || (itemReturn.Stage == Autodesk.Revit.DB.Events.ProgressStage.UserCancelled))
            {
                string previousEventData = "null";
                if (m_previousEvent != null)
                    previousEventData = m_previousEvent.ToString();

                //   textBox_log.Text += "Previous: "+  previousEventData + Environment.NewLine + "Current: " + itemReturn.ToString() + Environment.NewLine;
            }

            System.Windows.Forms.Application.DoEvents();

            m_previousEvent = itemReturn;


        }
        #endregion


        #region Handlers for 'Open' and 'Cancel' buttons
        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            ofd.DefaultExt = ".rvt";
            ofd.Filter = "rvt files (*.rvt)|*.rvt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "Open revit documents.";

            ofd.ShowDialog();
            if (!System.IO.File.Exists(ofd.FileName))
                return;

            label_FileName.Content = ofd.FileName;
            System.Windows.Forms.Application.DoEvents();
            Autodesk.Revit.DB.Document document = null;
            try
            {
                document = m_application.OpenDocumentFile(ofd.FileName);
                this.textBox_log.Text += "Opened filename = " + document.Title + Environment.NewLine;
            }
            catch (System.Exception ex)
            {
                if (m_receivedCancelEvent)
                {
                    string isNull;
                    if (document == null)
                        isNull = " is null.";
                    else
                        isNull = " is not null.";

                    this.textBox_log.Text += "Open Document has thrown an exception." + Environment.NewLine;
                    this.textBox_log.Text += "We just got a cancel event, so this exception is likely from 'Open' being canceled. Returned document" + isNull + Environment.NewLine;
                    m_receivedCancelEvent = false;
                }
                else
                    this.textBox_log.Text += ex.ToString() + Environment.NewLine;
            }



        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            m_cancel = true;
        }
        #endregion

        #region Data
        Autodesk.Revit.ApplicationServices.Application m_application;
        ProgressStack m_progressStack;
        bool m_cancel;

        private bool m_receivedCancelEvent;
        private ProgressItem m_previousEvent;
        #endregion




    }
}
