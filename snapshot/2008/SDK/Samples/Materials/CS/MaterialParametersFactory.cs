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
using System.Collections.Generic;
using System.Text;
using System.Reflection;


using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.Materials.CS
{
    /// <summary>
    /// this class is factory of MaterialParameters
    /// which is used to create a instance of 
    /// MaterialParameters's derived class 
    /// according to Material's type
    /// </summary>
    public class MaterialParametersFactory
    {
        /// <summary>
        /// use to Create a derived class of MaterialParameters 
        /// accordiong to the Type of Material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static MaterialParameters CreateMaterialParameters(Material material)
        {
            if (null == material)
            {
                return null;
            }

            //create a derived class of MaterialParameters
            //according to material's Type
            Type materialType = material.GetType();
            string materialTypename = materialType.Name;
            switch (materialTypename)
            {
                case "MaterialConcrete":
                    return new MaterialConcreteParameters(material as MaterialConcrete);
                case "MaterialGeneric":
                    return new MaterialGenericParameters(material as MaterialGeneric);
                case "MaterialOther":
                    return new MaterialOtherParameters(material as MaterialOther);
                case "MaterialSteel":
                    return new MaterialSteelParameters(material as MaterialSteel);
                case "MaterialWood":
                    return new MaterialWoodParameters(material as MaterialWood);
                default:
                    return null;
            }
        }
    }
}
