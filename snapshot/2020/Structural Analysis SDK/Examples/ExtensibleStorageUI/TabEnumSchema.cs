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

using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Attributes;
using Autodesk.Revit.UI.ExtensibleStorage.Framework.Attributes;

namespace ExtensibleStorageUI
{
    /// <summary>
    /// 
    /// </summary>
    [Schema("tabEnumSchema", "effcafc6-a56d-4090-9a8c-c3e8a04c3ef0")]
    public class TabEnumSchema : SchemaClass
    {
        # region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TabEnumSchema()
        {
            OptionListEnumImage = new List<EnumLocalized>();
            OptionListEnumText = new List<EnumLocalized>();
            OptionListEnumImageText = new List<EnumLocalized>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public TabEnumSchema(Document document)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="document"></param>
        public TabEnumSchema(Entity entity, Document document)
            : base(entity, document)
        {
        }
        #endregion Constructors

        # region ToggleButton
        /// <summary>
        /// EnumControl supporting  strings values and images presented as ToggleButton.
        /// Strings associated to AnEnumLocalized.Choice1, AnEnumLocalized.Choice2, AnEnumLocalized.Choice3 are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.ToggleButton,
             EnumType = typeof(EnumLocalized), 
            ImageSize = Autodesk.Revit.UI.ExtensibleStorage.Framework.ImageSize.Small ,        
            Item = PresentationItem.ImageWithText,
            Category = "ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "ToggleButtonEnumImageText",
            Tooltip = "ToggleButtonEnumImageTextToolTips"

            )]
        public EnumLocalized ToggleButtonEnumImageText { get; set; }

        /// <summary>
        /// EnumControl supporting  images presented as ToggleButton.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.ToggleButton,
             EnumType = typeof(EnumLocalized), 
            Item = PresentationItem.Image,
            ImageSize = Autodesk.Revit.UI.ExtensibleStorage.Framework.ImageSize.Medium ,   
            Category = "ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "ToggleButtonEnumImage",
            Tooltip = "ToggleButtonEnumImageToolTips"
            )]
        public EnumLocalized ToggleButtonEnumImage { get; set; }

        /// <summary>
        /// EnumControl supporting  strings values presented as ToggleButton.
        /// Strings associated to [Choice1,Choice2,Choice3] are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.ToggleButton,
            EnumType = typeof(EnumLocalized), 
            Item = PresentationItem.Text,
            ImageSize = Autodesk.Revit.UI.ExtensibleStorage.Framework.ImageSize.Small,
            Category = "ToggleButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "ToggleButtonEnumText",
            Tooltip = "ToggleButtonEnumTextToolTips"
            )]
        public EnumLocalized ToggleButtonEnumText { get; set; }

        # endregion ToggleButton

        # region OptionList

        /// <summary>
        /// EnumControl supporting  strings values presented as OptionList(List).
        /// Strings associated to [Choice1,Choice2,Choice3] are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.Text,
             EnumType = typeof(EnumLocalized), 
            Category = "OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 0,
            Localizable = true,
            Description = "OptionListEnumText",
            Tooltip = "OptionListEnumTextToolTips"
            )]
        public List<EnumLocalized> OptionListEnumText { get; set; }

        /// <summary>
        /// EnumControl supporting  strings values and images presented as OptionList(List).
        /// Strings associated to AnEnumLocalized.Choice1, AnEnumLocalized.Choice2, AnEnumLocalized.Choice3 are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.ImageWithText,
             EnumType = typeof(EnumLocalized), 
            Category = "OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 1,
            Localizable = true,
            Description = "OptionListEnumImageText",
            Tooltip = "OptionListEnumImageTextToolTips"
            )]
        public List<EnumLocalized> OptionListEnumImageText { get; set; }

        /// <summary>
        /// EnumControl supporting  images presented as OptionList(List).
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.Image,
             EnumType = typeof(EnumLocalized), 
            Category = "OptionList",
            IsVisible = true,
            IsEnabled = true,
            Index = 2,
            Localizable = true,
            Description = "OptionListEnumImage",
            Tooltip = "OptionListEnumImageToolTips"
            )]
        public List<EnumLocalized> OptionListEnumImage { get; set; }

        #endregion OptionList

        #region RadioButton
        /// <summary>
        /// EnumControl supporting  strings values and images presented as OptionList(RadioButton).
        /// Strings associated to AnEnumLocalized.Choice1, AnEnumLocalized.Choice2, AnEnumLocalized.Choice3 are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.ImageWithText,
             EnumType = typeof(EnumLocalized), 
            Category = "RadioButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "RadioButtonEnumImageText",
            Tooltip = "RadioButtonEnumImageTextToolTips"
            )]
        public EnumLocalized RadioButtonEnumImageText { get; set; }

        /// <summary>
        /// EnumControl supporting  images presented as OptionList(RadioButton).
        /// Uri for associated images are returned by the GetResourceImage(string key) function.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.Image,
             EnumType = typeof(EnumLocalized), 
            Category = "RadioButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "RadioButtonEnumImage",
            Tooltip = "RadioButtonEnumImageToolTips"
            )]
        public EnumLocalized RadioButtonEnumImage { get; set; }

        /// <summary>
        /// EnumControl supporting  strings values presented as OptionList(RadioButton).
        /// Strings associated to [Choice1,Choice2,Choice3] are part of the resources file.
        /// On the UI are visible strings "This is my choice 1","This is my choice 2","This is my choice 3"
        /// Enum field is stored as integer with default enumerator values.
        /// </summary>
        [SchemaProperty]
        [EnumControl(
            Presentation = PresentationMode.OptionList,
            Item = PresentationItem.Text,
             EnumType = typeof(EnumLocalized), 
            Category = "RadioButton",
            IsVisible = true,
            IsEnabled = true,
            Index = -1,
            Localizable = true,
            Description = "RadioButtonEnumText",
            Tooltip = "RadioButtonEnumTextToolTips"
            )]
        public EnumLocalized RadioButtonEnumText { get; set; }

        # endregion RadioButton 

      
    }
}