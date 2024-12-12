//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.TagBeam.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            try
            {
                //prepare data
                TagBeamData dataBuffer = new TagBeamData(commandData);

                // show UI
                using (TagBeamForm displayForm = new TagBeamForm(dataBuffer))
                {
                    DialogResult result = displayForm.ShowDialog();
                    if (DialogResult.OK != result)
                    {
                        return IExternalCommand.Result.Cancelled;
                    }
                }

                return IExternalCommand.Result.Succeeded;
            }
            catch(Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }            
        }
        #endregion IExternalCommand Members Implementation
    }

  public class TagRebar : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        // Get the active document and view
        Document revitDoc = commandData.Application.ActiveDocument;
        Autodesk.Revit.Elements.View view = revitDoc.ActiveView;
        foreach( Element elem in revitDoc.Selection.Elements )
        {
          if( elem.GetType() == typeof( Autodesk.Revit.Elements.Rebar ) )
          {
            // cast to Rebar and get its first curve
            Autodesk.Revit.Elements.Rebar rebar = (Autodesk.Revit.Elements.Rebar) elem;
            Autodesk.Revit.Geometry.Curve curve = rebar.Curves.get_Item( 0 );

            // create a rebar tag at the first end point of the first curve
            IndependentTag tag = revitDoc.Create.NewTag( view, rebar, true,
                Autodesk.Revit.Enums.TagMode.TM_ADDBY_CATEGORY,
                Autodesk.Revit.Enums.TagOrientation.TAG_HORIZONTAL, curve.get_EndPoint( 0 ) );
            return IExternalCommand.Result.Succeeded;
          }
        }
        message = "No rebar selected!";
        return IExternalCommand.Result.Failed;
      }
      catch( Exception e )
      {
        message = e.Message;
        return IExternalCommand.Result.Failed;
      }
    }
  }

  public class CreateText : IExternalCommand
  {
    public IExternalCommand.Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      try
      {
        // get the active document and view
        Document revitDoc = commandData.Application.ActiveDocument;
        Autodesk.Revit.Elements.View view = revitDoc.ActiveView;
        foreach( Element elem in revitDoc.Selection.Elements )
        {
          if( elem.GetType() == typeof( Autodesk.Revit.Elements.Rebar ) )
          {
            // cast to Rebar and get its first curve
            Autodesk.Revit.Elements.Rebar rebar = (Autodesk.Revit.Elements.Rebar) elem;
            Autodesk.Revit.Geometry.Curve curve = rebar.Curves.get_Item( 0 );

            // calculate necessary arguments
            Autodesk.Revit.Geometry.XYZ origin = curve.get_EndPoint( 0 );
            origin.X += curve.Length; // draw the text at the right size
            Autodesk.Revit.Geometry.XYZ baseVec = new Autodesk.Revit.Geometry.XYZ( 1, 0, 0 );
            Autodesk.Revit.Geometry.XYZ upVec = new Autodesk.Revit.Geometry.XYZ( 0, 0, 1 );
            double lineWidth = curve.Length / 50;
            string strText = "This is " + rebar.Category.Name + " : " + rebar.Name;

            // create the text
            Autodesk.Revit.Elements.TextNote text = revitDoc.Create.NewTextNote( view, origin, baseVec, upVec, lineWidth,
                Autodesk.Revit.Enums.TextAlignFlags.TEF_ALIGN_LEFT, strText );
            text.Width = curve.Length ; // set the text width
            return IExternalCommand.Result.Succeeded;
          }
        }
        message = "No rebar selected!";
        return IExternalCommand.Result.Failed;
      }
      catch( Exception e )
      {
        message = e.Message;
        return IExternalCommand.Result.Failed;
      }
    }
  }
}
