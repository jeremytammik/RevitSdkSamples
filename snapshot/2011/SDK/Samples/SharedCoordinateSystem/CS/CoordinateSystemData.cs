//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.SharedCoordinateSystem.CS
{
    /// <summary>
    /// this class is used to get, set and manage information about Location
    /// </summary>
    public class CoordinateSystemData
   {
      ExternalCommandData m_command; // the ExternalCommandData reference
      Autodesk.Revit.UI.UIApplication m_application; //the revit application reference

      const double Modulus = 0.0174532925199433; //a modulus for degree convert to pi 
      const int Precision = 3; //default precision 

      string m_currentLocationName;   //the location of the active project;
      List<string> m_locationnames = new List<string>(); //a list to store all the location name
      double m_angle;        //Angle from True North
      double m_eastWest;     //East to West offset
      double m_northSouth;   //North to South offset
      double m_elevation;    //Elevation above ground level

      /// <summary>
      /// the value of the angle form true north
      /// </summary>
      public double AngleOffset
      {
         get
         {
            return m_angle;
         }
      }

      /// <summary>
      /// return the East to West offset
      /// </summary>
      public double EastWestOffset
      {
         get
         {
            return m_eastWest;
         }
      }

      /// <summary>
      /// return the North to South offset
      /// </summary>
      public double NorthSouthOffset
      {
         get
         {
            return m_northSouth;
         }
      }

      /// <summary>
      /// return the Elevation above ground level
      /// </summary>
      public double PositionElevation
      {
         get
         {
            return m_elevation;
         }
      }

      /// <summary>
      /// get and set the current project location name of the project
      /// </summary>
      public string LocationName
      {
         get
         {
            return m_currentLocationName;
         }
         set
         {
            m_currentLocationName = value;
         }
      }

      /// <summary>
      /// get all the project locations' name of the project
      /// </summary>
      public List<string> LocationNames
      {
         get
         {
            return m_locationnames;
         }
      }

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="commandData">the ExternalCommandData reference</param>
      public CoordinateSystemData(ExternalCommandData commandData)
      {
         m_command = commandData;
         m_application = m_command.Application;
      }


      /// <summary>
      /// get the shared coordinate system data of the project 
      /// </summary>
      public void GatData()
      {
         this.GetLocationData();
      }

      /// <summary>
      /// get the information of all the project locations associated with this project
      /// </summary>
      public void GetLocationData()
      {
         m_locationnames.Clear();
         ProjectLocation currentLocation = m_application.ActiveUIDocument.Document.ActiveProjectLocation;
         //get the current location name
         m_currentLocationName = currentLocation.Name;
         //Retrieve all the project locations associated with this project
         ProjectLocationSet locations = m_application.ActiveUIDocument.Document.ProjectLocations;

         ProjectLocationSetIterator iter = locations.ForwardIterator();
         iter.Reset();
         while (iter.MoveNext())
         {
            ProjectLocation locationTransform = iter.Current as ProjectLocation;
            string transformName = locationTransform.Name;
            m_locationnames.Add(transformName); //add the location's name to the list
         }
      }


      /// <summary>
      /// duplicate a new project location
      /// </summary>
      /// <param name="locationName">old location name</param>
      /// <param name="newLocationName">new location name</param>
      public void DuplicateLocation(string locationName, string newLocationName)
      {
         ProjectLocationSet locationSet = m_application.ActiveUIDocument.Document.ProjectLocations;
         foreach (ProjectLocation projectLocation in locationSet)
         {
            if (projectLocation.Name == locationName ||
                        projectLocation.Name + " (current)" == locationName)
            {
               //duplicate a new project location
               projectLocation.Duplicate(newLocationName);
               break;
            }
         }
      }


      /// <summary>
      /// change the current project location
      /// </summary>
      /// <param name="locationName"></param>
      public void ChangeCurrentLocation(string locationName)
      {
         ProjectLocationSet locations = m_application.ActiveUIDocument.Document.ProjectLocations;
         foreach (ProjectLocation projectLocation in locations)
         {
            //find the project location which is selected by user and
            //set it to the current projecte location 
            if (projectLocation.Name == locationName)
            {
               m_application.ActiveUIDocument.Document.ActiveProjectLocation = projectLocation;
               m_currentLocationName = locationName;
               break;
            }
         }
      }


      /// <summary>
      /// get the offset values of the project position 
      /// </summary>
      /// <param name="locationName"></param>
      public void GetOffset(string locationName)
      {
         ProjectLocationSet locationSet = m_application.ActiveUIDocument.Document.ProjectLocations;
         foreach (ProjectLocation projectLocation in locationSet)
         {
            if (projectLocation.Name == locationName ||
                        projectLocation.Name + " (current)" == locationName)
            {
               Autodesk.Revit.DB.XYZ origin = new Autodesk.Revit.DB.XYZ (0, 0, 0);
               //get the project position
               ProjectPosition pp = projectLocation.get_ProjectPosition(origin);
               m_angle = (pp.Angle /= Modulus); //convert to unit degree  
               m_eastWest = pp.EastWest;     //East to West offset
               m_northSouth = pp.NorthSouth; //north to south offset
               m_elevation = pp.Elevation;   //Elevation above ground level
               break;
            }
         }
         this.ChangePrecision();
      }


      /// <summary>
      /// change the offset value for the project position
      /// </summary>
      /// <param name="locationName">location name</param>
      /// <param name="newAngle">angle from true north</param>
      /// <param name="newEast">East to West offset</param>
      /// <param name="newNorth">north to south offset</param>
      /// <param name="newElevation">Elevation above ground level</param>
      public void EditPosition(string locationName, double newAngle, double newEast,
                                       double newNorth, double newElevation)
      {
         ProjectLocationSet locationSet = m_application.ActiveUIDocument.Document.ProjectLocations;
         foreach (ProjectLocation location in locationSet)
         {
            if (location.Name == locationName ||
                        location.Name + " (current)" == locationName)
            {
               //get the project position
               Autodesk.Revit.DB.XYZ origin = new Autodesk.Revit.DB.XYZ (0, 0, 0);
               ProjectPosition projectPosition = location.get_ProjectPosition(origin);
               //change the offset value of the project position
               projectPosition.Angle = newAngle * Modulus; //convert the unit 
               projectPosition.EastWest = newEast;
               projectPosition.NorthSouth = newNorth;
               projectPosition.Elevation = newElevation;
               //set the value of the project position
               location.set_ProjectPosition(origin, projectPosition);
            }
         }
      }


      /// <summary>
      /// change the Precision of the value
      /// </summary>
      private void ChangePrecision()
      {
         m_angle = UnitConversion.DealPrecision(m_angle, Precision);
         m_eastWest = UnitConversion.DealPrecision(m_eastWest, Precision);
         m_northSouth = UnitConversion.DealPrecision(m_northSouth, Precision);
         m_elevation = UnitConversion.DealPrecision(m_elevation, Precision);
      }
   }
}
