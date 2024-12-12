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
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace RstLink
{
  /// <summary>
  /// Class for structural member (column or framing categories).
  /// Only some example properties mapped and only straight members assumed!
  /// </summary>
  [Serializable()]
  public class RSMember : RsLinkElement
  {
    [CLSCompliant(false)]
    public string _usage;

    [CLSCompliant(false)]
    public string _type;
    
    [CLSCompliant(false)]
    public RSLine _geom;

    public RSMember( int id, string usage, string type, RSLine geom ) : base( id )
    {
      _usage = usage;
      _type = type;
      _geom = geom;
    }
  }
}
