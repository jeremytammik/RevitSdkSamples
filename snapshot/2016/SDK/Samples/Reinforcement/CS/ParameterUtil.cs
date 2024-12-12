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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.Reinforcement.CS
{
   /// <summary>
   /// contain utility methods find or set certain parameter
   /// </summary>
   public class ParameterUtil
   {
      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
       /// <param name="element">the host object of the parameter</param>
      /// <param name="parameterName">parameter name</param>
      /// <param name="value">the value of the parameter with integer type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, string parameterName, int value)
      {
          ParameterSet parameters = element.Parameters;//a set containing all of the parameters 
         //find a parameter according to the parameter's name
         Parameter findParameter = FindParameter(parameters, parameterName);

         if (null == findParameter)
         {
            return false;
         }

         //judge whether the parameter is readonly before change its value
         if (!findParameter.IsReadOnly)
         {
            //judge whether the type of the value is the same as the parameter's
            StorageType parameterType = findParameter.StorageType;
            if (StorageType.Integer != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            findParameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="parameterName">parameter name</param>
      /// <param name="value">the value of the parameter with double type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, string parameterName, double value)
      {
         ParameterSet parameters = element.Parameters;
         Parameter findParameter = FindParameter(parameters, parameterName);

         if (null == findParameter)
         {
            return false;
         }

         if (!findParameter.IsReadOnly)
         {
            StorageType parameterType = findParameter.StorageType;
            if (StorageType.Double != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            findParameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="parameterName">parameter name</param>
      /// <param name="value">the value of the parameter with string type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, string parameterName, string value)
      {
         ParameterSet parameters = element.Parameters;
         Parameter findParameter = FindParameter(parameters, parameterName);

         if (null == findParameter)
         {
            return false;
         }

         if (!findParameter.IsReadOnly)
         {
            StorageType parameterType = findParameter.StorageType;
            if (StorageType.String != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            findParameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="parameterName">parameter name</param>
      /// <param name="value">the value of the parameter with Autodesk.Revit.DB.ElementId type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, string parameterName, ref Autodesk.Revit.DB.ElementId value)
      {
         ParameterSet parameters = element.Parameters;
         Parameter findParameter = FindParameter(parameters, parameterName);

         if (null == findParameter)
         {
            return false;
         }

         if (!findParameter.IsReadOnly)
         {
            StorageType parameterType = findParameter.StorageType;
            if (StorageType.ElementId != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            findParameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// set certain parameter of given element to int value
      /// </summary>
      /// <param name="element">given element</param>
      /// <param name="paraIndex">BuiltInParameter</param>
      /// <param name="value">the value of the parameter with integer type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, BuiltInParameter paraIndex, int value)
      {
         //find a parameter according to the builtInParameter name
         Parameter parameter = element.get_Parameter(paraIndex);
         if (null == parameter)
         {
            return false;
         }

         if (!parameter.IsReadOnly)
         {
            StorageType parameterType = parameter.StorageType;
            if (StorageType.Integer != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            parameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="paraIndex">parameter index</param>
      /// <param name="value">the value of the parameter with double type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, BuiltInParameter paraIndex, double value)
      {
         Parameter parameter = element.get_Parameter(paraIndex);
         if (null == parameter)
         {
            return false;
         }

         if (!parameter.IsReadOnly)
         {
            StorageType parameterType = parameter.StorageType;
            if (StorageType.Double != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            parameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="paraIndex">parameter index</param>
      /// <param name="value">the value of the parameter with string type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, BuiltInParameter paraIndex, string value)
      {
         Parameter parameter = element.get_Parameter(paraIndex);
         if (null == parameter)
         {
            return false;
         }

         if (!parameter.IsReadOnly)
         {
            StorageType parameterType = parameter.StorageType;
            if (StorageType.String != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            parameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="element">the host object of the parameter</param>
      /// <param name="paraIndex">parameter index</param>
      /// <param name="value">the value of the parameter with Autodesk.Revit.DB.ElementId type</param>
      /// <returns>if find the parameter return true</returns>
      public static bool SetParameter(Element element, 
          BuiltInParameter paraIndex, ref Autodesk.Revit.DB.ElementId value)
      {
         Parameter parameter = element.get_Parameter(paraIndex);
         if (null == parameter)
         {
            return false;
         }

         if (!parameter.IsReadOnly)
         {
            StorageType parameterType = parameter.StorageType;
            if (StorageType.ElementId != parameterType)
            {
               throw new Exception("The types of value and parameter are different!");
            }
            parameter.Set(value);
            return true;
         }

         return false;
      }

      /// <summary>
      /// set null id to a parameter
      /// </summary>
      /// <param name="parameter">the parameter which wanted to change the value</param>
      /// <returns>if set parameter's value successful return true</returns>
      public static bool SetParaNullId(Parameter parameter)
      {
          Autodesk.Revit.DB.ElementId id = new ElementId(-1);

         if (!parameter.IsReadOnly)
         {
            parameter.Set(id);
            return true;
         }
         return false;
      }


      /// <summary>
      /// find a parameter according to the parameter's name
      /// </summary>
      /// <param name="parameters">parameter set</param>
      /// <param name="name">parameter name</param>
      /// <returns>found parameter</returns>
      public static Parameter FindParameter(ParameterSet parameters, string name)
      {
         Parameter findParameter = null;

         foreach (Parameter parameter in parameters)
         {
            if (parameter.Definition.Name == name)
            {
               findParameter = parameter;
            }
         }

         return findParameter;
      }
   }
}
