'
' (C) Copyright 2003-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted,
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'

''' <summary>
''' Form class to pop up a hello message
''' </summary>
Public Class HelloRevitForm
    Inherits System.Windows.Forms.Form

    ''' <summary>
    ''' construction method
    ''' </summary>
    Public Sub New()
        MyBase.New()

        InitializeComponent()

    End Sub

    ''' <summary>
    ''' resource clear method
    ''' </summary>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

   Private components As System.ComponentModel.IContainer
   Private WithEvents oKbutton As System.Windows.Forms.Button

   Friend WithEvents LabelHelloRevit As System.Windows.Forms.Label
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.LabelHelloRevit = New System.Windows.Forms.Label
      Me.oKbutton = New System.Windows.Forms.Button
      Me.SuspendLayout()
      '
      'LabelHelloRevit
      '
      Me.LabelHelloRevit.Location = New System.Drawing.Point(42, 40)
      Me.LabelHelloRevit.Name = "LabelHelloRevit"
      Me.LabelHelloRevit.Size = New System.Drawing.Size(64, 16)
      Me.LabelHelloRevit.TabIndex = 0
      Me.LabelHelloRevit.Text = "Hello Revit!"
      '
      'oKbutton
      '
      Me.oKbutton.DialogResult = System.Windows.Forms.DialogResult.OK
      Me.oKbutton.Location = New System.Drawing.Point(45, 67)
      Me.oKbutton.Name = "oKbutton"
      Me.oKbutton.Size = New System.Drawing.Size(61, 23)
      Me.oKbutton.TabIndex = 1
      Me.oKbutton.Text = "&OK"
      Me.oKbutton.UseVisualStyleBackColor = True
      '
      'HelloRevitForm
      '
      Me.AcceptButton = Me.oKbutton
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.CancelButton = Me.oKbutton
      Me.ClientSize = New System.Drawing.Size(157, 102)
      Me.Controls.Add(Me.oKbutton)
      Me.Controls.Add(Me.LabelHelloRevit)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "HelloRevitForm"
      Me.ShowInTaskbar = False
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "Hello Revit"
      Me.ResumeLayout(False)

   End Sub

   Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oKbutton.Click
      Me.Close()
   End Sub
End Class
