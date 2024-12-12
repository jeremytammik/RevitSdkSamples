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

using System.Collections.Generic;

using Autodesk.REX.Framework;
using REX.Common;
using REX.DRevitFreezeDrawing.Main;
using REX.DRevitFreezeDrawing.Resources.Dialogs;

namespace REX.DRevitFreezeDrawing
{
   /// <summary>
   /// Main class for development
   /// </summary>
   internal sealed class Extension : REXExtension
   {
      public Extension(REXApplication App)
          : base(App)
      {
         DataRef = new Data(this);
         ResultsRef = new Results(this);
      }
      //******************************************************************************************
      /// <summary>
      /// For standard funcionality fill interface methods
      /// </summary>
      #region Public overrided interface methods

      public override bool OnInitialize()
      {

         if (ThisApplication.Context2.Product.Type == REXInterfaceType.Revit)
            Revit.UsingParameters = false;
         return base.OnInitialize();
      }
      //******************************************************************************************
      /// <summary>
      /// Create needed structures and verify needs
      /// if you add error message to system errors the module is not load
      /// </summary>
      public override void OnCreate()
      {
         base.OnCreate();

         // System.SystemBase.Errors.AddError("ErrCreate", "Bd komunikacji z programem Revit!", null);
      }
      //******************************************************************************************
      /// <summary>
      /// Create sub dialogs
      /// </summary>
      public override void OnCreateDialogs()
      {
         base.OnCreateDialogs();
         m_ExpImpMng = new REXExpImpMng(this);
         m_RevitData = new REXRevitData(Revit.CommandData());
         m_RevitData.Initialize();
         m_ErrorType = 0;

         System.LoadFromFile(false);
         FreezeControl = new FreezeMainCtr(this);
         dlgOptions = new DialogOptions(this);
         dlgExportOptions = new DialogExportOptions(this);
         dlgViewSel = new DialogViewSel(this);
      }
      //******************************************************************************************
      /// <summary>
      /// Create layout items
      /// </summary>
      public override void OnCreateLayout()
      {
         base.OnCreateLayout();

         UI.HideCommand("ToolStripSeparatorBPrint");
         UI.HideCommand("ToolStripMenuItemPrint");
         UI.HideCommand("toolStripSeparator7");
         UI.HideCommand("MenuItemPrint");
         UI.HideCommand("MenuItemCalculations");

         Layout.ConstOptions = (long)REXUI.SetupOptions.FormFixed;
         Layout.AddLayout(new REXLayoutItem(REXLayoutItem.LayoutType.Layout, null, null, null, (long)REXUI.SetupOptions.TabDialog, FreezeControl));

         dlgOptions.SetProjectName(m_ExpImpMng.GetProjectName());

         System.SetCaption();
      }
      //******************************************************************************************
      /// <summary>
      /// Set data to dialogs
      /// </summary>
      public override void OnSetDialogs()
      {
         base.OnSetDialogs();
         dlgViewSel.SetLists(m_RevitData.ViewList);

      }
      //******************************************************************************************
      /// <summary>
      /// Get data from dialogs
      /// </summary>
      public override void OnSetData()
      {
         base.OnSetData();
         m_ErrorType = 0;
         if (!FreezeControl.CurrentView)
         {
            List<Autodesk.Revit.DB.View> viewList = dlgViewSel.GetSelectedViews();
            if (viewList.Count <= 0)
            {
               System.SystemBase.Errors.AddError("", "", null);
               m_ErrorType = 1;
            }
         }
         else
         {
            Autodesk.Revit.DB.View v = Revit.CommandData().Application.ActiveUIDocument.Document.ActiveView;

            if (v.ViewType == Autodesk.Revit.DB.ViewType.Internal || v.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet || !v.CanBePrinted)

            {
               System.SystemBase.Errors.AddError("", "", null);
               m_ErrorType = 2;
            }
         }

      }
      //******************************************************************************************
      /// <summary>
      /// </summary>
      public override bool OnCanRun()
      {
         //return true;      
         this.ExtensionContext.Control.ShowErrorsDialog = false;
         if (m_ErrorType == 1)
            return RunQuestion(Resources.Strings.Texts.REX_ModuleDescription, Resources.Strings.Texts.ErrorNoViewsDetected);
         else if (m_ErrorType == 2)
            return RunQuestion(Resources.Strings.Texts.REX_ModuleDescription, Resources.Strings.Texts.CurrentViewNotSet);
         else
            return true;
      }
      //******************************************************************************************
      /// <summary>
      /// Run process calculations and etc.
      /// </summary>
      public override void OnRun()
      {
         base.OnRun();
         List<ViewPath> fileNames = new List<ViewPath>();
         Autodesk.Revit.DB.DWGExportOptions dwgOpt = dlgExportOptions.GetExportOptions(dlgOptions.Copy);
         dwgOpt.FileVersion = dlgOptions.GetVersion();

         Autodesk.Revit.DB.DWGImportOptions dwgImpOpt = dlgOptions.GetImportOptions();
         dwgImpOpt.Unit = dlgExportOptions.GetExportUnits(dlgOptions.Copy);

         m_ExpImpMng.Clean();


         try //for demo mode
         {
            List<Autodesk.Revit.DB.View> viewList;
            if (FreezeControl.CurrentView)
            {
               Autodesk.Revit.DB.View v = Revit.CommandData().Application.ActiveUIDocument.Document.ActiveView;

               if (!(v.ViewType == Autodesk.Revit.DB.ViewType.Internal || v.ViewType == Autodesk.Revit.DB.ViewType.DrawingSheet || !v.CanBePrinted))
               {
                  viewList = new List<Autodesk.Revit.DB.View>();
                  viewList.Add(Revit.CommandData().Application.ActiveUIDocument.Document.ActiveView);
               }
               else
               {
                  viewList = new List<Autodesk.Revit.DB.View>();
               }
            }
            else
            {
               viewList = dlgViewSel.GetSelectedViews();
            }

            if (viewList.Count > 0)
            {
               IREXProgress Progress = System.SystemBase.Tools.Prograss;
               Progress.Steps = 2 * viewList.Count;
               Progress.Show(GetForm());
               GetForm().Hide();

               fileNames = m_ExpImpMng.Export(viewList, dwgOpt, Progress);
               m_ExpImpMng.Import(dlgOptions.Copy, dlgOptions.Browse, dlgOptions.BaseName, dwgImpOpt, fileNames, Progress);

               if (dlgOptions.DeleteView)
                  m_ExpImpMng.DeleteViews(viewList);

               Progress.Hide();
               global::System.Windows.Forms.MessageBox.Show(GetForm(), Resources.Strings.Texts.FreezingFinSuccesfull, Resources.Strings.Texts.REX_ModuleDescription);
            }
         }
         catch
         {
         }

         m_ExpImpMng.Clean();
         DataRef.OptionPath = dlgOptions.Browse;
         System.SaveToFile(false, false);
      }
      //******************************************************************************************
      /// <summary>
      /// Printing results.
      /// </summary>
      public override void OnShowResults()
      {
         base.OnShowResults();
      }
      //******************************************************************************************
      /// <summary>
      /// If return true dialog will close after OK button clicked
      /// </summary>
      public override bool OnCanClose(REXCanCloseType Type)
      {
         return base.OnCanClose(Type);
      }
      //******************************************************************************************
      /// <summary>
      /// </summary>
      public override void OnClose()
      {
         base.OnClose();
      }
      //******************************************************************************************
      /// <summary>
      /// Activate or deactivate layout - module can modify layout before or after activation
      /// </summary>
      public override void OnActivateLayout(REXLayoutItem LItem, bool Activate)
      {
         base.OnActivateLayout(LItem, Activate);

         // -- insert your code here --- 
         if (Activate)
         {

         }
         else
         {
         }
      }
      //******************************************************************************************
      /// <summary>
      /// Error selecting handler
      /// </summary>
      public override void OnErrorSelected(REXErrorsEventArgs e)
      {
         base.OnErrorSelected(e);

         // -- insert your code here --- 
      }
      //******************************************************************************************
      /// <summary>
      /// Get text from resources
      /// </summary>
      public override string OnGetText(string Name)
      {
         // For obfuscation strings
         if (Name == REXConst.ENG_ResModuleDescription)
            return Resources.Strings.Texts.REX_ModuleDescription;
         return Resources.Strings.Texts.ResourceManager.GetString(Name);
      }
      #endregion

      //******************************************************************************************
      /// <summary>
      /// For non standard funcionality fill other methods
      /// </summary>
      #region Public overrided methods for develeopment

      /// <summary>
      /// Create all needed objects
      /// </summary>
      public override bool Create()
      {
         bool result = base.Create();

         // -- insert your code here --- 

         return result;
      }
      //******************************************************************************************
      /// <summary>
      /// Create all needed references and data
      /// </summary>
      public override void Setup()
      {
         base.Setup();
      }
      //******************************************************************************************
      /// <summary>
      /// Command handler
      /// </summary>
      public override object Command(ref REXCommand CommandStruct)
      {
         return base.Command(ref CommandStruct);
      }
      //******************************************************************************************
      /// <summary>
      /// Event handler
      /// </summary>
      public override object Event(ref REXEvent EventStruct)
      {
         return base.Event(ref EventStruct);
      }
      //******************************************************************************************
      #endregion

      #region Main control methods

      public MainControl MainControl()
      {
         if (object.ReferenceEquals(ControlRef, null))
            ControlRef = new MainControl(this);

         return ControlRef;
      }
      #endregion

      #region Public overrided UI methods - Form and Control reference
      public override REXExtensionForm GetForm()
      {
         if (object.ReferenceEquals(FormRef, null))
            FormRef = new MainForm(this);

         return (REXExtensionForm)FormRef;
      }
      //******************************************************************************************
      public override REXExtensionControl GetControl()
      {
         return MainControl();
      }
      //******************************************************************************************
      #endregion

      #region Public overrided data and results
      public Data Data
      {
         get
         {
            return DataRef;
         }
      }
      //******************************************************************************************
      public Results Results
      {
         get
         {
            return ResultsRef;
         }
      }
      //******************************************************************************************
      public override REXResults GetResults()
      {
         return ResultsRef;
      }
      //******************************************************************************************
      public override REXData GetData()
      {
         return DataRef;
      }
      #endregion

      #region <other>

      public void ShowOptionsDialog()
      {
         dlgOptions.ShowDialog(this.GetForm());
      }

      public void ShowExportOptionsDialog()
      {
         dlgExportOptions.ShowDialog(GetForm());
      }

      public void ShowSelectViews()
      {
         dlgViewSel.ShowDialog(GetForm());
      }

      public REXRevitData RevitData
      {
         get
         {
            return m_RevitData;
         }
      }

      #endregion

      #region Private members
      private Data DataRef;
      private Results ResultsRef;

      private MainForm FormRef;
      private MainControl ControlRef;

      private DialogOptions dlgOptions;
      private DialogExportOptions dlgExportOptions;
      private DialogViewSel dlgViewSel;

      private FreezeMainCtr FreezeControl;
      private REXExpImpMng m_ExpImpMng;

      private REXRevitData m_RevitData;

      private int m_ErrorType;
      #endregion
   }
}
