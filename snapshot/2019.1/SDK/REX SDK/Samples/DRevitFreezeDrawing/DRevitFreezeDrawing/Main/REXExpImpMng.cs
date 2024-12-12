//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
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

using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using RevitElement = Autodesk.Revit.DB.Element;

using global::System.IO;
using Autodesk.REX.Framework;
using REX.Common;
using REX.Common.Revit.Geometry;
using REX.DRevitFreezeDrawing.Resources.Dialogs;


namespace REX.DRevitFreezeDrawing.Main
{
   /// <summary>
   /// REX Export/Import manager class
   /// </summary>
   class REXExpImpMng : REXExtensionObject
   {
      #region<members>

      Autodesk.Revit.UI.ExternalCommandData m_CommandData;
      string m_path; //system temp path

      List<FileInfo> m_FileDwgList;//list of read files before export to the directory

      #endregion


      #region<constructors>
      public REXExpImpMng(REXExtension argExt)
          : base(argExt)
      {
         m_CommandData = argExt.Revit.CommandData();

         m_FileDwgList = new List<FileInfo>();
      }
      #endregion

      #region<methods>

      //******************************************************************************************
      /// <summary>
      /// this function export View from the list
      /// </summary>  
      public List<ViewPath> Export(List<View> argListView, DWGExportOptions setDwg, IREXProgress argProgress)
      {
         IREXProgress Progress = argProgress;
         Progress.Steps = 2 * argListView.Count;
         Progress.Position = 0;
         Progress.Header = Resources.Strings.Texts.FreezeInProgress + " - " + Resources.Strings.Texts.Export;

         List<ViewPath> nameStr = new List<ViewPath>();

         m_path = getTempPath();

         //reading files from temp directory  

         setDwg.LayerMapping = GetLayMap(setDwg.LayerMapping);

         foreach (View v in argListView)
         {
            List<ElementId> vSet = new List<ElementId>();
            if (v != null)
            {
               View3D tmp3D = null;
               Autodesk.Revit.DB.ViewSheet viewSheet = null;
               bool proceed = false;

               if (v is Autodesk.Revit.DB.View3D)
               {
                  try
                  {
                     FilteredElementCollector filteredElementCollector = new FilteredElementCollector(ThisExtension.Revit.ActiveDocument);
                     filteredElementCollector.OfClass(typeof(FamilySymbol));
                     filteredElementCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                     IList<RevitElement> titleBlocks = filteredElementCollector.ToElements();
                     List<FamilySymbol> familySymbols = new List<FamilySymbol>();
                     foreach (Element element in titleBlocks)
                     {
                        FamilySymbol f = element as FamilySymbol;
                        if (f != null)
                           familySymbols.Add(f);
                     }

                     if (titleBlocks.Count != 0)
                     {
                        Autodesk.Revit.DB.FamilySymbol fs = null;
                        foreach (Autodesk.Revit.DB.FamilySymbol f in familySymbols)
                        {
                           if (null != f) { fs = f; break; }
                        }
                        viewSheet = Autodesk.Revit.DB.ViewSheet.Create(ThisExtension.Revit.ActiveDocument, fs.Id);
                        if (null != viewSheet)
                        {
                           UV location = new UV((viewSheet.Outline.Max.U - viewSheet.Outline.Min.U) / 2,
                                                (viewSheet.Outline.Max.V - viewSheet.Outline.Min.V) / 2);

                           try
                           {
                              Viewport.Create(ThisExtension.Revit.ActiveDocument, viewSheet.Id, v.Id, new XYZ(location.U, location.V, 0.0));
                           }
                           catch
                           {
                              try
                              {
                                 XYZ tmpXYZ = new XYZ(-v.ViewDirection.X, -v.ViewDirection.Y, -v.ViewDirection.Z);
                                 BoundingBoxXYZ tmpBoundingBox = v.CropBox;
                                 bool tmpCropBoxActive = v.CropBoxActive;

                                 IList<Element> viewTypes = REXGeometryRvt.FilterElements(ThisExtension.Revit.ActiveDocument, typeof(ViewFamilyType));

                                 ElementId viewTypeid = null;

                                 foreach (Element viewType in viewTypes)
                                 {
                                    ViewFamilyType famType = viewType as ViewFamilyType;

                                    if (famType != null && famType.ViewFamily == ViewFamily.ThreeDimensional)
                                    {
                                       viewTypeid = famType.Id;
                                       break;
                                    }
                                 }

                                 if (viewTypeid != null)
                                 {

                                    tmp3D = View3D.CreateIsometric(ThisExtension.Revit.ActiveDocument, viewTypeid);

                                    tmp3D.ApplyViewTemplateParameters(v);
                                    tmp3D.CropBox = tmpBoundingBox;

                                    Viewport.Create(ThisExtension.Revit.ActiveDocument, viewSheet.Id, tmp3D.Id, new XYZ(location.U, location.V, 0.0));
                                 }
                              }
                              catch
                              {
                              }
                           }

                           vSet.Add(viewSheet.Id);
                           proceed = true;
                        }
                     }
                  }
                  catch { }
               }
               else
               {
                  vSet.Add(v.Id);
                  proceed = true;
               }

               if (proceed)
               {
                  string fam = v.get_Parameter(BuiltInParameter.VIEW_FAMILY).AsString();
                  Progress.Step(fam + "-" + v.Name);

                  string vName = ValidateFileName(v.Name);
                  m_CommandData.Application.ActiveUIDocument.Document.Export(m_path, fam + "-" + vName, vSet, setDwg);

                  nameStr.Add(new ViewPath(m_path + "\\" + fam + "-" + vName + ".dwg", v, fam + "-" + v.Name));
                  Progress.Step(fam + "-" + v.Name);

                  if (viewSheet != null)
                  {
                     ElementId elementId = viewSheet.Id;
                     ThisExtension.Revit.ActiveDocument.Delete(elementId);
                  }

                  if (tmp3D != null)
                  {
                     ElementId elementId = tmp3D.Id;
                     ThisExtension.Revit.ActiveDocument.Delete(elementId);
                  }
               }
            }
         }
         return nameStr;
      }
      //******************************************************************************************        
      private string ValidateFileName(string name)
      {
         string newname = name.Replace("/", "-");
         newname = newname.Replace("\\", "-");
         newname = newname.Replace(":", "-");

         return newname;
      }
      //******************************************************************************************
      /// <summary>
      /// reading layer map from txt file in Configuration directory
      /// </summary>  
      private string GetLayMap(string argLayMap)
      {
         string Dir = m_CommandData.Application.ActiveUIDocument.Document.GetType().Assembly.Location;
         string DirProgram = Directory.GetParent(Dir).FullName;
         string DirData = Directory.GetParent(DirProgram).FullName + "\\Data";

         REXEnvironment Env = new REXEnvironment(REXConst.ENG_DedicatedVersionName);
         string path = Env.GetModulePath(REXEnvironment.PathType.Configuration, ThisExtension.ThisApplication.Context.Name);

         string pathDirMap;
         switch (argLayMap)
         {
            case "AIA":
               pathDirMap = DirData + "\\exportlayers-dwg-AIA.txt";
               if (File.Exists(pathDirMap))
                  return pathDirMap;
               else
               {
                  pathDirMap = path + "\\exportlayers-dwg-AIA.txt";
                  if (File.Exists(pathDirMap))
                     return pathDirMap;
                  else
                     return "";
               }

            case "BS1192":
               pathDirMap = DirData + "\\exportlayers-dwg-BS1192.txt";
               if (File.Exists(pathDirMap))
                  return pathDirMap;
               else
               {
                  pathDirMap = path + "\\exportlayers-dwg-BS1192.txt";
                  if (File.Exists(pathDirMap))
                     return pathDirMap;
                  else
                     return "";
               }
            case "ISO13567":
               pathDirMap = DirData + "\\exportlayers-dwg-ISO13567.txt";
               if (File.Exists(pathDirMap))
                  return pathDirMap;
               else
               {
                  pathDirMap = path + "\\exportlayers-dwg-ISO13567.txt";
                  if (File.Exists(pathDirMap))
                     return pathDirMap;
                  else
                     return "";
               }
            case "CP38":
               pathDirMap = DirData + "\\exportlayers-dwg-CP83.txt";
               if (File.Exists(pathDirMap))
                  return pathDirMap;
               else
               {
                  pathDirMap = path + "\\exportlayers-dwg-CP83.txt";
                  if (File.Exists(pathDirMap))
                     return pathDirMap;
                  else
                     return "";
               }
            default:
               pathDirMap = "";
               return "";
         }
      }

      //******************************************************************************************
      /// <summary>
      /// this function imports all views from the list and create new draft views for them based on m_path
      /// </summary>  
      public void Import(bool argCopy, string argBrowse, string argBaseName, Autodesk.Revit.DB.DWGImportOptions argOpt, List<ViewPath> argViewList, IREXProgress argProgress)
      {
         ViewDrafting draftView;
         DWGImportOptions setDwgImp;
         IREXProgress Progress = argProgress;

         DialogMessageExists dlgMsg = new DialogMessageExists(ThisExtension);
         dlgMsg.Text = Resources.Strings.Texts.REX_ModuleDescription;

         //getting file list from the directory
         string[] FileList = Directory.GetFiles(m_path, "*.dwg");

         List<string> SelectFileList = new List<string>();

         string newname;
         string strPost = " (" + Resources.Strings.Texts.Freezed + ")";

         Progress.Position = 0;
         Progress.Steps = 2 * argViewList.Count;
         Progress.Header = Resources.Strings.Texts.FreezeInProgress + " - " + Resources.Strings.Texts.Import;

         ViewFamilyType viewFamilyType = null;
         FilteredElementCollector collector = new FilteredElementCollector(m_CommandData.Application.ActiveUIDocument.Document);
         var viewFamilyTypes = collector.OfClass(typeof(ViewFamilyType)).ToElements();
         foreach (Element e in viewFamilyTypes)
         {
            ViewFamilyType v = e as ViewFamilyType;
            if (v.ViewFamily == ViewFamily.Drafting)
            {
               viewFamilyType = v;
               break;
            }
         }

         //importing files to Revit
         foreach (ViewPath v in argViewList)
         {
            draftView = ViewDrafting.Create(m_CommandData.Application.ActiveUIDocument.Document, viewFamilyType.Id);

            newname = ReplaceForbiddenSigns(v.fullViewName);

            string tempName = newname;
            int i = 1;
            for (;;)
            {
               try
               {
                  draftView.Name = newname + strPost;
                  break;
               }
               catch
               {
                  if (i > 10)
                  {
                     try
                     {
                        draftView.Name = draftView.Name + strPost;
                     }
                     catch
                     {
                     }
                     break;
                  }
                  newname = tempName + "-" + i.ToString();
                  i++;
               }
            }

            draftView.Scale = v.ViewRevit.Scale;

            Progress.Step(draftView.Name);

            //properties
            setDwgImp = new DWGImportOptions();
            setDwgImp.ColorMode = argOpt.ColorMode;
            setDwgImp.OrientToView = argOpt.OrientToView;
            setDwgImp.Unit = argOpt.Unit;
            setDwgImp.CustomScale = argOpt.CustomScale;

            //import
            RevitElement el = (RevitElement)draftView;
            if (File.Exists(v.path))
            {
               ElementId id;
               m_CommandData.Application.ActiveUIDocument.Document.Import(v.path, setDwgImp, draftView, out id);
               v.DraftingName = draftView.Name;

               //copying to user directory
               if (argCopy)
               {
                  CopyToUserDirViewPath(v, argBaseName, argBrowse, dlgMsg);
               }
            }
            Progress.Step(draftView.Name);
         }
         dlgMsg.Dispose();
      }

      //******************************************************************************************
      /// <summary>
      /// this function gets view nam based on file name
      /// </summary>
      private bool CopyToUserDirViewPath(ViewPath argView, string argBaseName, string argBrowse, DialogMessageExists dlgMsg)
      {
         string newFileName;
         if (argBaseName != "")
            newFileName = argBrowse + "\\" + argBaseName + "-" + argView.DraftingName + ".dwg";
         else
            newFileName = argBrowse + "\\" + argView.DraftingName + ".dwg";

         string dirref = Directory.GetParent(argView.path).FullName;
         string dwgreffile = argView.fullViewName.Replace(" ", "");
         string dwgref = Path.GetFileNameWithoutExtension(dwgreffile) + "-*.dwg";
         string dwggrefpath = GetRefDWGFilePath(dwgref, dirref);
         string dd = Path.GetFileName(dwggrefpath);
         string dwgrefdestpath = Path.Combine(argBrowse, dd);
         if (File.Exists(newFileName))
         {
            if (dlgMsg.Context == ReplaceContext.Leave || dlgMsg.Context == ReplaceContext.Replace)
            {
               dlgMsg.ShowMessageDialog(newFileName, ThisExtension.GetForm());

               if (dlgMsg.Context == ReplaceContext.Replace || dlgMsg.Context == ReplaceContext.ReplaceAll)
               {
                  try
                  {
                     File.Copy(argView.path, newFileName, true);
                     if (dwggrefpath != "")
                        File.Copy(dwggrefpath, dwgrefdestpath, true);
                     return true;
                  }
                  catch
                  {
                     System.Windows.Forms.MessageBox.Show(ThisExtension.GetForm(), Resources.Strings.Texts.ErrorAccessDenied + newFileName, Resources.Strings.Texts.REX_ModuleDescription);
                     return false;
                  }
               }

               return true;
            }
            else if (dlgMsg.Context == ReplaceContext.ReplaceAll)
            {
               try
               {
                  File.Copy(argView.path, newFileName, true);
                  if (dwggrefpath != "")
                     File.Copy(dwggrefpath, dwgrefdestpath, true);
                  return true;
               }
               catch
               {
                  System.Windows.Forms.MessageBox.Show(ThisExtension.GetForm(), Resources.Strings.Texts.ErrorAccessDenied + newFileName, Resources.Strings.Texts.REX_ModuleDescription);
                  return false;
               }
            }
            else
            {
               return true;
            }
         }
         else
         {
            global::System.IO.File.Copy(argView.path, newFileName);
            if (dwggrefpath != "")
               System.IO.File.Copy(dwggrefpath, dwgrefdestpath, true);
            return true;
         }
      }

      //******************************************************************************************
      /// <summary>
      /// this function gets view nam based on file name
      /// </summary> 
      private string getViewNameDwgDel(string fileName)
      {
         int indeks;
         indeks = fileName.IndexOf(".dwg");

         if (indeks < 0)
            return "";

         string name2 = fileName.Substring(0, indeks);
         return name2;
      }

      //******************************************************************************************
      /// <summary>
      /// Search for files with particular searchpattern = filename in directory dirref.
      /// </summary>
      /// <param name="filename">Searched filename</param>
      /// <param name="dirref">Directory reference</param>
      /// <returns></returns>
      private string GetRefDWGFilePath(string filename, string dirref)
      {
         string pathtofile = "";
         string searchPattern = filename;

         DirectoryInfo di = new DirectoryInfo(dirref);

         System.IO.FileInfo[] files = di.GetFiles(searchPattern, SearchOption.AllDirectories);

         foreach (System.IO.FileInfo file in files)
         {
            pathtofile = file.FullName;
         }
         return pathtofile;
      }

      //******************************************************************************************
      /// <summary>
      /// this function returns temp system path
      /// </summary> 
      private string getTempPath()
      {
         string path = Environment.GetEnvironmentVariable("temp");
         if (!Directory.Exists(path))
         {
            path = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..\\Temp"));
            if (!Directory.Exists(path))
               Directory.CreateDirectory(path);
         }

         path = path + "\\DRevitFreezeDrawing Temp";

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

         return path;
      }

      //******************************************************************************************
      /// <summary>
      /// this function checks if file is on the list
      /// </summary> 
      private bool CheckIfStrOnList(string str, List<FileInfo> list)
      {
         foreach (FileInfo el in list)
         {
            if (str == el.fileName)
               return true;
         }
         return false;
      }

      //******************************************************************************************
      /// <summary>
      /// this function checks if date of the file has changed
      /// </summary> 
      private bool CheckIfDateChanged(FileInfo file, List<FileInfo> list)
      {
         foreach (FileInfo el in list)
         {
            if (file.fileName == el.fileName)
            {
               if (file.fileDateTime != el.fileDateTime)
                  return true;
            }
         }
         return false;
      }

      //******************************************************************************************
      /// <summary>
      /// this function returns view which is connected with created dwg file
      /// </summary> 
      private View GetViewFileNameBased(string fileName)
      {
         foreach (View v in ((Extension)ThisExtension).RevitData.ViewList)
         {
            if (fileName.Contains(v.Name + "."))
               return v;
         }
         return null;
      }

      //******************************************************************************************
      /// <summary>
      /// this function returns view which is connected with created dwg file
      /// </summary> 
      private View GetView(string fileName)
      {
         foreach (View v in ((Extension)ThisExtension).RevitData.ViewList)
         {
            if (fileName == v.Name + ".dwg")
               return v;
         }
         return null;
      }


      //******************************************************************************************
      /// <summary>
      /// this function changes format of the file (for example name.dwg -> name.pcp)
      /// </summary> 
      private string ChangeNameFormat(string file, string OldFormat, string NewFormat)
      {
         int indeks = file.LastIndexOf(OldFormat);
         string newName = (file.Substring(0, indeks) + NewFormat);
         return newName;
      }

      //******************************************************************************************
      /// <summary>
      /// this function cleans temp path directory
      /// </summary> 
      public bool Clean()
      {
         if (Directory.Exists(m_path))
         {
            try
            {
               Directory.Delete(m_path, true);
               return true;
            }
            catch
            {
               return false;
            }
         }
         return false;
      }

      //************************************************************************
      public string GetProjectName()
      {
         return m_CommandData.Application.ActiveUIDocument.Document.Title;
      }
      #endregion

      //************************************************************************
      /// <summary>
      /// replaces forbiden signs with "" in given name string
      /// </summary>
      private string ReplaceForbiddenSigns(string name)
      {
         name = name.Replace("[", "");
         name = name.Replace("]", "");
         name = name.Replace("}", "");
         name = name.Replace("{", "");
         name = name.Replace("|", "");
         name = name.Replace("?", "");
         name = name.Replace("'", "");
         name = name.Replace(":", "");
         name = name.Replace("\\", "");
         name = name.Replace("~", "");
         name = name.Replace(">", "");
         name = name.Replace("<", "");
         name = name.Replace(";", "");

         return name;
      }

      //************************************************************************
      /// <summary>
      /// deletes in document all views from given list argListView
      /// </summary>
      public void DeleteViews(List<View> argListView)
      {
         foreach (View v in argListView)
            m_CommandData.Application.ActiveUIDocument.Document.Delete(v.Id);
      }
   }

   //******************************************************************************************
   /// <summary>
   /// this class preserves all needed information about the file
   /// </summary>
   class FileInfo
   {
      public string fileName;
      public DateTime fileDateTime;

      public FileInfo(string name, DateTime date)
      {
         fileName = name;
         fileDateTime = date;
      }
   }

   class ViewPath
   {
      public ViewPath(string argPath, View argView, string argFullName)
      {
         path = argPath;
         ViewRevit = argView;
         fullViewName = argFullName;
      }
      public string path;
      public View ViewRevit;
      public string fullViewName;
      public string DraftingName;
   }

   enum ReplaceContext
   {
      Replace,
      ReplaceAll,
      Leave,
      LeaveAll
   }
}
