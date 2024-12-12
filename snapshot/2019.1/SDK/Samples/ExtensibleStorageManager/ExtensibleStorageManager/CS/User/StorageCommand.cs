//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Reflection;



namespace ExtensibleStorageManager
{
   /// <summary>
   /// An enum to select which sample schema to create.
   /// </summary>
   public enum SampleSchemaComplexity
   {
      SimpleExample =1,
      ComplexExample = 2
   }

    /// <summary>
    /// A static class that issues sample commands to a SchemaWrapper to demonstrate
    /// schema and data storage.
    /// </summary>
    /// 
    public static class StorageCommand
    {

       #region Create new sample schemas and write data to them

       /// <summary>
       ///  Creates a new sample Schema, creates an instance of that Schema (an Entity) in the given element,
       ///  sets data on that element's entity, and exports the schema to a given XML file.
       /// </summary>
       /// <returns>A new SchemaWrapper</returns>
       public static SchemaWrapperTools.SchemaWrapper CreateSetAndExport(Element storageElement, string xmlPathOut, Guid schemaId, AccessLevel readAccess, AccessLevel writeAccess, string vendorId, string applicationId, string name, string documentation, SampleSchemaComplexity schemaComplexity)
        {

            #region Start a new transaction, and create a new Schema

            if (Schema.Lookup(schemaId) != null)
            {
               throw new Exception("A Schema with this Guid already exists in this document -- another one cannot be created.");
            }
            Transaction storageWrite = new Transaction(storageElement.Document, "storageWrite");
            storageWrite.Start();

            //Create a new schema.
            SchemaWrapperTools.SchemaWrapper mySchemaWrapper = SchemaWrapperTools.SchemaWrapper.NewSchema(schemaId, readAccess, writeAccess, vendorId, applicationId, name, documentation);
            mySchemaWrapper.SetXmlPath(xmlPathOut);
            #endregion
           
            Entity storageElementEntityWrite = null;
         
            //Create some sample schema fields.  There are two sample schemas hard coded here, "simple" and "complex."
            switch (schemaComplexity)
            {
               case SampleSchemaComplexity.SimpleExample:
                  SimpleSchemaAndData(mySchemaWrapper, out storageElementEntityWrite);
                  break;
               case SampleSchemaComplexity.ComplexExample:
                  ComplexSchemaAndData(mySchemaWrapper, storageElement, xmlPathOut, schemaId, readAccess, writeAccess, vendorId, applicationId, name, documentation, out storageElementEntityWrite);
                  break;
            }


            #region Store the main entity in an element, save the Serializeable SchemaWrapper to xml, and finish the transaction

            storageElement.SetEntity(storageElementEntityWrite);
            TransactionStatus storageResult = storageWrite.Commit();
            if (storageResult != TransactionStatus.Committed)
            {
               throw new Exception("Error storing Schema.  Transaction status: " + storageResult.ToString());
            }
            else
            {
               mySchemaWrapper.ToXml(xmlPathOut);
               return mySchemaWrapper;
            }
            #endregion
        }

       #region Helper methods for CreateSetAndExport
       /// <summary>
       /// Adds several small, simple fields to a SchemaWrapper and Entity
       /// </summary>
       private static void SimpleSchemaAndData(SchemaWrapperTools.SchemaWrapper mySchemaWrapper, out Entity storageElementEntityWrite)
       {
          //Create some sample fields.
          mySchemaWrapper.AddField<int>(int0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<short>(short0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<double>(double0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<float>(float0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<bool>(bool0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<string>(string0Name, UnitType.UT_Undefined, null);
          
          //Generate the Autodesk.Revit.DB.ExtensibleStorage.Schema.
          mySchemaWrapper.FinishSchema();


          //Get the  fields
          Field fieldInt0 = mySchemaWrapper.GetSchema().GetField(int0Name);
          Field fieldShort0 = mySchemaWrapper.GetSchema().GetField(short0Name);
          Field fieldDouble0 = mySchemaWrapper.GetSchema().GetField(double0Name);
          Field fieldFloat0 = mySchemaWrapper.GetSchema().GetField(float0Name);
          Field fieldBool0 = mySchemaWrapper.GetSchema().GetField(bool0Name);
          Field fieldString0 = mySchemaWrapper.GetSchema().GetField(string0Name);

          storageElementEntityWrite = null;
          //Create a new entity of the given Schema
          storageElementEntityWrite = new Entity(mySchemaWrapper.GetSchema());

          //Set data in the entity.
          storageElementEntityWrite.Set<int>(fieldInt0, 5);
          storageElementEntityWrite.Set<short>(fieldShort0, 2);
          storageElementEntityWrite.Set<double>(fieldDouble0, 7.1, DisplayUnitType.DUT_METERS);
          storageElementEntityWrite.Set<float>(fieldFloat0, 3.1f, DisplayUnitType.DUT_METERS);
          storageElementEntityWrite.Set(fieldBool0, false);
          storageElementEntityWrite.Set(fieldString0, "hello");

       }

    

       /// <summary>
       /// Adds a simple fields, arrays, maps, subEntities, and arrays and maps of subEntities to a SchemaWrapper and Entity
       /// </summary>
       private static void ComplexSchemaAndData(SchemaWrapperTools.SchemaWrapper mySchemaWrapper, Element storageElement, string xmlPathOut, Guid schemaId, AccessLevel readAccess, AccessLevel writeAccess, string vendorId, string applicationId, string name, string documentation, out Entity storageElementEntityWrite)
       {
          #region Add Fields to the SchemaWrapper
          mySchemaWrapper.AddField<int>(int0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<short>(short0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<double>(double0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<float>(float0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<bool>(bool0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<string>(string0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<ElementId>(id0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<XYZ>(point0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<UV>(uv0Name, UnitType.UT_Length, null);
          mySchemaWrapper.AddField<Guid>(guid0Name, UnitType.UT_Undefined, null);

          //Note that we use IDictionary<> for map types and IList<> for array types
          mySchemaWrapper.AddField<IDictionary<string, string>>(map0Name, UnitType.UT_Undefined, null);
          mySchemaWrapper.AddField<IList<bool>>(array0Name, UnitType.UT_Undefined, null);

          //Create a sample subEntity
          SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper0 = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid0, readAccess, writeAccess, vendorId, applicationId, entity0Name, "A sub entity");
          mySubSchemaWrapper0.AddField<int>("subInt0", UnitType.UT_Undefined, null);
          mySubSchemaWrapper0.FinishSchema();
          Entity subEnt0 = new Entity(mySubSchemaWrapper0.GetSchema());
          subEnt0.Set<int>(mySubSchemaWrapper0.GetSchema().GetField("subInt0"), 11);
          mySchemaWrapper.AddField<Entity>(entity0Name, UnitType.UT_Undefined, mySubSchemaWrapper0);

          //
          //Create a sample map of subEntities (An IDictionary<> with key type "int" and value type "Entity")
          //
            //Create a new sample schema.
            SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper1_Map = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid_Map1, readAccess, writeAccess, vendorId, applicationId, map1Name, "A map of int to Entity");
            mySubSchemaWrapper1_Map.AddField<int>("subInt1", UnitType.UT_Undefined, null);
            mySubSchemaWrapper1_Map.FinishSchema();
            //Create a new sample Entity.
            Entity subEnt1 = new Entity(mySubSchemaWrapper1_Map.GetSchema());
            //Set data in that entity.
            subEnt1.Set<int>(mySubSchemaWrapper1_Map.GetSchema().GetField("subInt1"), 22);
            //Add a new map field to the top-level Schema.  We will add the entity we just created after all top-level
            //fields are created.
            mySchemaWrapper.AddField<IDictionary<int, Entity>>(map1Name, UnitType.UT_Undefined, mySubSchemaWrapper1_Map);




          //
          //Create a sample array of subentities (An IList<> of type "Entity")
          //
           //Create a new sample schema
            SchemaWrapperTools.SchemaWrapper mySubSchemaWrapper2_Array = SchemaWrapperTools.SchemaWrapper.NewSchema(subEntityGuid_Array2, readAccess, writeAccess, vendorId, applicationId, array1Name, "An array of Entities");
            mySubSchemaWrapper2_Array.AddField<int>("subInt2", UnitType.UT_Undefined, null);
            mySubSchemaWrapper2_Array.FinishSchema();
            //Create a new sample Entity.
            Entity subEnt2 = new Entity(mySubSchemaWrapper2_Array.GetSchema());
            //Set the data in that Entity.
            subEnt2.Set<int>(mySubSchemaWrapper2_Array.GetSchema().GetField("subInt2"), 33);
            //Add a new array field to the top-level Schema We will add the entity we just crated after all top-level fields
            //are created.
            mySchemaWrapper.AddField<IList<Entity>>(array1Name, UnitType.UT_Undefined, mySubSchemaWrapper2_Array);
          
          
          #endregion

          #region Populate the Schema in the SchemaWrapper with data

          mySchemaWrapper.FinishSchema();

          #endregion

          #region Create a new entity to store an instance of schema data
          storageElementEntityWrite = null;

          storageElementEntityWrite = new Entity(mySchemaWrapper.GetSchema());


          #endregion

          #region Get fields and set data in them
          Field fieldInt0 = mySchemaWrapper.GetSchema().GetField(int0Name);
          Field fieldShort0 = mySchemaWrapper.GetSchema().GetField(short0Name);
          Field fieldDouble0 = mySchemaWrapper.GetSchema().GetField(double0Name);
          Field fieldFloat0 = mySchemaWrapper.GetSchema().GetField(float0Name);

          Field fieldBool0 = mySchemaWrapper.GetSchema().GetField(bool0Name);
          Field fieldString0 = mySchemaWrapper.GetSchema().GetField(string0Name);

          Field fieldId0 = mySchemaWrapper.GetSchema().GetField(id0Name);
          Field fieldPoint0 = mySchemaWrapper.GetSchema().GetField(point0Name);
          Field fieldUv0 = mySchemaWrapper.GetSchema().GetField(uv0Name);
          Field fieldGuid0 = mySchemaWrapper.GetSchema().GetField(guid0Name);

          Field fieldMap0 = mySchemaWrapper.GetSchema().GetField(map0Name);
          Field fieldArray0 = mySchemaWrapper.GetSchema().GetField(array0Name);

          Field fieldEntity0 = mySchemaWrapper.GetSchema().GetField(entity0Name);

          Field fieldMap1 = mySchemaWrapper.GetSchema().GetField(map1Name);
          Field fieldArray1 = mySchemaWrapper.GetSchema().GetField(array1Name);


          storageElementEntityWrite.Set<int>(fieldInt0, 5);
          storageElementEntityWrite.Set<short>(fieldShort0, 2);

          storageElementEntityWrite.Set<double>(fieldDouble0, 7.1, DisplayUnitType.DUT_METERS);
          storageElementEntityWrite.Set<float>(fieldFloat0, 3.1f, DisplayUnitType.DUT_METERS);


          storageElementEntityWrite.Set(fieldBool0, false);
          storageElementEntityWrite.Set(fieldString0, "hello");
          storageElementEntityWrite.Set(fieldId0, storageElement.Id);
          storageElementEntityWrite.Set(fieldPoint0, new XYZ(1, 2, 3), DisplayUnitType.DUT_METERS);
          storageElementEntityWrite.Set(fieldUv0, new UV(1, 2), DisplayUnitType.DUT_METERS);
          storageElementEntityWrite.Set(fieldGuid0, new Guid("D8301329-F207-43B8-8AA1-634FD344F350"));

          //Note that we must pass an IDictionary<>, not a Dictionary<> to Set().
          IDictionary<string, string> myMap0 = new Dictionary<string, string>();
          myMap0.Add("mykeystr", "myvalstr");
          storageElementEntityWrite.Set(fieldMap0, myMap0);

          //Note that we must pass an IList<>, not a List<> to Set().
          IList<bool> myBoolArrayList0 = new List<bool>();
          myBoolArrayList0.Add(true);
          myBoolArrayList0.Add(false);
          storageElementEntityWrite.Set(fieldArray0, myBoolArrayList0);
          storageElementEntityWrite.Set(fieldEntity0, subEnt0);


          //Create a map of Entities
          IDictionary<int, Entity> myMap1 = new Dictionary<int, Entity>();
          myMap1.Add(5, subEnt1);
          //Set the map of Entities.
          storageElementEntityWrite.Set(fieldMap1, myMap1);

          //Create a list of entities
          IList<Entity> myEntArrayList1 = new List<Entity>();
          myEntArrayList1.Add(subEnt2);
          myEntArrayList1.Add(subEnt2);
          //Set the list of entities.
          storageElementEntityWrite.Set(fieldArray1, myEntArrayList1);
          #endregion
       }
       #endregion
       #endregion

       #region Create and query SchemaWrappers from existing schemas or XML
       /// <summary>
        /// Given an Autodesk.Revit.DB.ExtensibleStorage.Schema that already exists,
        /// create a SchemaWrapper containing that Schema's data.
        /// </summary>
        /// <param name="schemaId">The Guid of the existing Schema</param>
       public static void CreateWrapperFromSchema(Guid schemaId, out SchemaWrapperTools.SchemaWrapper schemaWrapper)
        {
            Schema toLookup = Schema.Lookup(schemaId);
            if (toLookup == null)
            {
               throw new Exception("Schema not found in current document.");
            }
            else
            {
               schemaWrapper = SchemaWrapperTools.SchemaWrapper.FromSchema(toLookup);
            }
        }


       /// <summary>
       /// Create a SchemaWrapper from a Schema Guid and try to find an Entity of a matching Guid
       /// in a given Element.  If successfull, try to change the data in that Entity.
       /// </summary>
       /// <param name="storageElement"></param>
       /// <param name="schemaId"></param>
       /// <param name="schemaWrapper"></param>
       public static void EditExistingData(Element storageElement, Guid schemaId, out SchemaWrapperTools.SchemaWrapper schemaWrapper)
        {

           //Try to find the schema in the active document.
           Schema schemaLookup = Schema.Lookup(schemaId);
           if (schemaLookup == null)
           {
              throw new Exception("Schema not found in current document.");
           }
            
           //Create a SchemaWrapper.
           schemaWrapper = SchemaWrapperTools.SchemaWrapper.FromSchema(schemaLookup);


           //Try to get an Entity of the given Schema
           Entity storageElementEntityWrite = storageElement.GetEntity(schemaLookup);
           if (storageElementEntityWrite.SchemaGUID != schemaId)
           {
              throw new Exception("SchemaID of found entity does not match the SchemaID passed to GetEntity.");
           }

           if (storageElementEntityWrite == null)
           {
              throw new Exception("Entity of given Schema not found.");
           }

           //Get the fields of the schema
           Field fieldInt0 = schemaWrapper.GetSchema().GetField(int0Name);
           Field fieldShort0 = schemaWrapper.GetSchema().GetField(short0Name);
           Field fieldDouble0 = schemaWrapper.GetSchema().GetField(double0Name);
           Field fieldFloat0 = schemaWrapper.GetSchema().GetField(float0Name);
           Field fieldBool0 = schemaWrapper.GetSchema().GetField(bool0Name);
           Field fieldString0 = schemaWrapper.GetSchema().GetField(string0Name);

           //Edit the fields.
           Transaction tStore = new Transaction(storageElement.Document, "tStore");
           tStore.Start();
           storageElementEntityWrite = null;
           storageElementEntityWrite = new Entity(schemaWrapper.GetSchema());

           storageElementEntityWrite.Set<int>(fieldInt0, 10);
           storageElementEntityWrite.Set<short>(fieldShort0, 20);
           storageElementEntityWrite.Set<double>(fieldDouble0, 14.2, DisplayUnitType.DUT_METERS);
           storageElementEntityWrite.Set<float>(fieldFloat0, 6.12f, DisplayUnitType.DUT_METERS);
           storageElementEntityWrite.Set(fieldBool0, true);
           storageElementEntityWrite.Set(fieldString0, "goodbye");
           //Set the entity back into the storage element.
           storageElement.SetEntity(storageElementEntityWrite);
           tStore.Commit();

          
        }


        /// <summary>
        /// Given an element, try to find an entity containing instance data from a given Schema Guid.
        /// </summary>
        /// <param name="storageElement">The element to query</param>
        /// <param name="schemaId">The id of the Schema to query</param>
       public static void LookupAndExtractData(Element storageElement, Guid schemaId, out SchemaWrapperTools.SchemaWrapper schemaWrapper)
        {

           Schema schemaLookup = Schema.Lookup(schemaId);
           if (schemaLookup == null)
           {
              throw new Exception("Schema not found in current document.");
           }
           schemaWrapper = SchemaWrapperTools.SchemaWrapper.FromSchema(schemaLookup);

           Entity storageElementEntityRead = storageElement.GetEntity(schemaLookup);
           if (storageElementEntityRead.SchemaGUID != schemaId)
           {
              throw new Exception("SchemaID of found entity does not match the SchemaID passed to GetEntity.");
           }

           if (storageElementEntityRead == null)
           {
              throw new Exception("Entity of given Schema not found.");
           }

        }

       /// <summary>
       /// Given an xml path containing serialized schema data, create a new Schema and SchemaWrapper
       /// </summary>
       public static void ImportSchemaFromXml(string path, out SchemaWrapperTools.SchemaWrapper sWrapper)
        {
           sWrapper = SchemaWrapperTools.SchemaWrapper.FromXml(path);
           sWrapper.SetXmlPath(path);
        }
       #endregion

       #region Helper methods
       /// <summary>
       /// Create a new pseudorandom Guid
       /// </summary>
       /// <returns></returns>
        public static Guid NewGuid()
        {
           
           byte[] guidBytes = new byte[16];
           Random randomGuidBytes = new Random(s_counter);
           randomGuidBytes.NextBytes(guidBytes);
           s_counter++;
           return new Guid(guidBytes);
        }
        #endregion

       #region Data

        //A counter field used to assist in creating pseudorandom Guids
        private static int s_counter = System.DateTime.Now.Second;
 
       
        //Field names and schema guids used in sample schemas
        private static string int0Name = "int0Name";
        private static string double0Name = "double0Name";
        private static string bool0Name = "bool0Name";
        private static string string0Name = "string0Name";
        private static string id0Name = "id0Name";
        private static string point0Name = "point0Name";
        private static string uv0Name = "uv0Name";
        private static string float0Name = "float0Name";
        private static string short0Name = "short0Name";
        private static string guid0Name = "guid0Name";
        private static string map0Name = "map0Name";
        private static string array0Name = "array0Name";


        private static Guid subEntityGuid0 = NewGuid();
        private static string entity0Name = "entity0Name";

        private static Guid subEntityGuid_Map1 = NewGuid(); 
        private static string entity1Name_Map = "entity1Name_Map"; 

        private static Guid subEntityGuid_Array2 =  NewGuid();
        private static string entity2Name_Array = "entity2Name_Array";

        private static string array1Name = entity2Name_Array;
        private static string map1Name = entity1Name_Map;

        #endregion
    }
}
