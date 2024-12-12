' 
' (C) Copyright 2003-2010 by Autodesk, Inc.
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
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
' 
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'

Imports System.Windows.Forms

Public Class ViewerWindow
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents RenderPane As System.Windows.Forms.PictureBox
  Friend WithEvents ViewerToolTip As System.Windows.Forms.ToolTip
  Friend WithEvents PanLeftButton As System.Windows.Forms.Button
  Friend WithEvents PanUpButton As System.Windows.Forms.Button
  Friend WithEvents PanRightButton As System.Windows.Forms.Button
  Friend WithEvents PanDownButton As System.Windows.Forms.Button
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents RotateDownButton As System.Windows.Forms.Button
  Friend WithEvents RotateRightButton As System.Windows.Forms.Button
  Friend WithEvents RotateLeftButton As System.Windows.Forms.Button
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents RotateUpButton As System.Windows.Forms.Button
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents ZoomInButton As System.Windows.Forms.Button
  Friend WithEvents ZoomOutButton As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ViewerWindow))
        Me.RenderPane = New System.Windows.Forms.PictureBox
        Me.ViewerToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.PanLeftButton = New System.Windows.Forms.Button
        Me.PanUpButton = New System.Windows.Forms.Button
        Me.PanRightButton = New System.Windows.Forms.Button
        Me.PanDownButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.RotateDownButton = New System.Windows.Forms.Button
        Me.RotateRightButton = New System.Windows.Forms.Button
        Me.RotateLeftButton = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.RotateUpButton = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.ZoomInButton = New System.Windows.Forms.Button
        Me.ZoomOutButton = New System.Windows.Forms.Button
        CType(Me.RenderPane, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RenderPane
        '
        Me.RenderPane.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RenderPane.BackColor = System.Drawing.Color.Black
        Me.RenderPane.Location = New System.Drawing.Point(9, 9)
        Me.RenderPane.Name = "RenderPane"
        Me.RenderPane.Size = New System.Drawing.Size(454, 310)
        Me.RenderPane.TabIndex = 0
        Me.RenderPane.TabStop = False
        Me.ViewerToolTip.SetToolTip(Me.RenderPane, "Use the arrow keys to move + Ctrl=Rotate Shift=Zoom")
        '
        'ViewerToolTip
        '
        Me.ViewerToolTip.ShowAlways = True
        '
        'PanLeftButton
        '
        Me.PanLeftButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanLeftButton.Image = CType(resources.GetObject("PanLeftButton.Image"), System.Drawing.Image)
        Me.PanLeftButton.Location = New System.Drawing.Point(475, 37)
        Me.PanLeftButton.Name = "PanLeftButton"
        Me.PanLeftButton.Size = New System.Drawing.Size(27, 27)
        Me.PanLeftButton.TabIndex = 1
        Me.PanLeftButton.TabStop = False
        '
        'PanUpButton
        '
        Me.PanUpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanUpButton.Image = CType(resources.GetObject("PanUpButton.Image"), System.Drawing.Image)
        Me.PanUpButton.Location = New System.Drawing.Point(510, 9)
        Me.PanUpButton.Name = "PanUpButton"
        Me.PanUpButton.Size = New System.Drawing.Size(28, 28)
        Me.PanUpButton.TabIndex = 1
        Me.PanUpButton.TabStop = False
        '
        'PanRightButton
        '
        Me.PanRightButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanRightButton.Image = CType(resources.GetObject("PanRightButton.Image"), System.Drawing.Image)
        Me.PanRightButton.Location = New System.Drawing.Point(545, 37)
        Me.PanRightButton.Name = "PanRightButton"
        Me.PanRightButton.Size = New System.Drawing.Size(28, 27)
        Me.PanRightButton.TabIndex = 1
        Me.PanRightButton.TabStop = False
        '
        'PanDownButton
        '
        Me.PanDownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanDownButton.Image = CType(resources.GetObject("PanDownButton.Image"), System.Drawing.Image)
        Me.PanDownButton.Location = New System.Drawing.Point(510, 61)
        Me.PanDownButton.Name = "PanDownButton"
        Me.PanDownButton.Size = New System.Drawing.Size(28, 26)
        Me.PanDownButton.TabIndex = 1
        Me.PanDownButton.TabStop = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(510, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 19)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Pan"
        '
        'RotateDownButton
        '
        Me.RotateDownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RotateDownButton.Image = CType(resources.GetObject("RotateDownButton.Image"), System.Drawing.Image)
        Me.RotateDownButton.Location = New System.Drawing.Point(510, 159)
        Me.RotateDownButton.Name = "RotateDownButton"
        Me.RotateDownButton.Size = New System.Drawing.Size(28, 27)
        Me.RotateDownButton.TabIndex = 1
        Me.RotateDownButton.TabStop = False
        '
        'RotateRightButton
        '
        Me.RotateRightButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RotateRightButton.Image = CType(resources.GetObject("RotateRightButton.Image"), System.Drawing.Image)
        Me.RotateRightButton.Location = New System.Drawing.Point(545, 131)
        Me.RotateRightButton.Name = "RotateRightButton"
        Me.RotateRightButton.Size = New System.Drawing.Size(28, 28)
        Me.RotateRightButton.TabIndex = 1
        Me.RotateRightButton.TabStop = False
        '
        'RotateLeftButton
        '
        Me.RotateLeftButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RotateLeftButton.Image = CType(resources.GetObject("RotateLeftButton.Image"), System.Drawing.Image)
        Me.RotateLeftButton.Location = New System.Drawing.Point(475, 131)
        Me.RotateLeftButton.Name = "RotateLeftButton"
        Me.RotateLeftButton.Size = New System.Drawing.Size(27, 28)
        Me.RotateLeftButton.TabIndex = 1
        Me.RotateLeftButton.TabStop = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(502, 138)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 28)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Rotate"
        '
        'RotateUpButton
        '
        Me.RotateUpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RotateUpButton.Image = CType(resources.GetObject("RotateUpButton.Image"), System.Drawing.Image)
        Me.RotateUpButton.Location = New System.Drawing.Point(510, 103)
        Me.RotateUpButton.Name = "RotateUpButton"
        Me.RotateUpButton.Size = New System.Drawing.Size(28, 28)
        Me.RotateUpButton.TabIndex = 1
        Me.RotateUpButton.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(505, 240)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Zoom"
        '
        'ZoomInButton
        '
        Me.ZoomInButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ZoomInButton.Image = CType(resources.GetObject("ZoomInButton.Image"), System.Drawing.Image)
        Me.ZoomInButton.Location = New System.Drawing.Point(510, 205)
        Me.ZoomInButton.Name = "ZoomInButton"
        Me.ZoomInButton.Size = New System.Drawing.Size(28, 29)
        Me.ZoomInButton.TabIndex = 1
        Me.ZoomInButton.TabStop = False
        '
        'ZoomOutButton
        '
        Me.ZoomOutButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ZoomOutButton.Image = CType(resources.GetObject("ZoomOutButton.Image"), System.Drawing.Image)
        Me.ZoomOutButton.Location = New System.Drawing.Point(510, 261)
        Me.ZoomOutButton.Name = "ZoomOutButton"
        Me.ZoomOutButton.Size = New System.Drawing.Size(28, 29)
        Me.ZoomOutButton.TabIndex = 1
        Me.ZoomOutButton.TabStop = False
        '
        'ViewerWindow
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(584, 326)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RenderPane)
        Me.Controls.Add(Me.PanLeftButton)
        Me.Controls.Add(Me.PanUpButton)
        Me.Controls.Add(Me.PanRightButton)
        Me.Controls.Add(Me.PanDownButton)
        Me.Controls.Add(Me.RotateDownButton)
        Me.Controls.Add(Me.RotateRightButton)
        Me.Controls.Add(Me.RotateLeftButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RotateUpButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ZoomInButton)
        Me.Controls.Add(Me.ZoomOutButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ViewerWindow"
        Me.ShowInTaskbar = False
        Me.Text = "Revit Viewer"
        CType(Me.RenderPane, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    ''' <summary>
    ''' return the width to be fit.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
  Public Function ScaleWidth() As Double
    Return Me.Width * 0.8
  End Function

    ''' <summary>
    ''' return the height to be fit.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
  Public Function ScaleHeight() As Double
    Return Me.Height * 0.8
  End Function

    'the Wireframe object stores all primitive lines.
  Dim mViewerObject As Wireframe

    ''' <summary>
    ''' draw primitive line using the System.Drawing.Graphics object
    ''' </summary>
    ''' <param name="gs"></param>
    ''' <param name="x1"></param>
    ''' <param name="y1"></param>
    ''' <param name="x2"></param>
    ''' <param name="y2"></param>
    ''' <remarks></remarks>
  Public Sub DrawLine(ByRef gs As System.Drawing.Graphics, ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double)

    Dim startX As Single
    Dim startY As Single
    Dim endX As Single
    Dim endY As Single

    startX = x1 + (Me.ScaleWidth / 2)
    endX = x2 + (Me.ScaleWidth / 2)

    startY = -y1 + (Me.ScaleHeight / 2)
    endY = -y2 + (Me.ScaleHeight / 2)

    ' Create a Pen object.
    Dim pen As New Drawing.Pen(System.Drawing.Color.White, 1)
    gs.DrawLine(pen, startX, startY, endX, endY)

  End Sub

    ''' <summary>
    ''' clear the graphics' drawing.
    ''' </summary>
    ''' <param name="gs"></param>
    ''' <remarks></remarks>
  Public Sub Clear(ByRef gs As System.Drawing.Graphics)

    gs.Clear(RenderPane.BackColor)

  End Sub

    ''' <summary>
    ''' Occurs when a key is pressed while the control has focus.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ViewerWindow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

    RenderPane_KeyDown(sender, e)

  End Sub

    ''' <summary>
    ''' To handle keyboard events
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RenderPane_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles RenderPane.KeyDown

    mViewerObject.KeyDown(e.KeyCode, e.Shift, e.Control)

  End Sub

    ''' <summary>
    ''' Initialize wire frame to be display.
    ''' </summary>
    ''' <param name="viewerObject"></param>
    ''' <remarks></remarks>
    Public Sub Initialize(ByVal viewerObject As Wireframe)

        mViewerObject = viewerObject

    End Sub

    ''' <summary>
    ''' Occurs when the mouse wheel moves while the control has focus.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ViewerWindow_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel

    If (e.Delta > 0) Then
      mViewerObject.Zoom(1 / 1.2)
    Else
      mViewerObject.Zoom(1.2)
    End If

  End Sub

    ''' <summary>
    ''' translate model up.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub PanUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanUpButton.Click

    mViewerObject.KeyDown(38, False, False)

  End Sub

    ''' <summary>
    ''' translate model right.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub PanRightButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanRightButton.Click

    mViewerObject.KeyDown(39, False, False)

  End Sub

    ''' <summary>
    ''' translate model down.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub PanDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanDownButton.Click

    mViewerObject.KeyDown(40, False, False)

  End Sub

    ''' <summary>
    ''' translate model left.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub PanLeftButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanLeftButton.Click

    mViewerObject.KeyDown(37, False, False)

  End Sub

    ''' <summary>
    ''' rotate around X axis with default angle.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RotateUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateUpButton.Click

    mViewerObject.KeyDown(38, False, True)

  End Sub

    ''' <summary>
    ''' rotate around Y axis with default angle.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RotateRightButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateRightButton.Click

    mViewerObject.KeyDown(39, False, True)

  End Sub

    ''' <summary>
    ''' rotate around X axis with default angle.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RotateDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateDownButton.Click

    mViewerObject.KeyDown(40, False, True)

  End Sub

    ''' <summary>
    ''' rotate around Y axis with default angle.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RotateLeftButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateLeftButton.Click

    mViewerObject.KeyDown(37, False, True)

  End Sub

    ''' <summary>
    ''' zoom in model to display.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ZoomInButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomInButton.Click

    mViewerObject.KeyDown(38, True, False)

  End Sub

    ''' <summary>
    ''' zoom out model to display.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ZoomOutButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomOutButton.Click

    mViewerObject.KeyDown(40, True, False)

  End Sub

    ''' <summary>
    ''' Occurs when the Size property value changes. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ViewerWindow_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged

    If Not mViewerObject Is Nothing Then
      ' schedule a rescale in window next paint
      mViewerObject.ReFitRequired = True
    End If

    End Sub

    ''' <summary>
    ''' Occurs before a form is displayed for the first time. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub ViewerWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    If Not mViewerObject Is Nothing Then
      ' schedule a rescale in window next paint
      mViewerObject.ReFitRequired = True
    End If

  End Sub

    ''' <summary>
    ''' Occurs when the control is double-clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RenderPane_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles RenderPane.DoubleClick

    If Not mViewerObject Is Nothing Then
      ' schedule a rescale in window next paint
      mViewerObject.ReFitRequired = True
    End If

  End Sub

    ''' <summary>
    ''' Occurs when the control is redrawn.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
  Private Sub RenderPane_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles RenderPane.Paint

    If Not mViewerObject Is Nothing Then
      ' if the object needs to be fitted into the control
      If mViewerObject.ReFitRequired = True Then
        ' calculate the scales
        mViewerObject.Fit()
        ' reset the flag
        mViewerObject.ReFitRequired = False
      End If

      mViewerObject.RawDraw(e.Graphics)
    End If

  End Sub
End Class