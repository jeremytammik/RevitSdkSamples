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

    // as serie of  default value provider classes 
     public class XYZDefaultValueProvider:Autodesk.Revit.UI.ExtensibleStorage.Framework.IDefaultValueProvider
    {
        public object GetDefaultValue(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.DefaultValueQueryEventArgs e)
        {
            return new Autodesk.Revit.DB.XYZ(1, 1, 1);
        }
    }

     public class UVDefaultValueProvider : Autodesk.Revit.UI.ExtensibleStorage.Framework.IDefaultValueProvider
     {
         public object GetDefaultValue(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.DefaultValueQueryEventArgs e)
         {
             return new Autodesk.Revit.DB.UV(1, 1);
         }
     }

     public class GUIDDefaultValueProvider : Autodesk.Revit.UI.ExtensibleStorage.Framework.IDefaultValueProvider
     {
         public object GetDefaultValue(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.DefaultValueQueryEventArgs e)
         {
             return new Guid("6AED35BD-9143-4AAB-B568-7FC69C946824");
         }
     }

     public class Int16DefaultValueProvider : Autodesk.Revit.UI.ExtensibleStorage.Framework.IDefaultValueProvider
     {
         public object GetDefaultValue(object sender, Autodesk.Revit.UI.ExtensibleStorage.Framework.DefaultValueQueryEventArgs e)
         {
             Int16 int16 =2;  
             return int16 ;
         }
     }
}
