//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Revit.SDK.Samples.CreateTrianglesTopography.CS
{
    /// <summary>
    /// Triangles points and facets data which can be used to create topography surface.
    /// </summary>
    public class TrianglesData
    {
        /// The points represent an enclosed area in the XY plane.
        public IList<XYZ> Points { get; set; }

        ///Triangle faces composing a polygon mesh.
        public IList<IList<int>> Facets { get; set; }

        /// <summary>
        /// parse all points and facets stored in the TrianglesData.json
        /// </summary>
        /// <returns>an instance of TrianglesData</returns>
        public static TrianglesData Load()
        {
            string assemblyFileFolder = Path.GetDirectoryName(typeof(TrianglesData).Assembly.Location);
            string emmfilePath        = Path.Combine(assemblyFileFolder, "TrianglesData.json");
            string emmfileContent     = File.ReadAllText(emmfilePath);
            return JSONParse(emmfileContent);
        }
        private static TrianglesData JSONParse(String jsonString)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new XYZConverter() });

            return serializer.Deserialize(jsonString, typeof(TrianglesData)) as TrianglesData;
        }
    }

    /// <summary>
    ///The converter for Revit XYZ.
    /// </summary>
    public class XYZConverter : JavaScriptConverter
    {
        /// <summary>
        ///  Converts the provided dictionary into an object of Revit XYZ
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return new XYZ(Convert.ToDouble(dictionary["X"]), Convert.ToDouble(dictionary["Y"]), Convert.ToDouble(dictionary["Z"]));
        }

        /// <summary>
        ///  Converts the provided Revit XYZ object to a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var node = obj as XYZ;
            if (node == null)
                return null;
            dic.Add("X", node.X);
            dic.Add("Y", node.Y);
            dic.Add("Z", node.Z);

            return dic;
        }

        /// <summary>
        ///  gets a collection of the supported types
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new Type[] { typeof(XYZ) };
            }
        }
    }
}
