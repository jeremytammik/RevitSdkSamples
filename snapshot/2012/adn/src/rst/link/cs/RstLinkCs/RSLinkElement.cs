#region Header
// RstLink
//
// Copyright (C) 2007-2010 by Autodesk, Inc.
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

namespace RstLink
{
  /// <summary>
  /// Abstract base class for all elements, contains ID only.
  /// </summary>
  [Serializable()]
  public abstract class RsLinkElement
  {
    private int _revitId;

    public RsLinkElement()
    {
      _revitId = 0;
    }

    public RsLinkElement( int id )
    {
      _revitId = id;
    }

    public int revitId
    {
      get
      {
        return _revitId;
      }
    }
  }
}
