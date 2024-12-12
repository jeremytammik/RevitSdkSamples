#Region "Imports"
'' Import the following name spaces in the project properties/references. 
'' Note: VB.NET has a slighly different way of recognizing name spaces than C#. 
'' if you explicitely set them in each .vb file, you will need to specify full name spaces. 

''  Additionl references needed to UI Labs - Ribbon:
''    - WindowsBase 
''    - PresentationCore 
''    - RevitIntroVB or RevitIntroCS (From our Revit Intro Labs) 

'Imports System
'Imports System.Windows.Media.Imaging  '' for bitmap images. you will need to reference WindowsBase and PresentationCore  
'Imports Autodesk.Revit.DB
'Imports Autodesk.Revit.UI
'Imports Autodesk.Revit.ApplicationServices  '' Application class
'Imports Autodesk.Revit.Attributes '' specific this if you want to save typing for attributes. e.g., 
'Imports Autodesk.Revit.UI.Selection '' for selection 
Imports RevitIntroVB '' we'll be using commands we defined in Revit Intro labs. alternatively, you can use your own. Any command will do for this ribbon exercise. 

#End Region


''  Ribbon UI 
''
''  we'll be using commands we defined in Revit Intro labs. alternatively, 
''  you can use your own. Any command will do for this ribbon exercise. 
''  cf. Developer Guide, Section 3.8: Ribbon Panels and Controls. (pp 46). 
'' 

<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class UIRibbon
    Implements IExternalApplication

    ''  member variables/constant for this class. 
    ''  location of managed dll where we have defined the commands. and location of images for icons. 
    Const m_assembly As String = "D:\RevitAPI 2011\Training\Labs\Revit UI Labs\RevitUILabs\RevitIntroVB\bin\Debug\RevitIntroVB.dll"
    Const m_imageFolder As String = "D:\RevitAPI 2011\Training\Labs\Revit UI Labs\RevitUILabs\RevitUILabsVB\Images\"

    ''  OnShutdown() - called when Revit ends. 
    '' 
    Public Function OnShutdown(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnShutdown

        Return Result.Succeeded

    End Function

    ''  OnStartup() - called when Revit starts. 
    '' 
    Public Function OnStartup(ByVal application As UIControlledApplication) As Result _
        Implements IExternalApplication.OnStartup

        ''  show what kinds of cutom buttons and controls we can add to the Add-Ins tab 
        AddRibbonSampler(application)

        ''  add one for UI Labs, too. 
        AddUILabsButtons(application)

        Return Result.Succeeded

    End Function

    ''  create our own ribbon panel with verious buttons for our exercise 
    ''  we'll re-use commands we defined in our Revit Intro Labs here 
    ''  cf. Section 3.8 (pp 46) of Developer Guide. 
    ''
    Sub AddRibbonSampler(ByVal app As UIControlledApplication)

        ''  (1) create a ribbon panel 
        '' 
        Dim panel As RibbonPanel = app.CreateRibbonPanel("Ribbon Sampler")

        ''
        ''  below are samplers of ribbon items. uncomment functions of your interest to see how it looks like  
        ''

        ''  (2.1) add a simple push button for Hello World 
        AddPushButton(panel)
        'panel.AddSeparator()

        ''  (2.2) add split buttons for "Command Data", "DB Element" and "Element Filtering"
        AddSplitButton(panel)
        'panel.AddSeparator()

        ''  (2.3) add pulldown buttons for "Command Data", "DB Element" and "Element Filtering"
        AddPulldownButton(panel)
        'panel.AddSeparator()

        ''  (2.4) add radio/toggle buttons for "Command Data", "DB Element" and "Element Filtering" 
        ''  we put it on the slide-out below. 
        'AddRadioButton(panel)
        'panel.AddSeparator()

        ''  (2.5) add text box - TBD: this is used with the conjunction with event. Probably too complex for day one training. 
        ''   for now, without event. 
        ''  we put it on the slide-out below. 
        'AddTextBox(panel)
        'panel.AddSeparator()

        ''  (2.6) combo box - TBD: this is used with the conjunction with event. Probably too complex for day one training. 
        ''   for now, without event. show two groups: Element Bascis (3 push buttons) and Modification/Creation (2 push button)  
        AddComboBox(panel)
        'panel.AddSeparator()

        ''  (2.7) stacked items - 1. hello world push button, 2. pulldown element bscis (command data, DB element, element filtering)
        ''  3. pulldown modification/creation(element modification, model creation). 
        '' 
        'AddStackedButtons_Simple(panel) '' simple push button stack. 
        'panel.AddSeparator()
        AddStackedButtons_Complex(panel)
        'panel.AddSeparator()

        ''
        ''  (2.8) slide out - if you don't have enough space, you can add additional space below the panel. 
        ''   anything which comes after this will be on the slide out. 
        panel.AddSlideOut()
        'AddSlideOut(panel)
        'panel.AddSeparator()

        ''  (2.4) radio button - what it is 
        AddRadioButton(panel)
        ''  (2.5) text box - what it is 
        AddTextBox(panel)

    End Sub

    ''  we create our own buttons for UI Labs, too.  
    ''  cf. Section 3.8 (pp 46) of Developer Guide. 
    ''
    Sub AddUILabsButtons(ByVal app As UIControlledApplication)

        ''  create a ribbon panel 
        '' 
        Dim panel As RibbonPanel = app.CreateRibbonPanel("UI Labs")

        ''  (3) adding buttons for the current labs itself. 
        ''      you may modify this AFTER each command are defined in each lab. 
        'AddUILabsCommandButtons_Template(panel) '' dummy 
        AddUILabsCommandButtons(Panel) '' after subsequence labs are done 

        ''  (4) This is for Lab4 event and dynamic update.  
        AddUILabsCommandButtons2(Panel)

    End Sub


    ''  simple push button for "Hello World" 
    '' 
    Sub AddPushButton(ByVal panel As RibbonPanel)

        ''  set the information about the command we will be assigning to the button 
        Dim pushButtonDataHello As New PushButtonData("PushButtonHello", "Hello World", m_assembly, "RevitIntroVB.HelloWorld")
        ''  add a button to the panel 
        Dim pushButtonHello As PushButton = panel.AddItem(pushButtonDataHello)
        ''  add an icon 
        ''  make sure you references to WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace.
        pushButtonHello.LargeImage = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorld.png"))
        ''  add a tooltip
        pushButtonHello.ToolTip = "simple push button"

    End Sub

    ''  split button for "Command Data", "DB Element" and "Element Filtering"
    ''
    Sub AddSplitButton(ByVal panel As RibbonPanel)

        '' create three push buttons for split button drop down
        '' #1 
        Dim pushButtonData1 As New PushButtonData("SplitCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData")
        pushButtonData1.LargeImage = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorld.png")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("SplitDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement")
        pushButtonData2.LargeImage = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorld.png")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("SplitElementFiltering", "ElementFiltering", m_assembly, "RevitIntroVB.ElementFiltering")
        pushButtonData3.LargeImage = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorld.png")) '' MH: To Do. need image. 

        '' make a split button now 
        Dim splitBtnData As New SplitButtonData("SplitButton", "Split Button")
        Dim splitBtn As SplitButton = panel.AddItem(splitBtnData)
        splitBtn.AddPushButton(pushButtonData1)
        splitBtn.AddPushButton(pushButtonData2)
        splitBtn.AddPushButton(pushButtonData3)

    End Sub

    ''  pulldown button for "Command Data", "DB Element" and "Element Filtering"
    ''
    Sub AddPulldownButton(ByVal panel As RibbonPanel)

        '' create three push buttons for pulldown button drop down
        '' #1 
        Dim pushButtonData1 As New PushButtonData("PulldownCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData")
        pushButtonData1.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("PulldownDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement")
        pushButtonData2.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("PulldownElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering")
        pushButtonData3.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' make a pulldown button now 
        Dim pulldownBtnData As New PulldownButtonData("PulldownButton", "Pulldown")
        Dim pulldownBtn As PulldownButton = panel.AddItem(pulldownBtnData)
        pulldownBtn.AddPushButton(pushButtonData1)
        pulldownBtn.AddPushButton(pushButtonData2)
        pulldownBtn.AddPushButton(pushButtonData3)

    End Sub

    ''  radio/toggle button for "Command Data", "DB Element" and "Element Filtering"
    ''
    Sub AddRadioButton(ByVal panel As RibbonPanel)

        '' create three toggle buttons for radio button group
        '' #1 
        Dim toggleButtonData1 As New ToggleButtonData("RadioCommandData", "Command" + vbCr + "Data", m_assembly, "RevitIntroVB.CommandData")
        toggleButtonData1.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim toggleButtonData2 As New ToggleButtonData("RadioDBElement", "DB" + vbCr + "Element", m_assembly, "RevitIntroVB.DBElement")
        toggleButtonData2.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #3  
        Dim toggleButtonData3 As New ToggleButtonData("RadioElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering")
        toggleButtonData3.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' make a radio button group now 
        Dim radioBtnGroupData As New RadioButtonGroupData("RadioButton")
        Dim radioBtnGroup As RadioButtonGroup = panel.AddItem(radioBtnGroupData)
        radioBtnGroup.AddItem(toggleButtonData1)
        radioBtnGroup.AddItem(toggleButtonData2)
        radioBtnGroup.AddItem(toggleButtonData3)

    End Sub

    ''  text box 
    ''  text box i used in conjunction with event. we'll come to this later. 
    ''  for now, just shows how to make a text box. 
    ''
    Sub AddTextBox(ByVal panel As RibbonPanel)

        ''  fill the text gox information
        Dim txtBoxData As New TextBoxData("TextBox")
        txtBoxData.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        txtBoxData.Name = "Text Box"
        txtBoxData.ToolTip = "Enter text here"
        txtBoxData.LongDescription = "<p>This is Revit UI Labs.</p><p>Ribbon Lab</p>"
        txtBoxData.ToolTipImage = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorld.png"))

        ''  create the text box item on the panel 
        Dim txtBox As TextBox = panel.AddItem(txtBoxData)
        txtBox.PromptText = "Enter a comment"
        txtBox.ShowImageAsButton = True
        txtBox.Width = 180

        '' p51. we'll talk about event in Lab4.  
        ''txtBox.EnterPressed += New EventHandler < EventArgs.textboxenterpressedEventArgs > (pressText)

    End Sub

    ''  combo box - 5 items in 2 groups. 
    ''  combo box is used in conjunction with event. we'll come back later. 
    ''  for now, just demonstrates how to make a combo box. 
    ''
    Sub AddComboBox(ByVal panel As RibbonPanel)

        '' create five combo box members with two groups 
        '' #1 
        Dim comboBoxMemberData1 As New ComboBoxMemberData("ComboCommandData", "Command Data")
        comboBoxMemberData1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 
        comboBoxMemberData1.GroupName = "DB Basics"

        '' #2 
        Dim comboBoxMemberData2 As New ComboBoxMemberData("ComboDBElement", "DB Element")
        comboBoxMemberData2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 
        comboBoxMemberData2.GroupName = "DB Basics"

        '' #3  
        Dim comboBoxMemberData3 As New ComboBoxMemberData("ComboElementFiltering", "Filtering")
        comboBoxMemberData3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 
        comboBoxMemberData3.GroupName = "DB Basics"

        '' #4
        Dim comboBoxMemberData4 As New ComboBoxMemberData("ComboElementModification", "Modify")
        comboBoxMemberData4.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 
        comboBoxMemberData4.GroupName = "Modeling"

        '' #5
        Dim comboBoxMemberData5 As New ComboBoxMemberData("ComboModelCreation", "Create")
        comboBoxMemberData5.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 
        comboBoxMemberData5.GroupName = "Modeling"


        '' make a radio button group now 
        Dim comboBxData As New ComboBoxData("ComboBox")
        Dim comboBx As ComboBox = panel.AddItem(comboBxData)
        comboBx.ToolTip = "Select an Option"
        comboBx.LongDescription = "select a command you want to run"
        comboBx.AddItem(comboBoxMemberData1)
        comboBx.AddItem(comboBoxMemberData2)
        comboBx.AddItem(comboBoxMemberData3)
        comboBx.AddItem(comboBoxMemberData4)
        comboBx.AddItem(comboBoxMemberData5)

    End Sub

    ''  stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
    ''                    (no radio button group, split buttons).  
    ''  here we stack three push buttons for "Command Data", "DB Element" and "Element Filtering". 
    ''
    Sub AddStackedButtons_Simple(ByVal panel As RibbonPanel)

        '' create three push buttons to stack up 
        '' #1 
        Dim pushButtonData1 As New PushButtonData("StackSimpleCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("StackSimpleDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement")
        pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("StackSimpleElementFiltering", "Element Filtering", m_assembly, "RevitIntroVB.ElementFiltering")
        pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 


        '' put them on stack  
        Dim stackedButtons As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3)

    End Sub

    ''  stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
    ''                    (no radio button group, split buttons).  
    ''  here we define 6 buttons, make grouping of 1, 3, 2 items, and stack them in three layer: 
    ''                 (1) simple push button with "Hello World" 
    ''                 (2) pull down with 3 items: "Command Data", "DB Element" and "Element Filtering". 
    ''                 (3) pull down with 2 items: "Element Modification" and "Model Creation"
    ''
    Sub AddStackedButtons_Complex(ByVal panel As RibbonPanel)

        '' create six push buttons to group for pull down and stack up 

        '' #0 
        Dim pushButtonData0 As New PushButtonData("StackComplexHelloWorld", "Hello World", m_assembly, "RevitIntroVB.HelloWorld")
        pushButtonData0.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #1 
        Dim pushButtonData1 As New PushButtonData("StackComplexCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("StackComplexDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement")
        'pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("StackComplexElementFiltering", "Filtering", m_assembly, "RevitIntroVB.ElementFiltering")
        'pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #4 
        Dim pushButtonData4 As New PushButtonData("StackComplexElementModification", "Modify", m_assembly, "RevitIntroVB.ElementModification")
        'pushButtonData4.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #5  
        Dim pushButtonData5 As New PushButtonData("StackComplexModelCreation", "Create", m_assembly, "RevitIntroVB.ModelCreation")
        'pushButtonData5.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        ''  make two sets of pull down 
        '' 
        Dim pulldownBtnData1 As New PulldownButtonData("StackComplePulldownButton1", "DB Basics")
        Dim pulldownBtnData2 As New PulldownButtonData("StackComplePulldownButton2", "Modeling")

        ''  create three item stack. 
        Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData0, pulldownBtnData1, pulldownBtnData2)
        ''Dim pulldownBtn1 As PulldownButton = stackedItems(0) '' the first is simple bush button. 
        Dim pulldownBtn2 As PulldownButton = stackedItems(1)
        Dim pulldownBtn3 As PulldownButton = stackedItems(2)

        pulldownBtn2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        pulldownBtn3.Image = New BitmapImage(New Uri(m_imageFolder + "House.ico")) '' MH: To Do. need image.

        ''  add each sub items 
        Dim button1 As PushButton = pulldownBtn2.AddPushButton(pushButtonData1)
        Dim button2 As PushButton = pulldownBtn2.AddPushButton(pushButtonData2)
        Dim button3 As PushButton = pulldownBtn2.AddPushButton(pushButtonData3)
        Dim button4 As PushButton = pulldownBtn3.AddPushButton(pushButtonData4)
        Dim button5 As PushButton = pulldownBtn3.AddPushButton(pushButtonData5)

        ''  note: we need to set the image later.  if we do in button bata, it won't show in the Ribbon. 
        button1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button4.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button5.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.

    End Sub

    ''  slide out  
    ''
    'Sub AddSlideOut(ByVal panel As RibbonPanel)

    '    '' create two push button to place to the slide out. 
    '    '' #1 
    '    Dim pushButtonData1 As New PushButtonData("SlideOutCommandData", "Command Data", m_assembly, "RevitIntroVB.CommandData")
    '    pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

    '    '' #2 
    '    Dim pushButtonData2 As New PushButtonData("SlideOutDBElement", "DB Element", m_assembly, "RevitIntroVB.DBElement")
    '    pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

    '    ''  make a slide out 
    '    panel.AddSlideOut()

    '    ''  make a stack with two push buttons 
    '    Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2)
    '    'Dim item1 As PushButton = stackedItems(0)
    '    Dim item2 As PushButton = stackedItems(1)

    'End Sub

    ''  add buttons for the commands we define in this labs. 
    ''  here we stack three push buttons and repeat it as we get more. 
    ''  this is a template to use during the Ribbon lab exercise prior to going to following labs. 
    ''  
    Sub AddUILabsCommandButtons_Template(ByVal panel As RibbonPanel)

        ''  get the location of this dll. 
        Dim assembly As String = [GetType]().Assembly.Location

        '' create three push buttons to stack up 
        '' #1 
        Dim pushButtonData1 As New PushButtonData("UILabsCommand1", "Command1", assembly, "RevitUILabsVB.Command1")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("UILabsCommand2", "Command2", assembly, "RevitUILabsVB.Command2")
        pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("UILabsCommand3", "Command3", assembly, "RevitUILabsVB.Command3")
        pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image. 


        '' put them on stack  
        Dim stackedButtons As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3)

    End Sub

    ''  add buttons for the commands we define in this labs. 
    ''  here we stack three push buttons and repeat it as we get more. 
    '' 
    Sub AddUILabsCommandButtons(ByVal panel As RibbonPanel)

        ''  get the location of this dll. 
        Dim assembly As String = [GetType]().Assembly.Location

        '' create three push buttons to stack up 
        '' #1 
        Dim pushButtonData1 As New PushButtonData("UILabsSelection", "Pick Sampler", assembly, "RevitUILabsVB.UISelection")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("UILabsCreateHouseUI", "Create House Pick", assembly, "RevitUILabsVB.CreateHouseUI")
        pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "House.ico")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, "RevitUILabsVB.UITaskDialog")
        pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #4
        Dim pushButtonData4 As New PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, "RevitUILabsVB.CreateHouseDialog")
        pushButtonData4.Image = New BitmapImage(New Uri(m_imageFolder + "House.ico")) '' MH: To Do. need image. 

        '' #5  
        'Dim pushButtonData5 As New PushButtonData("UILabsEvent", "Event Msg On/Off", assembly, "RevitUILabsVB.UIEvent")
        'pushButtonData5.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' put them on stack  
        'Dim stackedButtons As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3)

        ''  make three sets of pull down 
        '' 
        Dim pulldownBtnData1 As New PulldownButtonData("UILabsPulldownButton1", "Selection")
        Dim pulldownBtnData2 As New PulldownButtonData("UILabsPulldownButton2", "Task Dialog")
        'Dim pulldownBtnData3 As New PulldownButtonData("UILabsPulldownButton3", "Event")

        ''  create three item stack. 
        'Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2, pulldownBtnData3)
        Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2)
        Dim pulldownBtn1 As PulldownButton = stackedItems(0)
        Dim pulldownBtn2 As PulldownButton = stackedItems(1)
        'Dim pulldownBtn3 As PulldownButton = stackedItems(2)

        pulldownBtn1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico"))
        pulldownBtn2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        'pulldownBtn3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.

        ''  add each sub items 
        Dim button1 As PushButton = pulldownBtn1.AddPushButton(pushButtonData1)
        Dim button2 As PushButton = pulldownBtn1.AddPushButton(pushButtonData2)
        Dim button3 As PushButton = pulldownBtn2.AddPushButton(pushButtonData3)
        Dim button4 As PushButton = pulldownBtn2.AddPushButton(pushButtonData4)
        'Dim button5 As PushButton = pulldownBtn3.AddPushButton(pushButtonData5)

        ''  note: we need to set the image later.  if we do in button bata, it won't show in the Ribbon. 
        button1.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button2.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button3.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        button4.Image = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image.
        'button5.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.

    End Sub

    ''  add buttons for the commands we define in this labs. 
    ''  here we stack 2 x 2-push buttons and repeat it as we get more. 
    ''  TBD: still thinking which version is better ... 
    ''  
    Sub AddUILabsCommandButtons_v2(ByVal panel As RibbonPanel)

        ''  get the location of this dll. 
        Dim assembly As String = [GetType]().Assembly.Location

        '' create push buttons to stack up 
        '' #1 
        Dim pushButtonData1 As New PushButtonData("UILabsSelection", "Pick Sampler", assembly, "RevitUILabsVB.UISelection")
        pushButtonData1.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim pushButtonData2 As New PushButtonData("UILabsCreateHouseUI", "Create House Pick", assembly, "RevitUILabsVB.CreateHouseUI")
        pushButtonData2.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #3  
        Dim pushButtonData3 As New PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, "RevitUILabsVB.UITaskDialog")
        pushButtonData3.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #4
        Dim pushButtonData4 As New PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, "RevitUILabsVB.CreateHouseDialog")
        pushButtonData4.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' #5  
        'Dim pushButtonData5 As New PushButtonData("UILabsEvent", "Event Msg On/Off", assembly, "RevitUILabsVB.UIEvent")
        'pushButtonData5.Image = New BitmapImage(New Uri(m_imageFolder + "basics.ico")) '' MH: To Do. need image. 

        '' put them on stack  
        'Dim stackedButtons As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3)

        ''  make three sets of pull down 
        '' 
        'Dim pulldownBtnData1 As New PulldownButtonData("UILabsPulldownButton1", "Selection")
        'Dim pulldownBtnData2 As New PulldownButtonData("UILabsPulldownButton2", "Task Dialog")
        'Dim pulldownBtnData3 As New PulldownButtonData("UILabsPulldownButton3", "Event")

        ''  create 2 x 2-item stack. 
        'Dim stackedItems As IList(Of RibbonItem) = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2, pulldownBtnData3)
        Dim stackedItems1 As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData1, pushButtonData2)
        Dim stackedItems2 As IList(Of RibbonItem) = panel.AddStackedItems(pushButtonData3, pushButtonData4)



        'Dim pulldownBtn1 As PulldownButton = stackedItems1(0)
        'Dim pulldownBtn2 As PulldownButton = stackedItems1(1)
        ''Dim pulldownBtn3 As PulldownButton = stackedItems(2)

        'pulldownBtn1.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png"))
        'pulldownBtn2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.
        ''pulldownBtn3.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.

        ' ''  add each sub items 
        'Dim button1 As PushButton = pulldownBtn1.AddPushButton(pushButtonData1)
        'Dim button2 As PushButton = pulldownBtn1.AddPushButton(pushButtonData2)
        'Dim button3 As PushButton = pulldownBtn2.AddPushButton(pushButtonData3)
        'Dim button4 As PushButton = pulldownBtn2.AddPushButton(pushButtonData4)
        ''Dim button5 As PushButton = pulldownBtn3.AddPushButton(pushButtonData5)

        ' ''  note: we need to set the image later.  if we do in button bata, it won't show in the Ribbon. 
        'button1.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.
        'button2.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.
        'button3.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.
        'button4.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.
        ''button5.Image = New BitmapImage(New Uri(m_imageFolder + "ImgHelloWorldSmall.png")) '' MH: To Do. need image.

    End Sub

    ''  control buttons for Event and Dynamic Model Update 
    ''
    Sub AddUILabsCommandButtons2(ByVal panel As RibbonPanel)

        ''  get the location of this dll. 
        Dim assembly As String = [GetType]().Assembly.Location

        '' create three toggle buttons for radio button group
        '' #1 
        Dim toggleButtonData1 As New ToggleButtonData("UILabsEventOn", "Event" + vbCr + "Off", assembly, "RevitUILabsVB.UIEventOff")
        toggleButtonData1.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' #2 
        Dim toggleButtonData2 As New ToggleButtonData("UILabsEventOff", "Event" + vbCr + "On", assembly, "RevitUILabsVB.UIEventOn")
        toggleButtonData2.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Basics.ico")) '' MH: To Do. need image. 

        '' create three toggle buttons for radio button group
        '' #3 
        Dim toggleButtonData3 As New ToggleButtonData("UILabsDynUpdateOn", "Center" + vbCr + "Off", assembly, "RevitUILabsVB.UIDynamicModelUpdateOff")
        toggleButtonData3.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Families.ico")) '' MH: To Do. need image. 

        '' #4 
        Dim toggleButtonData4 As New ToggleButtonData("UILabsDynUpdateOff", "Center" + vbCr + "On", assembly, "RevitUILabsVB.UIDynamicModelUpdateOn")
        toggleButtonData4.LargeImage = New BitmapImage(New Uri(m_imageFolder + "Families.ico")) '' MH: To Do. need image. 

        '' make event pn/off radio button group 
        Dim radioBtnGroupData1 As New RadioButtonGroupData("EventNotification")
        Dim radioBtnGroup1 As RadioButtonGroup = panel.AddItem(radioBtnGroupData1)
        radioBtnGroup1.AddItem(toggleButtonData1)
        radioBtnGroup1.AddItem(toggleButtonData2)

        '' make dyn update on/off radio button group 
        Dim radioBtnGroupData2 As New RadioButtonGroupData("WindowDoorCenter")
        Dim radioBtnGroup2 As RadioButtonGroup = panel.AddItem(radioBtnGroupData2)
        radioBtnGroup2.AddItem(toggleButtonData3)
        radioBtnGroup2.AddItem(toggleButtonData4)

    End Sub

End Class

#Region "Helper Classes"
''=============================================================================
''  Helper Classes 
''=============================================================================
''
''  This lab uses Revit Intro Labs.  If you prefer to use a dummy command, you can do so.
''  Providing a command template here. 
''
<Transaction(TransactionMode.Automatic)> _
<Regeneration(RegenerationOption.Manual)> _
Public Class DummyCommand1
    Implements IExternalCommand

    Public Function Execute(ByVal commandData As ExternalCommandData, _
                            ByRef message As String, _
                            ByVal elements As ElementSet) _
                            As Result _
                            Implements IExternalCommand.Execute

        ''  write your command here 
        TaskDialog.Show("Revit UI Labs", "You have called Command1")

        Return Result.Succeeded
    End Function

End Class

#End Region