//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

 
namespace Revit.SDK.Samples.EventsMonitor.CS
{
    partial class EventsInfoWindows
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.appEventsLogDataGridView = new System.Windows.Forms.DataGridView();
            this.timeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.appEventsLogDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // appEventsLogDataGridView
            // 
            this.appEventsLogDataGridView.AllowUserToAddRows = false;
            this.appEventsLogDataGridView.AllowUserToDeleteRows = false;
            this.appEventsLogDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.appEventsLogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.appEventsLogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timeColumn,
            this.eventColumn,
            this.typeColumn});
            this.appEventsLogDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appEventsLogDataGridView.Location = new System.Drawing.Point(0, 0);
            this.appEventsLogDataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.appEventsLogDataGridView.Name = "appEventsLogDataGridView";
            this.appEventsLogDataGridView.ReadOnly = true;
            this.appEventsLogDataGridView.RowHeadersVisible = false;
            this.appEventsLogDataGridView.RowTemplate.Height = 24;
            this.appEventsLogDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.appEventsLogDataGridView.Size = new System.Drawing.Size(475, 116);
            this.appEventsLogDataGridView.TabIndex = 0;
            this.appEventsLogDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.appEventsLogDataGridView_RowsAdded);
            // 
            // timeColumn
            // 
            this.timeColumn.HeaderText = "Time";
            this.timeColumn.Name = "timeColumn";
            this.timeColumn.ReadOnly = true;
            this.timeColumn.Width = 120;
            // 
            // eventColumn
            // 
            this.eventColumn.HeaderText = "Event";
            this.eventColumn.Name = "eventColumn";
            this.eventColumn.ReadOnly = true;
            this.eventColumn.Width = 150;
            // 
            // typeColumn
            // 
            this.typeColumn.HeaderText = "Type";
            this.typeColumn.Name = "typeColumn";
            this.typeColumn.ReadOnly = true;
            this.typeColumn.Width = 200;
            // 
            // EventsInfoWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 116);
            this.Controls.Add(this.appEventsLogDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EventsInfoWindows";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Events History";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.applicationEventsInfoWindows_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InformationWindows_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.appEventsLogDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView appEventsLogDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
    }
}