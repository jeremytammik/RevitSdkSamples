using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace ASCE_7_10
{
    public class RevitApplicationDB : Autodesk.Revit.DB.IExternalDBApplication
    {
        #region IExternalDBApplication Members

        public Autodesk.Revit.DB.ExternalDBApplicationResult OnShutdown(Autodesk.Revit.ApplicationServices.ControlledApplication application)
        {
            return Autodesk.Revit.DB.ExternalDBApplicationResult.Succeeded;
        }

        public Autodesk.Revit.DB.ExternalDBApplicationResult OnStartup(Autodesk.Revit.ApplicationServices.ControlledApplication application)
        {
            application.ApplicationInitialized += new EventHandler<Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs>(application_ApplicationInitialized);


            return Autodesk.Revit.DB.ExternalDBApplicationResult.Succeeded;
        }

        void application_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
        {
            Server server = new Server();

            Autodesk.Revit.DB.CodeChecking.LoadCombination.Service.AddServer(server);

        }

        #endregion
    }
}
