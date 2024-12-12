' 
' (C) Copyright 2003-2019 by Autodesk, Inc.
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

Imports System.Math

''' <summary>
''' Wrapped wire frame of element to be drawn.
''' </summary>
''' <remarks></remarks>
Public Class Wireframe

  Dim mLines() As Line
  Dim mWindow As New ViewerWindow
  Dim xOrigin As Double
  Dim yOrigin As Double
  Dim zOrigin As Double
  Const PI As Double = 3.14159265358979
  Dim mScale As Double

  ' constructor
  Public Sub New()

    mScale = 1
        mWindow.Initialize(Me)

  End Sub

    ''' <summary>
    ''' update the bound to be display.
    ''' </summary>
    ''' <param name="xMin"></param>
    ''' <param name="xMax"></param>
    ''' <param name="yMin"></param>
    ''' <param name="yMax"></param>
    ''' <param name="zMin"></param>
    ''' <param name="zMax"></param>
    ''' <remarks></remarks>
  Public Sub FindMinMax(ByRef xMin As Double, ByRef xMax As Double, ByRef yMin As Double, ByRef yMax As Double, ByRef zMin As Double, ByRef zMax As Double)

    xMin = 0
    xMax = 0
    yMin = 0
    yMax = 0
    zMin = 0
    zMax = 0

    Dim valSet As Boolean = False
    Dim i As Integer
    Dim size As Integer = GetTotalNumberLines()
    For i = 0 To size

      Dim aLine As Line
      aLine = mLines(i)

      If valSet = False Then

        xMin = aLine.startPoint.X
        xMax = xMin

        yMin = aLine.startPoint.Y
        yMax = yMin

        zMin = aLine.startPoint.Z
        zMax = zMin

        valSet = True

      End If

      If (aLine.startPoint.X < xMin) Then
        xMin = aLine.startPoint.X
      ElseIf (aLine.startPoint.X > xMax) Then
        xMax = aLine.startPoint.X
      End If

      If (aLine.startPoint.Y < yMin) Then
        yMin = aLine.startPoint.Y
      ElseIf (aLine.startPoint.Y > yMax) Then
        yMax = aLine.startPoint.Y
      End If

      If (aLine.startPoint.Z < zMin) Then
        zMin = aLine.startPoint.Z
      ElseIf (aLine.startPoint.Z > zMax) Then
        zMax = aLine.startPoint.Z
      End If



      If (aLine.endPoint.X < xMin) Then
        xMin = aLine.endPoint.X
      ElseIf (aLine.endPoint.X > xMax) Then
        xMax = aLine.endPoint.X
      End If

      If (aLine.endPoint.Y < yMin) Then
        yMin = aLine.endPoint.Y
      ElseIf (aLine.endPoint.Y > yMax) Then
        yMax = aLine.endPoint.Y
      End If

      If (aLine.endPoint.Z < zMin) Then
        zMin = aLine.endPoint.Z
      ElseIf (aLine.endPoint.Z > zMax) Then
        zMax = aLine.endPoint.Z
      End If

    Next i


  End Sub

    ''' <summary>
    ''' zoom the model to fit to display.
    ''' </summary>
    ''' <remarks></remarks>
  Public Sub Fit()

    ' reset the origins
    xOrigin = 0.0
    yOrigin = 0.0
    zOrigin = 0.0

    Dim xMin As Double
    Dim xMax As Double
    Dim yMin As Double
    Dim yMax As Double
    Dim zMin As Double
    Dim zMax As Double

    FindMinMax(xMin, xMax, yMin, yMax, zMin, zMax)

    Dim xOffset As Double
    Dim yOffset As Double
    Dim zOffset As Double

    xOffset = ((xMin + xMax) / 2)
    yOffset = ((yMin + yMax) / 2)
    zOffset = ((zMin + zMax) / 2)

    Dim i As Integer
    Dim size As Integer = GetTotalNumberLines()
    For i = 0 To size
      mLines(i).startPoint.X = mLines(i).startPoint.X - xOffset
      mLines(i).startPoint.Y = mLines(i).startPoint.Y - yOffset
      mLines(i).startPoint.Z = mLines(i).startPoint.Z - zOffset
      mLines(i).endPoint.X = mLines(i).endPoint.X - xOffset
      mLines(i).endPoint.Y = mLines(i).endPoint.Y - yOffset
      mLines(i).endPoint.Z = mLines(i).endPoint.Z - zOffset
    Next

    FindMinMax(xMin, xMax, yMin, yMax, zMin, zMax)

    Dim halfWidth As Double
    Dim halfHeight As Double

    halfWidth = (mWindow.ScaleWidth / 2) * 0.8
    halfHeight = (mWindow.ScaleHeight / 2) * 0.8

    Dim largestXOffset As Double
    Dim largestYOffset As Double

    largestXOffset = Abs(xMin)
    If Abs(xMax) > largestXOffset Then
      largestXOffset = Abs(xMax)
    End If

    largestYOffset = Abs(yMin)
    If Abs(yMax) > largestYOffset Then
      largestYOffset = Abs(yMax)
    End If

    Dim scaleX As Double
    Dim scaleY As Double

    scaleX = 0
    scaleY = 0

    If (largestXOffset > 0) Then
      scaleX = halfWidth / largestXOffset
    End If

    If (largestYOffset > 0) Then
      scaleY = halfHeight / largestYOffset
    End If

    Dim smallestScale As Double

    smallestScale = scaleX
    If (smallestScale = 0 Or (scaleY <> 0 And scaleY < smallestScale)) Then
      smallestScale = scaleY
    End If

    If smallestScale <> 0 Then
      Zoom(smallestScale)
    End If



  End Sub

    ''' <summary>
    ''' add the primititive line.
    ''' </summary>
    ''' <param name="x1">the the first coordinate of start point</param>
    ''' <param name="y1">the the second coordinate of start point</param>
    ''' <param name="z1">the the third coordinate of start point</param>
    ''' <param name="x2">the the first coordinate of end point</param>
    ''' <param name="y2">the the second coordinate of end point</param>
    ''' <param name="z2">the the third coordinate of end point</param>
    ''' <remarks></remarks>
  Public Sub Add(ByVal x1 As Double, ByVal y1 As Double, ByVal z1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal z2 As Double)

    Dim size As Integer = GetTotalNumberLines()
    size = size + 1
    ReDim Preserve mLines(size)

    Dim newLine As New Line
    newLine.startPoint.X = x1
    newLine.startPoint.Y = y1
    newLine.startPoint.Z = z1

    newLine.endPoint.X = x2
    newLine.endPoint.Y = y2
    newLine.endPoint.Z = z2

    mLines(UBound(mLines)) = newLine

    'Draw

  End Sub

  Sub Draw()

    ' force OnPaint call
    mWindow.Invalidate(True)

  End Sub

  Sub RawDraw(ByRef gs As System.Drawing.Graphics)

    ' clear the control
    mWindow.Clear(gs)
    ' draw the lines
    Dim i As Integer
    Dim size As Integer = GetTotalNumberLines()
    For i = 0 To size
      mWindow.DrawLine(gs, mLines(i).startPoint.X + xOrigin, mLines(i).startPoint.Y + yOrigin, mLines(i).endPoint.X + xOrigin, mLines(i).endPoint.Y + yOrigin)
    Next i

  End Sub

    ''' <summary>
    ''' rotate around Z axis with spcified angle.
    ''' </summary>
    ''' <param name="angle">the angle to rotate.</param>
    ''' <remarks></remarks>
    Public Sub RotateXY(ByVal angle As Double)

        Dim tempX As Double
        Dim tempY As Double
        Dim i As Integer
        Dim size As Integer = GetTotalNumberLines()
        For i = 0 To size
            'start Point
            tempX = mLines(i).startPoint.X
            tempY = mLines(i).startPoint.Y
            mLines(i).startPoint.X = tempX * Cos(angle) - tempY * Sin(angle)
            mLines(i).startPoint.Y = tempX * Sin(angle) + tempY * Cos(angle)

            'end point
            tempX = mLines(i).endPoint.X
            tempY = mLines(i).endPoint.Y
            mLines(i).endPoint.X = tempX * Cos(angle) - tempY * Sin(angle)
            mLines(i).endPoint.Y = tempX * Sin(angle) + tempY * Cos(angle)
        Next

        Draw()

    End Sub

    ''' <summary>
    ''' rotate around X axis with spcified angle.
    ''' </summary>
    ''' <param name="angle">the angle to rotate.</param>
    ''' <remarks></remarks>
    Public Sub RotateYZ(ByVal angle As Double)
        Dim tempY As Double
        Dim tempZ As Double
        Dim i As Integer
        Dim size As Integer = GetTotalNumberLines()
        For i = 0 To size
            'start Point
            tempY = mLines(i).startPoint.Y
            tempZ = mLines(i).startPoint.Z
            mLines(i).startPoint.Y = tempY * Cos(angle) - tempZ * Sin(angle)
            mLines(i).startPoint.Z = tempY * Sin(angle) + tempZ * Cos(angle)

            'end point
            tempY = mLines(i).endPoint.Y
            tempZ = mLines(i).endPoint.Z
            mLines(i).endPoint.Y = tempY * Cos(angle) - tempZ * Sin(angle)
            mLines(i).endPoint.Z = tempY * Sin(angle) + tempZ * Cos(angle)
        Next

        Draw()

    End Sub

    ''' <summary>
    ''' rotate around Y axis with spcified angle.
    ''' </summary>
    ''' <param name="angle">the angle to rotate.</param>
    ''' <remarks></remarks>
    Public Sub RotateXZ(ByVal angle As Double)

        Dim tempX As Double
        Dim tempZ As Double
        Dim i As Integer
        Dim size As Integer = GetTotalNumberLines()
        For i = 0 To size
            'start Point
            tempX = mLines(i).startPoint.X
            tempZ = mLines(i).startPoint.Z
            mLines(i).startPoint.X = tempX * Cos(angle) - tempZ * Sin(angle)
            mLines(i).startPoint.Z = tempX * Sin(angle) + tempZ * Cos(angle)

            'end point
            tempX = mLines(i).endPoint.X
            tempZ = mLines(i).endPoint.Z
            mLines(i).endPoint.X = tempX * Cos(angle) - tempZ * Sin(angle)
            mLines(i).endPoint.Z = tempX * Sin(angle) + tempZ * Cos(angle)
        Next

        Draw()

    End Sub

    ''' <summary>
    ''' translate in X axis with the specified distance.
    ''' </summary>
    ''' <param name="translation">the specified distance.</param>
    ''' <remarks></remarks>
  Public Sub TranslateX(ByVal translation As Double)
    xOrigin = xOrigin + translation

    Draw()

  End Sub

    ''' <summary>
    ''' translate in Y axis with the specified distance.
    ''' </summary>
    ''' <param name="translation">the specified distance.</param>
    ''' <remarks></remarks>
  Public Sub TranslateY(ByVal translation As Double)
    yOrigin = yOrigin + translation

    Draw()
  End Sub

    ''' <summary>
    ''' zoom the model with the specified amount.
    ''' </summary>
    ''' <param name="amount">the specified amount.</param>
    ''' <remarks></remarks>
  Public Sub Zoom(ByVal amount As Double)

    Dim i As Integer
    Dim size As Integer = GetTotalNumberLines()
    For i = 0 To size
      mLines(i).startPoint.X = mLines(i).startPoint.X * amount
      mLines(i).startPoint.Y = mLines(i).startPoint.Y * amount
      mLines(i).startPoint.Z = mLines(i).startPoint.Z * amount
      mLines(i).endPoint.X = mLines(i).endPoint.X * amount
      mLines(i).endPoint.Y = mLines(i).endPoint.Y * amount
      mLines(i).endPoint.Z = mLines(i).endPoint.Z * amount
    Next

    mScale = mScale * amount

    Draw()

  End Sub

    ''' <summary>
    ''' respond the UI operation.
    ''' </summary>
    ''' <param name="keyCode">the pressed key code</param>
    ''' <param name="shiftKey">if the shift key is pressed</param>
    ''' <param name="ctrlKey">if the ctrl key is pressed</param>
    ''' <remarks></remarks>
  Public Sub KeyDown(ByVal keyCode As Integer, ByVal shiftKey As Boolean, ByVal ctrlKey As Boolean)

    Dim offsetX As Double
    Dim offsetY As Double

    Dim useScale As Double

    useScale = mScale
    'If (useScale < 1) Then
    useScale = 1
    'End If

    offsetX = (mWindow.RenderPane.Width / useScale) * 0.1
    offsetY = (mWindow.RenderPane.Height / useScale) * 0.1

    If shiftKey And ctrlKey Then

    ElseIf shiftKey Then

      Select Case keyCode
        Case 37
          TranslateX(-offsetX)
        Case 39
          TranslateX(offsetX)
        Case 40
          Zoom(1 / 1.05)
        Case 38
          Zoom(1.05)
      End Select

    ElseIf ctrlKey Then

      Select Case keyCode
        Case 37
          RotateXZ(-PI / 24)
        Case 39
          RotateXZ(PI / 24)
        Case 40
          RotateYZ(-PI / 24)
        Case 38
          RotateYZ(PI / 24)
      End Select

    Else
      Select Case keyCode
        Case 37
          TranslateX(-offsetX)
        Case 39
          TranslateX(offsetX)
        Case 40
          TranslateY(-offsetY)
        Case 38
          TranslateY(offsetY)
      End Select

    End If

  End Sub

    ''' <summary>
    ''' draw the wire frame in a picture box.
    ''' </summary>
    ''' <remarks></remarks>
  Public Sub ShowModal()

    mWindow.ShowDialog()

  End Sub

    ''' <summary>
    ''' get the total number of lines.
    ''' </summary>
    ''' <returns>the total number of lines.</returns>
    ''' <remarks></remarks>
  Private Function GetTotalNumberLines() As Integer
    ' be safe about the array
    Dim size As Integer
    If mLines Is Nothing Then
      size = -1
    Else
      size = UBound(mLines)
    End If

    Return size

  End Function
  ' flag properties for drawing
  Private drawRequired As Boolean
  Property ReDrawRequired() As Boolean
    Get
      Return drawRequired
    End Get
    Set(ByVal Value As Boolean)
      drawRequired = Value
    End Set
  End Property

    ' flag properties for fit
  Private fitRequired As Boolean
  Property ReFitRequired() As Boolean
    Get
      Return fitRequired
    End Get
    Set(ByVal Value As Boolean)
      fitRequired = Value
    End Set
  End Property

End Class
