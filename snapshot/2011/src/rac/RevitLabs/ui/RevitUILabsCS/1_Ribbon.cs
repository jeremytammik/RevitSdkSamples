#region Copyright
//
// (C) Copyright 2010 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Migrated to C# by Saikat Bhattacharya
// 
#endregion // Copyright

#region "Imports"

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes; // specify this if you want to save typing for attributes. e.g.
using System.Windows.Media.Imaging;


#endregion

namespace RevitUILabsCS
{
    //' Ribbon UI 
    //' 
    //' we'll be using commands we defined in Revit Intro labs. alternatively, 
    //' you can use your own. Any command will do for this ribbon exercise. 
    //' cf. Developer Guide, Section 3.8: Ribbon Panels and Controls. (pp 46). 
    //' 

    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIRibbon : IExternalApplication
    {

        //' member variables/constant for this class. 
        //' location of managed dll where we have defined the commands. and location of images for icons. 

        String m_assembly = @"D:\RevitAPI 2011 Training\Labs\Revit UI Labs\bin\Debug\RevitIntroVB.dll";
        String m_imageFolder = @"D:\RevitAPI 2011 Training\Labs\Revit UI Labs\bin\Debug\Images\";

        //String m_assembly = String.Empty; 
        //String m_imageFolder = string.Empty;

        //' OnShutdown() - called when Revit ends. 
        //' 
        public Result OnShutdown(UIControlledApplication application)
        {


            return Result.Succeeded;
        }

        //' OnStartup() - called when Revit starts. 
        //' 
        public Result OnStartup(UIControlledApplication application)
        {

            //' show what kinds of cutom buttons and controls we can add to the Add-Ins tab 
            AddRibbonSampler(application);

            //' add one for UI Labs, too. 
            AddUILabsButtons(application);


            return Result.Succeeded;
        }

        //' create our own ribbon panel with verious buttons for our exercise 
        //' we'll re-use commands we defined in our Revit Intro Labs here 
        //' cf. Section 3.8 (pp 46) of Developer Guide. 
        //' 
        public void AddRibbonSampler(UIControlledApplication app)
        {

            //m_assembly = this.GetType().Assembly.Location;
            //m_imageFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(m_assembly), "Images\\");

            //' (1) create a ribbon panel 
            //' 
            RibbonPanel panel = app.CreateRibbonPanel("Ribbon Sampler");

            //' 
            //' below are samplers of ribbon items. uncomment functions of your interest to see how it looks like 
            //' 

            //' (2.1) add a simple push button for Hello World 
            AddPushButton(panel);

            //' (2.2) add split buttons for "Command Data", "DB Element" and "Element Filtering" 
            AddSplitButton(panel);

            //' (2.3) add pulldown buttons for "Command Data", "DB Element" and "Element Filtering" 
            AddPulldownButton(panel);

            AddComboBox(panel);

            //' (2.7) stacked items - 1. hello world push button, 2. pulldown element bscis (command data, DB element, element filtering) 
            //' 3. pulldown modification/creation(element modification, model creation). 
            //' 
            AddStackedButtons_Complex(panel);

            //' 
            //' (2.8) slide out - if you don't have enough space, you can add additional space below the panel. 
            //' anything which comes after this will be on the slide out. 
            panel.AddSlideOut();

            //' (2.4) radio button - what it is 
            AddRadioButton(panel);
            //' (2.5) text box - what it is 

            AddTextBox(panel);
        }

        //' we create our own buttons for UI Labs, too. 
        //' cf. Section 3.8 (pp 46) of Developer Guide. 
        //' 
        public void AddUILabsButtons(UIControlledApplication app)
        {

            //' create a ribbon panel 
            //' 
            RibbonPanel panel = app.CreateRibbonPanel("UI Labs");

            //' (3) adding buttons for the current labs itself. 
            //' you may modify this AFTER each command are defined in each lab. 
            //AddUILabsCommandButtons_Template(panel) '' dummy 
            AddUILabsCommandButtons(panel);

            //' (4) This is for Lab4 event and dynamic update. 

            AddUILabsCommandButtons2(panel);
        }


        //' simple push button for "Hello World" 
        //' 
        public void AddPushButton(RibbonPanel panel)
        {

            //' set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataHello = new PushButtonData("PushButtonHello", "Hello World", m_assembly, "RevitIntroVB.HelloWorld");
            //' add a button to the panel 
            PushButton pushButtonHello = panel.AddItem(pushButtonDataHello) as PushButton;
            //' add an icon 
            //' make sure you references to WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            pushButtonHello.LargeImage = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorld.png"));
            //' add a tooltip 

            pushButtonHello.ToolTip = "simple push button";
        }

        //' split button for "Command Data", "DB Element" and "Element Filtering" 
        //' 
        public void AddSplitButton(RibbonPanel panel)
        {

            //' create three push buttons for split button drop down 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("SplitCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData");
            pushButtonData1.LargeImage = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorld.png"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("SplitDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement");
            pushButtonData2.LargeImage = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorld.png"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("SplitElementFiltering", "ElementFiltering", m_assembly, "RevitIntroVB.ElementFiltering");
            pushButtonData3.LargeImage = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorld.png"));

            //' make a split button now 
            SplitButtonData splitBtnData = new SplitButtonData("SplitButton", "Split Button");
            SplitButton splitBtn = panel.AddItem(splitBtnData) as SplitButton;
            splitBtn.AddPushButton(pushButtonData1);
            splitBtn.AddPushButton(pushButtonData2);

            splitBtn.AddPushButton(pushButtonData3);
        }

        //' pulldown button for "Command Data", "DB Element" and "Element Filtering" 
        //' 
        public void AddPulldownButton(RibbonPanel panel)
        {

            //' create three push buttons for pulldown button drop down 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("PulldownCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData");
            pushButtonData1.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("PulldownDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement");
            pushButtonData2.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("PulldownElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering");
            pushButtonData3.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' make a pulldown button now 
            PulldownButtonData pulldownBtnData = new PulldownButtonData("PulldownButton", "Pulldown");
            PulldownButton pulldownBtn = panel.AddItem(pulldownBtnData) as PulldownButton;
            pulldownBtn.AddPushButton(pushButtonData1);
            pulldownBtn.AddPushButton(pushButtonData2);

            pulldownBtn.AddPushButton(pushButtonData3);
        }

        //' radio/toggle button for "Command Data", "DB Element" and "Element Filtering" 
        //' 
        public void AddRadioButton(RibbonPanel panel)
        {

            //' create three toggle buttons for radio button group 
            //' #1 
            ToggleButtonData toggleButtonData1 = new ToggleButtonData("RadioCommandData", "Command" + "\n Data", m_assembly, "RevitIntroVB.CommandData");
            toggleButtonData1.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #2 
            ToggleButtonData toggleButtonData2 = new ToggleButtonData("RadioDBElement", "DB" + "\n Element", m_assembly, "RevitIntroVB.DBElement");
            toggleButtonData2.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #3 
            ToggleButtonData toggleButtonData3 = new ToggleButtonData("RadioElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering");
            toggleButtonData3.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' make a radio button group now 
            RadioButtonGroupData radioBtnGroupData = new RadioButtonGroupData("RadioButton");
            RadioButtonGroup radioBtnGroup = panel.AddItem(radioBtnGroupData) as RadioButtonGroup;
            radioBtnGroup.AddItem(toggleButtonData1);
            radioBtnGroup.AddItem(toggleButtonData2);

            radioBtnGroup.AddItem(toggleButtonData3);
        }

        //' text box 
        //' text box i used in conjunction with event. we'll come to this later. 
        //' for now, just shows how to make a text box. 
        //' 
        public void AddTextBox(RibbonPanel panel)
        {

            //' fill the text gox information 
            TextBoxData txtBoxData = new TextBoxData("TextBox");
            txtBoxData.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            txtBoxData.Name = "Text Box";
            txtBoxData.ToolTip = "Enter text here";
            txtBoxData.LongDescription = "<p>This is Revit UI Labs.</p><p>Ribbon Lab</p>";
            txtBoxData.ToolTipImage = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorld.png"));

            //' create the text box item on the panel 
            TextBox txtBox = panel.AddItem(txtBoxData) as TextBox;
            txtBox.PromptText = "Enter a comment";
            txtBox.ShowImageAsButton = true;

            txtBox.EnterPressed += new EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(txtBox_EnterPressed);
            txtBox.Width = 180;
        }

        void txtBox_EnterPressed(object sender, Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs e)
        {
            // cast sender as TextBox to retrieve text value
            TextBox textBox = sender as TextBox;
            TaskDialog.Show("TextBox Input", "This is what you typed in: " + textBox.Value.ToString());
        }

        //' combo box - 5 items in 2 groups. 
        //' combo box is used in conjunction with event. we'll come back later. 
        //' for now, just demonstrates how to make a combo box. 
        //' 
        public void AddComboBox(RibbonPanel panel)
        {

            //' create five combo box members with two groups 
            //' #1 
            ComboBoxMemberData comboBoxMemberData1 = new ComboBoxMemberData("ComboCommandData", "Command Data");
            comboBoxMemberData1.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            comboBoxMemberData1.GroupName = "DB Basics";

            //' #2 
            ComboBoxMemberData comboBoxMemberData2 = new ComboBoxMemberData("ComboDBElement", "DB Element");
            comboBoxMemberData2.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            comboBoxMemberData2.GroupName = "DB Basics";

            //' #3 
            ComboBoxMemberData comboBoxMemberData3 = new ComboBoxMemberData("ComboElementFiltering", "Filtering");
            comboBoxMemberData3.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            comboBoxMemberData3.GroupName = "DB Basics";

            //' #4 
            ComboBoxMemberData comboBoxMemberData4 = new ComboBoxMemberData("ComboElementModification", "Modify");
            comboBoxMemberData4.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            comboBoxMemberData4.GroupName = "Modeling";

            //' #5 
            ComboBoxMemberData comboBoxMemberData5 = new ComboBoxMemberData("ComboModelCreation", "Create");
            comboBoxMemberData5.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            comboBoxMemberData5.GroupName = "Modeling";


            //' make a radio button group now 
            ComboBoxData comboBxData = new ComboBoxData("ComboBox");
            ComboBox comboBx = panel.AddItem(comboBxData) as ComboBox;
            comboBx.ToolTip = "Select an Option";
            comboBx.LongDescription = "select a command you want to run";
            comboBx.AddItem(comboBoxMemberData1);
            comboBx.AddItem(comboBoxMemberData2);
            comboBx.AddItem(comboBoxMemberData3);
            comboBx.AddItem(comboBoxMemberData4);

            comboBx.AddItem(comboBoxMemberData5);

            comboBx.CurrentChanged += new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(comboBx_CurrentChanged);
        }

        void comboBx_CurrentChanged(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        {
            // cast sender as TextBox to retrieve text value
            ComboBox combodata = sender as ComboBox;
            ComboBoxMember member = combodata.Current;
            TaskDialog.Show("Combobox Selection", "Your new selection: " + member.ItemText);
        }

        //' stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
        //' (no radio button group, split buttons). 
        //' here we stack three push buttons for "Command Data", "DB Element" and "Element Filtering". 
        //' 
        public void AddStackedButtons_Simple(RibbonPanel panel)
        {

            //' create three push buttons to stack up 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("StackSimpleCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData");
            pushButtonData1.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("StackSimpleDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement");
            pushButtonData2.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("StackSimpleElementFiltering", "Element Filtering", m_assembly, "RevitIntroVB.ElementFiltering");
            pushButtonData3.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));


            //' put them on stack 
            IList<RibbonItem> stackedButtons = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3);
        }

        //' stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
        //' (no radio button group, split buttons). 
        //' here we define 6 buttons, make grouping of 1, 3, 2 items, and stack them in three layer: 
        //' (1) simple push button with "Hello World" 
        //' (2) pull down with 3 items: "Command Data", "DB Element" and "Element Filtering". 
        //' (3) pull down with 2 items: "Element Modification" and "Model Creation" 
        //' 
        public void AddStackedButtons_Complex(RibbonPanel panel)
        {

            //' create six push buttons to group for pull down and stack up 

            //' #0 
            PushButtonData pushButtonData0 = new PushButtonData("StackComplexHelloWorld", "Hello World", m_assembly, "RevitIntroVB.HelloWorld");
            pushButtonData0.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("StackComplexCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData");
            pushButtonData1.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("StackComplexDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement");

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("StackComplexElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering");

            //' #4 
            PushButtonData pushButtonData4 = new PushButtonData("StackComplexElementModification", "Modify", m_assembly, "RevitIntroVB.ElementModification");

            //' #5 
            PushButtonData pushButtonData5 = new PushButtonData("StackComplexModelCreation", "Create", m_assembly, "RevitIntroVB.ModelCreation");

            //' make two sets of pull down 
            //' 
            PulldownButtonData pulldownBtnData1 = new PulldownButtonData("StackComplePulldownButton1", "DB Basics");
            PulldownButtonData pulldownBtnData2 = new PulldownButtonData("StackComplePulldownButton2", "Modeling");

            //' create three item stack. 
            IList<RibbonItem> stackedItems = panel.AddStackedItems(pushButtonData0, pulldownBtnData1, pulldownBtnData2);
            PulldownButton pulldownBtn2 = stackedItems[1] as PulldownButton;
            PulldownButton pulldownBtn3 = stackedItems[2] as PulldownButton;

            pulldownBtn2.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            pulldownBtn3.Image = new BitmapImage(new Uri(m_imageFolder + "House.ico"));

            //' add each sub items 
            PushButton button1 = pulldownBtn2.AddPushButton(pushButtonData1);
            PushButton button2 = pulldownBtn2.AddPushButton(pushButtonData2);
            PushButton button3 = pulldownBtn2.AddPushButton(pushButtonData3);
            PushButton button4 = pulldownBtn3.AddPushButton(pushButtonData4);
            PushButton button5 = pulldownBtn3.AddPushButton(pushButtonData5);

            //' note: we need to set the image later. if we do in button bata, it won't show in the Ribbon. 
            button1.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            button2.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            button3.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            button4.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            button5.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
        }

        //' add buttons for the commands we define in this labs. 
        //' here we stack three push buttons and repeat it as we get more. 
        //' this is a template to use during the Ribbon lab exercise prior to going to following labs. 
        //' 
        public void AddUILabsCommandButtons_Template(RibbonPanel panel)
        {

            //' get the location of this dll. 
            string assembly = GetType().Assembly.Location;

            //' create three push buttons to stack up 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("UILabsCommand1", "Command1", assembly, "RevitUILabsVB.Command1");
            pushButtonData1.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("UILabsCommand2", "Command2", assembly, "RevitUILabsVB.Command2");
            pushButtonData2.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("UILabsCommand3", "Command3", assembly, "RevitUILabsVB.Command3");
            pushButtonData3.Image = new BitmapImage(new Uri(m_imageFolder + "ImgHelloWorldSmall.png"));


            //' put them on stack 

            IList<RibbonItem> stackedButtons = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3);
        }

        //' add buttons for the commands we define in this labs. 
        //' here we stack three push buttons and repeat it as we get more. 
        //' 
        public void AddUILabsCommandButtons(RibbonPanel panel)
        {

            //' get the location of this dll. 
            string assembly = GetType().Assembly.Location;

            //' create three push buttons to stack up 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("UILabsSelection", "Pick Sampler", assembly, "RevitUILabsCS.UISelection");
            pushButtonData1.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("UILabsCreateHouseUI", "Create House Pick", assembly, "RevitUILabsCS.CreateHouseUI");
            pushButtonData2.Image = new BitmapImage(new Uri(m_imageFolder + "House.ico"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, "RevitUILabsCS.UITaskDialog");
            pushButtonData3.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));

            //' #4 
            PushButtonData pushButtonData4 = new PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, "RevitUILabsCS.CreateHouseDialog");
            pushButtonData4.Image = new BitmapImage(new Uri(m_imageFolder + "House.ico"));

            //' #5 
            //' make three sets of pull down 
            //' 
            PulldownButtonData pulldownBtnData1 = new PulldownButtonData("UILabsPulldownButton1", "Selection");
            PulldownButtonData pulldownBtnData2 = new PulldownButtonData("UILabsPulldownButton2", "Task Dialog");

            //' create three item stack. 
            IList<RibbonItem> stackedItems = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2);
            PulldownButton pulldownBtn1 = stackedItems[0] as PulldownButton;
            PulldownButton pulldownBtn2 = stackedItems[1] as PulldownButton;

            pulldownBtn1.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            pulldownBtn2.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' add each sub items 
            PushButton button1 = pulldownBtn1.AddPushButton(pushButtonData1);
            PushButton button2 = pulldownBtn1.AddPushButton(pushButtonData2);
            PushButton button3 = pulldownBtn2.AddPushButton(pushButtonData3);
            PushButton button4 = pulldownBtn2.AddPushButton(pushButtonData4);

            //' note: we need to set the image later. if we do in button bata, it won't show in the Ribbon. 
            button1.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            button2.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
            button3.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            button4.Image = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));
        }

        //' add buttons for the commands we define in this labs. 
        //' here we stack 2 x 2-push buttons and repeat it as we get more. 
        //' TBD: still thinking which version is better ... 
        //' 
        public void AddUILabsCommandButtons_v2(RibbonPanel panel)
        {

            //' get the location of this dll. 
            string assembly = GetType().Assembly.Location;

            //' create push buttons to stack up 
            //' #1 
            PushButtonData pushButtonData1 = new PushButtonData("UILabsSelection", "Pick Sampler", assembly, "RevitUILabsCS.UISelection");
            pushButtonData1.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));

            //' #2 
            PushButtonData pushButtonData2 = new PushButtonData("UILabsCreateHouseUI", "Create House Pick", assembly, "RevitUILabsCS.CreateHouseUI");
            pushButtonData2.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));

            //' #3 
            PushButtonData pushButtonData3 = new PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, "RevitUILabsCS.UITaskDialog");
            pushButtonData3.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));

            //' #4 
            PushButtonData pushButtonData4 = new PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, "RevitUILabsCS.CreateHouseDialog");
            pushButtonData4.Image = new BitmapImage(new Uri(m_imageFolder + "basics.ico"));
            
            //' create 2 x 2-item stack. 
            //Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2, pulldownBtnData3) 
            IList<RibbonItem> stackedItems1 = panel.AddStackedItems(pushButtonData1, pushButtonData2);

            IList<RibbonItem> stackedItems2 = panel.AddStackedItems(pushButtonData3, pushButtonData4);
        }

        //' control buttons for Event and Dynamic Model Update 
        //' 
        public void AddUILabsCommandButtons2(RibbonPanel panel)
        {

            //' get the location of this dll. 
            string assembly = GetType().Assembly.Location;

            //' create three toggle buttons for radio button group 
            //' #1 
            ToggleButtonData toggleButtonData1 = new ToggleButtonData("UILabsEventOn", "Event" + "\n Off", assembly, "RevitUILabsCS.UIEventOff");
            toggleButtonData1.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' #2 
            ToggleButtonData toggleButtonData2 = new ToggleButtonData("UILabsEventOff", "Event" + "\n On", assembly, "RevitUILabsCS.UIEventOn");
            toggleButtonData2.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Basics.ico"));

            //' create three toggle buttons for radio button group 
            //' #3 
            ToggleButtonData toggleButtonData3 = new ToggleButtonData("UILabsDynUpdateOn", "Center" + "\n Off", assembly, "RevitUILabsCS.UIDynamicModelUpdateOff");
            toggleButtonData3.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Families.ico"));

            //' #4 
            ToggleButtonData toggleButtonData4 = new ToggleButtonData("UILabsDynUpdateOff", "Center" + "\n On", assembly, "RevitUILabsCS.UIDynamicModelUpdateOn");
            toggleButtonData4.LargeImage = new BitmapImage(new Uri(m_imageFolder + "Families.ico"));

            //' make event pn/off radio button group 
            RadioButtonGroupData radioBtnGroupData1 = new RadioButtonGroupData("EventNotification");
            RadioButtonGroup radioBtnGroup1 = panel.AddItem(radioBtnGroupData1) as RadioButtonGroup;
            radioBtnGroup1.AddItem(toggleButtonData1);
            radioBtnGroup1.AddItem(toggleButtonData2);

            //' make dyn update on/off radio button group 
            RadioButtonGroupData radioBtnGroupData2 = new RadioButtonGroupData("WindowDoorCenter");
            RadioButtonGroup radioBtnGroup2 = panel.AddItem(radioBtnGroupData2) as RadioButtonGroup;
            radioBtnGroup2.AddItem(toggleButtonData3);

            radioBtnGroup2.AddItem(toggleButtonData4);
        }

    }

    #region "Helper Classes"
    //'============================================================================= 
    //' Helper Classes 
    //'============================================================================= 
    //' 
    //' This lab uses Revit Intro Labs. If you prefer to use a dummy command, you can do so. 
    //' Providing a command template here. 
    //' 
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    public class DummyCommand1 : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //' write your command here 
            TaskDialog.Show("Revit UI Labs", "You have called Command1");

            return Result.Succeeded;
        }

    }

    #endregion 
}
