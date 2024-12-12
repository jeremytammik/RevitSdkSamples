//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ProjectInfo.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // initialize global information
                RevitStartInfo.RevitApp = commandData.Application;
                RevitStartInfo.RevitDoc = commandData.Application.ActiveDocument;
                RevitStartInfo.RevitVersionName = commandData.Application.VersionName;

                // get current project information
                Autodesk.Revit.Elements.ProjectInfo pi = commandData.Application.ActiveDocument.ProjectInformation;
                
                // show main form
                using (ProjectInfoForm pif = new ProjectInfoForm(new ProjectInfoWrapper(pi)))
                {
                    pif.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    if (pif.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return IExternalCommand.Result.Succeeded;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }
            
            return IExternalCommand.Result.Cancelled;
        }

        #endregion
    }

    /// <summary>
    /// Preserves global information
    /// </summary>
    public static class RevitStartInfo
    {
        #region Fields
        /// <summary>
        /// Revit version name for MEP
        /// </summary>
        public const string RME = "Autodesk Revit MEP 2010";

        /// <summary>
        /// Revit version name for Architecture
        /// </summary>
        public const string RAC = "Autodesk Revit Architecture 2010";

        /// <summary>
        /// Revit version name for Structure
        /// </summary>
        public const string RST = "Autodesk Revit Structure 2010";

        /// <summary>
        /// Current Revit application
        /// </summary>
        public static Application RevitApp;

        /// <summary>
        /// Active Revit document
        /// </summary>
        public static Document RevitDoc;

        /// <summary>
        /// Current Revit version name
        /// </summary>
        public static string RevitVersionName;

        /// <summary>
        /// Time Zone Array
        /// </summary>
        public static string[] TimeZones;

        /// <summary>
        /// BuildingType and its display string map.
        /// </summary>
        public static Dictionary<object, string> BuildingTypeMap;

        /// <summary>
        /// ServiceType and its display string map.
        /// </summary>
        public static Dictionary<object, string> ServiceTypeMap;

        /// <summary>
        /// ExportComplexity and its display string map.
        /// </summary>
        public static Dictionary<object, string> ExportComplexityMap;
        
        /// <summary>
        /// Initialize some static members
        /// </summary>
        static RevitStartInfo()
        {
            #region TimeZones
            TimeZones = new string[]{
            "(GMT-12:00) International Date Line West",
            "(GMT-11:00) Midway Island, Samoa",
            "(GMT-10:00) Hawaii",
            "(GMT-09:00) Alaska",
            "(GMT-08:00) Pacific Time (US/Canada)",
            "(GMT-08:00) Tijuana, Baja California",
            "(GMT-07:00) Arizona",
            "(GMT-07:00) Chihuahua, La Paz, Mazatlan - New",
            "(GMT-07:00) Chihuahua, La Paz, Mazatlan - Old",
            "(GMT-07:00) Mountain Time (US/Canada)",
            "(GMT-06:00) Central America",
            "(GMT-06:00) Central Time (US/Canada)",
            "(GMT-06:00) Guadalajara, Mexico City, Monterrey - New",
            "(GMT-06:00) Guadalajara, Mexico City, Monterrey - Old",
            "(GMT-06:00) Saskatchewan",
            "(GMT-05:00) Bogota, Lima, Quito, Rio Branco",
            "(GMT-05:00) Eastern Time (US/Canada)",
            "(GMT-05:00) Indiana (East)",
            "(GMT-04:00) Atlantic Time (Canada)",
            "(GMT-04:00) Caracas, La Paz",
            "(GMT-04:00) Santiago",
            "(GMT-03:30) Newfoundland",
            "(GMT-03:00) Brazilia",
            "(GMT-03:00) Buanos Aires, Georgetown",
            "(GMT-03:00) Greenland",
            "(GMT-03:00) Montevideo",
            "(GMT-02:00) Mid-Atlantic",
            "(GMT-01:00) Azores",
            "(GMT-01:00) Cape Verde Is.",
            "(GMT) Casablanca, Monrovia,Reykjavik",
            "(GMT) Greenwich Time: Dublin, Edinburgh, Lisbon, London",
            "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna",
            "(GMT+01:00) Belgrade, Brastislava, Budapest, Ljubljana, Prague",
            "(GMT+01:00) Brussels, Copenhagen, Madrid, Paris",
            "(GMT+01:00) Sarajevo, Skopje, Sofija, Vilnus, Warsaw, Zagreb",
            "(GMT+01:00) West Central Africa",
            "(GMT+02:00) Amman",
            "(GMT+02:00) Athens, Bucharest, Istanbul",
            "(GMT+02:00) Beirut",
            "(GMT+02:00) Cairo",
            "(GMT+02:00) Harare, Pretoria",
            "(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius",
            "(GMT+02:00) Jerusalem",
            "(GMT+02:00) Minsk",
            "(GMT+02:00) Windhoek",
            "(GMT+03:00) Baghdad",
            "(GMT+03:00) Kuwait, Riyadh",
            "(GMT+03:00) Moscow, St. Petersburg, Volgograd",
            "(GMT+03:00) Nairobi",
            "(GMT+03:00) Tbilisi",
            "(GMT+03:00) Tehran",
            "(GMT+04:00) Abu Dhabi, Muscat",
            "(GMT+04:00) Baku",
            "(GMT+04:00) Yerevan",
            "(GMT+04:30) Kabul",
            "(GMT+05:00) Ekaterinburg",
            "(GMT+05:00) Islamabad, Karachi, Tashkent",
            "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi",
            "(GMT+05:30) Sri Jayawardenepura",
            "(GMT+05:45) Kathmandu ",
            "(GMT+06:00) Almaty, Novosibirsk",
            "(GMT+06:00) Astana, Dhaka",
            "(GMT+06:30) Yangon (Rangoon)",
            "(GMT+07:00) Bangkok, Hanoi, Jakarta ",
            "(GMT+07:00) Krasnoyarsk ",
            "(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi ",
            "(GMT+08:00) Irkutsk, Ulaan Bataar ",
            "(GMT+08:00) Kuala Lumpur, Singapore ",
            "(GMT+08:00) Perth",
            "(GMT+08:00) Taipei",
            "(GMT+09:00) Osaka, Sapporo, Tokyo",
            "(GMT+09:00) Seoul",
            "(GMT+09:00) Yakutsk",
            "(GMT+09:30) Adelaide",
            "(GMT+09:30) Darwin",
            "(GMT+10:00) Brisbane",
            "(GMT+10:00) Canberra, Melbourne, Sydney",
            "(GMT+10:00) Guam, Port Moresby",
            "(GMT+10:00) Hobart",
            "(GMT+10:00) Vladivostok",
            "(GMT+11:00) Magadan, Solomon Is., New Caledonia ",
            "(GMT+12:00) Aukland, Wellington ",
            "(GMT+12:00) Fiji, Kamchatka, Marshall Is.",
            "(GMT+13:00) Nubu'alofa" };
            #endregion

            #region BuildingTypeMap
            BuildingTypeMap = new Dictionary<object, string>();
            BuildingTypeMap.Add(BuildingType.kAutomotiveFacility, "Automotive Facility");
            BuildingTypeMap.Add(BuildingType.kConventionCenter, "Convention Center");
            BuildingTypeMap.Add(BuildingType.kCourthouse, "Courthouse");
            BuildingTypeMap.Add(BuildingType.kDiningBarLoungeOrLeisure, "Dining Bar Lounge or Leisure");
            BuildingTypeMap.Add(BuildingType.kDiningCafeteriaFastFood, "Dining Cafeteria Fast Food");
            BuildingTypeMap.Add(BuildingType.kDiningFamily, "Dining Family");
            BuildingTypeMap.Add(BuildingType.kDormitory, "Dormitory");
            BuildingTypeMap.Add(BuildingType.kExerciseCenter, "Exercise Center");
            BuildingTypeMap.Add(BuildingType.kFireStation, "Fire Station");
            BuildingTypeMap.Add(BuildingType.kGymnasium, "Gymnasium");
            BuildingTypeMap.Add(BuildingType.kHospitalOrHealthcare, "Hospital or Healthcare");
            BuildingTypeMap.Add(BuildingType.kHotel, "Hotel");
            BuildingTypeMap.Add(BuildingType.kLibrary, "Library");
            BuildingTypeMap.Add(BuildingType.kManufacturing, "Manufacturing");
            BuildingTypeMap.Add(BuildingType.kMotel, "Motel");
            BuildingTypeMap.Add(BuildingType.kMotionPictureTheatre, "Motion Picture Theatre");
            BuildingTypeMap.Add(BuildingType.kMultiFamily, "Multi Family");
            BuildingTypeMap.Add(BuildingType.kMuseum, "Museum");
            BuildingTypeMap.Add(BuildingType.kNoOfBuildingTypes, "None");
            BuildingTypeMap.Add(BuildingType.kOffice, "Office");
            BuildingTypeMap.Add(BuildingType.kParkingGarage, "Parking Garage");
            BuildingTypeMap.Add(BuildingType.kPenitentiary, "Penitentiary");
            BuildingTypeMap.Add(BuildingType.kPerformingArtsTheater, "Performing Arts Theater");
            BuildingTypeMap.Add(BuildingType.kPoliceStation, "Police Station");
            BuildingTypeMap.Add(BuildingType.kPostOffice, "Post Office");
            BuildingTypeMap.Add(BuildingType.kReligiousBuilding, "Religious Building");
            BuildingTypeMap.Add(BuildingType.kRetail, "Retail");
            BuildingTypeMap.Add(BuildingType.kSchoolOrUniversity, "School or University");
            BuildingTypeMap.Add(BuildingType.kSportsArena, "Sports Arena");
            BuildingTypeMap.Add(BuildingType.kTownHall, "Town Hall");
            BuildingTypeMap.Add(BuildingType.kTransportation, "Transportation");
            BuildingTypeMap.Add(BuildingType.kWarehouse, "Warehouse");
            BuildingTypeMap.Add(BuildingType.kWorkshop, "Workshop");
            #endregion

            #region ServiceTypeMap
            ServiceTypeMap = new Dictionary<object, string>();
            ServiceTypeMap.Add(ServiceType.kActiveChilledBeams, "Active Chilled Beams");
            ServiceTypeMap.Add(ServiceType.kCentralHeatingConvectors, "Central Heating: Convectors");
            ServiceTypeMap.Add(ServiceType.kCentralHeatingHotAir, "Central Heating: Hot Air");
            ServiceTypeMap.Add(ServiceType.kCentralHeatingRadiantFloor, "Central Heating: Radiant Floor");
            ServiceTypeMap.Add(ServiceType.kCentralHeatingRadiators, "Central Heating: Radiators");
            ServiceTypeMap.Add(ServiceType.kConstantVolumeDualDuct, "Constant Volume - Dual Duct");
            ServiceTypeMap.Add(ServiceType.kConstantVolumeFixedOA, "Constant Volume - Fixed OA");
            ServiceTypeMap.Add(ServiceType.kConstantVolumeTerminalReheat, "Constant Volume - Terminal Reheat");
            ServiceTypeMap.Add(ServiceType.kConstantVolumeVariableOA, "Constant Volume - Variable OA");
            ServiceTypeMap.Add(ServiceType.kFanCoilSystem, "Fan Coil System");
            ServiceTypeMap.Add(ServiceType.kForcedConvectionHeaterFlue, "Forced Convection Heater - Flue");
            ServiceTypeMap.Add(ServiceType.kForcedConvectionHeaterNoFlue, "Forced Convection Heater - No Flue");
            ServiceTypeMap.Add(ServiceType.kInductionSystem, "Induction System");
            ServiceTypeMap.Add(ServiceType.kMultizoneHotDeckColdDeck, "Multi-zone - Hot Deck / Cold Deck");
            ServiceTypeMap.Add(ServiceType.kNoServiceType, "None");
            ServiceTypeMap.Add(ServiceType.kOtherRoomHeater, "Other Room Heater");
            ServiceTypeMap.Add(ServiceType.kRadiantCooledCeilings, "Radiant Cooled Ceilings");
            ServiceTypeMap.Add(ServiceType.kRadiantHeaterFlue, "Radiant Heater - Flue");
            ServiceTypeMap.Add(ServiceType.kRadiantHeaterMultiburner, "Radiant Heater - Multi-burner");
            ServiceTypeMap.Add(ServiceType.kRadiantHeaterNoFlue, "Radiant Heater - No Flue");
            ServiceTypeMap.Add(ServiceType.kSplitSystemsWithMechanicalVentilation, "Split System(s) with Mechanical Ventilation");
            ServiceTypeMap.Add(ServiceType.kSplitSystemsWithMechanicalVentilationWithCooling, "Split System(s) with Mechanical Ventilation with Cooling");
            ServiceTypeMap.Add(ServiceType.kSplitSystemsWithNaturalVentilation, "Split System(s) with Natural Ventilation");
            ServiceTypeMap.Add(ServiceType.kVariableRefrigerantFlow, "Variable Refrigerant Flow");
            ServiceTypeMap.Add(ServiceType.kVAVDualDuct, "VAV - Dual Duct");
            ServiceTypeMap.Add(ServiceType.kVAVIndoorPackagedCabinet, "VAV - Indoor Packaged Cabinet");
            ServiceTypeMap.Add(ServiceType.kVAVSingleDuct, "VAV - Single Duct");
            ServiceTypeMap.Add(ServiceType.kWaterLoopHeatPump, "Water Loop Heat Pump"); 
            #endregion

            #region ExportComplexityMap
            ExportComplexityMap = new Dictionary<object, string>();
            ExportComplexityMap.Add(ExportComplexity.Complex, "Complex");
            ExportComplexityMap.Add(ExportComplexity.ComplexWithMullionsAndShadingSurfaces, "Complex With Mullions And Shading Surfaces");
            ExportComplexityMap.Add(ExportComplexity.ComplexWithShadingSurfaces, "Complex With Shading Surfaces");
            ExportComplexityMap.Add(ExportComplexity.Simple, "Simple");
            ExportComplexityMap.Add(ExportComplexity.SimpleWithShadingSurfaces, "Simple With Shading Surfaces");
            #endregion
        }
        #endregion
    };
}
