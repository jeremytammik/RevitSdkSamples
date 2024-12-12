//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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

using System.Windows.Media.Imaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.IO.MemoryMappedFiles;


namespace Revit.SDK.Samples.DockableDialogs.CS
{
   /// <summary>
   /// A simple utility class to route calls from Console.WriteLine and other standard IO to 
   /// a TextBox.  Note that one side effect of this system is that any time a host application calls
   /// Console.WriteLine, cout, printf, or something similar, the output will be funneled through here,
   /// giving occasional output you may not have expected.
   /// </summary>
   public class StandardIORouter : TextWriter
   {

      /// <summary>
      /// Create a new router given a WPF Textbox to output to.
      /// </summary>
      public StandardIORouter(TextBox output)
      {
         m_outputTextBox = output;
      }
      
      /// <summary>
      /// Write a character from standardIO to a Textbox.
      /// </summary>
      public override void Write(char oneCharacter)
      {
         m_outputTextBox.AppendText(oneCharacter.ToString());
         m_outputTextBox.ScrollToEnd();
      }

      /// <summary>
      /// A default override to use UTF8 text
      /// </summary>
      public override Encoding Encoding
      {
         get { return System.Text.Encoding.UTF8; }
      }
      
      /// <summary>
      /// A stored reference of a textbox to output to.
      /// </summary>
      private TextBox m_outputTextBox = null;
   }
}
