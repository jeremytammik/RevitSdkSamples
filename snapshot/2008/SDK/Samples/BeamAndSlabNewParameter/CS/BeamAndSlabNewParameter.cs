//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.BeamAndSlabNewParameter.CS
{
	/// <summary>
	/// Display how to add a parameter to an element and set value to the parameter.
	/// the class  supports the IExternalCommand interface
	/// </summary>
    public class Command : IExternalCommand
	{
        Autodesk.Revit.Application m_revit; // application of Revit
        ElementSet m_elements; // correspond to parameter elements in in Execute(...)

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
		public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData revit, 
											   ref string message, 
                                               ElementSet elements)
		{
			// Set currently executable application to private variable m_revit
            m_revit    = revit.Application;           
            m_elements = elements;
          
			// Show UI
            using (BeamAndSlabParametersForm displayForm = new BeamAndSlabParametersForm(this))
            {
                displayForm.ShowDialog();
            }

			return Autodesk.Revit.IExternalCommand.Result.Succeeded;
		}

		/// <summary>
		/// Add a new parameter, "Unique ID", to the beams and slabs
		/// The following process should be followed: 
		/// Open the shared parameters file, via the Document.OpenSharedParameterFile method. 
		/// Access an existing group or create a new group, via the DefinitionFile.Groups property. 
		/// Access an existing or create a new external parameter definition, 
		/// via the DefinitionGroup.Definitions property. 
		/// Create a new Binding with the categories to which the parameter will be bound
		/// using an InstanceBinding or a TypeBinding.
		/// Finally add the binding and definition to the document
		/// using the Document.ParameterBindings object.
		/// </summary>
		/// <returns>bool type,a value that signifies  if  add parameter was successful</returns>
		public bool SetNewParameterToBeamsAndSlabs ()
		{
			//Open the shared parameters file 
			// via the private method AccessOrCreateExternalSharedParameterFile
			DefinitionFile informationFile = AccessOrCreateExternalSharedParameterFile();

			if (null == informationFile)
			{
				return false;
			}
	
			// Access an existing or create a new group in the shared parameters file
			DefinitionGroups informationCollections = informationFile.Groups;
			DefinitionGroup  informationCollection  = null;

			informationCollection = informationCollections.get_Item("MyParameters");

			if (null == informationCollection)
			{
				informationCollections.Create("MyParameters");
				informationCollection = informationCollections.get_Item("MyParameters");
			}

			// Access an existing or create a new external parameter definition 
			// belongs to a specific group
			Definition information = informationCollection.Definitions.get_Item("Unique ID");
				
			if (null == information)
			{
				informationCollection.Definitions.Create("Unique ID", Autodesk.Revit.Parameters.ParameterType.Text);
				information = informationCollection.Definitions.get_Item("Unique ID");
			}

			// Create a new Binding object with the categories to which the parameter will be bound
			CategorySet categories              = m_revit.Create.NewCategorySet();
			Category structuralFramingCategorie = null;
			Category floorsClassification       = null;

            // use category in instead of the string name to get category 
            structuralFramingCategorie = m_revit.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
			floorsClassification       = m_revit.ActiveDocument.Settings.Categories.get_Item(BuiltInCategory.OST_Floors);
			categories.Insert(structuralFramingCategorie);
			categories.Insert(floorsClassification); 

			InstanceBinding caseTying = m_revit.Create.NewInstanceBinding(categories);
			
			// Add the binding and definition to the document
			bool boundResult = m_revit.ActiveDocument.ParameterBindings.Insert(information, caseTying);		

			return boundResult;
		}

		/// <summary>
		/// Set value(uuid) to Unique ID parameter
		/// </summary>
		public void SetValueToUniqueIDParameter()
		{
			ElementIterator i = m_revit.ActiveDocument.Elements;

			// Enumerate  all elements of Current document.
			// If the element is a beam or a slab set value to its Unique ID parameter.
			i.Reset();
			bool moreElements = i.MoveNext();

			while (moreElements)
			{
				// Find elements whose type is FamilyInstance or Floor .
				// Because the type of beams is FamilyInstance and the type of slabs is Floor.
				FamilyInstance beam = i.Current as FamilyInstance;
				Floor slab          = i.Current as Floor;

				if ((null == beam) && (null == slab))
				{	
					moreElements = i.MoveNext();
					continue;
				}

				// Get a beam or  a slab
				Element component = i.Current as Autodesk.Revit.Element;

                if (null == component.Category)
                {
                    moreElements = i.MoveNext();
                    continue;
                }

				if (("Structural Framing" != component.Category.Name) && 
					("Floors" != component.Category.Name))	
				{
					moreElements = i.MoveNext();
					continue;
				}

				// Find the parameter which is named "Unique ID" 
				// belongs to a specifically beam or slab
				ParameterSet attributes = component.Parameters;
				IEnumerator j           = attributes.GetEnumerator();

				j.Reset();
				bool moreParameters = j.MoveNext();
				while (moreParameters)
				{
					Parameter attribute    = j.Current as Autodesk.Revit.Parameter;
					Definition information = attribute.Definition;

					if (null == information)
					{
						moreParameters = j.MoveNext();
						continue;
					}

					if (("Unique ID" == information.Name) && (null == attribute.AsString()))
					{
						// The shared parameter "Unique ID" then be set to a UUID
						Guid uuid = Guid.NewGuid();
						attribute.Set(uuid.ToString());
					}

					moreParameters = j.MoveNext();
				}
				moreElements = i.MoveNext();
			}
		}

		/// <summary>
		/// Display the value of Unique ID parameter in a list box
		/// </summary>
		/// <returns></returns>
		public System.Collections.ArrayList SendValueToListBox()
		{
			ElementSet elements = m_revit.ActiveDocument.Selection.Elements;

			// all the elements of current document
			IEnumerator i = elements.GetEnumerator();  

			ArrayList parameterValueArrangeBox = new ArrayList();

			// if the selections include beams and slabs, find out their Unique ID's value for display
			i.Reset();
			bool moreElements = i.MoveNext();

			while (moreElements)
			{
				// Get beams and slabs from selections
				Element component = i.Current as Autodesk.Revit.Element;

				if (null == component)
				{
					moreElements = i.MoveNext();
					continue;
				}

                if (null == component.Category)
                {
                    moreElements = i.MoveNext();
                    continue;
                }

				if (("Structural Framing" != component.Category.Name) &&
					("Floors" != component.Category.Name))
				{
					moreElements = i.MoveNext();
					continue;
				}

				// Get "Unique ID" parameter and display it's value in a list box 
				ParameterSet attributes = component.Parameters;

				foreach(object o in attributes) 
				{
					Parameter attribute = o as Parameter;

					if ("Unique ID" == attribute.Definition.Name)
					{	
						if (null == attribute.AsString())
						{
							break;
						}

						parameterValueArrangeBox.Add(attribute.AsString());
						break;
					}			
				}
				moreElements = i.MoveNext();
			}	
			return parameterValueArrangeBox;
		}

        /// <summary>
        /// found the element which using the GUID 
        /// that was assigned to the shared parameter in the shared parameters file.
        /// </summary>
        /// <param name="UniqueIdValue"></param>
        public void FindElement(string UniqueIdValue)
        {
            SelElementSet seleElements = m_revit.ActiveDocument.Selection.Elements;

            // all the elements of current document
            IEnumerator i = seleElements.GetEnumerator();


            // if the selections include beams and slabs, 
            // find out the element useing the select value for display
            i.Reset();
            bool moreElements = i.MoveNext();

            while (moreElements)
            {
                // Get beams and slabs from selections
                Element component = i.Current as Autodesk.Revit.Element;

                if (null == component)
                {
                    moreElements = i.MoveNext();
                    continue;
                }

                if (null == component.Category)
                {
                    moreElements = i.MoveNext();
                    continue;
                }

                if (("Structural Framing" != component.Category.Name) &&
                    ("Floors" != component.Category.Name))
                {
                    moreElements = i.MoveNext();
                    continue;
                }

                // Get "Unique ID" parameter
                ParameterSet attributes = component.Parameters;

                foreach (object o in attributes)
                {
                    Parameter attribute = o as Parameter;

                    if ("Unique ID" == attribute.Definition.Name)
                    {
                        if (null == attribute.AsString())
                        {
                            break;
                        }

                        // compare if the parameter's value is the same as the selected value.
                        // Clear the SelElementSet and add the found element into it. 
                        // So this element will highlight in Revit UI
                        if (UniqueIdValue == attribute.AsString())
                        {
                            seleElements.Clear();
                            seleElements.Add(component);
                            return;
                        }

                        break;
                    }
                }

                moreElements = i.MoveNext();
            }

        }

		/// <summary>
		/// Access an existing or create a new shared parameters file
		/// </summary>
		/// <returns>a shared parameters file </returns>
		private DefinitionFile  AccessOrCreateExternalSharedParameterFile()
		{	
			// The Path of Revit.exe
			string currentExecutablePath = System.Windows.Forms.Application.ExecutablePath;
 
			// The path of ourselves's shared parameters file
			string sharedParameterFile = Path.GetDirectoryName(currentExecutablePath);

			sharedParameterFile = sharedParameterFile + "\\MySharedParameters.txt";
			
			//Method's return
			DefinitionFile informationFile = null; 

			// Check if the file is exit
			System.IO.FileInfo documentMessage = new FileInfo(sharedParameterFile);
			bool fileExist = documentMessage.Exists;

			// Create file for external shared parameter since it does not exist
			if (!fileExist)
			{
				FileStream fileFlow = File.Create(sharedParameterFile);
				fileFlow.Close();	
			}
			
			// Set  ourselves's file to  the externalSharedParameterFile 
			m_revit.Options.SharedParametersFilename = sharedParameterFile;
			informationFile = m_revit.OpenSharedParameterFile();

			return informationFile;
		}
	}
}
