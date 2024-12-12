using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCE_7_10
{
    class RevitApplicationUI : Autodesk.Revit.UI.IExternalApplication
    {
        #region IExternalApplication Members

        public Autodesk.Revit.UI.Result OnShutdown(Autodesk.Revit.UI.UIControlledApplication application)
        {
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        public Autodesk.Revit.UI.Result OnStartup(Autodesk.Revit.UI.UIControlledApplication application)
        {
            ServerUI server = new ServerUI();
            Autodesk.Revit.UI.CodeChecking.LoadCombination.ServiceUI.BindUIServerWithDBServer(Server.ID, server);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion
    }
}
