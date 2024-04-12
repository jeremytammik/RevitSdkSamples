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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;

namespace Revit.SDK.Samples.ExternalResourceDBServer.CS
{

   /// <summary>
   /// <para>Derive from the IExternalResourceServer interface to create a server class
   /// for providing external resources (files or data) to Revit.  This example server
   /// provides both keynote data and Revit links.</para>
   /// <para>The external resource framework has been designed so that the end user
   /// loads external resources by browsing with an "open file" dialog.  When the user
   /// chooses an external resource server, the file dialog will display the contents
   /// of the server's "root folder."  From there, the user can navigate through a system
   /// of folders and files specified by the IExternalResourceServer implementation.  The
   /// files and folders might be the actual contents of a directory tree only accessible
   /// by the server, or might be some other logical structure relevant to the resource
   /// that is being loaded.</para>
   /// <para>To demonstrate the flexibility of the framework, this example server retrieves
   /// keynote data from a fictitious database for users in France and Germany, and from
   /// files for all other users.  Revit link models are also obtained from files.  To keep
   /// the demonstration simple, the "root" folder for file-based resources is assumed to be
   /// \SampleResourceServerRoot, located immediately under the directory where the DLL for
   /// this project is placed.  See the separate *.rtf file for additional documentation.</para>
   /// </summary>
   public class SampleExternalResourceDBServer : IExternalResourceServer
   {

      /// <summary>
      /// Default constructor
      /// </summary>
      public SampleExternalResourceDBServer()
      { }



      // Methods that must be implemented by a server for any of Revit's external services
      #region IExternalServer Implementation

      /// Indicate which of Revit's external services this server supports.
      /// Servers derived from IExternalResourceServer *must* return
      /// ExternalServices.BuiltInExternalServices.ExternalResourceService.
      public ExternalServiceId GetServiceId()
      {
         return ExternalServices.BuiltInExternalServices.ExternalResourceService;
      }

      /// Uniquely identifies this server to Revit's ExternalService registry
      public System.Guid GetServerId()
      {
         return new Guid("5F3CAA13-F073-4F93-BDC2-B7F4B806CDAF");
      }


      /// <summary>
      /// Implement this method to return the name of the server.
      /// </summary>
      public System.String GetName()
      {
         return "SDK Sample ExtRes Server";
      }


      /// <summary>
      /// # Implement this method to return the id of the vendor of the server.   
      /// </summary>
      public System.String GetVendorId()
      {
         return "ADSK";
      }

      /// <summary>
      /// Provide a short description of the server for display to the end user.
      /// </summary>
      public System.String GetDescription()
      {
         return "A Revit SDK sample external resource server which provides keynote data and Revit links.";
      }

      #endregion IExternalServer Implementation




      ///  Methods implemented specifically by servers for the ExternalResource service
      #region IExternalResourceServer Implementation

      public string GetShortName()
      {
         return GetName();
      }


      /// <summary>
      /// Returns a URL address of the provider of this Revit add-in.
      /// </summary>
      public virtual String GetInformationLink()
      {
         return "http://www.autodesk.com";
      }

      /// <summary>
      /// Specify an image to be displayed in browser dialogs when the end user is selecting a resource to load into Revit.
      /// </summary>
      /// <returns>A string containing the full path to an icon file containing 48x48, 32x32 and 16x16 pixel images.</returns>
      public string GetIconPath()
      {
         return string.Empty;
      } 


      /// <summary>
      /// IExternalResourceServer classes can support more than one type of external resource.
      /// This one supports keynotes and Revit links.
      /// </summary>
      public bool SupportsExternalResourceType(ExternalResourceType resourceType)
      {
         return (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable
                    ||
                 resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.RevitLink);
      }


      /// <summary>
      /// Return a list of resources and sub folders under a folder.
      /// The Revit user always loads external resources by browsing with a file navigation
      /// dialog, much like they would when selecting files on a locally-accessible drive.
      /// The SetupBrowserData method allows the server implementer to simulate an organized
      /// system of files and folders to support browsing for external resources.
      /// Purely for demonstration purposes, this sample server obtains its keynote data from
      /// a "database," and creates a "fake" directory structure for either German or French users
      /// to browse keynote data.  However, for all other users - and for Revit Links, file browsing
      /// data is generated using actual files on in a folder location under the directory containing
      /// this DLL.
      /// </summary>
      public void SetupBrowserData(ExternalResourceBrowserData browserData)
      {
         ExternalResourceMatchOptions matchOptions = browserData.GetMatchOptions();

         ExternalResourceType resourceType = matchOptions.ResourceType;

         CultureInfo currentCulture = CultureInfo.CurrentCulture;
         String currentCultureName = currentCulture.ToString();

         // Revit will call SupportsExternalResourceType() before calling this method, so we
         // can assume that resourceType will be KeynoteTable or RevitLink.
         if (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable
                &&
              (currentCultureName == "de-DE" || currentCultureName == "fr-FR"))
         {
            // French and German Keynote Data Are Obtained From a "Database"
            SetupKeynoteDatabaseBrowserData(browserData, currentCultureName);
         }
         else
         {
            // Keynote Data in Other Languages, and Revit Links, are Obtained From File
            SetupFileBasedBrowserData(browserData);
         }
      }


      /// <summary>
      /// Checks whether the given ExternalResourceReference is formatted correctly for this server.
      /// The format should match one of the formats created in the SetupBrowserData method.
      /// </summary>
      public bool IsResourceWellFormed(ExternalResourceReference extRef)
      {
         if ( extRef.ServerId != GetServerId() )
            return false;
         if ( !extRef.HasValidDisplayPath() )
            return false;

         // Either the ExternalResourceReference has a valid database key (German/French keynote case) ...
         IDictionary<string, string> refMap = extRef.GetReferenceInformation();
         if (refMap.ContainsKey(RefMapDBKeyEntry))
         {
            return KeynotesDatabase.IsValidDBKey(refMap[RefMapDBKeyEntry]);
         }
         else if (refMap.ContainsKey(RefMapLinkPathEntry))  // ... OR it is a Revit link file
         {
            return File.Exists(GetFullServerLinkFilePath(extRef));
         }

         // ... OR it is a keynote file (non- French/German cases).
         return File.Exists(GetFullServerKeynoteFilePath(extRef));
      }


      /// <summary>
      /// Implement this method to compare two ExternalResourceReferences.
      /// </summary>
      /// <param name="referenceInformation_1">The string-string IDictionary of reference information stored in one ExternalResourceReference</param>
      /// <param name="referenceInformation_2">The string-string IDictionary of reference information stored in a second ExternalResourceReference</param>
      /// <returns></returns>
      public virtual bool AreSameResources(IDictionary<string, string> referenceInformation_1, IDictionary<string, string> referenceInformation_2)
      {
         bool same = true;
         if (referenceInformation_1.Count != referenceInformation_2.Count)
         {
            same = false;
         }
         else
         {
            foreach (string key in referenceInformation_1.Keys)
            {
               if (!referenceInformation_2.ContainsKey(key) || referenceInformation_1[key] != referenceInformation_2[key])
               {
                  same = false;
                  break;
               }
            }
         }

         return same;
      }


      /// <summary>
      /// Servers can override the name for UI purposes, but here we just return the names that we
      /// used when we first created the Resources in SetupBrowserData().
      /// </summary>        
      public String GetInSessionPath(ExternalResourceReference err, String savedPath)
      {
         return savedPath;
      }


      /// <summary>
      /// Loads the resources.
      /// </summary>
      /// <param name="loadRequestId">A GUID that uniquely identifies this resource load request from Revit.</param>
      /// <param name="resourceType">The type of external resource that Revit is asking the server to load.</param>
      /// <param name="resourceReference">An ExternalResourceReference identifying which particular resource to load.</param>
      /// <param name="loadContext">Context information, including the name of Revit document that is calling the server,
      /// </param>the resource that is currently loaded and the type of load operation (automatic or user-driven).
      /// <param name="loadContent">An ExternalResourceLoadContent object that will be populated with load data by the
      /// server.  There are different subclasses of ExternalResourceLoadContent for different ExternalResourceTypes.</param>
      public void LoadResource(Guid loadRequestId, ExternalResourceType resourceType, ExternalResourceReference resourceReference, ExternalResourceLoadContext loadContext, ExternalResourceLoadContent loadContent)
      {
         loadContent.LoadStatus = ExternalResourceLoadStatus.Failure;

         // The following checks can help with testing.  However, they should not be necessary, since Revit checks all input paramters
         // before calling this method.
         if (loadRequestId == Guid.Empty)
            throw new ArgumentNullException("loadRequestId");
         if (resourceType == null)
            throw new ArgumentNullException("resourceType");
         if (resourceReference == null)
            throw new ArgumentNullException("resourceReference");
         if (loadContext == null)
            throw new ArgumentNullException("loadContext");
         if (loadContent == null)
            throw new ArgumentNullException("loadContent");
         if (!SupportsExternalResourceType(resourceType))
            throw new ArgumentOutOfRangeException("resourceType", "The specified resource type is not supported by this server.");


         // The server indicates what version of the resource is being loaded.
         loadContent.Version = GetCurrentlyAvailableResourceVersion(resourceReference);


         // resourceReference is for Keynote Data
         if (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable)
         {
            LoadKeynoteDataResource(resourceReference, loadContent);
         }
         else   // resourceReference is a Revit Link
         {
            LoadRevitLink(resourceReference, loadContent);
         }
      }


      /// <summary>
      /// Indicates whether the given version of a resource is the most current
      /// version of the data.
      /// </summary>
      /// <param name="extRef">The ExternalResourceReference to check.</param>
      /// <returns>An enum indicating whether the resource is current, out of date, or of unknown status</returns>
      public ResourceVersionStatus GetResourceVersionStatus(ExternalResourceReference extRef)
      {
         // Determine whether currently loaded version is out of date, and return appropriate status.
         String currentlyLoadedVersion = extRef.Version;

         if (currentlyLoadedVersion == String.Empty)
            return ResourceVersionStatus.Unknown;

         return currentlyLoadedVersion == GetCurrentlyAvailableResourceVersion(extRef) ? ResourceVersionStatus.Current
                                                                                       : ResourceVersionStatus.OutOfDate;
      }


      /// <summary>
      /// Implement this to extend the base IExternalResourceServer interface with additional methods
      /// that are specific to particular types of external resource (for example, Revit Links).
      /// NOTE: There are no extension methods required for keynote resources.
      /// </summary>
      /// <param name="extensions">An ExternalResourceServerExtensions object that can be populated with
      /// sub-interface objects which can perform operations related to specific types of External Resource.</param>
      public virtual void GetTypeSpecificServerOperations(ExternalResourceServerExtensions extensions)
      {
         RevitLinkOperations revitLinkOps = extensions.GetRevitLinkOperations();
      //revitLinkOps.SetGetLocalPathForOpenCallback(new GetLinkPathForOpen()); // jeremy
      //revitLinkOps.SetOnLocalLinkSharedCoordinatesSavedCallback(new LocalLinkSharedCoordinatesSaved()); // jeremy
    }

    #endregion IExternalResourceServer Implementation



    #region SampleExternalResourceDBServer Implementation

    /// <summary>
    /// <para>Returns the path of the server's root folder.  The contents of this folder will be displayed
    /// when the user first selects this server while browsing to load keynote data or a Revit link
    /// (unless the user loading keynote data is in France or Germany).</para>
    /// <para>For this example server, the root folder is simply a directory immediately under the folder
    /// where the DLL for this assembly is located.</para>
    /// </summary>
    private static String RootFolder
      {
         get
         {
            string assemblyFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string rootFolderName = assemblyFolder + "\\SampleResourceServerRoot";
            DirectoryInfo rootFolder = new DirectoryInfo(rootFolderName);
            if (!rootFolder.Exists)
               rootFolder.Create();

            return rootFolderName;
         }
      }

      /// <summary>
      /// Returns the string used to access the French and German keynotes database key that is
      /// stored in an ExternalResourceReference's reference information string-string Dictionary.
      /// See the SetupKeynoteDatabaseBrowserData method.
      /// </summary>
      private static String RefMapDBKeyEntry
      {
         get
         {
            return "DBKey";
         }
      }

      /// <summary>
      /// Returns the string used to access the server-based relative path to the Revit link file
      /// that is stored in an ExternalResourceReference's reference information string-string Dictionary.
      /// </summary>
      private static String RefMapLinkPathEntry
      {
         get
         {
            return "Path";
         }
      }


      /// <summary>
      /// Returns a string specifying the version of an external resource available from the server.
      /// </summary>
      /// <param name="extResRef">The ExternalResourceReference of the resource whose version is requested.</param>
      /// <returns>A string containing the version of the specified resource.</returns>
      private String GetCurrentlyAvailableResourceVersion(ExternalResourceReference extResRef)
      {
         IDictionary<string, string> refMap = extResRef.GetReferenceInformation();
         if (refMap.ContainsKey(RefMapDBKeyEntry))
         {
            return KeynotesDatabase.CurrentVersion;
         }
         else if (refMap.ContainsKey(RefMapLinkPathEntry))  // ... OR it is a Revit link file
         {
            String serverLinkPath = GetFullServerLinkFilePath(extResRef);
            return GetFileVersion(serverLinkPath);
         }

         // ... OR it is a keynote file (non- French/German cases).
         String serverKeynoteFilePath = GetFullServerKeynoteFilePath(extResRef);
         return GetFileVersion(serverKeynoteFilePath);
      }


      /// <summary>
      /// Provides Revit's file browser dialog with information for navigating the
      /// fictitious database containing French and German keynotes data.
      /// </summary>
      private void SetupKeynoteDatabaseBrowserData(ExternalResourceBrowserData browserData, String currentCultureName)
      {
         string folderPath = browserData.FolderPath;

         // To make this demonstration simple and clear, this code creates two sub-folders with names hard-coded
         // in French and German, and one keynote data source (must have the extension *.txt) under each of
         // these sub-folders.
         // To make subsequent recognition of these resources easier (in the server's LoadResource method and
         // elsewhere), a database "key" is stored in each resource's string-string Dictionary.
         if (currentCultureName == "de-DE")
         {
            if (folderPath == "/")  // Top-level
            {
               browserData.AddSubFolder("Unterordner1");
               browserData.AddSubFolder("Unterordner2");
            }
            else if (folderPath.EndsWith("/Unterordner1"))
            {
               var refMap = new Dictionary<String, String>();
               refMap[RefMapDBKeyEntry] = "1";
               browserData.AddResource("Keynotes1_de-DE.txt", KeynotesDatabase.CurrentVersion, refMap);
            }
            else if (folderPath.EndsWith("/Unterordner2"))
            {
               var refMap = new Dictionary<String, String>();
               refMap[RefMapDBKeyEntry] = "2";
               browserData.AddResource("Keynotes2_de-DE.txt", KeynotesDatabase.CurrentVersion, refMap);
            }
         }
         else if (currentCultureName == "fr-FR")
         {
            if (folderPath == "/")  // Top-level
            {
               browserData.AddSubFolder("Sous-dossier1");
               browserData.AddSubFolder("Sous-dossier2");
            }
            else if (folderPath.EndsWith("/Sous-dossier1"))
            {
               var refMap = new Dictionary<String, String>();
               refMap[RefMapDBKeyEntry] = "3";
               browserData.AddResource("Keynotes1_fr-FR.txt", KeynotesDatabase.CurrentVersion, refMap);
            }
            else if (folderPath.EndsWith("/Sous-dossier2"))
            {
               var refMap = new Dictionary<String, String>();
               refMap[RefMapDBKeyEntry] = "4";
               browserData.AddResource("Keynotes2_fr-FR.txt", KeynotesDatabase.CurrentVersion, refMap);
            }
         }
      }


      /// <summary>
      /// A convenience utility method.  For resources obtained from files on the server, we consider
      /// the resource version to be the last-modified date of the file.
      /// </summary>
      /// <param name="filePath">The full path of a file on disk.</param>
      /// <returns>The version (last-modified data) of the specified file.</returns>
      private String GetFileVersion(String filePath)
      {
         FileInfo fileInfo = new FileInfo(filePath);
         DateTime lastModifiedTime = fileInfo.LastWriteTimeUtc;
         CultureInfo enUs = new CultureInfo("en-us");
         return lastModifiedTime.ToString(enUs);
      }


      /// <summary>
      /// Computes the full path of a ExternalResourceReference's keynote data file location on the server.
      /// </summary>
      /// <param name="extRef">An ExternalResourceReference for a keynote data file stored on this server.</param>
      /// <returns>A string representing the full path to the Revit link resource on the server drive.</returns>
      private String GetFullServerKeynoteFilePath(ExternalResourceReference extRef)
      {
         String inSessionPath = extRef.InSessionPath;
         String serverName = GetName();
         // As an alternative to using the inSessionPath to determine the actual file path, the
         // preferred method is to store the relative server path of the file in the
         // ExternalResourceReference's referenceInformation (as is done for links).
         // This would be particularly true if you were overriding the default in-session path in
         // the GetInSessionPath() method.
         String serverKeynoteFilePath = inSessionPath.Replace(serverName + "://", RootFolder + "\\");
         return serverKeynoteFilePath;
      }


      /// <summary>
      /// <para>Provides Revit's file browser dialog with information for navigating a
      /// directory stucture of files available from the server.</para>
      /// <para>In this example implementation, the server simply echoes the directories
      /// and files that it locates under its RootFolder (subject to the appropriate file
      /// type filter pattern). </para>
      /// </summary>
      private void SetupFileBasedBrowserData(ExternalResourceBrowserData browserData)
      {
         ExternalResourceMatchOptions matchOptions = browserData.GetMatchOptions();
         // filter some resources out by specified filter pattern
         ExternalResourceType resourceType = matchOptions.ResourceType;
         string filterPattern = resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable
                                 ? "*.txt"
                                 : "*.rvt";

         // Expose a "real" directory structure for the user to browse.
         // The server's root folder is specified in the RootFolder property.
         string folderPath = browserData.FolderPath;
         string path = RootFolder + folderPath.Replace('/', '\\');
         if (Directory.Exists(path))
         {
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
               browserData.AddSubFolder(subDir.Name);
            }
            FileInfo[] subFiles = dir.GetFiles(filterPattern, SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in subFiles)
            {
               if (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable)
                  browserData.AddResource(file.Name, GetFileVersion(file.FullName));
               else
               {
                  var refMap = new Dictionary<String, String>();
                  // Relative Path of Link File is Stored in the ExternalResourceReference that
                  // Will Be Addded to the BrowserData.
                  refMap[RefMapLinkPathEntry] = folderPath.TrimEnd('/') + '/' + file.Name;
                  browserData.AddResource(file.Name, GetFileVersion(file.FullName), refMap);
               }
            }
         }
         else
         {
            throw new ArgumentException("Path is invalid");
         }
      }


      /// <summary>
      /// Loads the keynote data resources, either from the fictitious French/German database, or from a file.
      /// </summary>
      /// <param name="resourceReference">An ExternalResourceReference identifying which particular resource to load.</param>
      /// <param name="loadContent">An ExternalResourceLoadContent object that will be populated with load data by the
      /// server.  There are different subclasses of ExternalResourceLoadContent for different ExternalResourceTypes.</param>
      private void LoadKeynoteDataResource(ExternalResourceReference resourceReference, ExternalResourceLoadContent loadContent)
      {
         KeyBasedTreeEntriesLoadContent kdrlc = (KeyBasedTreeEntriesLoadContent)loadContent;
         if (kdrlc == null)
            throw new ArgumentException("Wrong type of ExternalResourceLoadContent (expecting a KeyBasedTreeEntriesLoadContent) for keynote data.", "loadContent");

         kdrlc.Reset();

         // Either the ExternalResourceReference has a valid database key (German/French case) ...
         IDictionary<string, string> refMap = resourceReference.GetReferenceInformation();
         if (refMap.ContainsKey(RefMapDBKeyEntry))
         {
            try
            {
               KeynotesDatabase.LoadKeynoteEntries(refMap[RefMapDBKeyEntry], ref kdrlc);
               kdrlc.BuildEntries();
               loadContent.LoadStatus = ExternalResourceLoadStatus.Success;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (ArgumentNullException)
            {
            }
         }
         else
         {
            // ... OR it is a real file (all other cases).
            String serverKeynoteFilePath = GetFullServerKeynoteFilePath(resourceReference);
            bool doesReadingSucceed = KeynoteEntries.LoadKeynoteEntriesFromFile(serverKeynoteFilePath, kdrlc);
            if (doesReadingSucceed)
            {
               kdrlc.BuildEntries();
               loadContent.LoadStatus = ExternalResourceLoadStatus.Success;
            }
         }
      }


      /// <summary>
      /// Loads a specified Revit link external resource.
      /// </summary>
      /// <param name="resourceReference">An ExternalResourceReference identifying which particular resource to load.</param>
      /// <param name="loadContent">An ExternalResourceLoadContent object that will be populated with load data by the
      /// server.  There are different subclasses of ExternalResourceLoadContent for different ExternalResourceTypes.</param>
      private void LoadRevitLink(ExternalResourceReference resourceReference, ExternalResourceLoadContent loadContent)
      {
         LinkLoadContent linkLoadContent = (LinkLoadContent)loadContent;
         if (linkLoadContent == null)
            throw new ArgumentException("Wrong type of ExternalResourceLoadContent (expecting a LinkLoadContent) for Revit links.", "loadContent");

         try
         {
            // Copy the file from the path under the server "root" folder to a secret "cache" folder on the users machine
            String fullCachedPath = GetFullLinkCachedFilePath(resourceReference);
            String cacheFolder = System.IO.Path.GetDirectoryName(fullCachedPath);
            if (!System.IO.Directory.Exists(cacheFolder))
            {
               System.IO.Directory.CreateDirectory(cacheFolder);
            }
            String serverLinkPath = GetFullServerLinkFilePath(resourceReference);
            System.IO.File.Copy(serverLinkPath, fullCachedPath, true);  // Overwrite

            ModelPath linksPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(fullCachedPath);
            linkLoadContent.SetLinkDataPath(linksPath);
            loadContent.LoadStatus = ExternalResourceLoadStatus.Success;
         }
         catch (System.Exception)
         {
         }

      }


      /// <summary>
      /// Computes the full path of a Revit link ExternalResourceReference's location on the server.
      /// </summary>
      /// <param name="resource">An ExternalResourceReference for a Revit link stored on this server.</param>
      /// <returns>A string representing the full path to the Revit link resource on the server drive.</returns>
      public static String GetFullServerLinkFilePath(ExternalResourceReference resource)
      {
         if (resource == null)
            return "";

         IDictionary<String, String> refMap = resource.GetReferenceInformation();
         if (!refMap.ContainsKey(RefMapLinkPathEntry))
            return "";

         return RootFolder + resource.GetReferenceInformation()[RefMapLinkPathEntry].Replace("/", "\\");
      }


      /// <summary>
      /// <para>Returns the path of the root folder of the cache where the server will make
      /// local copies of Revit links.  For this example server, the cache root folder is
      /// simply a directory immediately under the folder where the DLL for this assembly is
      /// located.</para>
      /// </summary>
      private static String LocalLinkCacheFolder
      {
         get
         {
            string assemblyFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string cacheRootFolderName = assemblyFolder + "\\SampleResourceServerLinkCache";
            DirectoryInfo cacheRootFolder = new DirectoryInfo(cacheRootFolderName);
            if (!cacheRootFolder.Exists)
               cacheRootFolder.Create();

            return cacheRootFolderName;
         }
      }


      /// <summary>
      /// <para>Computes the full path on the end user's local drive where an
      /// ExternalResourceReference's link model will be copied when the user loads the
      /// link from the server.</para>
      ///<para>The paths of the local copies relative to this cache root folder will be the
      /// same as the paths of the original server copies relative to the Revit links
      /// to the root folder on the (compare this method with the GetFullServerLinkFilePath method).</para>
      /// </summary>
      /// <param name="resource">An ExternalResourceReference for a Revit link stored on this server.</param>
      /// <returns>A string representing the full path to the link model on the end user's local drive.</returns>
      public static String GetFullLinkCachedFilePath(ExternalResourceReference resource)
      {
         if (resource == null)
            return "";

         IDictionary<String, String> refMap = resource.GetReferenceInformation();
         if (!refMap.ContainsKey(RefMapLinkPathEntry))
            return "";

         return LocalLinkCacheFolder + resource.GetReferenceInformation()[RefMapLinkPathEntry].Replace("/", "\\");
      }

      #endregion SampleExternalResourceDBServer Implementation


      

      #region SampleExternalResourceDBServer Member Variables
      #endregion SampleExternalResourceDBServer Member Variables
   }
}
