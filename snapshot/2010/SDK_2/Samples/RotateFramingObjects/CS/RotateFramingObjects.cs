//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.IO;
using System.Windows.Forms;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.RotateFramingObjects.CS
{

    /// <summary>
    /// Rotate the objects that were selected when the command was executed.
    /// and allow the user input the amount, in degrees that the objects should be rotated. 
    /// the dialog contain option for the user to specify this value is absolute or relative. 
    /// </summary>
    public class RotateFramingObjects : IExternalCommand
    {
        Autodesk.Revit.Application m_revit = null;    // application of Revit
        double m_receiveRotationTextBox;            // receive change of Angle     
        bool m_isAbsoluteChecked;                    // true if moving absolute
        const string AngleDefinitionName = "Cross-Section Rotation";

        /// <summary>
        /// receive change of Angle    
        /// </summary>
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

        /// <summary>
        /// is moving absolutely
        /// </summary>
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
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            Autodesk.Revit.Application revit = commandData.Application;

            m_revit = revit;
            RotateFramingObjectsForm displayForm = new RotateFramingObjectsForm(this);
            displayForm.StartPosition = FormStartPosition.CenterParent;
            ElementSet selection = revit.ActiveDocument.Selection.Elements;
            bool isSingle = true;                   //selection is single object
            bool isAllFamilyInstance = true;  //all is not familyInstance

            // There must be beams, braces or columns selected
            if (selection.IsEmpty)
            {
                // nothing selected
                message = "Please select some beams, braces or columns.";
                return IExternalCommand.Result.Failed;
            }
            else if (1 != selection.Size)
            {
                isSingle = false;
                try
                {
                    if (DialogResult.OK != displayForm.ShowDialog())
                    {
                        return IExternalCommand.Result.Cancelled;
                    }
                }
                catch (Exception)
                {
                    return IExternalCommand.Result.Failed;
                }
                //    return IExternalCommand.Result.Succeeded;
                // more than one object selected            
            }

            // if the selected elements are familyInstances, try to get their existing rotation
            foreach (Autodesk.Revit.Element e in selection)
            {
                FamilyInstance familyComponent = e as FamilyInstance;
                if (familyComponent != null)
                {
                    if (StructuralType.Beam == familyComponent.StructuralType
                 || StructuralType.Brace == familyComponent.StructuralType)
                    {
                        // selection is a beam or brace
                        string returnValue = this.FindParameter(AngleDefinitionName, familyComponent);
                        displayForm.rotationTextBox.Text = returnValue.ToString();
                    }
                    else if (StructuralType.Column == familyComponent.StructuralType)
                    {
                        // selection is a column
                        Location columnLocation = familyComponent.Location;
                        LocationPoint pointLocation = columnLocation as LocationPoint;
                        double temp = pointLocation.Rotation;
                        string output = (Math.Round(temp * 180 / (Math.PI), 3)).ToString();
                        displayForm.rotationTextBox.Text = output;
                    }
                    else
                    {
                        // other familyInstance can not be rotated
                        message = "It is not a beam, brace or column.";
                        elements.Insert(familyComponent);
                        return IExternalCommand.Result.Failed;
                    }
                }
                else
                {
                    if (isSingle)
                    {
                        message = "It is not a FamilyInstance.";
                        elements.Insert(e);
                        return IExternalCommand.Result.Failed;
                    }
                    // there is some objects is not familyInstance
                    message = "They are not FamilyInstances";
                    elements.Insert(e);
                    isAllFamilyInstance = false;
                }
            }

            if (isSingle)
            {
                try
                {
                    if (DialogResult.OK != displayForm.ShowDialog())
                    {
                        return IExternalCommand.Result.Cancelled;
                    }
                }
                catch (Exception)
                {
                    return IExternalCommand.Result.Failed;
                }
            }

            if (isAllFamilyInstance)
            {
                return IExternalCommand.Result.Succeeded;
            }
            else
            {
                //output error information
                return IExternalCommand.Result.Failed;
            }
        }
        /// <summary>
        /// The function set value to rotation of the beams and braces
        /// and rotate columns. 
        /// </summary>        
        public void RotateElement()
        {
            ElementSet selection = m_revit.ActiveDocument.Selection.Elements;
            foreach (Autodesk.Revit.Element e in selection)
            {
                FamilyInstance familyComponent = e as FamilyInstance;
                if (familyComponent == null)
                {
                    //is not a familyInstance
                    continue;
                }
                // if be familyInstance,judge the types of familyInstance
                if (StructuralType.Beam == familyComponent.StructuralType
                   || StructuralType.Brace == familyComponent.StructuralType)
                {
                    // selection is a beam or Brace
                    ParameterSetIterator paraIterator = familyComponent.Parameters.ForwardIterator();
                    paraIterator.Reset();

                    while (paraIterator.MoveNext())
                    {
                        object para = paraIterator.Current;
                        Parameter objectAttribute = para as Parameter;
                        //set generic property named "Cross-Section Rotation"                           
                        if (objectAttribute.Definition.Name.Equals(AngleDefinitionName))
                        {
                            Double originDegree = objectAttribute.AsDouble();
                            double rotateDegree = m_receiveRotationTextBox * Math.PI / 180;
                            if (!m_isAbsoluteChecked)
                            {
                                // absolute rotation
                                rotateDegree += originDegree;
                            }
                            objectAttribute.Set(rotateDegree);
                            // relative rotation
                        }
                    }
                }
                else if (StructuralType.Column == familyComponent.StructuralType)
                {
                    // rotate a column
                    Autodesk.Revit.Location columnLocation = familyComponent.Location;
                    // get the location object
                    LocationPoint pointLocation = columnLocation as LocationPoint;
                    XYZ insertPoint = pointLocation.Point;
                    // get the location point
                    double temp = pointLocation.Rotation;
                    //existing rotation
                    XYZ directionPoint = new XYZ(0, 0, 1);
                    // define the vector of axis
                    Line rotateAxis =
                        m_revit.Create.NewLineUnbound(insertPoint, directionPoint);
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

        /// <summary>
        /// get the parameter value according given parameter name
        /// </summary>
        public string FindParameter(string parameterName, FamilyInstance familyInstanceName)
        {
            ParameterSetIterator i = familyInstanceName.Parameters.ForwardIterator();
            i.Reset();
            string valueOfParameter = null;
            bool iMoreAttribute = i.MoveNext();
            while (iMoreAttribute)
            {
                bool isFound = false;
                object o = i.Current;
                Parameter familyAttribute = o as Parameter;
                if (familyAttribute.Definition.Name == parameterName)
                {
                    //find the parameter whose name is same to the given parameter name 
                    Autodesk.Revit.Parameters.StorageType st = familyAttribute.StorageType;
                    switch (st)
                    {
                        //get the storage type
                        case StorageType.Double:
                            if (parameterName.Equals(AngleDefinitionName))
                            {
                                //make conversion between degrees and radians
                                Double temp = familyAttribute.AsDouble();
                                valueOfParameter = Math.Round(temp * 180 / (Math.PI), 3).ToString();
                            }
                            else
                            {
                                valueOfParameter = familyAttribute.AsDouble().ToString();
                            }
                            break;
                        case StorageType.ElementId:
                            //get elementId as string 
                            valueOfParameter = familyAttribute.AsElementId().Value.ToString();
                            break;
                        case StorageType.Integer:
                            //get Integer as string
                            valueOfParameter = familyAttribute.AsInteger().ToString();
                            break;
                        case StorageType.String:
                            //get string 
                            valueOfParameter = familyAttribute.AsString();
                            break;
                        case StorageType.None:
                            valueOfParameter = familyAttribute.AsValueString();
                            break;
                        default:
                            break;
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
            return valueOfParameter;
        }
    }
}