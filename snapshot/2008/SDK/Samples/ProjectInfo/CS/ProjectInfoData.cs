//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Collections.ObjectModel;

using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Provides functions to get and set the data of Revit project information
    /// </summary>
    public class ProjectInfoData
    {
        private Autodesk.Revit.Elements.ProjectInfo m_projectInfo = null;   //A reference to an object of ProjectInfo of which the information will be shown
        
        //A dictionary stores all the pairs of enumerable Building Type and it's corresponding text
        private static Dictionary<BuildingType, string> s_buildingTypes = new Dictionary<BuildingType, string>();   

        #region Fields
        /// <summary>
        /// Project Name
        /// </summary>
        public string Name
        {
            get 
            { 
                return m_projectInfo.get_Parameter(BuiltInParameter.PROJECT_NAME).AsString();
            }
            set 
            {
                try
                {
                    m_projectInfo.get_Parameter(BuiltInParameter.PROJECT_NAME).Set(value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Project Number
        /// </summary>
        public string Number
        {
            get 
            { 
                return m_projectInfo.Number;
            }
            set 
            {
                try
                {
                    m_projectInfo.Number = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Client Name
        /// </summary>
        public string ClientName
        {
            get 
            { 
                return m_projectInfo.ClientName; 
            }
            set
            {
                try
                {
                    m_projectInfo.ClientName = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public string Status
        {
            get 
            { 
                return m_projectInfo.Status;
            }
            set
            {
                try
                {
                    m_projectInfo.Status = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Issue Date
        /// </summary>
        public string IssueDate
        {
            get 
            { 
                return m_projectInfo.IssueDate; 
            }
            set
            {
                try
                {
                    m_projectInfo.IssueDate = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Address
        /// </summary>
        public string Address
        {
            get 
            { 
                return m_projectInfo.Address; 
            }
            set
            {
                try
                {
                    m_projectInfo.Address = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// ZipCode
        /// </summary>
        public string ZipCode
        {
            get 
            { 
                return m_projectInfo.gbXMLSettings.ZIPCode;
            }
            set
            {
                try
                {
                    m_projectInfo.gbXMLSettings.ZIPCode = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// BuildingType
        /// </summary>
        public string BuildingType
        {
            get
            { 
                return s_buildingTypes[m_projectInfo.gbXMLSettings.BuildingType]; 
            }
            set
            {
                bool success = false;
                try
                {
                    foreach (KeyValuePair<BuildingType, string> pair in s_buildingTypes)
                    {
                        if (pair.Value == value)
                        {
                            m_projectInfo.gbXMLSettings.BuildingType = pair.Key;
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (!success)
                {
                    MessageBox.Show("Can't find the building type: " + value);
                }

            }
        }

        /// <summary>
        /// String collection of all building types
        /// </summary>
        public static ReadOnlyCollection<string> AllBuildingTypes
        {
            get 
            {
                List<string> allBuildingTypes = new List<string>();
                foreach (BuildingType buildingType in Enum.GetValues(typeof(BuildingType)))
                {
                    allBuildingTypes.Add(s_buildingTypes[buildingType]);
                }
                return new ReadOnlyCollection<string>(allBuildingTypes);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the static member s_buildingTypes
        /// </summary>
        static ProjectInfoData()
        {
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kNoOfBuildingTypes, "None");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kAutomotiveFacility, "Automotive Facility");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kConventionCenter, "Convention Center");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kCourthouse, "Courthouse");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kDiningBarLoungeOrLeisure, "Dining Bar Lounge or Leisure");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kDiningCafeteriaFastFood, "Dining Cafeteria Fast Food");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kDiningFamily, "Dining Family");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kDormitory, "Dormitory");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kExerciseCenter, "Exercise Center");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kFireStation, "Fire Station");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kGymnasium, "Gymnasium");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kHospitalOrHealthcare, "Hospital or Healthcare");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kHotel, "Hotel");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kLibrary, "Library");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kManufacturing, "Manufacturing");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kMotel, "Motel");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kMotionPictureTheatre, "Motion Picture Theatre");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kMultiFamily, "Multi Family");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kMuseum, "Museum");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kOffice, "Office");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kParkingGarage, "Parking Garage");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kPenitentiary, "Penitentiary");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kPerformingArtsTheater, "Performing Arts Theater");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kPoliceStation, "Police Station");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kPostOffice, "Post Office");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kReligiousBuilding, "Religious Building");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kRetail, "Retail");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kSchoolOrUniversity, "School or University");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kSportsArena, "Sports Arena");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kTownHall, "Town Hall");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kTransportation, "Transportation");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kWarehouse, "Warehouse");
            s_buildingTypes.Add(Autodesk.Revit.Parameters.BuildingType.kWorkshop, "Workshop");
        }

        /// <summary>
        /// Initializes a new instance of ProjectInfoData 
        /// with an object of Autodesk.Revit.Elements.ProjectInfo
        /// </summary>
        /// <param name="projectInfo">project information </param>
        public ProjectInfoData(Autodesk.Revit.Elements.ProjectInfo projectInfo)
        {
            m_projectInfo = projectInfo;
        }
        #endregion
    }
}
