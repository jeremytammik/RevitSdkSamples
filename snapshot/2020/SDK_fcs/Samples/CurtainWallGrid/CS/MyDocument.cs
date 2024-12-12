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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
    /// <summary>
    /// maintains all the data used in the sample
    /// </summary>
    public class MyDocument
    {
        #region Fields
        // object which contains reference of Revit Application
        ExternalCommandData m_commandData;
        /// <summary>
        /// object which contains reference of Revit Application
        /// </summary>
        public ExternalCommandData CommandData
        {
            get
            {
                return m_commandData;
            }
        }

        // the active document of Revit
        UIDocument m_uiDocument;
        /// <summary>
        /// the active document of Revit
        /// </summary>
        public UIDocument UIDocument
        {
            get
            {
                return m_uiDocument;
            }
        }

        // the active document of Revit
        Document m_document;
        /// <summary>
        /// the active document of Revit
        /// </summary>
        public Document Document
        {
            get
            {
                return m_document;
            }
        }

        // stores all the Curtain WallTypes in the active Revit document
        List<WallType> m_wallTypes;

        // stores all the ViewPlans in the active Revit document
        List<Autodesk.Revit.DB.View> m_views;

        // stores the wall creation related data and operations
        WallGeometry m_wallGeometry;

        // stores the curtain wall created
        Wall m_curtainWall;

        // indicates whether the curtain wall has been created
        bool m_wallCreated;

        // store the grid information of the created curtain wall
        GridGeometry m_gridGeometry;

        // store the active grid line operation
        LineOperation m_activeOperation;

        // the length unit type for the active Revit document
        DisplayUnitType m_LengthUnitType;

        // store the message of the sample
        private KeyValuePair<string/*msgText*/, bool/*is warningOrError*/> m_message;




        /// <summary>
        /// stores all the Curtain WallTypes in the active Revit document 
        /// </summary>
        public List<WallType> WallTypes
        {
            get
            {
                return m_wallTypes;
            }
        }

        /// <summary>
        /// stores all the ViewPlans in the active Revit document
        /// </summary>
        public List<Autodesk.Revit.DB.View> Views
        {
            get
            {
                return m_views;
            }
        }

        /// <summary>
        /// stores the wall creation related data and operations
        /// </summary>
        public WallGeometry WallGeometry
        {
            get
            {
                return m_wallGeometry;
            }
        }

        /// <summary>
        /// stores the curtain wall created
        /// </summary>
        public Wall CurtainWall
        {
            get
            {
                return m_curtainWall;
            }
            set
            {
                m_curtainWall = value;
            }
        }

        /// <summary>
        /// indicates whether the curtain wall has been created
        /// </summary>
        public bool WallCreated
        {
            get
            {
                return m_wallCreated;
            }
            set
            {
                m_wallCreated = value;
            }
        }

        /// <summary>
        /// store the grid information of the created curtain wall
        /// </summary>
        public GridGeometry GridGeometry
        {
            get
            {
                return m_gridGeometry;
            }
        }

        /// <summary>
        /// store the active grid line operation
        /// </summary>
        public LineOperation ActiveOperation
        {
            get
            {
                return m_activeOperation;
            }
            set
            {
                m_activeOperation = value;
            }
        }

        public DisplayUnitType LengthUnitType
        {
            get
            {
                return m_LengthUnitType;
            }
        }

        /// <summary>
        /// store the message of the sample
        /// </summary>
        public KeyValuePair<string/*msgText*/, bool/*is warningOrError*/> Message
        {
            get
            {
                return m_message;
            }
            set
            {
                m_message = value;
                if (null != MessageChanged)
                {
                    MessageChanged();
                }
            }
        }
        #endregion

        #region Delegates and Events
        // occurs only when the message was updated
        public delegate void MessageChangedHandler();
        public event MessageChangedHandler MessageChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="commandData">
        /// object which contains reference of Revit Application
        /// </param>
        public MyDocument(ExternalCommandData commandData)
        {
            if (null != commandData.Application.ActiveUIDocument)
            {
                m_commandData = commandData;
                m_uiDocument = m_commandData.Application.ActiveUIDocument;
                m_document = m_uiDocument.Document;
                m_views = new List<Autodesk.Revit.DB.View>();
                m_wallTypes = new List<WallType>();
                m_wallGeometry = new WallGeometry(this);
                m_wallCreated = false;
                m_gridGeometry = new GridGeometry(this);

                // get all the wall types and all the view plans
                InitializeData();

                // get the length unit type of the active Revit document
                GetLengthUnitType();

                // initialize the curtain grid operation type
                m_activeOperation = new LineOperation(LineOperationType.Waiting);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get current length display unit type
        /// </summary>
        private void GetLengthUnitType()
        {
            UnitType unitType = UnitType.UT_Length;
            Units projectUnit = m_document.GetUnits();
            try
            {
                Autodesk.Revit.DB.FormatOptions formatOption = projectUnit.GetFormatOptions(unitType);
                m_LengthUnitType = formatOption.DisplayUnits;
            }
            catch (System.Exception /*e*/)
            {
                m_LengthUnitType = DisplayUnitType.DUT_DECIMAL_FEET;
            }
        }

        /// <summary>
        /// get all the wall types for curtain wall and all the view plans from the active document
        /// </summary>
        private void InitializeData()
        {
            // get all the wall types
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_document);
            filteredElementCollector.OfClass(typeof(WallType));
            // just get all the curtain wall type
            m_wallTypes = filteredElementCollector.Cast<WallType>().Where<WallType>(wallType => wallType.Kind == WallKind.Curtain).ToList<WallType>();

            // sort them alphabetically
            WallTypeComparer wallComp = new WallTypeComparer();
            m_wallTypes.Sort(wallComp);

            // get all the ViewPlans
            m_views = SkipTemplateViews(GetElements<Autodesk.Revit.DB.View>());

            // sort them alphabetically
            ViewComparer viewComp = new ViewComparer();
            m_views.Sort(viewComp);

            // get one of the mullion types
            MullionTypeSet mullTypes = m_document.MullionTypes;
            foreach (MullionType type in mullTypes)
            {
                if (null != type)
                {
                    BuiltInParameter bip = BuiltInParameter.ALL_MODEL_FAMILY_NAME;
                    Parameter para = type.get_Parameter(bip);
                    if (null != para)
                    {
                        string name = para.AsString().ToLower();
                        if (name.StartsWith("circular mullion"))
                        {
                            m_gridGeometry.MullionType = type;
                        }
                    }

                }
            }
        }

        protected List<T> GetElements<T>() where T : Element
        {
            List<T> returns = new List<T>();
            FilteredElementCollector collector = new FilteredElementCollector(m_document);
            ICollection<Element> founds = collector.OfClass(typeof(T)).ToElements();
            foreach (Element elem in founds)
            {
                returns.Add(elem as T);
            }
            return returns;
        }

        /// <summary>
        /// View elements filtered by new iteration will include template views.
        /// These views are not invalid for test(because they're invisible in project browser) 
        /// Skip template views for regression test
        /// </summary>
        /// <param name="views"></param>
        /// <returns></returns>
        private List<T> SkipTemplateViews<T>(List<T> views) where T : Autodesk.Revit.DB.View
        {
            List<T> returns = new List<T>();
            foreach (Autodesk.Revit.DB.View curView in views)
            {
                if (null != curView && !curView.IsTemplate)
                    returns.Add(curView as T);
            }
            return returns;
        }
        #endregion
    }
}
