using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;

namespace SchemaWrapperTools
{
   /// <summary>
   /// A class to store schema field information
   /// </summary>
   [Serializable]
   public class FieldData
   {

      #region Constructors

      /// <summary>
      /// For serialization only -- Do not use.
      /// </summary>
      internal FieldData() { }

      /// <summary>
      /// Create a new FieldData object
      /// </summary>
      /// <param name="name">The name of the field</param>
      /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
      /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
      public FieldData(string name, string typeIn, UnitType unit) : this(name, typeIn, unit, null)
      {
          
      }

      /// <summary>
      /// Create a new FieldData object
      /// </summary>
      /// <param name="name">The name of the field</param>
      /// <param name="typeIn">The AssemblyQualifiedName of the Field's data type</param>
      /// <param name="unit">The unit type of the Field (set to UT_Undefined for non-floating point types</param>
      /// <param name="subSchema">The SchemaWrapper of the field's subSchema, if the field is of type "Entity"</param>
      public FieldData(string name, string typeIn, UnitType unit, SchemaWrapper subSchema) 
      { 
         m_Name = name; 
         m_Type = typeIn;
         m_Unit = unit;
         m_SubSchema = subSchema;
      }
      #endregion

      #region Other helper functions
      public override string ToString()
      {
          StringBuilder strBuilder = new StringBuilder();
          strBuilder.Append("   Field: ");
          strBuilder.Append(Name);
          strBuilder.Append(", ");
          strBuilder.Append(Type);
          strBuilder.Append(", ");
          strBuilder.Append(Unit.ToString());


          if (SubSchema != null)
          {
              strBuilder.Append(Environment.NewLine + "   " + SubSchema.ToString());
          }
          return strBuilder.ToString();
      }
      #endregion

      #region Properties
      /// <summary>
      /// The name of a schema field
      /// </summary>
      public string Name
      {
         get { return m_Name; }
         set { m_Name = value; }
      }

      /// <summary>
      /// The string representation of a schema field type (e.g. System.Int32)
      /// </summary>
      public string Type
      {
         get { return m_Type; }
         set { m_Type = value; }
      }

       /// <summary>
       /// The Unit type of the field
       /// </summary>
      public UnitType Unit
      {
         get { return m_Unit; }
         set { m_Unit = value; }
      }

       /// <summary>
       /// The SchemaWrapper of the field's sub-Schema, if is of type "Entity"
       /// </summary>
      public SchemaWrapper SubSchema
      {
          get { return m_SubSchema; }
          set { m_SubSchema = value; }
      }
      #endregion

      #region Data
      private SchemaWrapper m_SubSchema;
      private string m_Name;
      private string m_Type;
      private UnitType m_Unit;
      #endregion

   }
}
