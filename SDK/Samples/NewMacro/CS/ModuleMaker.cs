using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.NewMacro.CS
{
   /// <summary>
   /// Implements the Revit Macro interface IModuleMaker.
   /// </summary>
   internal class ModuleMaker : IModuleMaker
   {
      /// <summary>
      /// Implement this method as an external function to create MacroModule.
      /// </summary>
      /// <param name="folder">Current Module Folder,Add All Macro Project File to this folder.</param>
      internal const string MacroName = "HelloWorld";
      internal const string ProjectName = "MacroSample";
      internal const string DllName = "NewMacro";

      public ModuleMaker()
      {
      }
      public void Execute(string folder)
      {
         try
         {
            if (string.Compare(ProjectName, Path.GetFileName(folder)) != 0)
            {
               Autodesk.Revit.UI.TaskDialog.Show($"Error", $"New project name don't pair default project name");
               return;
            }
            string addinFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string srcDir = Path.Combine(addinFolder, DllName, ProjectName);
            CopyDir(srcDir, folder);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
         }
      }

      /// <summary>
      /// Copy all file in source directory to destination directory.
      /// If destination directory exists the file which has the same name in source directory, will not replace it.
      /// </summary>     
      /// <param name="SrcDir">A directory contains source files.</param>
      /// <param name="DestDir">Destination directory</param>
      private void CopyDir(string SrcDir, string DestDir)
      {
         if(!Directory.Exists(SrcDir))
         {
            Autodesk.Revit.UI.TaskDialog.Show($"Error", $"Create Project Failed, Can't found the Project in {SrcDir}");
         }

         if (!Directory.Exists(DestDir))
         {
            Directory.CreateDirectory(DestDir);
         }
         string[] files = Directory.GetFiles(SrcDir);
         foreach (string file in files)
         {
            string name =Path.GetFileName(file);
            string dest = Path.Combine(DestDir, name);
            File.Copy(file, dest);
         }

         string[] subDirs = Directory.GetDirectories(SrcDir);
         foreach (string subDir in subDirs)
         {
            string subName = Path.GetFileName(subDir);
            string subDest = Path.Combine(DestDir, subName);
            CopyDir(subDir, subDest);
         }

      }
   }

}
