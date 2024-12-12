//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
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
using System.Text;
using System.Windows;
using System.Threading;
using System.Reflection;

using REX.Common;
using Autodesk.REX.Framework;

namespace REX.ContentGeneratorWPF
{
    /// <summary>
    /// Main class for development.
    /// </summary>
    internal class Extension : REXExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Extension"/> class.
        /// </summary>
        /// <param name="App">The reference to REXApplication object.</param>
        public Extension(REXApplication App)
            : base(App)
        {
            DataRef = new Data(this);
            ResultsRef = new Results(this);
        }

        /// <summary>
        /// For standard funcionality fill following methods.
        /// </summary>
        #region Public overrided methods for develeopment

        /// <summary>
        /// Called when Initialize.
        /// Creates dedicated extension class according main context.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
       
        public override bool OnInitialize()
        {
            if (ThisApplication.Context2.Product.Type == REXInterfaceType.Revit)
                ExtensionRef = new Main.Revit.ExtensionRevit(this);

            if (ExtensionRef != null)
            {
                if (!ExtensionRef.OnInitialize())
                    return false;
            }

            Converter = new REX.ContentGenerator.Converters.REXFamilyConverter();

            return base.OnInitialize();
        }

        /// <summary>
        /// Creates needed structures and verify needs.
        /// </summary>      
        public override void OnCreate()
        {
            base.OnCreate();

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnCreate();
        }

        /// <summary>
        /// Creates sub dialog.
        /// </summary>      
        public override void OnCreateDialogs()
        {
            base.OnCreateDialogs();

            SelectedElementControlRef = new REX.ContentGeneratorWPF.Resources.Dialogs.SelectedElementControl(this);
            CGReadControlRef = new REX.ContentGeneratorWPF.Resources.ContentGeneratorReadControl(this);
            CGCreateControlRef = new REX.ContentGeneratorWPF.Resources.Dialogs.ContentGeneratorCreateControl(this);

            if (ExtensionRef != null)
                ExtensionRef.OnCreateDialogs();
        }

        /// <summary>
        /// Creates layout items and setups layout.
        /// </summary>
        public override void OnCreateLayout()
        {
            base.OnCreateLayout();

            System.SetCaption();

            global::System.Windows.Media.Imaging.BitmapImage logoImage = REXLibrary.GetResourceImage(GetType().Assembly, "Resources/Other/Images/REX_logo.png");

            // UI.SetLogo(logoImage);

            Layout.ConstOptions = (long)REXUI.SetupOptions.HSplitFixed
                                | (long)REXUI.SetupOptions.VSplitFixed
                                | (long)REXUI.SetupOptions.TabDialog
                                | (long)REXUI.SetupOptions.List
                                | (long)REXUI.SetupOptions.FormFixed;

            Layout.AddLayout(new REXLayoutItem(REXLayoutItem.LayoutType.Layout, "Element", "", "Element", (long)0, SelectedElementControlRef, null, logoImage));
            Layout.AddLayout(new REXLayoutItem(REXLayoutItem.LayoutType.Layout, "Recognition", "", "Recognition", (long)0, CGReadControlRef, null, logoImage));
            Layout.AddLayout(new REXLayoutItem(REXLayoutItem.LayoutType.Layout, "Creation", "", "Creation", (long)0, CGCreateControlRef, null, logoImage));

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnCreateLayout();

            SelectedElementControlRef.SetDialog();
            CGReadControlRef.SetDialog();
            CGCreateControlRef.SetDialog();
        }

        /// <summary>
        /// Set data on dialogs.
        /// </summary>       
        public override void OnSetDialogs()
        {
            base.OnSetDialogs();

            // insert code here.

            Layout.SelectLayout("L1");

            if (ExtensionRef != null)
                ExtensionRef.OnSetDialogs();
        }

        /// <summary>
        /// Get data from dialogs and sets Data's strucutres.
        /// </summary>      
        public override void OnSetData()
        {
            base.OnSetData();

            CGCreateControlRef.SetData();

            if (ExtensionRef != null)
                ExtensionRef.OnSetData();
        }

        /// <summary>
        /// Called when data are loaded.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
        public override bool OnDataLoad()
        {
            return true;
        }

        /// <summary>
        /// Called when data loaded.
        /// </summary>
        /// <param name="Result">If set to <c>true</c> operation succeeded; Otherwise some errors found.</param>
        public override void OnDataLoaded(bool Result)
        {
        }

        /// <summary>
        /// Called when data are saved.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
        public override bool OnDataSave()
        {
            return true;
        }

        /// <summary>
        /// Called when data are saved.
        /// </summary>
        /// <param name="Result">If set to <c>true</c> operation succeeded; Otherwise some errors found.</param>
        public override void OnDataSaved(bool Result)
        {
        }

        /// <summary>
        /// Called when data are cleared.
        /// </summary>
        /// <param name="Implemented">If returns <c>true</c> method was implemented.</param>
        /// <returns>Returns result of clear operation.</returns>
        public override bool OnClear(out bool Implemented)
        {
            Implemented = false;

            return true;
        }

        /// <summary>
        /// Called when template is loaded.
        /// Only if <c>ExtensionContext.Control.StandardRunContext</c> is equals to <c>REXExtensionContext.REXStandardRunContext.OperationEditTemplate</c>.
        /// </summary>
        /// <param name="Result">If set to <c>true</c> operation succeeded; Otherwise some errors found.</param>
        public override bool OnLoadTemplate()
        {
            return true;
        }

        /// <summary>
        /// Called when template is saved.
        /// </summary>
        /// <param name="Result">If set to <c>true</c> operation succeeded; Otherwise some errors found.</param>
        public override bool OnSaveTemplate()
        {
            return true;
        }

        /// <summary>
        /// Checks status of module and shows some information for user if module can't excecute action.
        /// </summary>
        /// <returns>Returns true if module can do some action in OnRun method; otherwise false.</returns>     
        public override bool OnCanRun()
        {
            bool result = true;



            if (ExtensionRef != null)
                result &= ExtensionRef.OnCanRun();



            // insert code here.

            return result;
        }

        /// <summary>
        /// Runs process calculations or other operations.
        /// </summary>       
        public override void OnRun()
        {
            base.OnRun();

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnRun();
        }

        /// <summary>
        /// Shows results.
        /// </summary>
        public override void OnShowResults()
        {
            base.OnShowResults();

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnShowResults();
        }

        /// <summary>
        /// If returns true dialog will close after OK button is clicked.
        /// </summary>
        /// <param name="Type">The closing type. See <see cref="REXCanCloseType"/> enum.</param>
        /// <returns>Returns true if module can be closed.</returns>
        public override bool OnCanClose(REXCanCloseType Type)
        {
            bool result = true;

            if (ExtensionRef != null)
                result &= ExtensionRef.OnCanClose(Type);
                        
            return result;
        }

        /// <summary>
        /// Called during closing module.
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnClose();
        }

        /// <summary>
        /// Activates or deactivates layout - module can modify layout before or after activation.
        /// </summary>
        /// <param name="LItem">The layout item.</param>
        /// <param name="Activate">If set to <c>true</c> activate.</param>       
        public override void OnActivateLayout(REXLayoutItem LItem, bool Activate)
        {
            base.OnActivateLayout(LItem, Activate);

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnActivateLayout(LItem, Activate);
        }

        /// <summary>
        /// Called when an error is selected on errors list dialog.
        /// Raises the <see cref="ErrorSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Autodesk.REX.Framework.REXErrorsEventArgs"/> instance containing the event data.</param>
        public override void OnErrorSelected(REXErrorsEventArgs e)
        {
            base.OnErrorSelected(e);

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnErrorSelected(e);
        }

        /// <summary>
        /// Called when preferences changed.
        /// </summary>
        public override void OnPreferencesChanged()
        {
            DFM.BeforeParamsChange();

            base.OnPreferencesChanged();

            // insert code here.

            if (ExtensionRef != null)
                ExtensionRef.OnPreferencesChanged();

            DFM.AfterParamsChange();
        }

        /// <summary>
        /// Get specified text from resources. Needed for obfuscated strings tables.
        /// </summary>
        /// <param name="Name">The name of string.</param>
        /// <returns>Returns specified text.</returns>        
        public override string OnGetText(string Name)
        {
            if (Name == REXConst.ENG_ResModuleDescription)
                return Resources.Strings.Texts.REX_ModuleDescription;
            if (Name == REXConst.ENG_ResVersionInfo)
                return Resources.Strings.Texts.REX_VersionInfo;

            // insert code here.

            if (ExtensionRef != null)
            {
                string result = ExtensionRef.OnGetText(Name);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }

            return Resources.Strings.Texts.ResourceManager.GetString(Name);
        }

        /// <summary>
        /// Commands the specified command struct.
        /// </summary>
        /// <param name="CommandStruct">The command struct.</param>
        /// <returns>Returns result of event operation.</returns>
        public override object OnCommand(ref REXCommand CommandStruct)
        {
            object result = null;

            if (ExtensionRef != null)
                result = ExtensionRef.OnCommand(ref CommandStruct);

            // insert code here.

            if (result == null)
                return base.OnCommand(ref CommandStruct);
            else
                return result;
        }

        /// <summary>
        /// Called when event.
        /// </summary>
        /// <param name="EventStruct">The event struct.</param>
        /// <returns>Returns result of event operation.</returns>
        public override object OnEvent(ref REXEvent EventStruct)
        {
            object result = null;

            if (ExtensionRef != null)
                result = ExtensionRef.OnEvent(ref EventStruct);

            // insert code here.

            if (result == null)
                return base.OnEvent(ref EventStruct);
            else
                return result;
        }

        /// <summary>
        /// Commands the specified command struct.
        /// </summary>
        /// <param name="CommandStruct">The command struct.</param>
        /// <returns>Returns result of event operation.</returns>
        public override object Command(ref REXCommand CommandStruct)
        {
            // insert code here.

            return base.Command(ref CommandStruct);
        }

        /// <summary>
        /// Events the specified event struct.
        /// </summary>
        /// <param name="EventStruct">The event struct.</param>
        /// <returns>Returns result of event operation.</returns>
        public override object Event(ref REXEvent EventStruct)
        {
            // insert code here.

            return base.Event(ref EventStruct);
        }
        #endregion


        /// <summary>
        /// For non standard funcionality fill following methods.
        /// </summary>
        #region Public overrided methods for develeopment

        /// <summary>
        /// Create all needed objects
        /// </summary>
        public override bool Create()
        {
            bool result = base.Create();

            // insert code here.

            return result;
        }

        /// <summary>
        /// Creates all needed references and data.
        /// </summary>
        public override void Setup()
        {
            base.Setup();

            // insert code here.
        }
        #endregion

        #region Public overrided UI methods - Form and Control reference
        /// <summary>
        /// Get reference to main control which will be embedded on MainForm.
        /// </summary>
        /// <returns>Returns reference to MainControl object based on REXExtensionControl.</returns>
        public Resources.Dialogs.MainControl MainControl()
        {
            if (object.ReferenceEquals(ControlRef, null))
                ControlRef = new Resources.Dialogs.MainControl(this);

            return ControlRef;
        }

        /// <summary>
        /// Get reference to main form.
        /// </summary>
        /// <returns>Returns reference to MainForm object based on REXExtensionForm.</returns>
        public override REXExtensionWindow GetWindow()
        {
            if (object.ReferenceEquals(WindowRef, null))
                WindowRef = new Resources.Dialogs.MainWindow(this);

            return (REXExtensionWindow)WindowRef;
        }

        /// <summary>
        /// Get the main control. Internally calls <see cref="MainControl()"/> method.
        /// </summary>
        /// <returns>Returns reference to MainControl object based on REXExtensionControl.</returns>
        public override REXExtensionControl GetControl()
        {
            return MainControl();
        }
        #endregion

        #region Public overrided data and results
        /// <summary>
        /// Get the data.
        /// </summary>
        /// <value>The data.</value>
        public Data Data
        {
            get
            {
                return DataRef;
            }
        }

        /// <summary>
        /// Get the results.
        /// </summary>
        /// <value>The results.</value>
        public Results Results
        {
            get
            {
                return ResultsRef;
            }
        }

        /// <summary>
        /// Get the API.
        /// </summary>
        /// <returns>Returns reference to main API object.</returns>
        public override Object GetAPI()
        {
            return null;
        }

        /// <summary>
        /// Get the results.
        /// </summary>
        /// <returns>Returns reference to object based on REXResults object.</returns>
        public override REXResults GetResults()
        {
            return ResultsRef;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>Returns reference to object based on REXData object.</returns>
        public override REXData GetData()
        {
            return DataRef;
        }

        /// <summary>
        /// Get the settings.
        /// </summary>
        /// <returns>Returns reference to object based on REXExtensionSettings object.</returns>
        public override REXExtensionSettings GetSettings()
        {
            if (ExtensionSettingsRef == null)
                ExtensionSettingsRef = new ExtensionSettings(this);
            return ExtensionSettingsRef;
        }
        #endregion

        //DOM-IGNORE-BEGIN
        #region Private members
        private Data DataRef;
        private Results ResultsRef;

        private Resources.Dialogs.MainWindow WindowRef;
        private Resources.Dialogs.MainControl ControlRef;

        private Resources.Dialogs.SelectedElementControl SelectedElementControlRef;
        private REX.ContentGeneratorWPF.Resources.ContentGeneratorReadControl CGReadControlRef;
        private REX.ContentGeneratorWPF.Resources.Dialogs.ContentGeneratorCreateControl CGCreateControlRef;

        private ExtensionSettings ExtensionSettingsRef;

        private IREXExtension ExtensionRef;
        #endregion

        #region public members
        public REX.ContentGenerator.Converters.REXFamilyConverter Converter { get; set; }
        #endregion
        //DOM-IGNORE-END

    }
}

