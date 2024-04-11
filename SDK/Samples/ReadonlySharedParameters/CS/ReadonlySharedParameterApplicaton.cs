//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ReadonlySharedParameters.CS
{
    public class ReadonlySharedParameterApplication : IExternalApplication
    {
        #region IExternalApplication Members

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = application.CreateRibbonPanel("Shared parameters");

            PushButtonData data = new PushButtonData("BindSP", "Bind Shared\nParameters", 
                                    this.GetType().Assembly.Location, typeof(BindNewReadonlySharedParametersToDocument).FullName);
            panel.AddItem(data);

            panel.AddSeparator();

            PushButtonData data1 = new PushButtonData("SetIds1", "Set ids: GUID",
                                    this.GetType().Assembly.Location, typeof(SetReadonlyId1).FullName);

            PushButtonData data2 = new PushButtonData("SetIds2", "Set ids: short",
                                    this.GetType().Assembly.Location, typeof(SetReadonlyId2).FullName);

            panel.AddStackedItems(data1, data2);

            panel.AddSeparator();

            data1 = new PushButtonData("SetCosts1", "Set cost: random",
                                    this.GetType().Assembly.Location, typeof(SetReadonlyCost1).FullName);

            data2 = new PushButtonData("SetCosts2", "Set cost: sequence",
                                    this.GetType().Assembly.Location, typeof(SetReadonlyCost2).FullName);

            panel.AddStackedItems(data1, data2);

            return Result.Succeeded;
        }

        #endregion
    }
}
