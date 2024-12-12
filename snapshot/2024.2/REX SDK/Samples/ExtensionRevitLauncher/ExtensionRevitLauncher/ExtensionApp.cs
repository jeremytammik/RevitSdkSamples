using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace ExtensionRevitLauncher
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication. This class provides a simple
    /// system to launch Extension modules from Revit. It adds the RibbonItem which is used to start
    /// modules. The list of modules attached to the application is configured in extensions.xml file.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ExtensionApp : IExternalApplication
    {
        /// <summary>
        /// The file which contains all modules to be launched. It should be in the same directory as the current assembly.      
        /// </summary>
        static string ExtensionsFile = "extensions.xml";
        /// <summary>
        /// The default image of all extensions.
        /// </summary>
        static string ExtensionsImage = "extension.png";
        #region IExternalApplication Members

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        /// <param name="application">A handle to the application being shut down.</param>
        /// <returns>
        /// Indicates if the external application completes its work successfully.
        /// </returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        /// <param name="application">A handle to the application being started.</param>
        /// <returns>
        /// Indicates if the external application completes its work successfully.
        /// </returns>
        public Result OnStartup(UIControlledApplication application)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(dir,ExtensionsFile);
            List<ExtensionModuleData> modules = ExtensionXMLReader.ReadFile(path);

            RibbonPanel ribbonSamplePanel = application.CreateRibbonPanel("Extension SDK");
            SplitButtonData splitButtonData = new SplitButtonData("ExtensionSDK", "SDK");
            SplitButton splitButton = ribbonSamplePanel.AddItem(splitButtonData) as SplitButton;
            string imgPath = Path.Combine(dir, ExtensionsImage);
                        
            foreach (ExtensionModuleData module in modules)
            {
                if (System.IO.File.Exists(module.Path))
                {
                    PushButton pushButton = splitButton.AddPushButton(new PushButtonData(module.Name, module.Description, module.Path, module.Namespace + ".DirectRevitAccess"));

                    string imgModulePath = (File.Exists(module.Img)) ? module.Img : imgPath;
                    pushButton.LargeImage = new BitmapImage(new Uri(imgModulePath));
                }
            }

            return Result.Succeeded;
        }

        #endregion
    }
}
