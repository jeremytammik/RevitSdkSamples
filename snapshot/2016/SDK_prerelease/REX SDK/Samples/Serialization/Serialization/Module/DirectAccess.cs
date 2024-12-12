//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.


/// <summary>
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.REX.Framework;

#if (REVIT)
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endif

namespace REX.Serialization
{
#if (REVIT)
    /// <summary>
    /// The class enables direct connection between Revit and extension.  
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class DirectRevitAccess : IExternalCommand
#else
    /// <summary>
    /// The class enables direct connection between application and extension.  
    /// </summary>
    public class DirectAccess
#endif
    {
#if (REVIT)
        /// <summary>
        /// Executes the extension.
        /// </summary>
        /// <param name="commandData">The command data.</param>
        /// <param name="message">The message.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>Returns execution result.</returns>
        public Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, ElementSet elements)
#else
        /// <summary>
        /// Executes the extension.
        /// </summary>
        public bool Run()
#endif
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            if (currentDomain != null)
                currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

#if (REVIT)
            return Proxy.Execute(commandData, ref message, elements);
#else
            return Proxy.Run();
#endif
        }

        System.Reflection.Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Autodesk.REX.Framework.REXConfiguration.Initialize(System.Reflection.Assembly.GetExecutingAssembly());
            return Autodesk.REX.Framework.REXAssemblies.Resolve(sender, args, "2014", System.Reflection.Assembly.GetExecutingAssembly());
        }

        private static class Proxy
        {
#if (REVIT)
            public static Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExtensionDirectRevitAccess extensionDirectRevitAccess = new ExtensionDirectRevitAccess();
                return extensionDirectRevitAccess.Execute(commandData, ref message, elements);
            }
#else
            public static bool Run()
            {
                ExtensionDirectAccess extensionDirectAccess = new ExtensionDirectAccess();
                return extensionDirectAccess.Run();
            }
#endif
        }
    }




#if (REVIT)
    /// <summary>
    /// The class enables direct connection between Revit and extension.  
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExtensionDirectRevitAccess : REX.Common.REXDirectRevitAccess, IExternalCommand
#else
    /// <summary>
    /// The class enables direct connection between application and extension.  
    /// </summary>
    public class ExtensionDirectAccess
#endif
    {
#if (REVIT)
        /// <summary>
        /// Executes the extension.
        /// </summary>
        /// <param name="commandData">The command data.</param>
        /// <param name="message">The message.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>Returns execution result.</returns>
        public Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, ElementSet elements)
#else
        /// <summary>
        /// Executes the extension.
        /// </summary>
        public bool Run()
#endif
        {
            try
            {
                AppRef = new Application();

                REXContext context = new REXContext();
                context.Control.Mode = REXMode.Dialog;
                context.Control.ControlMode = REXControlMode.ModalDialog;
#if (REVIT)
                PresetContext(ref commandData, ref  message, ref elements, ref context);
#else
                context.Product.Type = REXInterfaceType.None;
#endif

                REXEnvironment Env = new REXEnvironment(REX.Common.REXConst.ENG_DedicatedVersionName);
                if (Env.LoadEngine(ref context))
                {
                    if (AppRef.Create(ref context))
                    {
                        AppRef.Show();
#if (REVIT)
                    }
                }
                if (context.Extension.Result == REXResultType.None || context.Extension.Result == REXResultType.Succeeded)
                    return Result.Succeeded;
                else if (context.Extension.Result == REXResultType.Cancelled)
                    return Result.Cancelled;
                else
                    return Result.Failed;
#else
                        return true;
                    }
                }
                return false;
#endif
            }
            catch (Exception ex)
            {
#if (REVIT)
                message = ex.Message;
                return Result.Failed;
#else
                return false;
#endif
            }
        }

        private Application AppRef;
    }
}
