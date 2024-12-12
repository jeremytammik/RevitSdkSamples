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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.AutoParameter.CS
{
   /// <summary>
   /// add parameters(family parameters/shared parameters) to the opened family file
   /// the parameters are recorded in txt file following certain formats
   /// </summary>
   class FamilyParameterAssigner
   {
      #region Memeber Fields
      private Autodesk.Revit.Application m_app;
      private FamilyManager m_manager = null;
      string m_assemblyPath;
      // indicate whether the parameter files have been loaded. If yes, no need to load again.
      bool m_paramLoaded;

      // set the paramName as key of dictionary for exclusiveness (the names of parameters should be unique)
      private Dictionary<string /*paramName*/, FamilyParam> m_familyParams;
      private DefinitionFile m_sharedFile;
      private string m_familyFilePath = string.Empty;
      private string m_sharedFilePath = string.Empty;
      #endregion

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="app">
      /// the active revit application
      /// </param>
      /// <param name="doc">
      /// the family document which will have parameters added in
      /// </param>
      public FamilyParameterAssigner(Autodesk.Revit.Application app, Document doc)
      {
         m_app = app;
         m_manager = doc.FamilyManager;
         m_familyParams = new Dictionary<string, FamilyParam>();
         m_assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         m_paramLoaded = false;
      }

      /// <summary>
      /// load the family parameter file (if exists) and shared parameter file (if exists)
      /// only need to load once
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      public bool LoadParametersFromFile()
      {
         if (m_paramLoaded)
         {
            return true;
         }
         // load family parameter file
         bool famParamFileExist;
         bool succeeded = LoadFamilyParameterFromFile(out famParamFileExist);
         if (!succeeded)
         {
            return false;
         }

         // load shared parameter file
         bool sharedParamFileExist;
         succeeded = LoadSharedParameterFromFile(out sharedParamFileExist);
         if (!(famParamFileExist || sharedParamFileExist))
         {
            MessageManager.MessageBuff.AppendLine("Neither familyParameter.txt nor sharedParameter.txt exists in the assembly folder.");
            return false;
         }
         if (!succeeded)
         {
            return false;
         }
         m_paramLoaded = true;
         return true;
      }

      /// <summary>
      /// load family parameters from the text file
      /// </summary>
      /// <returns>
      /// return true if succeeded; otherwise false
      /// </returns>
      private bool LoadFamilyParameterFromFile(out bool exist)
      {
         exist = true;
         // step 1: find the file "FamilyParameter.txt" and open it
         string fileName = m_assemblyPath + "\\FamilyParameter.txt";
         if (!File.Exists(fileName))
         {
            exist = false;
            return true;
         }

         FileStream file = null;
         StreamReader reader = null;
         try
         {
            file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);

            // step 2: read each line, if the line records the family parameter data, store it
            // record the content of the current line
            string line;
            // record the row number of the current line
            int lineNumber = 0;
            while (null != (line = reader.ReadLine()))
            {
               ++lineNumber;
               // step 2.1: verify the line
               // check whether the line is blank line (contains only whitespaces)
               Match match = Regex.Match(line, @"^\s*$");
               if (true == match.Success)
               {
                  continue;
               }

               // check whether the line starts from "#" or "*" (comment line)
               match = Regex.Match(line, @"\s*['#''*'].*");
               if (true == match.Success)
               {
                  continue;
               }

               // step 2.2: get the parameter data
               // it's a valid line (has the format of "paramName   paramGroup    paramType    isInstance", separate by tab or by spaces)
               // split the line to an array containing parameter items (format of string[] {"paramName", "paramGroup", "paramType", "isInstance"})
               string[] lineData = Regex.Split(line, @"\s+");
               // check whether the array has blank items (containing only spaces)
               List<string> values = new List<string>();
               foreach (string data in lineData)
               {
                  match = Regex.Match(data, @"^\s*$");
                  if (true == match.Success)
                  {
                     continue;
                  }

                  values.Add(data);
               }

               // verify the parameter items (should have 4 items exactly: paramName, paramGroup, paramType and isInstance)
               if (4 != values.Count)
               {
                  MessageManager.MessageBuff.Append("Loading family parameter data from \"FamilyParam.txt\".");
                  MessageManager.MessageBuff.Append("Line [\"" + line + "]\"" + "doesn't follow the valid format.\n");
                  return false;
               }

               // get the paramName
               string paramName = values[0];
               // get the paramGroup
               string groupString = values[1];
               // in case the groupString is format of "BuiltInParameterGroup.PG_Text", need to remove the "BuiltInParameterGroup.",
               // keep the "PG_Text" only
               int index = -1;
               if (0 <= (index = groupString.ToLower().IndexOf("builtinparametergroup")))
               {
                  // why +1? need to remove the "." after "builtinparametergroup"
                  groupString = groupString.Substring(index + 1 + "builtinparametergroup".Length);
               }
               BuiltInParameterGroup paramGroup = (BuiltInParameterGroup)Enum.Parse(typeof(BuiltInParameterGroup), groupString);

               // get the paramType
               string typeString = values[2];
               if (0 <= (index = typeString.ToLower().IndexOf("parametertype")))
               {
                  // why +1? need to remove the "." after "builtinparametergroup"
                  typeString = typeString.Substring(index + 1 + "parametertype".Length);
               }
               ParameterType paramType = (ParameterType)Enum.Parse(typeof(ParameterType), typeString);
               // get data "isInstance"
               string isInstanceString = values[3];
               bool isInstance = Convert.ToBoolean(isInstanceString);

               // step 2.3: store the parameter fetched, check for exclusiveness (as the names of parameters should keep unique)
               FamilyParam param = new FamilyParam(paramName, paramGroup, paramType, isInstance, lineNumber);
               // the family parameter with the same name has already been stored to the dictionary, raise an error
               if (m_familyParams.ContainsKey(paramName))
               {
                  FamilyParam duplicatedParam = m_familyParams[paramName];
                  string warning = "Line " + param.Line + "has a duplicate parameter name with Line " + duplicatedParam.Line + "\n";
                  MessageManager.MessageBuff.Append(warning);
                  continue;
               }
               m_familyParams.Add(paramName, param);
            }
         }
         catch (System.Exception e)
         {
            MessageManager.MessageBuff.AppendLine(e.Message);
            return false;
         }
         finally
         {
            if (null != reader)
            {
               reader.Close();
            }
            if (null != file)
            {
               file.Close();
            }
         }

         return true;
      }

      /// <summary>
      /// load family parameters from the text file
      /// </summary>
      /// <param name="exist">
      /// indicate whether the shared parameter file exists
      /// </param>
      /// <returns>
      /// return true if succeeded; otherwise false
      /// </returns>
      private bool LoadSharedParameterFromFile(out bool exist)
      {
         exist = true;
         string filePath = m_assemblyPath + "\\SharedParameter.txt";
         if (!File.Exists(filePath))
         {
            exist = false;
            return true;
         }

         m_app.Options.SharedParametersFilename = filePath;
         try
         {
            m_sharedFile = m_app.OpenSharedParameterFile();
         }
         catch (System.Exception e)
         {
            MessageManager.MessageBuff.AppendLine(e.Message);
            return false;
         }

         return true;
      }

      /// <summary>
      /// add parameters to the family file
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      public bool AddParameters()
      {
         // add the loaded family parameters to the family
         bool succeeded = AddFamilyParameter();
         if (!succeeded)
         {
            return false;
         }

         // add the loaded shared parameters to the family
         succeeded = AddSharedParameter();
         if (!succeeded)
         {
            return false;
         }

         return true;
      }

      /// <summary>
      /// add family parameter to the family
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      private bool AddFamilyParameter()
      {
         bool allParamValid = true;
         if (File.Exists(m_familyFilePath) && 
            0 == m_familyParams.Count)
         {
            MessageManager.MessageBuff.AppendLine("No family parameter available for adding.");
            return false;
         }

         foreach (FamilyParameter param in m_manager.Parameters)
         {
            string name = param.Definition.Name;
            if (m_familyParams.ContainsKey(name))
            {
               allParamValid = false;
               FamilyParam famParam = m_familyParams[name];
               MessageManager.MessageBuff.Append("Line " + famParam.Line + ": paramName \"" + famParam.Name + "\"already exists in the family document.\n");
            }
         }

         // there're errors in the family parameter text file
         if (!allParamValid)
         {
            return false;
         }

         foreach (FamilyParam param in m_familyParams.Values)
         {
            try
            {
               m_manager.AddParameter(param.Name, param.Group, param.Type, param.IsInstance);  
            }
            catch(Exception e)
            {
               MessageManager.MessageBuff.AppendLine(e.Message); 
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// load shared parameters from shared parameter file and add them to family
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      private bool AddSharedParameter()
      {
         if (File.Exists(m_sharedFilePath) &&
             null == m_sharedFile)
         {
            MessageManager.MessageBuff.AppendLine("SharedParameter.txt has an invalid format.");
            return false;
         }

         foreach (DefinitionGroup group in m_sharedFile.Groups)
         {
            foreach (ExternalDefinition def in group.Definitions)
            {
               // check whether the parameter already exists in the document
               FamilyParameter param = m_manager.get_Parameter(def.Name);
               if (null != param)
               {
                  continue;
               }

               try
               {
                  m_manager.AddParameter(def, def.ParameterGroup, true);
               }
               catch (System.Exception e)
               {
                  MessageManager.MessageBuff.AppendLine(e.Message);
                  return false;
               }
            }
         }

         return true;
      }

   }// end of class "FamilyParameterAssigner"

   /// <summary>
   /// record the data of a parameter: its name, its group, etc
   /// </summary>
   class FamilyParam
   {
      string m_name;
      /// <summary>
      /// the caption of the parameter
      /// </summary>
      public string Name
      {
         get { return m_name; }
      }

      BuiltInParameterGroup m_group;
      /// <summary>
      /// the group of the parameter
      /// </summary>
      public BuiltInParameterGroup Group
      {
         get { return m_group; }
      }

      ParameterType m_type;
      /// <summary>
      /// the type of the parameter
      /// </summary>
      public ParameterType Type
      {
         get { return m_type; }
      }

      bool m_isInstance;
      /// <summary>
      /// indicate whether the parameter is an instance parameter or a type parameter
      /// </summary>
      public bool IsInstance
      {
         get { return m_isInstance; }
      }

      int m_line;
      /// <summary>
      /// record the location of this parameter in the family parameter file
      /// </summary>
      public int Line
      {
         get { return m_line; }
      }

      /// <summary>
      /// default constructor, hide this by making it "private"
      /// </summary>
      private FamilyParam() 
      { 
      }

      /// <summary>
      /// constructor which exposes for invoking
      /// </summary>
      /// <param name="name">
      /// parameter name
      /// </param>
      /// <param name="group">
      /// indicate which group the parameter belongs to
      /// </param>
      /// <param name="type">
      /// the type of the parameter
      /// </param>
      /// <param name="isInstance">
      /// indicate whethe the parameter is an instance parameter
      /// </param>
      /// <param name="line">
      /// record the location of this parameter in the family parameter file
      /// </param>
      public FamilyParam(string name, BuiltInParameterGroup group, ParameterType type, bool isInstance, int line)
      {
         m_name = name;
         m_group = group;
         m_type = type;
         m_isInstance = isInstance;
         m_line = line;
      }
   }
}
