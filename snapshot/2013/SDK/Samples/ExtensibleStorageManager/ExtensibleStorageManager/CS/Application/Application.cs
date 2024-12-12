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
