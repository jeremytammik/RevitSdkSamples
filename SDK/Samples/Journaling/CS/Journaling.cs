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
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Creation = Autodesk.Revit.Creation;

namespace Revit.SDK.Samples.Journaling.CS
{
    public class Journaling
    {
        /// <summary>
        /// Define a comparer which compare WallType by name property
        /// </summary>
        class WallTypeComparer : IComparer<WallType>
        {
            int IComparer<WallType>.Compare(WallType first, WallType second)
            {
                return String.Compare(first.Name, second.Name);
            }
        }

        // Private members
        ExternalCommandData m_commandData;  // Store the reference of command data
        bool m_canReadData;                 // Indicate whether has journal data

        Autodesk.Revit.DB.XYZ m_startPoint;       // Store the start point of the created wall
        Autodesk.Revit.DB.XYZ m_endPoint;         // Store the end point of the created wall
        Level m_createlevel;    // Store the level which the created wall on
        WallType m_createType;  // Store the type of the created wall

        List<Level> m_levelList;        // Store all levels in revit
        List<WallType> m_wallTypeList;  // Store all wall types in revit


        // Properties
        /// <summary>
        /// Give all levels in revit, and this information can be showed in UI
        /// </summary>
        public ReadOnlyCollection<Level> Levels
        {
            get
            {
                return new ReadOnlyCollection<Level>(m_levelList);
            }
        }


        /// <summary>
        /// Give all wall types in revit, and this information can be showed in UI
        /// </summary>
        public ReadOnlyCollection<WallType> WallTypes
        {
            get
            {
                return new ReadOnlyCollection<WallType>(m_wallTypeList);
            }
        }


        // Methods
        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="commandData">the external command data which revit give RevitAPI</param>
        public Journaling(ExternalCommandData commandData)
        {
            // Initialize the data members
            m_commandData = commandData;
            m_canReadData = (0 < commandData.JournalData.Count) ? true : false;

            // Initialize the two list data members
            m_levelList = new List<Level>();
            m_wallTypeList = new List<WallType>();
            InitializeListData();
        }


        /// <summary>
        /// This is the main deal method in this sample.
        /// It invoke methods to read and write journal data and create a wall using these data
        /// </summary>
        public void Run()
        {
            // According to it has journal data or not, this sample create wall in two ways
            if (m_canReadData)      // if it has journal data
            {
                ReadJournalData();  // read the journal data
                CreateWall();       // create a wall using the data
            }
            else                    // if it doesn't have journal data
            {
                if (!DisplayUI())   // display a form to collect some necessary data
                {
                    return;         // if the user cancels the form, only return
                }

                CreateWall();       // create a wall using the collected data
                WriteJournalData(); // write the journal data
            }

        }


        /// <summary>
        /// Set the necessary data which support the wall creation
        /// </summary>
        /// <param name="startPoint">the start point of the wall</param>
        /// <param name="endPoint">the end point of the wall</param>
        /// <param name="level">the level which the wall base on</param>
        /// <param name="type">the type of the wall</param>
        public void SetNecessaryData(Autodesk.Revit.DB.XYZ startPoint, Autodesk.Revit.DB.XYZ endPoint, Level level, WallType type)
        {
            m_startPoint = startPoint;  // start point
            m_endPoint = endPoint;      // end point
            m_createlevel = level;      // the level information
            m_createType = type;        // the wall type
        }


        /// <summary>
        /// Get the levels and wall types from revit and insert into the lists
        /// </summary>
        private void InitializeListData()
        {
            // Assert the lists have been constructed
            if (null == m_wallTypeList || null == m_levelList)
            {
                throw new Exception("necessary data members don't initialize.");
            }

            // Get all wall types from revit
            Document document = m_commandData.Application.ActiveUIDocument.Document;
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
            filteredElementCollector.OfClass(typeof(WallType));
            m_wallTypeList = filteredElementCollector.Cast<WallType>().ToList<WallType>();

            // Sort the wall type list by the name property
            WallTypeComparer comparer = new WallTypeComparer();
            m_wallTypeList.Sort(comparer);

            // Get all levels from revit 
            FilteredElementIterator iter = (new FilteredElementCollector(document)).OfClass(typeof(Level)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Level level = iter.Current as Level;
                if (null == level)
                {
                    continue;
                }
                m_levelList.Add(level);
            }
        }


        /// <summary>
        /// Read the journal data from the journal.
        /// All journal data is stored in commandData.Data.
        /// </summary>
        private void ReadJournalData()
        {
            // Get the journal data map from API
            Document doc = m_commandData.Application.ActiveUIDocument.Document;
            IDictionary<string, string> dataMap = m_commandData.JournalData;

            String dataValue = null;    // store the journal data value temporarily

            // Get the wall type from the journal
            dataValue = GetSpecialData(dataMap, "Wall Type Name"); // get wall type name
            foreach (WallType type in m_wallTypeList)   // get the wall type by the name
            {
                if (dataValue == type.Name)
                {
                    m_createType = type;
                    break;
                }
            }
            if (null == m_createType)   // assert the wall type is exist
            {
                throw new InvalidDataException("Can't find the wall type from the journal.");
            }

            // Get the level information from the journal
            dataValue = GetSpecialData(dataMap, "Level Id");   // get the level id
            Autodesk.Revit.DB.ElementId id = Autodesk.Revit.DB.ElementId.Parse(dataValue);     // get the level by its id

            m_createlevel = doc.GetElement(id) as Level;
            if (null == m_createlevel)  // assert the level is exist
            {
                throw new InvalidDataException("Can't find the level from the journal.");
            }

            // Get the start point information from the journal
            dataValue = GetSpecialData(dataMap, "Start Point");
            m_startPoint = StirngToXYZ(dataValue);

            // Get the end point information from the journal
            dataValue = GetSpecialData(dataMap, "End Point");
            m_endPoint = StirngToXYZ(dataValue);

            // Create wall don't allow the start point equals end point
            if (m_startPoint.Equals(m_endPoint))
            {
                throw new InvalidDataException("Start point is equal to end point.");
            }
        }


        /// <summary>
        /// Display the UI form to collect some necessary information for create wall.
        /// The information will be write into the journal
        /// </summary>
        /// <returns></returns>
        private bool DisplayUI()
        {
            // Display the form and allow the user to input some information for wall creation
            using (JournalingForm displayForm = new JournalingForm(this))
            {
                displayForm.ShowDialog();
                if (DialogResult.OK != displayForm.DialogResult)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Create a wall with the collected data or the data read from journal 
        /// </summary>
        private void CreateWall()
        {
            // Get the create classes.
            Creation.Application createApp = m_commandData.Application.Application.Create;
            Creation.Document createDoc = m_commandData.Application.ActiveUIDocument.Document.Create;

            // Create geometry line(curve)
            Line geometryLine = Line.CreateBound(m_startPoint, m_endPoint);
            if (null == geometryLine)           // assert the creation is successful
            {
                throw new Exception("Create the geometry line failed.");
            }

            // Create the wall using the wall type, level and created geometry line
            Wall createdWall = Wall.Create(m_commandData.Application.ActiveUIDocument.Document, geometryLine, m_createType.Id, m_createlevel.Id,
                                    15, m_startPoint.Z + m_createlevel.Elevation, true, true);
            if (null == createdWall)            // assert the creation is successful
            {
                throw new Exception("Create the wall failed.");
            }
        }


        /// <summary>
        /// Write the support data into the journal
        /// </summary>
        private void WriteJournalData()
        {
            // Get the StringStringMap class which can write support into.
            IDictionary<string, string> dataMap = m_commandData.JournalData;
            dataMap.Clear();

            // Begin to add the support data
            dataMap.Add("Wall Type Name", m_createType.Name);    // add wall type name
            dataMap.Add("Level Id", m_createlevel.Id.ToString());  // add level id
            dataMap.Add("Start Point", XYZToString(m_startPoint));   // add start point
            dataMap.Add("End Point", XYZToString(m_endPoint));   // add end point
        }


        /// <summary>
        /// The helper function which convert a format string to a Autodesk.Revit.DB.XYZ point
        /// </summary>
        /// <param name="pointString">a format string</param>
        /// <returns>the converted Autodesk.Revit.DB.XYZ point</returns>
        private static Autodesk.Revit.DB.XYZ StirngToXYZ(String pointString)
        {
            // Define some temporary data
            double x = 0;   // Store the temporary x coordinate
            double y = 0;   // Store the temporary y coordinate
            double z = 0;   // Store the temporary z coordinate
            String subString;   // A part of the format string 

            // Get the data string from the format string
            subString = pointString.TrimStart('(');
            subString = subString.TrimEnd(')');
            String[] coordinateString = subString.Split(',');
            if (3 != coordinateString.Length)
            {
                throw new InvalidDataException("The point information in journal is incorrect");
            }

            // Return a Autodesk.Revit.DB.XYZ point using the collected data
            try
            {
                x = Convert.ToDouble(coordinateString[0]);
                y = Convert.ToDouble(coordinateString[1]);
                z = Convert.ToDouble(coordinateString[2]);
            }
            catch (Exception)
            {
                throw new InvalidDataException("The point information in journal is incorrect");
            }
            return new Autodesk.Revit.DB.XYZ(x, y, z);
        }


        /// <summary>
        /// The helper function which convert a Autodesk.Revit.DB.XYZ point to a format string
        /// </summary>
        /// <param name="point">A Autodesk.Revit.DB.XYZ point</param>
        /// <returns>The format string which store the information of the point</returns>
        private static String XYZToString(Autodesk.Revit.DB.XYZ point)
        {

            String pointString = "(" + point.X.ToString() + "," + point.Y.ToString() + ","
                                                    + point.Z.ToString() + ")";
            return pointString;
        }


        /// <summary>
        /// Get the data which is related to a special key in journal
        /// </summary>
        /// <param name="dataMap">the map which store the journal data</param>
        /// <param name="key">a key which indicate which data to get</param>
        /// <returns>The gotten data in string format</returns>
        private static String GetSpecialData(IDictionary<string, string> dataMap, String key)
        {
            String dataValue = dataMap[key];

            if (String.IsNullOrEmpty(dataValue))
            {
                throw new Exception(key + "information is not exist in journal.");
            }
            return dataValue;
        }
    }
}
