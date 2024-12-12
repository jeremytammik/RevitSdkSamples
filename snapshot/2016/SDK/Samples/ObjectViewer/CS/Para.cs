//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Data;
using System.Runtime.Serialization;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// a class used to define a parameter of Element
    /// </summary>
    public class Para
    {
        private Parameter m_parameter;   //one parameter of a element

        /// <summary>
        /// parameter's name
        /// </summary>
        public string ParaName
        {
            get
            {
                try
                {
                    return m_parameter.Definition.Name;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// parameter's value
        /// </summary>
        //public Object Value // jeremy
        public string Value // jeremy
        {
            get
            {
                try
                {
                    return GetParameterValue(m_parameter);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    SetParameterValue(m_parameter,value);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// get parameter's value
        /// </summary>
        /// <param name="parameter">parameter of Element</param>
        /// <returns>parameter's value include unit if have</returns>
        public static string GetParameterValue(Parameter parameter) // jeremy
        //public static Object GetParameterValue(Parameter parameter) // jeremy
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    //get value with unit, AsDouble() can get value without unit
                    return parameter.AsValueString();
                case StorageType.ElementId:
                    return parameter.AsElementId().IntegerValue.ToString();
                case StorageType.Integer:
                    //get value with unit, AsInteger() can get value without unit
                    return parameter.AsValueString();
                case StorageType.None:
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return "";
            }
        }

        /// <summary>
        /// set parameter's value
        /// </summary>
        /// <param name="parameter">parameter of a Material</param>
        /// <param name="value">
        /// value will be set to parameter
        /// </param>
        public static void SetParameterValue(Parameter parameter, Object value)
        {
            //first,check whether this parameter is read only
            if(parameter.IsReadOnly)
            {
                return;
            }

            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    //set value with unit, Set() can set value without unit
                    parameter.SetValueString(value as string);
                    break;
                case StorageType.ElementId:
                    Autodesk.Revit.DB.ElementId elementId = (Autodesk.Revit.DB.ElementId)(value);
                    parameter.Set(elementId);
                    break;
                case StorageType.Integer:
                    //set value with unit, Set() can set value without unit
                    parameter.SetValueString(value as string);
                    break;
                case StorageType.None:
                    parameter.SetValueString(value as string);
                    break;
                case StorageType.String:
                    parameter.Set(value as string);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// constructor of Para
        /// </summary>
        /// <param name="parameter"></param>
        public Para(Parameter parameter)
        {
            m_parameter = parameter;
        }
    }
}
