//
// (C) Copyright 1994-2005 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

/// <summary>
/// Gets the number of studs and camber sizes for beams.
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using MacroCSharpSamples;
using MacroSamples_RVT;


namespace Revit.SDK.Samples.RotateFramingObjects.CS
{

   /// <summary>
   /// rotate the objects that were selected when the command was executed.
   /// and allow the user input the amount, in degrees that the objects should be rotated. 
   /// the dialog contain option for the user to specify this value is absolute or relative. 
   /// </summary>
   public class RotateFramingObjects
   {
      double m_receiveRotationTextBox;            // receive change of Angle		 
      bool m_isAbsoluteChecked;                  // true if moving absolute
      ThisApplication? m_app; //document data for Macro

      public double ReceiveRotationTextBox
      {
         get
         {
            return m_receiveRotationTextBox;
         }
         set
         {
            m_receiveRotationTextBox = value;
         }
      }
      public bool IsAbsoluteChecked
      {
         get
         {
            return m_isAbsoluteChecked;
         }
         set
         {
            m_isAbsoluteChecked = value;
         }
      }

      /// <summary>
      /// Default constructor of RotateFramingObjects
      /// </summary>
      public RotateFramingObjects()
      { }


      /// <summary>
      /// Ctor with ThisDocument as 
      /// </summary>
      /// <param name="hostApp">ThisDocument handler</param>
      public RotateFramingObjects(ThisApplication hostApp)
      {
         m_app = hostApp;
      }

      /// <summary>
      /// Run this sample
      /// </summary>
      public void Run()
      {
         if (m_app == null)
            return;
         RotateFramingObjectsForm displayForm = new RotateFramingObjectsForm(this);
         ICollection<ElementId> selection = m_app.ActiveUIDocument.Selection.GetElementIds();
         bool isSingle = true;                   //selection is single object
         bool isAllFamilyInstance = true;  //all is not familyInstance

         // There must be beams, braces or columns selected
         if (selection.Count == 0)
         {
            // nothing selected
            MessageBox.Show("Please select FamilyInstance.(such as column)", "RotateFramingObjects");
            return;
         }
         else if (1 != selection.Count)
         {

            isSingle = false;
            try
            {
               if (DialogResult.OK != displayForm.ShowDialog())
               {
                  return;
               }
            }
            catch (Exception)
            {
               return;
            }
            //	return IExternalCommand.Result.Succeeded;
            // more that one object selected			
         }

         // if the selection are familyInstance, try to get their existing rotation
         foreach (Autodesk.Revit.DB.ElementId id in selection)
         {
            FamilyInstance? familyComponent = m_app.ActiveUIDocument.Document.GetElement(id) as FamilyInstance;
            if (familyComponent != null)
            {
               if (Autodesk.Revit.DB.Structure.StructuralType.Beam == familyComponent.StructuralType
                  || Autodesk.Revit.DB.Structure.StructuralType.Brace == familyComponent.StructuralType)
               {
                  // selection is a beam or brace
                  string returnValue = this.FindParameter("Angle", familyComponent);
                  if (displayForm.rotationTextBox != null)
                     displayForm.rotationTextBox.Text = returnValue.ToString();

               }
               else if (Autodesk.Revit.DB.Structure.StructuralType.Column == familyComponent.StructuralType)
               {
                  // selection is a column
                  Autodesk.Revit.DB.Location columnLocation = familyComponent.Location;
                  Autodesk.Revit.DB.LocationPoint? pointLocation = columnLocation as Autodesk.Revit.DB.LocationPoint;
                  if (pointLocation != null)
                  {
                     double temp = pointLocation.Rotation;
                     string output = (Math.Round(temp * 180 / (Math.PI), 3)).ToString();
                     if (displayForm.rotationTextBox != null)
                        displayForm.rotationTextBox.Text = output;
                  }

               }
               else
               {
                  // other familyInstance can not be rotated
                  MessageBox.Show("Can not deal with it.", "RotateFramingObjects");
                  m_app.ActiveUIDocument.Selection.GetElementIds().Add(familyComponent.Id);
                  return;
               }
            }
            else
            {
               if (isSingle)
               {
                  MessageBox.Show("It is a Non-FamilyInstance.", "RotateFramingObjects");
                  m_app.ActiveUIDocument.Selection.GetElementIds().Add(id);
                  return;
               }
               // there is some objects is not familyInstance
               //MessageBox.Show("There is Non-FamilyInstance.", "RotateFramingObjects");
               m_app.ActiveUIDocument.Selection.GetElementIds().Add(id);
               isAllFamilyInstance = false;
            }
         }


         if (isSingle)
         {
            try
            {
               if (DialogResult.OK != displayForm.ShowDialog())
               {
                  return;
               }
            }
            catch (Exception)
            {
               return;
            }
         }

         if (isAllFamilyInstance)
         {
            return;
         }
         else
         {
            //output error information
            return;
         }

      }
      /// <summary>
      /// The function set value to rotation of the beams and braces
      /// and rotate columns. 
      /// </summary>		
      public void RotateElement()
      {
         if (m_app == null)
            return;
         ICollection<ElementId> selection = m_app.ActiveUIDocument.Selection.GetElementIds();
         foreach (ElementId id in selection)
         {
            FamilyInstance? familyComponent = m_app.ActiveUIDocument.Document.GetElement(id) as FamilyInstance;
            if (familyComponent == null)
            {
               //is not a familyInstance
               continue;
            }
            // if be familyInstance,judge the types of familyInstance
            if (Autodesk.Revit.DB.Structure.StructuralType.Beam == familyComponent.StructuralType
               || Autodesk.Revit.DB.Structure.StructuralType.Brace == familyComponent.StructuralType)
            {
               // selection is a beam or Brace
               ParameterSetIterator j = familyComponent.Parameters.ForwardIterator();
               j.Reset();

               bool jMoreAttribute = j.MoveNext();
               while (jMoreAttribute)
               {
                  object a = j.Current;
                  Parameter? objectAttribute = a as Parameter;
                  //set generic property named ��Angle��
                  if (objectAttribute != null)
                  {
                     int p = objectAttribute.Definition.Name.CompareTo("Angle");
                     if (0 == p)
                     {
                        Double temp = objectAttribute.AsDouble();
                        double rotateDegree = m_receiveRotationTextBox * Math.PI / 180;
                        if (!m_isAbsoluteChecked)
                        {
                           // absolute rotation
                           rotateDegree += temp;
                        }
                        objectAttribute.Set(rotateDegree);
                        // relative rotation
                     }
                  }
                  jMoreAttribute = j.MoveNext();
               }
            }
            else if (Autodesk.Revit.DB.Structure.StructuralType.Column == familyComponent.StructuralType)
            {
               // rotate a column
               Autodesk.Revit.DB.Location columnLocation = familyComponent.Location;
               // get the location object
               Autodesk.Revit.DB.LocationPoint? pointLocation = columnLocation as Autodesk.Revit.DB.LocationPoint;
               Autodesk.Revit.DB.XYZ? insertPoint = pointLocation?.Point;
               double temp = 0;
               // get the location point
               if (pointLocation != null)
               {
                  temp = pointLocation.Rotation;
                  //existing rotation
                  XYZ directionPoint = m_app.ActiveUIDocument.Document.Application.Create.NewXYZ(0, 0, 1);
                  // define the vector of axis
                  Autodesk.Revit.DB.Line rotateAxis = Line.CreateUnbound(insertPoint, directionPoint);
                  double rotateDegree = m_receiveRotationTextBox * Math.PI / 180;
                  // rotate column by rotate method
                  if (m_isAbsoluteChecked)
                  {
                     rotateDegree -= temp;
                  }
                  bool rotateResult = pointLocation.Rotate(rotateAxis, rotateDegree);
                  if (rotateResult == false)
                  {
                     MessageBox.Show("Rotate Failed.");
                  }
               }
            }
         }
      }

      /// <summary>
      /// get the parameter value according given parameter name
      /// </summary>
      public string FindParameter(string parameterName, FamilyInstance familyInstanceName)
      {
         ParameterSetIterator i = familyInstanceName.Parameters.ForwardIterator();
         i.Reset();
         string? valueOfParameter = null;
         bool iMoreAttribute = i.MoveNext();
         while (iMoreAttribute)
         {
            bool isFound = false;
            object o = i.Current;
            Parameter? familyAttribute = o as Parameter;
            if (familyAttribute != null)
            {
               if (familyAttribute.Definition.Name == parameterName)
               {
                  //find the parameter whose name is same to the given parameter name 
                  Autodesk.Revit.DB.StorageType st = familyAttribute.StorageType;
                  switch (st)
                  {
                     //get the storage type
                     case (Autodesk.Revit.DB.StorageType.Double):
                        {
                           if (parameterName == "Angle")
                           {
                              //make conversion between degrees and radians
                              Double temp = familyAttribute.AsDouble();
                              valueOfParameter = Math.Round(temp * 180 / (Math.PI), 3).ToString();//+ "'";
                           }
                           else
                           {
                              valueOfParameter = familyAttribute.AsDouble().ToString();
                           }
                           break;
                        }
                     case (Autodesk.Revit.DB.StorageType.ElementId):
                        {
                           //get elementId as string 
                           valueOfParameter = familyAttribute.AsElementId().ToString();
                           break;
                        }
                     case (Autodesk.Revit.DB.StorageType.Integer):
                        {
                           //get Integer as string
                           valueOfParameter = familyAttribute.AsInteger().ToString();
                           break;
                        }
                     case (Autodesk.Revit.DB.StorageType.String):
                        {
                           //get string 
                           valueOfParameter = familyAttribute.AsString();
                           break;
                        }
                     default:
                        {
                           break;
                        }
                  }
                  isFound = true;
               }
               if (isFound)
               {
                  break;
               }
               iMoreAttribute = i.MoveNext();
            }
            //return the value.
         }
         if (valueOfParameter == null)
            return string.Empty;
         return valueOfParameter;
      }
   }
}