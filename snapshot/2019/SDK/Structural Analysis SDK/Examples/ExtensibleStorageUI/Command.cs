//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;
using  Autodesk.Revit.DB.ExtensibleStorage.Framework; 
using Autodesk.Revit.UI;
using ExtensibleStorageUI.Properties;
using Category = Autodesk.Revit.UI.ExtensibleStorage.Framework.Category;

namespace ExtensibleStorageUI
{
    /// <summary>
    /// For a selected element, this command will store data defined by user or read data if this data exist on element.
    /// This command will expose a serie of tabs. Data exposed for each tab are defined via associated schema   
    /// 2 objectives here:
    /// 1) Review all controls exposed by the Extensible Storage Framework
    /// 2) Learn how to save data inside element and later retreive them
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class Command : IExternalCommand, IServerUI
    {
        
       // A set of private members that will  be used as data source   
        private readonly List<double> doubleList = new List<double> {10.0, 20.0, 30.0, 40.0};
        private readonly List<Int16> int16List = new List<Int16> { 1, 2, 3, 4 };
        private readonly List<Int32> int32List = new List<Int32> { 1, 2, 3, 4 };
        private readonly List<string> stringList = new List<string> {"Choice 1", "Choice 2", "Choice 3", "Choice 4"};
        private readonly List<XYZ> xyzList = new List<XYZ> {new XYZ(1.0,1.0,1.0), new XYZ(2.0, 2.0, 2.0), new XYZ(3.0, 3.0, 3.0)};
        private readonly List<UV> uvList = new List<UV> { new UV(1.0, 1.0), new UV(2.0, 2.0), new UV(3.0, 3.0) };
        private readonly List<bool> boolList = new List<bool> { true, false };
        private readonly List<Guid> guidList = new List<Guid> { new Guid("6AED35BD-9143-4AAB-B568-7FC69C946824"), new Guid("F6F9D635-6AF3-4336-9D52-E734DFA9F97E"), new Guid("E72993A5-CDFE-4501-9A34-D3A6DA407CD6") };
        private List<RebarBarType> rebarList = null;

        // A set boolean    
        private bool isTabTextBoxSchemaExistsOnElement = false;
        private bool isTabGridSchemaExistsOnElement = false;
        private bool isTabKeySchemaExistsOnElement = false;
        private bool isTabNumericUpDownSchemaExistsOnElement = false;
        private bool isTabComboBoxSchemaExistsOnElement = false;
        private bool isTabListTextBoxSchemaExistsOnElement = false;
        private bool isTabEnumSchemaExistsOnElement = false;
        private bool isTabListSchemaExistsOnElement = false;
        private bool isTabCategorySchemaExistsOnElement = false;
        private bool isTabMiscellaniousSchemaExistsOnElement = false;

        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // One element should be selected and will be the active element 
            Document document = commandData.Application.ActiveUIDocument.Document;
            if (commandData.Application.ActiveUIDocument.Selection.Elements.Size != 1)
            {
                TaskDialog.Show("Error", "One element should be selected");
                return Result.Cancelled;
            }

            // Check if five rebars are inside the project
            ElementId elementId = (new FilteredElementCollector(document).OfClass(typeof(RebarBarType)).ToElementIds() as List<ElementId>)[4] as ElementId   ;
            if (elementId == null)
            {
                TaskDialog.Show("Error", "At least five rebars should on this project");
                return Result.Cancelled;
            }

            Element activeElement = null;
            foreach (Element element in commandData.Application.ActiveUIDocument.Selection.Elements)
                activeElement = element;

            //Create all schema instances associated to tabs instances with default constructor                       
            var instanceTabTextBox = new TabTextBoxSchema();
            var instanceTabGrid = new TabGridSchema();
            var instanceTabKey = new TabKeySchema();
            var instanceTabNumericUpDown = new TabNumericUpDownSchema();
            var instanceTabComboBox = new TabComboBoxSchema();
            var instanceTabListTextBox = new TabListTextBoxSchema(document);
            var instanceTabEnum = new TabEnumSchema();
            var instanceTabList = new TabListSchema();
            var instanceTabCategory = new TabCategorySchema();
            var instanceTabMiscellanious = new TabMiscellaniousSchema();


            // Data preparation 
            // initialization of element from the Revit project
            instanceTabComboBox.ComboBoxElementId = elementId; 
            instanceTabTextBox.TextBoxElementId = activeElement.Id;
  
            rebarList = new FilteredElementCollector(document).OfClass(typeof(RebarBarType)).ToElements().Cast<RebarBarType>().ToList();  

            // Check if we are in reading mode by quering active element  
            isTabTextBoxSchemaExistsOnElement = instanceTabTextBox.Exists(activeElement);
            isTabGridSchemaExistsOnElement = instanceTabGrid.Exists(activeElement);
            isTabKeySchemaExistsOnElement = instanceTabKey.Exists(activeElement);
            isTabNumericUpDownSchemaExistsOnElement = instanceTabNumericUpDown.Exists(activeElement);
            isTabComboBoxSchemaExistsOnElement = instanceTabComboBox.Exists(activeElement);
            isTabListTextBoxSchemaExistsOnElement = instanceTabListTextBox.Exists(activeElement);
            isTabEnumSchemaExistsOnElement = instanceTabEnum.Exists(activeElement);
            isTabListSchemaExistsOnElement = instanceTabList.Exists(activeElement);
            isTabCategorySchemaExistsOnElement = instanceTabCategory.Exists(activeElement);
            isTabMiscellaniousSchemaExistsOnElement = instanceTabMiscellanious.Exists(activeElement); 
        
            //Load data
            instanceTabTextBox.Load(activeElement);
            instanceTabGrid.Load(activeElement);
            instanceTabKey.Load(activeElement);   
            instanceTabNumericUpDown.Load(activeElement);
            instanceTabComboBox.Load(activeElement);
            instanceTabListTextBox.Load(activeElement);
            instanceTabEnum.Load(activeElement);
            instanceTabList.Load(activeElement);
            instanceTabCategory.Load(activeElement);
            instanceTabMiscellanious.Load(activeElement);

            //Create all layouts on schema
            Layout layoutTabTextBox = Layout.Build(typeof (TabTextBoxSchema), this);
            Layout layoutTabGrid = Layout.Build(typeof (TabGridSchema), this);
            Layout layoutTabKey = Layout.Build(typeof(TabKeySchema), this);
            Layout layoutTabNumericUpDown = Layout.Build(typeof (TabNumericUpDownSchema), this);
            Layout layoutTabComboBox = Layout.Build(typeof (TabComboBoxSchema), this);
            Layout layoutTabListTextBox = Layout.Build(typeof (TabListTextBoxSchema), this);
            Layout layoutTabEnum = Layout.Build(typeof (TabEnumSchema), this);
            Layout layoutTabList = Layout.Build(typeof (TabListSchema), this);
            Layout layoutTabCategory = Layout.Build(typeof (TabCategorySchema), this);
            Layout layoutTabMiscellanious = Layout.Build(typeof (TabMiscellaniousSchema), this);

            // add an image in line on the last tab
            var image = new Image 
                            {
                                Source = new Uri(@"pack://application:,,,/ExtensibleStorageUI;component/Images/bigImage1.png"),
                                Index = 1,
                                Key = "image1"
                            };
            
            layoutTabMiscellanious.Controls.Insert(0, image);
            
            // add a free text inline 
            var textBlock = new TextBlock
                            {
                                Text = "This a test of the textblock usage",
                                Index = 2,
                                Key = "text"
                            };
            layoutTabMiscellanious.Controls.Insert(1, textBlock);


            // build all layout objects 
            ILayoutControl layoutCtrTabTextBox = Layout.BuildControl(this, document, layoutTabTextBox, instanceTabTextBox.GetEntity());
            ILayoutControl layoutCtrTabGrid = Layout.BuildControl(this, document, layoutTabGrid, instanceTabGrid.GetEntity());
            ILayoutControl layoutCtrTabKey = Layout.BuildControl(this, document, layoutTabKey, instanceTabKey.GetEntity());
            ILayoutControl layoutCtrTabNumericUpDown = Layout.BuildControl(this, document, layoutTabNumericUpDown, instanceTabNumericUpDown.GetEntity());
            ILayoutControl layoutCtrTabComboBox = Layout.BuildControl(this, document, layoutTabComboBox, instanceTabComboBox.GetEntity());
            ILayoutControl layoutCtrTabListTextBox = Layout.BuildControl(this, document, layoutTabListTextBox, instanceTabListTextBox.GetEntity());
            ILayoutControl layoutCtrTabEnum = Layout.BuildControl(this, document, layoutTabEnum, instanceTabEnum.GetEntity());
            ILayoutControl layoutCtrTabList = Layout.BuildControl(this, document, layoutTabList, instanceTabList.GetEntity());
            ILayoutControl layoutCtrTabCategory = Layout.BuildControl(this, document, layoutTabCategory, instanceTabCategory.GetEntity());
            ILayoutControl layoutCtrTabMiscellanious = Layout.BuildControl(this, document, layoutTabMiscellanious, instanceTabMiscellanious);

            //create main window and add all object 
            var window = new MainWindows 
                            {
                                Name = "ExtensibleStorageUIOverview",
                                Title = "Extensible Storage UI Overview"
                            };
            var layout = new MainLayout
                            {
                                tabTextBox = {Content = layoutCtrTabTextBox},
                                tabGrid = {Content = layoutCtrTabGrid},
                                tabKey = { Content = layoutCtrTabKey },
                                tabNumericUpDown = {Content = layoutCtrTabNumericUpDown},
                                tabComboBox = {Content = layoutCtrTabComboBox},
                                tabListTextBox = {Content = layoutCtrTabListTextBox},
                                tabEnum = {Content = layoutCtrTabEnum},
                                tabList = {Content = layoutCtrTabList},
                                tabCategory = {Content = layoutCtrTabCategory},
                                tabMiscellanious = {Content = layoutCtrTabMiscellanious}
                            };
            window.layout.Children.Add(layout);
            window.ShowAndAssignParent();

            //Getting data from UI 
            instanceTabTextBox = new TabTextBoxSchema(layoutCtrTabTextBox.GetEntity(), document);
            instanceTabGrid = new TabGridSchema(layoutCtrTabGrid.GetEntity(), document);
            instanceTabKey = new TabKeySchema(layoutCtrTabKey.GetEntity(), document);
            instanceTabNumericUpDown = new TabNumericUpDownSchema(layoutCtrTabNumericUpDown.GetEntity(), document);
            instanceTabComboBox = new TabComboBoxSchema(layoutCtrTabComboBox.GetEntity(), document);
            instanceTabListTextBox = new TabListTextBoxSchema(layoutCtrTabListTextBox.GetEntity(), document);
            instanceTabEnum = new TabEnumSchema(layoutCtrTabEnum.GetEntity(), document);
            instanceTabList = new TabListSchema(layoutCtrTabList.GetEntity(), document);
            instanceTabCategory = new TabCategorySchema(layoutCtrTabCategory.GetEntity(), document);
            instanceTabMiscellanious = new TabMiscellaniousSchema(instanceTabMiscellanious.GetEntity(), document);
        
            //Saving Data into selected element 
            if (window.storeSchema)
            {
                    var t = new Transaction(document, "Save schemas");
                    t.Start();
                    instanceTabTextBox.Save(activeElement);
                    instanceTabGrid.Save(activeElement);
                    instanceTabKey.Save(activeElement);
                    instanceTabNumericUpDown.Save(activeElement);
                    instanceTabComboBox.Save(activeElement);
                    instanceTabListTextBox.Save(activeElement);
                    instanceTabEnum.Save(activeElement);
                    instanceTabList.Save(activeElement);
                    instanceTabCategory.Save(activeElement);
                    instanceTabMiscellanious.Save(activeElement);
                    t.Commit();
            }
            return Result.Succeeded;
        }
        #endregion

        #region IServerUI Members

       
        public IList GetDataSource(string key, Document document, DisplayUnitType unitType)
        {
            // switch to return proper data structure to controls
            switch (key)
            {
                case "DataSourceDoubleList":
                    return  UnitUtilsExt.Convert(this.doubleList, Autodesk.Revit.DB.DisplayUnitType.DUT_METERS, unitType); 
                case "DataSourceStringList":
                    return this.stringList;
                case "DataSourceInt16List":
                    return this.int16List;
                case "DataSourceInt32List":
                    return this.int32List;
                case "DataSourceBoolList":
                    return this.boolList;
                case "DataSourceUVList":
                    return this.uvList; 
                case "DataSourceGuidList":
                    return this.guidList;
                case "DataSourceRebarList":
                    return new FilteredElementCollector(document).OfClass(typeof(RebarBarType)).ToElements().Cast<RebarBarType>().ToList();
                case "DataSourceElementIdList":
                    return (new FilteredElementCollector(document).OfClass(typeof(RebarBarType)).ToElementIds() as List <ElementId>).GetRange(0,5);
                case "DataSourceXYZList":
                    return this.xyzList; 
                default:
                    return null;
            }
        }

        public string GetResource(string key, string context)
        {
            string txt = Resources.ResourceManager.GetString(key);
            if (!string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            return key;
        }

        public Uri GetResourceImage(string key, string context)
        {
            
            // Enum images
            if (key == EnumLocalized.Choice1.ToString() || key == EnumNotLocalized.Item1.ToString()  )
                return new Uri(@"pack://application:,,,/ExtensibleStorageUI;component/Images/smallImage1.png");
            else if (key == EnumLocalized.Choice2.ToString() || key == EnumNotLocalized.Item2.ToString())
                return new Uri(@"pack://application:,,,/ExtensibleStorageUI;component/Images/smallImage2.png");
            else if (key == EnumLocalized.Choice3.ToString() || key == EnumNotLocalized.Item3.ToString())
                return new Uri(@"pack://application:,,,/ExtensibleStorageUI;component/Images/smallImage3.png");
            return null;
        }

        void IServerUI.LayoutInitialized(object sender, LayoutInitializedEventArgs e)
        {
            var entity = e.Entity as Entity;
            if (entity == null) return; 
            else if (entity.Schema.SchemaName == "tabComboBoxSchema" && !isTabComboBoxSchemaExistsOnElement)
            {
                e.Editor.SetValue("ComboBoxUV", uvList[1], DisplayUnitType.DUT_METERS);
                e.Editor.SetValue("UnitComboBoxDoubleConstructor", 15, DisplayUnitType.DUT_METERS);
                e.Editor.SetValue("ComboBoxXYZ", xyzList[2], DisplayUnitType.DUT_METERS);
                e.Editor.SetValue("ComboBoxRebar", rebarList[2], DisplayUnitType.DUT_UNDEFINED);
            }
            else if (entity.Schema.SchemaName == "tabComboBoxSchema" && isTabComboBoxSchemaExistsOnElement)
            {
                var tab = new TabComboBoxSchema(entity, null);
                e.Editor.SetValue("ComboBoxUV", tab.ComboBoxUV, DisplayUnitType.DUT_METERS);
                e.Editor.SetValue("ComboBoxXYZ", tab.ComboBoxXYZ, DisplayUnitType.DUT_METERS);
                e.Editor.SetValue("UnitComboBoxDoubleConstructor", tab.UnitComboBoxDoubleConstructor, DisplayUnitType.DUT_METERS);
            }  
        }

        void IServerUI.ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

      
     
    }
}