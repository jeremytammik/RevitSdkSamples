To successfully build StructuralConnectionsSamples.sln open StructuralConnectionsSDKSamples.Common.props in any text editor and edit ASInstallDir property with the corresponding path for Revit installation (e.g. C:\Program Files\Autodesk\Revit2024\AddIns\SteelConnections)

//////////////////////////////Joint config xml////////////////////////////////////////////////////////////////
1.	Create a folder named ThirdPartySettings under the C:\ProgramData\Autodesk\Revit Steel Connections 2024 \en-US\ 
2.	Under the ThirdPartySettings folder create an .xml with the following contents (check SteelConnectionsSampleJoints.xml provided with the samples): 
<?xml version="1.0" encoding="utf-8"?>
<PaletteData Version="2.0" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ResourceDll>SteelConnectionsDotNetJointExample.dll</ResourceDll>
  <PreviewResourceDll>SteelConnectionsDotNetJointExample.dll</PreviewResourceDll>
  <Categories>
    <PaletteCategory>
      <Name>Plates at beam</Name>
      <Image>ResDummy.png</Image>
      <Items>
        <PaletteItem>
          <Description>Steel Connections Sample Joint</Description>
          <Command>SteelConnectionsJointExample</Command>
          <PreviewText>This is my test joint</PreviewText>
          <Images>
            <string>Dummy.png</string>
          </Images>
          <PreviewImages>
            <string>ResDummy.png</string>
          </PreviewImages>
          <TypeId>E8BB9834-E1A2-4644-9C59-1C9812C04E8E</TypeId>
        </PaletteItem>
      </Items>
    </PaletteCategory>
	 <PaletteCategory>
      <Name>Net joint</Name>
      <Image>ResDummy.png</Image>
      <Items>
        <PaletteItem>
          <Description>Bridge Girder DotNet Example</Description>
          <Command>BridgeGirderSample</Command>
          <PreviewText>This is my test joint 2</PreviewText>
          <Images>
            <string>Dummy.png</string>
          </Images>
          <PreviewImages>
            <string>ResDummy.png</string>
          </PreviewImages>
          <TypeId>36c78249-132f-4037-9822-3802a5cf9345</TypeId>
        </PaletteItem>
      </Items>
    </PaletteCategory>
<PaletteCategory>
      <Name>Net joint</Name>
      <Image>ResDummy.png</Image>
      <Items>
        <PaletteItem>
          <Description>Lap Joint Example</Description>
          <Command>LapJoint</Command>
          <PreviewText>This is my Lap joint</PreviewText>
          <Images>
            <string>Dummy.png</string>
          </Images>
          <PreviewImages>
            <string>ResDummy.png</string>
          </PreviewImages>
          <TypeId>B2E820AB-A8BD-4998-878C-1BE4364B1BD9</TypeId>
        </PaletteItem>
      </Items>
    </PaletteCategory>
	 <PaletteCategory>
      <Name>Net joint clip angle</Name>
      <Image>ResDummy.png</Image>
      <Items>
        <PaletteItem>
          <Description>Steel Connections Clip Angle Example</Description>
          <Command>SampleClipAngle</Command>
          <PreviewText>This is my test joint 3</PreviewText>
          <Images>
            <string>Dummy.png</string>
          </Images>
          <PreviewImages>
            <string>ResDummy.png</string>
          </PreviewImages>
          <TypeId>8B4FDDC8-946A-49B4-AF65-AC54C5615AB1</TypeId>
        </PaletteItem>
      </Items>
    </PaletteCategory>
	<PaletteCategory>
      <Name>Native Sample Clip Angle</Name>
      <Image>ResDummy.png</Image>
      <Items>
        <PaletteItem>
          <Description>Steel Connections Native Clip Angle</Description>
          <Command>SampleClipAngleNative</Command>
          <PreviewText>C++ Native Clip Angle Example</PreviewText>
          <Images>
            <string>Dummy.png</string>
          </Images>
          <PreviewImages>
            <string>ResDummy.png</string>
          </PreviewImages>
          <TypeId>FD62E6AB-0412-4F2B-9DB8-C5D5385F8042</TypeId>
        </PaletteItem>
      </Items>
    </PaletteCategory>
  </Categories>
</PaletteData>

/////////////////////////////////SteelConnectionsJointExample //////////////////////////////////////
1.	Build the "SteelConnectionsJointExample" and  "SampleDesign" projects.

2.	Copy  "SteelConnectionsJointExample.dll and "SampleDesign.dll" from "..\SDK\Samples\Binaries" to your local Revit installation under AddIns\SteelConnections subfolder (e.g." C:\Program Files\Autodesk\Revit 2024\AddIns\SteelConnections\”) 

3.	Add a record in the AstorRules.RulesDllSigned table for SteelConnectionsJointExample.dll
	Key: an integer outside the standard Revit range (e.g.: 20000)
	FileName: SteelConnectionsJointExample.dll
	Tech: 1
         

4.	 Add a record in the AstorRules.HRLDefinition for SteelConnectionsJointExample.dll
	Key: an integer outside the standard Revit range (e.g.: 20000)
	RuleRunName: SteelConnectionsJointExample
	InternalName: SteelConnectionsJointExample
	Category: SteelConnectionsJointExample
	Dll: - dll key set in step 3 (20000)
	ClassId: {E8BB9834-E1A2-4644-9C59-1C9812C04E8E}
	Version: 24
 

5.	Run the script "dbo.RULE_CreatePlate.sql" inside AstorRules.mdf creating a table called "RULE_CreatePlate". (found under ..\Samples\Projects\SampleJoint)

6.	Add a record in the table AstorJointsCalculation.NSAModuleDllSigned with the following values for each column:
    	Key: 12 (or an integer outside the standard Revit range - e.g.: 17000)
FileName: SampleDesign.dll
Tech: 1
 

7.	Add a record in the table AstorJointsCalculation.NSAModule with the following values for each column:
       Key: 59 (or an integer outside the standard Revit range - e.g.: 18000)
       StressAnalysisCodeID: 1
      JointInternalName: SteelConnectionsJointExample
      NSAModuleInternalName: CreatePlateDesign_NSAModule
      DefaultModule: 1
      SortOrder: 1
       

8.	Add a record in the table AstorJointsCalculation.NSAModuleDefinition with the following values for each column:
       Key: 59 (or an integer outside the standard Revit range - e.g.: 170000) make sure to use the same key from step (7).
       ModuleRunName: SampleDesign
       InternalName: CreatePlateDesign_NSAModule
       NSAModuleDll: 12
       ClassID: {C64ECF63-D01C-451F-B0E0-E3F85DFAC05E}
        

9.	Open Revit and create a structural column (make sure the connection to the databases is closed after adding the values above, before starting Revit).

10.	Open the connection Settings Dialog from the ‘Steel” tab and load the connection
                

11.	Use the “Connection” button on the “Steel” tab to apply the connection on the column created at step 12.

/////////////////////////////////SteelConnectionsDotNetJointExample//////////////////////////////////////
1.	Build the project SteelConnectionsDotNetJointExample

2.	Copy  "SteelConnectionsDotNetJointExample.dll from "..\SDK\Samples\Binaries" to your local Revit installation under AddIns\SteelConnections subfolder (e.g." C:\Program Files\Autodesk\Revit 2024\AddIns\SteelConnections\”) 

3.	Add a record in the AstorRules.RulesDllSigned table for SteelConnectionsDotNetJointExample.dll
	Key: an integer outside the standard Revit range (e.g.: 20001)
	FileName: SteelConnectionsDotNetJointExample.dll
	Tech: 2
                

4.	 Add a record in the AstorRules.HRLDefinition for SteelConnectionsDotNetJointExample.dll
	Key: an integer outside the standard Revit range (e.g.: 170000)
	RuleRunName: BridgeGirderSample
	InternalName: BridgeGirderSample
	Category: BridgeGirderSample
	Dll: - dll key set in step 3
	SubNameInDll: SteelConnectionsDotNetJointExample.BridgeGirderSample
	ClassId: {36c78249-132f-4037-9822-3802a5cf9345}
	Version: 24                                     
 

5. Open Revit and create a structural beam (make sure the connection to the databases is closed after adding the values above, before starting Revit).

6. Open the connection Settings Dialog from the ‘Steel” tab “Add” the “Steel Connection Joint DotNet Example“ connection in the right pane.
                 

8 .Use the “Connection” button on the “Steel” tab to apply the connection on the beam created at step 5.



/////////////////////////////////SampleLapJoint//////////////////////////////////////
1.	Build the project SampleLapJoint

2.	Copy  "SampleLapJoint.dll from "..\SDK\Samples\Binaries" to your local Revit installation under AddIns\SteelConnections subfolder (e.g." C:\Program Files\Autodesk\Revit 2024\AddIns\SteelConnections\”) 

3.	Add a record in the AstorRules.RulesDllSigned table for SampleLapJoint.dll
	Key: an integer outside the standard Revit range (e.g.: 20001)
	FileName: SampleLapJoint.dll
	Tech: 2
                

4.	 Add a record in the AstorRules.HRLDefinition for SampleLapJoint.dll
	Key: an integer outside the standard Revit range (e.g.: 170000)
	RuleRunName: LapJoint
	InternalName: LapJoint
	Category: LapJoint
	Dll: - dll key set in step 3
	SubNameInDll: SampleLapJoint.LapJoint
	ClassId: {B2E820AB-A8BD-4998-878C-1BE4364B1BD9}
	Version: 24                                     
 

5. Open Revit and create two structural overlapping flat beams (make sure the connection to the databases is closed after adding the values above, before starting Revit).

6. Open the connection Settings Dialog from the ‘Steel” tab “Add” the “Lap Joint Example“ connection in the right pane.
                 

8 .Use the “Connection” button on the “Steel” tab to apply the connection on the beam created at step 5.



/////////////////////////////////SampleClipAngle//////////////////////////////////////
1.Build the project SampleClipAngle

2.Copy  " SampleClipAngle.dll from "..\SDK\Samples\Binaries" to your local Revit installation under AddIns\SteelConnections subfolder (e.g." C:\Program Files\Autodesk\Revit 2024\AddIns\SteelConnections\”) 

3.Add a record in the AstorRules.RulesDllSigned table for SampleClipAngle.dll
	Key: an integer outside the standard Revit range (e.g.: 20002)
	FileName: SampleClipAngle.dll
	Tech: 1
                

4.  Add a record in the AstorRules.HRLDefinition for SampleClipAngle.dll
	Key: an integer outside the standard Revit range (e.g.: 20002)
	RuleRunName: SampleClipAngle
	InternalName: SampleClipAngle
	Category: SampleClipAngle
	Dll: - dll key set in step rulesDLLSigned table
	SubNameInDll: SampleClipAngle
	ClassId: {8B4FDDC8-946A-49B4-AF65-AC54C5615AB1}
	Version: 24                                     
 

5. Open Revit and create a structural column, and a structural beam perpendicular on the column (make sure the connection to the databases is closed after adding the values above, before starting Revit).

6. Open the connection Settings Dialog from the ‘Steel” tab “Add” the “Steel Connections Clip Angle Example“ connection in the right pane.

7 .Use the “Connection” button on the “Steel” tab to apply the connection on the beam created at step 5.


/////////////////////////////////SampleClipAngleNative//////////////////////////////////////
1.Build the project SampleClipAngleNative

2.Copy  " SampleClipAngleNative.dll from "..\SDK\Samples\Binaries" to your local Revit installation under AddIns\SteelConnections subfolder (e.g." C:\Program Files\Autodesk\Revit 2024\AddIns\SteelConnections\”) 

3.Add a record in the AstorRules.RulesDllSigned table for SampleClipAngleNative.dll
	Key: an integer outside the standard Revit range (e.g.: 20003)
	FileName: SampleClipAngleNative.dll
	Tech: 0
                

4.  Add a record in the AstorRules.HRLDefinition for SampleClipAngleNative.dll
	Key: an integer outside the standard Revit range (e.g.: 20003)
	RuleRunName: SampleClipAngleNative
	InternalName: SampleClipAngleNative
	Category: SampleClipAngleNative
	Dll: - dll key set in step rulesDLLSigned table
	SubNameInDll: SampleClipAngleNative
	ClassId: {FD62E6AB-0412-4F2B-9DB8-C5D5385F8042}
	Version: 24                                     
 

5. Open Revit and create a structural column, and a structural beam perpendicular on the column (make sure the connection to the databases is closed after adding the values above, before starting Revit).

6. Open the connection Settings Dialog from the ‘Steel” tab “Add” the “Steel Connections Native Clip Angle “ connection in the right pane.

7 .Use the “Connection” button on the “Steel” tab to apply the connection on the beam created at step 5.

