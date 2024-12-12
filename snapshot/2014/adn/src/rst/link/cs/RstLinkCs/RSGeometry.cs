#region Header
// RstLink
//
// Copyright (C) 2007-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

using System;
using System.Data;

namespace RstLink
{
  [Serializable()]
  public class RSPoint
  {
    private double _x;
    private double _y;
    private double _z;

    public RSPoint(double x, double y, double z)
    {
      _x = x;
      _y = y;
      _z = z;
    }

    public double X
    {
      get
      {
        return _x;
      }
      set
      {
        _x = value;
      }
    }

    public double Y
    {
      get
      {
        return _y;
      }
      set
      {
        _y = value;
      }
    }

    public double Z
    {
      get
      {
        return _z;
      }
      set
      {
        _z = value;
      }
    }
  }

  [Serializable()]
  public class RSLine
  {
    [CLSCompliant( false )]
    public RSPoint _StartPt;
    public RSPoint _EndPt;

    public RSLine( RSPoint startPt, RSPoint endPt )
    {
      _StartPt = startPt;
      _EndPt = endPt;
    }
  }
}
