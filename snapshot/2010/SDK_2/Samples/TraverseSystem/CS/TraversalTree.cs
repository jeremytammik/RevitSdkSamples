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
using System.Text;
using System.Xml;

using Autodesk.Revit;
using Autodesk.Revit.MEP;
using Autodesk.Revit.Elements;
using Autodesk.Revit.MEP.Enums;

namespace Revit.SDK.Samples.TraverseSystem.CS
{
    /// <summary>
    /// A TreeNode object represents an element in the system
    /// </summary>
    public class TreeNode
    {
        #region Member variables
        /// <summary>
        /// Id of the element
        /// </summary>
        private ElementId m_Id;
        /// <summary>
        /// Flow direction of the node
        /// For the starting element of the traversal, the direction will be the same as the connector
        /// connected to its following element; Otherwise it will be the direction of the connector connected to
        /// its previous element
        /// </summary>
        private FlowDirectionType m_direction;
        /// <summary>
        /// The parent node of the current node.
        /// </summary>
        private TreeNode m_parent;
        /// <summary>
        /// The connector of the previous element to which current element is connected
        /// </summary>
        private Connector m_inputConnector;
        /// <summary>
        /// The first-level child nodes of the current node
        /// </summary>
        private List<TreeNode> m_childNodes;
        /// <summary>
        /// Active document of Revit
        /// </summary>
        private Document m_document;
        #endregion

        #region Properties
        /// <summary>
        /// Id of the element
        /// </summary>
        public ElementId Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// Flow direction of the node
        /// </summary>
        public FlowDirectionType Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
            }
        }

        /// <summary>
        /// Gets and sets the parent node of the current node.
        /// </summary>
        public TreeNode Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value;
            }
        }

        /// <summary>
        /// Gets and sets the first-level child nodes of the current node
        /// </summary>
        public List<TreeNode> ChildNodes
        {
            get
            {
                return m_childNodes;
            }
            set
            {
                m_childNodes = value;
            }
        }

        /// <summary>
        /// The connector of the previous element to which current element is connected
        /// </summary>
        public Connector InputConnector
        {
            get
            {
                return m_inputConnector;
            }
            set
            {
                m_inputConnector = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <param name="id">Element's Id</param>
        public TreeNode(Document doc, ElementId id)
        {
            m_document = doc;
            m_Id = id;
            m_childNodes = new List<TreeNode>();
        }

        /// <summary>
        /// Get Element by its Id
        /// </summary>
        /// <param name="eid">Element's Id</param>
        /// <returns>Element</returns>
        private Element GetElementById(ElementId eid)
        {
            return m_document.get_Element(ref eid);
        }

        /// <summary>
        /// Dump the node into XML file
        /// </summary>
        /// <param name="writer">XmlWriter object</param>
        public void DumpIntoXML(XmlWriter writer)
        {
            // Write node information
            Element element = GetElementById(m_Id);
            FamilyInstance fi = element as FamilyInstance;
            if (fi != null)
            {
                MEPModel mepModel = fi.MEPModel;
                String type = String.Empty;
                if (mepModel is MechanicalEquipment)
                {
                    type = "MechanicalEquipment";
                    writer.WriteStartElement(type);
                }
                else if (mepModel is MechanicalFitting)
                {
                    MechanicalFitting mf = mepModel as MechanicalFitting;
                    type = "MechanicalFitting";
                    writer.WriteStartElement(type);
                    writer.WriteAttributeString("Category", element.Category.Name);
                    writer.WriteAttributeString("PartType", mf.PartType.ToString());
                }
                else
                {
                    type = "FamilyInstance";
                    writer.WriteStartElement(type);
                    writer.WriteAttributeString("Category", element.Category.Name);
                }

                writer.WriteAttributeString("Name", element.Name);
                writer.WriteAttributeString("Id", element.Id.Value.ToString());
                writer.WriteAttributeString("Direction", m_direction.ToString());
                writer.WriteEndElement();
            }
            else
            {
                String type = element.GetType().Name;

                writer.WriteStartElement(type);
                writer.WriteAttributeString("Name", element.Name);
                writer.WriteAttributeString("Id", element.Id.Value.ToString());
                writer.WriteAttributeString("Direction", m_direction.ToString());
                writer.WriteEndElement();
            }

            foreach (TreeNode node in m_childNodes)
            {
                if (m_childNodes.Count > 1)
                {
                    writer.WriteStartElement("Path");
                }
                
                node.DumpIntoXML(writer);

                if (m_childNodes.Count > 1)
                {
                    writer.WriteEndElement();
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Data structure of the traversal
    /// </summary>
    public class TraversalTree
    {
        #region Member variables
        // Active document of Revit
        private Document m_document;
        // The MEP system of the traversal
        private MEPSystem m_system;
        // The flag whether the MEP system of the traversal is a mechanical system or piping system
        private Boolean m_isMechanicalSystem;
        // The starting element node
        private TreeNode m_startingElementNode;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activeDocument">Revit document</param>
        /// <param name="system">The MEP system to traverse</param>
        public TraversalTree(Document activeDocument, MEPSystem system)
        {
            m_document = activeDocument;
            m_system = system;
            m_isMechanicalSystem = (system is MechanicalSystem);
        }

        /// <summary>
        /// Traverse the system
        /// </summary>
        public void Traverse()
        {
            // Get the starting element node
            m_startingElementNode = GetStartingElementNode();

            // Traverse the system recursively
            Traverse(m_startingElementNode);
        }

        /// <summary>
        /// Get the starting element node.
        /// If the system has base equipment then get it;
        /// Otherwise get the owner of the open connector in the system
        /// </summary>
        /// <returns>The starting element node</returns>
        private TreeNode GetStartingElementNode()
        {
            TreeNode startingElementNode = null;

            FamilyInstance equipment = m_system.BaseEquipment;
            //
            // If the system has base equipment then get it;
            // Otherwise get the owner of the open connector in the system
            if (equipment != null)
            {
                startingElementNode = new TreeNode(m_document, equipment.Id);
            }
            else
            {
                startingElementNode = new TreeNode(m_document, GetOwnerOfOpenConnector().Id);
            }

            startingElementNode.Parent = null;
            startingElementNode.InputConnector = null;

            return startingElementNode;
        }

        /// <summary>
        /// Get the owner of the open connector as the starting element
        /// </summary>
        /// <returns>The owner</returns>
        private Element GetOwnerOfOpenConnector()
        {
            Element element = null;

            //
            // Get an element from the system's terminals
            ElementSet elements = m_system.Elements;
            foreach (Element ele in elements)
            {
                element = ele;
                break;
            }

            // Get the open connector recursively
            Connector openConnector = GetOpenConnector(element, null);

            return openConnector.Owner;
        }

        /// <summary>
        /// Get the open connector of the system if the system has no base equipment
        /// </summary>
        /// <param name="element">An element in the system</param>
        /// <param name="inputConnector">The connector of the previous element 
        /// to which the element is connected </param>
        /// <returns>The found open connector</returns>
        private Connector GetOpenConnector(Element element, Connector inputConnector)
        {
            Connector openConnector = null;
            ConnectorManager cm = null;
            //
            // Get the connector manager of the element
            if (element is FamilyInstance)
            {
                FamilyInstance fi = element as FamilyInstance;
                cm = fi.MEPModel.ConnectorManager;
            }
            else
            {
                MEPCurve mepCurve = element as MEPCurve;
                cm = mepCurve.ConnectorManager;
            }

            foreach (Connector conn in cm.Connectors)
            {
                // Ignore the connector does not belong to any MEP System or belongs to another different MEP system
                if (conn.MEPSystem == null || !conn.MEPSystem.Id.Value.Equals(m_system.Id.Value))
                {
                    continue;
                }

                // If the connector is connected to the input connector, they will have opposite flow directions.
                if (inputConnector != null && conn.IsConnectedTo(inputConnector))
                { 
                    continue;
                }

                // If the connector is not connected, it is the open connector
                if (!conn.IsConnected)
                {
                    openConnector = conn;
                    break;
                }

                //
                // If open connector not found, then look for it from elements connected to the element
                foreach (Connector refConnector in conn.AllRefs)
                {
                    // Ignore non-EndConn connectors and connectors of the current element
                    if (refConnector.ConnectorType != ConnectorType.EndConn || 
                        refConnector.Owner.Id.Value.Equals(conn.Owner.Id.Value))
                    {
                        continue;
                    }

                    // Ignore connectors of the previous element
                    if (inputConnector != null && refConnector.Owner.Id.Value.Equals(inputConnector.Owner.Id.Value))
                    {
                        continue;
                    }

                    openConnector = GetOpenConnector(refConnector.Owner, conn);
                    if (openConnector != null)
                    {
                        return openConnector;
                    }
                }
            }

            return openConnector;
        }

        /// <summary>
        /// Traverse the system recursively by analyzing each element
        /// </summary>
        /// <param name="elementNode">The element to be analyzed</param>
        private void Traverse(TreeNode elementNode)
        {
            //
            // Find all child nodes and analyze them recursively
            AppendChildren(elementNode);
            foreach (TreeNode node in elementNode.ChildNodes)
            {
                Traverse(node);
            }
        }

        /// <summary>
        /// Find all child nodes of the specified element node
        /// </summary>
        /// <param name="elementNode">The specified element node to be analyzed</param>
        private void AppendChildren(TreeNode elementNode)
        {
            List<TreeNode> nodes = elementNode.ChildNodes;
            ConnectorSet connectors;
            //
            // Get connector manager
            Element element = GetElementById(elementNode.Id);
            FamilyInstance fi = element as FamilyInstance;
            if (fi != null)
            {
                connectors = fi.MEPModel.ConnectorManager.Connectors;
            }
            else
            {
                MEPCurve mepCurve = element as MEPCurve;
                connectors = mepCurve.ConnectorManager.Connectors;
            }

            // Find connected connector for each connector
            foreach (Connector connector in connectors)
            {
                MEPSystem mepSystem = connector.MEPSystem;
                // Ignore the connector does not belong to any MEP System or belongs to another different MEP system
                if (mepSystem == null || !mepSystem.Id.Value.Equals(m_system.Id.Value))
                {
                    continue;
                }

                //
                // Get the direction of the TreeNode object
                if (elementNode.Parent == null)
                {
                    if (connector.IsConnected)
                    {
                        elementNode.Direction = connector.Direction;
                    }
                }
                else
                {
                    // If the connector is connected to the input connector, they will have opposite flow directions.
                    // Then skip it.
                    if (connector.IsConnectedTo(elementNode.InputConnector))
                    {
                        elementNode.Direction = connector.Direction;
                        continue;
                    }
                }

                // Get the connector connected to current connector
                Connector connectedConnector = GetConnectedConnector(connector);
                if (connectedConnector != null)
                {
                    TreeNode node = new TreeNode(m_document, connectedConnector.Owner.Id);
                    node.InputConnector = connector;
                    node.Parent = elementNode;
                    nodes.Add(node);
                }
            }

            nodes.Sort(delegate(TreeNode t1, TreeNode t2)
                       {
                           return t1.Id.Value > t2.Id.Value ? 1 : (t1.Id.Value < t2.Id.Value ? -1 : 0);
                       }
            );
        }

        /// <summary>
        /// Get the connected connector of one connector
        /// </summary>
        /// <param name="connector">The connector to be analyzed</param>
        /// <returns>The connected connector</returns>
        static private Connector GetConnectedConnector(Connector connector)
        {
            Connector connectedConnector = null;
            ConnectorSet allRefs = connector.AllRefs;
            foreach (Connector conn in allRefs)
            {
                // Ignore non-EndConn connectors and connectors of the current element
                if (conn.ConnectorType != ConnectorType.EndConn || 
                    conn.Owner.Id.Value.Equals(connector.Owner.Id.Value))
                {
                    continue;
                }

                connectedConnector = conn;
                break;
            }

            return connectedConnector;
        }

        /// <summary>
        /// Get element by its id
        /// </summary>
        private Element GetElementById(ElementId eid)
        {
            return m_document.get_Element(ref eid);
        }

        /// <summary>
        /// Dump the traversal into an XML file
        /// </summary>
        /// <param name="fileName">Name of the XML file</param>
        public void DumpIntoXML(String fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            XmlWriter writer = XmlWriter.Create(fileName, settings);

            // Write the root element
            String mepSystemType = String.Empty;
            mepSystemType = (m_system is MechanicalSystem ? "MechanicalSystem" : "PipingSystem");
            writer.WriteStartElement(mepSystemType);

            // Write basic information of the MEP system
            WriteBasicInfo(writer);
            // Write paths of the traversal
            WritePaths(writer);

            // Close the root element
            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Write basic information of the MEP system into the XML file
        /// </summary>
        /// <param name="writer">XMLWriter object</param>
        private void WriteBasicInfo(XmlWriter writer)
        {
            MechanicalSystem ms = null;
            PipingSystem ps = null;
            if (m_isMechanicalSystem)
            {
                ms = m_system as MechanicalSystem;
            }
            else
            {
                ps = m_system as PipingSystem;
            }

            // Write basic information of the system
            writer.WriteStartElement("BasicInformation");

            // Write Name property
            writer.WriteStartElement("Name");
            writer.WriteString(m_system.Name);
            writer.WriteEndElement();

            // Write Id property
            writer.WriteStartElement("Id");
            writer.WriteValue(m_system.Id.Value);
            writer.WriteEndElement();

            // Write UniqueId property
            writer.WriteStartElement("UniqueId");
            writer.WriteString(m_system.UniqueId);
            writer.WriteEndElement();

            // Write SystemType property
            writer.WriteStartElement("SystemType");
            if (m_isMechanicalSystem)
            {
                writer.WriteString(ms.SystemType.ToString());
            }
            else
            {
                writer.WriteString(ps.SystemType.ToString());
            }
            writer.WriteEndElement();

            // Write Category property
            writer.WriteStartElement("Category");
            writer.WriteAttributeString("Id", m_system.Category.Id.Value.ToString());
            writer.WriteAttributeString("Name", m_system.Category.Name);
            writer.WriteEndElement();

            // Write IsWellConnected property
            writer.WriteStartElement("IsWellConnected");
            if (m_isMechanicalSystem)
            {
                writer.WriteValue(ms.IsWellConnected);
            }
            else
            {
                writer.WriteValue(ps.IsWellConnected);
            }
            writer.WriteEndElement();

            // Write IsDefaultSystem property
            writer.WriteStartElement("IsDefaultSystem");
            writer.WriteValue(m_system.IsDefaultSystem);
            writer.WriteEndElement();

            // Write HasBaseEquipment property
            writer.WriteStartElement("HasBaseEquipment");
            bool hasBaseEquipment = ((m_system.BaseEquipment == null) ? false : true);
            writer.WriteValue(hasBaseEquipment);
            writer.WriteEndElement();

            // Write TerminalElementsCount property
            writer.WriteStartElement("TerminalElementsCount");
            writer.WriteValue(m_system.Elements.Size);
            writer.WriteEndElement();

            // Write Flow property
            writer.WriteStartElement("Flow");
            if (m_isMechanicalSystem)
            {
                writer.WriteValue(ms.Flow);
            }
            else
            {
                writer.WriteValue(ps.Flow);
            }
            writer.WriteEndElement();

            // Close basic information
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write paths of the traversal into the XML file
        /// </summary>
        /// <param name="writer">XMLWriter object</param>
        private void WritePaths(XmlWriter writer)
        {
            writer.WriteStartElement("Path");
            m_startingElementNode.DumpIntoXML(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}
