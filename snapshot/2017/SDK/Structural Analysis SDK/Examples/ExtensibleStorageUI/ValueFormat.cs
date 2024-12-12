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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB; 
  
namespace ExtensibleStorageUI
{
  
    //A serie of field formater classes 
    /// <summary>
    /// UV field formater 
    /// </summary>
    class ValueFormatUV:Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {


        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {
            return value.ToString();
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
            if (!value.StartsWith("("))
                return null;
            if (!value.EndsWith(")"))
                return null;

            value = value.TrimStart('(');
            value = value.TrimEnd(')'); 
 
            string[] uv = value.Split(',');
            if (uv.Length != 2)
                return null;
            double tempu;
            double tempv;
            if (!double.TryParse(uv[0], out tempu))
                return null; 

             if (!double.TryParse(uv[1], out tempv))
                 return null; 
            return new UV(tempu, tempv);    
        }
    }
    /// <summary>
    /// XYZ filed formater
    /// </summary>
    class ValueFormatXYZ : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {

        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {
            return value.ToString();
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
            if (!value.StartsWith("("))
                return null;
            if (!value.EndsWith(")"))
                return null;

            value = value.TrimStart('(');
            value = value.TrimEnd(')');

            string[] xyz = value.Split(',');
            if (xyz.Length != 3)
                return null;
            double tempx;
            double tempy;
            double tempz;

            if (!double.TryParse(xyz[0], out tempx))
                return null;
            if (!double.TryParse(xyz[1], out tempy))
                return null;
            if (!double.TryParse(xyz[2], out tempz))
                return null;
            return new XYZ(tempx, tempy,tempz);
        }
    }
    /// <summary>
    /// Guid formater 
    /// </summary>
    class ValueFormatGuid : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {
        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {
            return value.ToString();
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
             Guid tempguid;
             if (!Guid.TryParse(value, out tempguid))
                 return null;
             return new Guid(value); 
         }
    }
    /// <summary>
    /// ElementId formater
    /// </summary>
    class ValueFormatElementId : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {
        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {
            return value.ToString();
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
            int tempid;
            if(!int.TryParse(value, out tempid))
                return null;
            ElementId eid = new ElementId(tempid);

            Element e = document.GetElement(eid);
            if (e == null)
                return null; 
            return eid; 
        }
    }
    /// <summary>
    /// UV list formater 
    /// </summary>
    class ValueFormatListUV : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {


        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value; 
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {
           List<UV > uvs = value as List<UV>; 
           string tempstring = ""; 
           for (int i=0; i < uvs.Count; i++)
            {
                tempstring += uvs[i].ToString();
                tempstring += ";";   
            }
           if (tempstring.EndsWith(";"))
               tempstring = tempstring.TrimEnd(';');
           return tempstring; 
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {

            string[] uvmain = value.Split(';');
            List<UV> uvs = new List<UV>();
            for (int i = 0; i < uvmain.Length; i++)
            {
                if (!uvmain[i].StartsWith("("))
                    return null;
                if (!uvmain[i].EndsWith(")"))
                    return null;
                uvmain[i] = uvmain[i].TrimStart('(');
                uvmain[i] = uvmain[i].TrimEnd(')');
                string[] uv = uvmain[i].Split(',');
                if (uv.Length != 2)
                    return null;
                double tempu;
                double tempv;
                if (!double.TryParse(uv[0], out tempu))
                    return null;
                if (!double.TryParse(uv[1], out tempv))
                    return null;
                uvs.Add(new UV(tempu, tempv));    
            }
            return uvs;
        }
    }
    /// <summary>
    /// XYZ list formater 
    /// </summary>
    class ValueFormatListXYZ : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {

        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {

            List<XYZ> xyzs = value as List<XYZ>;
            string tempstring = "";
            for (int i = 0; i < xyzs.Count; i++)
            {
                tempstring += xyzs[i].ToString();
                tempstring += ";";
            }
            if (tempstring.EndsWith(";"))
                tempstring = tempstring.TrimEnd(';');
            return tempstring; 
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {

            string[] xyzmain = value.Split(';');
            List<XYZ> xyzs = new List<XYZ>();
            for (int i = 0; i < xyzmain.Length; i++)
            {
                if (!xyzmain[i].StartsWith("("))
                    return null;
                if (!xyzmain[i].EndsWith(")"))
                    return null;
                xyzmain[i] = xyzmain[i].TrimStart('(');
                xyzmain[i] = xyzmain[i].TrimEnd(')');
                string[] xyz = xyzmain[i].Split(',');
                if (xyz.Length != 3)
                    return null;
                double tempx;
                double tempy;
                double tempz;
                if (!double.TryParse(xyz[0], out tempx))
                    return null;
                if (!double.TryParse(xyz[1], out tempy))
                    return null;
                if (!double.TryParse(xyz[2], out tempz))
                    return null;
                xyzs.Add(new XYZ(tempx, tempy, tempz));
            }
            return xyzs;

        }
    }
    /// <summary>
    /// Guid list formater
    /// </summary>
    class ValueFormatListGuid : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {
        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {

            List<Guid> guids = value as List<Guid>;
            string tempstring = "";
            for (int i = 0; i < guids.Count; i++)
            {
                tempstring += guids[i].ToString();
                tempstring += ";";
            }
            if (tempstring.EndsWith(";"))
                tempstring = tempstring.TrimEnd(';');
            return tempstring; 
        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
       
            string[] guidmain = value.Split(';');
            List<Guid> guids = new List<Guid>();
            for (int i = 0; i < guidmain.Length; i++)
            {
                Guid tempguid;
                if (!Guid.TryParse(guidmain[i].ToString(), out tempguid))
                    return null;
                guids.Add(tempguid);
            }
            return guids;
        }
    }
    /// <summary>
    /// Element ID formater 
    /// </summary>
    class ValueFormatListElementId : Autodesk.Revit.DB.ExtensibleStorage.Framework.IFieldFormat
    {
        public object Convert(object value, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputUnitType, Autodesk.Revit.DB.DisplayUnitType outputUnitType)
        {
            return value;
        }

        public string Format(object value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType inputDisplayUnitType, bool edit)
        {

            List<ElementId> elementids = value as List<ElementId>;
            string tempstring = "";
            for (int i = 0; i < elementids.Count; i++)
            {
                tempstring += elementids[i].ToString();
                tempstring += ";";
            }
            if (tempstring.EndsWith(";"))
                tempstring = tempstring.TrimEnd(';');
            return tempstring; 

        }

        public object Parse(string value, Autodesk.Revit.DB.Document document, Autodesk.Revit.DB.UnitType unitType, Autodesk.Revit.DB.DisplayUnitType outputDisplayUnitType)
        {
      

            string[] elementidmain = value.Split(';');
            List<ElementId> elementids = new List<ElementId>();
            for (int i = 0; i < elementidmain.Length; i++)
            {
                int tempid;
                if (!int.TryParse(elementidmain[i].ToString() , out tempid))
                    return null;
                ElementId eid = new ElementId(tempid);

                Element e = document.GetElement(eid);
                if (e == null)
                    return null;
                elementids.Add(eid); 
            }
            return elementids;
        }
    }

}
