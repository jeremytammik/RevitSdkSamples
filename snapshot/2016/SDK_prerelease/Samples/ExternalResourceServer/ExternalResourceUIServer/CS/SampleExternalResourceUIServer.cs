//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;

using Revit.SDK.Samples.ExternalResourceDBServer.CS;


namespace Revit.SDK.Samples.ExternalResourceUIServer.CS
{
   class SampleExternalResourceUIServer : IExternalResourceUIServer
   {

      // Methods that must be implemented by a server for any of Revit's external services
      #region IExternalServer Implementation
      /// <summary>
      /// Return the Id of the server. 
      /// </summary>
      public System.Guid GetServerId()
      {
         return m_myServerId;
      }

      /// <summary>
      /// Return the Id of the service that the server belongs to. 
      /// </summary>
      public ExternalServiceId GetServiceId()
      {
         return ExternalServices.BuiltInExternalServices.ExternalResourceUIService;
      }

      /// <summary>
      /// Return the server's name. 
      /// </summary>
      public System.String GetName()
      {
         return "SDK Sample ExtRes UI Server";
      }

      /// <summary>
      /// Return the server's vendor Id. 
      /// </summary>
      public System.String GetVendorId()
      {
         return "ADSK";
      }

      /// <summary>
      /// Return the description of the server. 
      /// </summary>
      public System.String GetDescription()
      {
         return "Simple UI server for the Revit SDK sample external resource server";
      }

      #endregion IExternalServer Implementation



      #region IExternalResourceUIServer Interface Implementation

      /// <summary>
      /// Return the Id of the related DB server. 
      /// </summary>
      /// 
      public System.Guid GetDBServerId()
      {
         return m_myDBServerId;
      }


      /// <summary>
      /// Reports the results of loads from the DB server (SampleExternalResourceServer).
      /// This method should be implemented to provide UI to communicate success or failure
      /// of a particular external resource load operation to the user.
      /// </summary>
      /// <param name="doc">The Revit model into which the External Resource was loaded.
      /// </param>
      /// <param name="loadDataList">Contains a list of ExternalResourceLoadData with results
      /// for all external resources loaded by the DB server.  It is possible for the DB server
      /// to have loaded more than one resource (for example, loading several linked files
      /// when a host file is opened by the user).
      /// </param>
      public void HandleLoadResourceResults(Document doc, IList<ExternalResourceLoadData> loadDataList)
      {
         foreach (ExternalResourceLoadData data in loadDataList)
         {
            ExternalResourceType resourceType = data.ExternalResourceType;

            // This message will be posted in a dialog box at the end of this method.
            String myMessage = String.Empty;

            ExternalResourceLoadContext loadContext = data.GetLoadContext();
            ExternalResourceReference desiredRef = data.GetExternalResourceReference();
            ExternalResourceReference currentlyLoadedRef = loadContext.GetCurrentlyLoadedReference();

            LoadOperationType loadType = loadContext.LoadOperationType;

            switch (loadType)
            {
               case LoadOperationType.Automatic:
                  myMessage = "This is an Automatic load operation. ";
                  break;

               case LoadOperationType.Explicit:
                  myMessage = "This is an Explicit load operation. ";
                  break;

               default:
                  myMessage = "There is no load type information!! ";
                  break;
            }


            bool bUnrecognizedStatus = false;
            if (data.LoadStatus == ExternalResourceLoadStatus.ResourceAlreadyCurrent)
            {
               if (data.GetLoadContext().LoadOperationType == LoadOperationType.Explicit)
               {
                  string resourcePath = currentlyLoadedRef.InSessionPath;
                  myMessage += "\n No new changes to load for link: " + resourcePath;
               }
               else
                  continue;
            }
            else if (data.LoadStatus == ExternalResourceLoadStatus.Uninitialized)
            {
               myMessage += "\n The load status is uninitialized - this generally shouldn't happen";
            }
            else if (data.LoadStatus == ExternalResourceLoadStatus.Failure)
            {
               myMessage += "\n The load failed and the reason is unknown.";
            }
            else if (data.LoadStatus == ExternalResourceLoadStatus.Success)
            {
               if (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable)
               {
                  string resourcePath = data.GetExternalResourceReference().InSessionPath;
                  myMessage += "\n Version " + data.GetLoadContent().Version + " of keynote data \'" + resourcePath + "\' has been loaded successfully";
               }
               else if (resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.RevitLink)
               {
                  string resourcePath = data.GetExternalResourceReference().InSessionPath;
                  LinkLoadContent ldrlc = (LinkLoadContent)(data.GetLoadContent());
                  string destinationPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(ldrlc.GetLinkDataPath());
                  myMessage += "\n Version " + data.GetLoadContent().Version +
                               " of the file: " + resourcePath +
                               " has been downloaded into the cached folder: " + destinationPath +
                               " for this Revit Link.";
               }
            }
            else
            {
               myMessage += "Unrecognized external resource load status.";
               bUnrecognizedStatus = true;
            }


            if (!bUnrecognizedStatus && resourceType == ExternalResourceTypes.BuiltInExternalResourceTypes.RevitLink)
            {
               // For Revit links, the UI server can also obtain a RevitLinkLoadResult which contains detailed
               // information about the status of the attempt to load the local copy of the link into Revit.
               LinkLoadContent ldrlc = (LinkLoadContent)(data.GetLoadContent());
               RevitLinkLoadResult loadResult = ldrlc.GetLinkLoadResult();
               if (loadResult != null)
               {
                  myMessage += "\n LinkLoadResultType: " + loadResult.LoadResult.ToString("g");
               }
            }
            System.Windows.Forms.MessageBox.Show(myMessage, "UI Server for SDK Sample External Resource Server");
         }
      }


      /// <summary>
      /// Use this method to report any problems that occurred while the user was browsing for External Resources.
      /// Revit will call this method each time the end user browses to a new folder location, or selects an item
      /// and clicks Open.
      /// </summary>
      public void HandleBrowseResult(ExternalResourceUIBrowseResultType resultType, string browsingItemPath)
      {
         if (resultType == ExternalResourceUIBrowseResultType.Success)
            return;

         String resultString = resultType.ToString("g");

         // While executing its SetupBrowserData() method, the "DB server" - SampleExternalResourceServer - can store
         // detailed information about browse failures that occurred (user not logged in, network down, etc.).
         // Subsequently, when Revit calls this method, the details can be read from the DB server and reported to the user.
         ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);
         if (externalResourceService == null)
         {
            System.Windows.Forms.MessageBox.Show("External Resource Service unexpectedly not found.");
            return;
         }
         SampleExternalResourceDBServer myDBServer = externalResourceService.GetServer(GetDBServerId()) as SampleExternalResourceDBServer;
         if (myDBServer == null)
         {
            System.Windows.Forms.MessageBox.Show("Cannot get SampleExternalResourceDBServer from ExternalResourceService.");
            return;
         }
         // ... Retrieve detailed failure information from SampleExternalResourceServer here.

         String message = String.Format("The browse result for <{0}> was: <{1}>.", browsingItemPath, resultString);
         System.Windows.Forms.MessageBox.Show(message);
      }

      #endregion IExternalResourceUIServer Interface Implementation



      #region SampleExternalResourceUIServer Implementations
      #endregion SampleExternalResourceUIServer Implementations



      #region SampleExternalResourceUIServer Member Variables

      private static Guid m_myServerId = new Guid("E9B6C194-62DE-4134-900D-BA8DF7AD33FA");
      private static Guid m_myDBServerId = new Guid("5F3CAA13-F073-4F93-BDC2-B7F4B806CDAF");

      #endregion SampleExternalResourceUIServer Member Variables

   }
}
