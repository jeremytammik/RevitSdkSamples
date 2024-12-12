//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REX.ElementReportHTML.Main
{
    /// <summary>
    /// Represents the node of the tree structure
    /// </summary>
    class Node
    {
        public const string levelRoot = "Root";
        public const string levelCategory = "Category";
        public const string levelElement = "Element";
        public const string levelParameter = "Parameter";

        /// <summary>
        /// Gets or sets the nodes list.
        /// </summary>
        /// <value>The nodes.</value>
        public Dictionary<string,Node> Nodes { get; private set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public string Level { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get
            {
                return Name + ": " + Id;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            Nodes = new Dictionary<string, Node>();
        }

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        /// <param name="level">The level.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Node AddNode(string name, string id, string level,string value)
        {
            if (!Nodes.ContainsKey(id))
            {
                Node newNode = new Node() { Name = name, Id = id, Level = level,Value = value};

                Nodes.Add(id, newNode);

                return newNode;
            }
            return null;
        }
        /// <summary>
        /// Gets the node of the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The node</returns>
        public Node GetNode(string id)
        {
            Node resultNode;

            if (Nodes.TryGetValue(id, out resultNode))
                return resultNode;

            return null;
        }
    }
}
