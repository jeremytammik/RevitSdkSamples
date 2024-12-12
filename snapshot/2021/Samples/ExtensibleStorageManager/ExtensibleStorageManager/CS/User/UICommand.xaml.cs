using System;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace ExtensibleStorageManager
{
   /// <summary>
   /// The main user dialog class for issuing sample commands to the a SchemaWrapper
   /// </summary>
   public partial class UICommand : Window
   {
      #region Constructor
      /// <summary>
      /// Create a new dialog object and store a reference to the active document and applicationID of this addin.
      /// </summary>
      public UICommand(Autodesk.Revit.DB.Document doc, string applicationId)
      {

         InitializeComponent();
         this.Closing += new System.ComponentModel.CancelEventHandler(UICommand_Closing);
         m_Document = doc;

         //Create a new empty schemaWrapper.
         m_SchemaWrapper = SchemaWrapperTools.SchemaWrapper.NewSchema(Guid.Empty, Autodesk.Revit.DB.ExtensibleStorage.AccessLevel.Public, Autodesk.Revit.DB.ExtensibleStorage.AccessLevel.Public, "adsk", applicationId, "schemaName", "Schema documentation");
         this.m_label_applicationAppId.Content = applicationId;
         UpdateUI();
      }
      #endregion

      #region Helper methods
      /// <summary>
      /// Return a convenient recommended path to save schema files in.
      /// </summary>
      private string GetStartingXmlPath()
      {
         string currentAssemby = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
         return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentAssemby), "schemas"); 
    
      }

      /// <summary>
      /// Synchronize all UI controls in the dialog with the data in m_SchemaWrapper.
      /// </summary>
      private void UpdateUI()
      {

         this.m_textBox_SchemaApplicationId.Text = m_SchemaWrapper.Data.ApplicationId;
         this.m_textBox_SchemaVendorId.Text = m_SchemaWrapper.Data.VendorId;
         this.m_textBox_SchemaPath.Content = m_SchemaWrapper.GetXmlPath();
         this.m_textBox_SchemaName.Text = m_SchemaWrapper.Data.Name;
         this.m_textBox_SchemaDocumentation.Text = m_SchemaWrapper.Data.Documentation;
         this.m_textBox_SchemaId.Text = m_SchemaWrapper.Data.SchemaId;
         if (this.m_textBox_SchemaId.Text == Guid.Empty.ToString())
            this.m_textBox_SchemaId.Text = Application.LastGuid;

         switch (m_SchemaWrapper.Data.ReadAccess)
         {
            case AccessLevel.Application:
               {
                  m_rb_ReadAccess_Application.IsChecked = true;
                  break;
               }
            case AccessLevel.Public:
               {
                  m_rb_ReadAccess_Public.IsChecked = true;
                  break;
               }
            case AccessLevel.Vendor:
               {
                  m_rb_ReadAccess_Vendor.IsChecked = true;
                  break;
               }
         }

         switch (m_SchemaWrapper.Data.WriteAccess)
         {
            case AccessLevel.Application:
               {
                  m_rb_WriteAccess_Application.IsChecked = true;
                  break;
               }
            case AccessLevel.Public:
               {
                  m_rb_WriteAccess_Public.IsChecked = true;
                  break;
               }
            case AccessLevel.Vendor:
               {
                  m_rb_WriteAccess_Vendor.IsChecked = true;
                  break;
               }
         }
      }


      /// <summary>
      /// Retrieve AccessLevel enums for read and write permissions from the UI
      /// </summary>
      private void GetUIAccessLevels(out AccessLevel read, out AccessLevel write)
      {
         read = AccessLevel.Public;
         write = AccessLevel.Public;


         if (m_rb_ReadAccess_Application.IsChecked == true)
            read = AccessLevel.Application;
         else if (m_rb_ReadAccess_Public.IsChecked  == true)
            read = AccessLevel.Public;
         else
            read = AccessLevel.Vendor;

         if (m_rb_WriteAccess_Application.IsChecked == true)
            write = AccessLevel.Application;
         else if (m_rb_WriteAccess_Public.IsChecked == true)
            write = AccessLevel.Public;
         else
            write = AccessLevel.Vendor;


      }

      /// <summary>
      /// Ensure that the values in the two text fields in the dialogs meant for Guids evaluate
      /// to valid Guids.
      /// </summary>
      private bool ValidateGuids()
      {
         bool retval = true;
         try
         {
            Guid schemaId = new Guid(this.m_textBox_SchemaId.Text);
            Guid applicationId = new Guid(this.m_textBox_SchemaApplicationId.Text);
         }
         catch (Exception)
         {
            retval = false;
         }

         return retval;
      }
      #endregion

      #region UI Handlers


      //Store the Guid of the last-used schema in the Application object for convenient access
      //later if the user re-creates and displays this dialog again.
      void UICommand_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         Application.LastGuid = m_textBox_SchemaId.Text;
      }

      //Put a new, arbitrary Guid in the schema text box.
      private void m_button_NewSchemaId_Click(object sender, RoutedEventArgs e)
      {
         m_textBox_SchemaId.Text = StorageCommand.NewGuid().ToString();
      }

      /// <summary>
      /// Handler for the "Create a simple schema" button.
      /// </summary>
      private void m_button_CreateSetSaveSimple_Click(object sender, RoutedEventArgs e)
      { 
         CreateSetSave(SampleSchemaComplexity.SimpleExample); 
      }

      /// <summary>
      /// Handler for the "Create a complex schema" button.
      /// </summary>
      private void m_button_CreateSetSaveComplex_Click(object sender, RoutedEventArgs e)
      { 
         CreateSetSave(SampleSchemaComplexity.ComplexExample);
      }


      /// <summary>
      /// Creates a sample schema, populates it with sample data, and saves it to an XML file
      /// </summary>
      /// <param name="schemaComplexity">The example schema to create</param>
      private void CreateSetSave(SampleSchemaComplexity schemaComplexity)
      {


         //Get read-write access levels and schema and application Ids from the active dialog
         AccessLevel read;
         AccessLevel write;
         GetUIAccessLevels(out read, out write);
         if (!ValidateGuids())
         {
            TaskDialog.Show("ExtensibleStorage Manager", "Invalid Schema or ApplicationId Guid.");
            return;
         }

         //Get a pathname for an XML file from the user.
         Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
         sfd.DefaultExt = ".xml";
         sfd.Filter = "SchemaWrapper Xml files (*.xml)|*.xml";
         sfd.InitialDirectory = GetStartingXmlPath();

         sfd.FileName = this.m_textBox_SchemaName.Text + "_" + this.m_textBox_SchemaVendorId.Text + "___" + this.m_textBox_SchemaId.Text.Substring(31) + ".xml";
         
         Nullable<bool> result = sfd.ShowDialog();
         
         if ((result.HasValue) && (result == true))
         {
            try
            {
               //Create a new sample SchemaWrapper, schema, and Entity and store it in the current document's ProjectInformation element.
               m_SchemaWrapper = StorageCommand.CreateSetAndExport(m_Document.ProjectInformation, sfd.FileName, new Guid(this.m_textBox_SchemaId.Text), read, write, this.m_textBox_SchemaVendorId.Text, this.m_textBox_SchemaApplicationId.Text, this.m_textBox_SchemaName.Text, this.m_textBox_SchemaDocumentation.Text, schemaComplexity);
            }
            catch (Exception ex)
            {
               TaskDialog.Show("ExtensibleStorage Manager", "Could not Create Schema.  " + ex.ToString());
               return;
            }


            UpdateUI();

            //Display the schema fields and sample data we just created in a dialog.
            ExtensibleStorageManager.UIData dataDialog = new ExtensibleStorageManager.UIData();
            string schemaData = this.m_SchemaWrapper.ToString();
            string entityData = this.m_SchemaWrapper.GetSchemaEntityData(m_Document.ProjectInformation.GetEntity(m_SchemaWrapper.GetSchema()));
            string allData = "Schema: " + Environment.NewLine + schemaData + Environment.NewLine + Environment.NewLine + "Entity" + Environment.NewLine + entityData;
            dataDialog.SetData(allData);
            dataDialog.ShowDialog();
         }
      }


      /// <summary>
      /// Handler for the "Create Wrapper from Schema" button
      /// </summary>
      private void m_button_CreateWrapperFromSchema_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            //Given a Guid that corresponds to a schema that already exists in a document, create a SchemaWrapper
            //from it and display its top-level data in the dialog.
            StorageCommand.CreateWrapperFromSchema(new Guid(m_textBox_SchemaId.Text), out m_SchemaWrapper);
            UpdateUI();
         }

         catch (Exception ex)
         {
            TaskDialog.Show("ExtensibleStorage Manager", "Could not Create SchemaWrapper from Schema.  " + ex.ToString());
            return;
         }
         //Display all of the schema's field data in a separate dialog.
         ExtensibleStorageManager.UIData dataDialog = new ExtensibleStorageManager.UIData();
         dataDialog.SetData(m_SchemaWrapper.ToString());
         dataDialog.ShowDialog();
      }

      /// <summary>
      /// Handler for the "Look up and extract" button
      /// </summary>
      private void m_button_LookupExtract_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            //Given a Guid that corresponds to a schema that already exists in a document, create a SchemaWrapper
            //from it and display its top-level data in the dialog.
            StorageCommand.LookupAndExtractData(m_Document.ProjectInformation, new Guid(m_textBox_SchemaId.Text), out m_SchemaWrapper);
         }
         catch (Exception ex)
         {
            TaskDialog.Show("ExtensibleStorage Manager", "Could not extract data from Schema.  " + ex.ToString());
            return;
         }
         UpdateUI();
         ExtensibleStorageManager.UIData dataDialog = new ExtensibleStorageManager.UIData();
         
         //Get and display the schema field data and the actual entity data in a separate dialog.
         string schemaData = this.m_SchemaWrapper.ToString();
         string entityData = this.m_SchemaWrapper.GetSchemaEntityData(m_Document.ProjectInformation.GetEntity(m_SchemaWrapper.GetSchema()));
         string allData = "Schema: " + Environment.NewLine + schemaData + Environment.NewLine + Environment.NewLine + "Entity" + Environment.NewLine + entityData;

         dataDialog.SetData(allData);
         dataDialog.ShowDialog();
      }

      /// <summary>
      /// Handler for the "Create Schema from XML" button
      /// </summary>
      private void m_button_CreateSchemaFromXml_Click(object sender, RoutedEventArgs e)
      {
         
         //Prompt the user for an xml file containing a serialized schema.
         Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
         ofd.InitialDirectory = GetStartingXmlPath();
         ofd.DefaultExt = ".xml";
         ofd.Filter = "SchemaWrapper Xml files (*.xml)|*.xml";
         Nullable<bool> result = ofd.ShowDialog();

         if (( result.HasValue) && (result == true))
         {
            try
            {
               //Given an xml file containing schema data, create a new SchemaWrapper object
               StorageCommand.ImportSchemaFromXml(ofd.FileName, out m_SchemaWrapper);
            }

            catch (Exception ex)
            {
               TaskDialog.Show("ExtensibleStorage Manager", "Could not import Schema from Xml.  " + ex.ToString());
               return;
            }
            //Display the top level schema data in the dialog.
            UpdateUI();

            //Display the field data of the schema in a separate dialog.
            ExtensibleStorageManager.UIData dataDialog = new ExtensibleStorageManager.UIData();
            dataDialog.SetData(m_SchemaWrapper.ToString());
            dataDialog.ShowDialog();
         }
      }

      /// <summary>
      /// Handler for the "Edit Exisiting Data" button
      /// </summary>
      private void m_button_EditExistingSimple_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            ///Given a guid corresponding to a Schema with Entity data in the current document's ProjectInformation element,
            ///change the entity data to new data, (assuming that the schemas and schema guids are identical).
            StorageCommand.EditExistingData(m_Document.ProjectInformation, new Guid(m_textBox_SchemaId.Text), out m_SchemaWrapper);
         }
         catch (Exception ex)
         {
            TaskDialog.Show("ExtensibleStorage Manager", "Could not extract data from Schema.  " + ex.ToString());
            return;
         }

         UpdateUI();
         
         ///Display the schema fields and new data in a separate dialog.
         ExtensibleStorageManager.UIData dataDialog = new ExtensibleStorageManager.UIData();
         string schemaData = this.m_SchemaWrapper.ToString();
         string entityData = this.m_SchemaWrapper.GetSchemaEntityData(m_Document.ProjectInformation.GetEntity(m_SchemaWrapper.GetSchema()));
         string allData = "Schema: " + Environment.NewLine + schemaData + Environment.NewLine + Environment.NewLine + "Entity" + Environment.NewLine + entityData;

         dataDialog.SetData(allData);
         dataDialog.ShowDialog();
      }
      #endregion

      #region Properties
      /// <summary>
      /// The active document in Revit that the dialog queries for Schema and Entity data.
      /// </summary>
      public Autodesk.Revit.DB.Document Document
      {
         get { return m_Document; }
         set { m_Document = value; }
      }
      #endregion

      #region Data
      /// <summary>
      /// The object that provides high level serialization access to an Autodesk.Revit.DB.ExtensibleStorage.Schema
      /// </summary>
      private SchemaWrapperTools.SchemaWrapper m_SchemaWrapper;
      private Autodesk.Revit.DB.Document m_Document;
      #endregion






   }
}
