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
using System.Runtime.Serialization;

namespace RstLink
{
  /// <summary>
  /// fix problem where assembly must reside in same directory as acad.exe or revit.exe
  /// see http://www.codeproject.com/soap/Serialization_Samples.asp
  /// - by jeremy tammik
  /// </summary>
  public sealed class RsLinkBinder : SerializationBinder
  {
    public override Type BindToType( string assemblyName, string typeName )
    {
      return Type.GetType( string.Format( "{0}, {1}", typeName, assemblyName ) );
    }
  }

  /*
  class RsLinkBinder : SerializationBinder
  {
    public override Type BindToType( string assemName, string typeName )
    {
      Type type = null;
      String myName = Assembly.GetExecutingAssembly().FullName;
      // If the serialized class is the first version, replace it with the 
      // second version of the class. Note the preceding period in class name.
      if( typeName == ".ClassV1" && assemName == myName )
      {
        type = Type.GetType( String.Format( "ClassV2, {0}", myName ) );
      }
      else
      {
        type = Type.GetType( String.Format( "{0}, {1}", typeName, assemName ) );
      }
      return type;
    }
  }
  */
}
