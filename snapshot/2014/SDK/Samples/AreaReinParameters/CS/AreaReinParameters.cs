//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.AreaReinParameters.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.Windows.Forms;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB.Structure;

    /// <summary>
    /// Entry point and main command class
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    class Command : IExternalCommand
    {
        static ExternalCommandData m_commandData;
        static Hashtable m_hookTypes;
        static Hashtable m_barTypes;
        AreaReinforcement m_areaRein;

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
        public Autodesk.Revit.UI.Result Execute(
          ExternalCommandData revit,
          ref string message,
          ElementSet elements)
        {
            m_commandData = revit;
            if (!PreData())
            {
                message = "Please select only one AreaReinforcement and ";
                message += "make sure there are Hook Types and Bar Types in current project.";
                return Autodesk.Revit.UI.Result.Failed;
            }

            IAreaReinData data = new WallAreaReinData();
            if (!data.FillInData(m_areaRein))
            {
                data = new FloorAreaReinData();
                if (!data.FillInData(m_areaRein))
                {
                    message = "Failed to get properties of selected AreaReinforcement.";
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }

            AreaReinParametersForm form = new AreaReinParametersForm(data);
            if (form.ShowDialog() == DialogResult.Cancel)
            {
                return Autodesk.Revit.UI.Result.Cancelled;
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// it is convenient for other class to get
        /// </summary>
        public static ExternalCommandData CommandData
        {
            get
            {
                return m_commandData;
            }
        }

        /// <summary>
        /// all hook types in current project
        /// it is static because of IConverter limitation
        /// </summary>
        public static Hashtable HookTypes
        {
            get
            {
                return m_hookTypes;
            }
        }

        /// <summary>
        /// all hook types in current project
        /// it is static because of IConverter limitation
        /// </summary>
        public static Hashtable BarTypes
        {
            get
            {
                return m_barTypes;
            }
        }

        /// <summary>
        /// check whether the selected is expected, find all hooktypes in current project
        /// </summary>
        /// <param name="selected">selected elements</param>
        /// <returns>whether the selected AreaReinforcement is expected</returns>
        private bool PreData()
        {
            ElementSet selected = m_commandData.Application.ActiveUIDocument.Selection.Elements;

            //selected is not only one AreaReinforcement
            if (selected.Size != 1)
            {
                return false;
            }
            foreach (Object o in selected)
            {
                m_areaRein = o as AreaReinforcement;
            }
            if (null == m_areaRein)
            {
                return false;
            }

            //make sure hook type and bar type exist in current project and get them
            m_hookTypes = new Hashtable();
            m_barTypes = new Hashtable();

            Document activeDoc = m_commandData.Application.ActiveUIDocument.Document;


            FilteredElementIterator itor = (new FilteredElementCollector(activeDoc)).OfClass(typeof(RebarHookType)).GetElementIterator();
            itor.Reset();
            while (itor.MoveNext())
            {
                RebarHookType hookType = itor.Current as RebarHookType;
                if (null != hookType)
                {
                    string hookTypeName = hookType.Name;
                    m_hookTypes.Add(hookTypeName, hookType.Id);
                }
            }

            itor = (new FilteredElementCollector(activeDoc)).OfClass(typeof(RebarBarType)).GetElementIterator();
            itor.Reset();
            while (itor.MoveNext())
            {
                RebarBarType barType = itor.Current as RebarBarType;
                if (null != barType)
                {
                    string barTypeName = barType.Name;
                    m_barTypes.Add(barTypeName, barType.Id);
                }
            }
            if (m_hookTypes.Count == 0 || m_barTypes.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    class RebarParas : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(
          ExternalCommandData revit,
          ref string message,
          ElementSet elements)
        {
            try
            {
                // Get the active document and view
                UIDocument revitDoc = revit.Application.ActiveUIDocument;
                Autodesk.Revit.DB.View view = revitDoc.Document.ActiveView;
                foreach (Autodesk.Revit.DB.Element elem in revitDoc.Selection.Elements)
                {
                    //if( elem.GetType() == typeof( Autodesk.Revit.DB.Structure.Rebar ) )
                    if (elem is Rebar)
                    {
                        string str = "";
                        Autodesk.Revit.DB.Structure.Rebar rebar = (Autodesk.Revit.DB.Structure.Rebar)elem;
                        ParameterSet pars = rebar.Parameters;
                        foreach (Parameter param in pars)
                        {
                            string val = "";
                            string name = param.Definition.Name;
                            Autodesk.Revit.DB.StorageType type = param.StorageType;
                            switch (type)
                            {
                                case Autodesk.Revit.DB.StorageType.Double:
                                    val = param.AsDouble().ToString();
                                    break;
                                case Autodesk.Revit.DB.StorageType.ElementId:
                                    Autodesk.Revit.DB.ElementId id = param.AsElementId();
                                    Autodesk.Revit.DB.Element paraElem = revitDoc.Document.GetElement(id);
                                    if (paraElem != null)
                                        val = paraElem.Name;
                                    break;
                                case Autodesk.Revit.DB.StorageType.Integer:
                                    val = param.AsInteger().ToString();
                                    break;
                                case Autodesk.Revit.DB.StorageType.String:
                                    val = param.AsString();
                                    break;
                                default:
                                    break;
                            }
                            str = str + name + ": " + val + "\r\n";
                        }
                        TaskDialog.Show("Rebar parameters", str);
                        return Autodesk.Revit.UI.Result.Succeeded;
                    }
                }
                message = "No rebar selected!";
                return Autodesk.Revit.UI.Result.Failed;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
    }
}
