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

using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExtensibleStorageManager
{

    public class Application : IExternalApplication
    {


       /// <summary>
       /// There is no cleanup needed in this application  -- default implementation
       /// </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
           
            return Result.Succeeded;
        }

       /// <summary>
       /// Add a button to the Ribbon and attach it to the IExternalCommand defined in Command.cs
       /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {

            RibbonPanel rp = application.CreateRibbonPanel("Extensible Storage Manager");
            string currentAssembly = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
        
            PushButton pb = rp.AddItem(new PushButtonData("Extensible Storage Manager", "Extensible Storage Manager", currentAssembly, "ExtensibleStorageManager.Command")) as PushButton;
          
           return Result.Succeeded;
           
        }


       /// <summary>
       /// The Last Schema Guid value used in the UICommand dialog is stored here for future retrieval
       /// after the dialog is closed.
       /// </summary>
        public static string LastGuid
        {
           get { return m_lastGuid; }
           set { m_lastGuid = value; }
        }
        private static string m_lastGuid;

    }
}
