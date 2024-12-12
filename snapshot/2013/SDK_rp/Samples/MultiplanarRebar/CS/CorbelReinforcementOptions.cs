using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    /// <summary>
    /// Represent the reinforcement options of corbel.
    /// The options include bar type and bar counts which are collected from user via UI input.
    /// </summary>
    class CorbelReinforcementOptions
    {
        /// <summary>
        /// Active Revit DB Document.
        /// </summary>
        public Document RevitDoc { get; set; }

        /// <summary>
        /// List of RebarBarTypes in active document.
        /// </summary>
        public List<RebarBarType> RebarBarTypes { get; set; }

        /// <summary>
        /// RebarBarType for corbel top straight bars.
        /// </summary>
        public RebarBarType TopBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel stirrup bars.
        /// </summary>
        public RebarBarType StirrupBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel multi-planar bar.
        /// </summary>
        public RebarBarType MultiplanarBarType { get; set; }

        /// <summary>
        /// RebarBarType for corbel host straight bars.
        /// </summary>
        public RebarBarType HostStraightBarType { get; set; }

        /// <summary>
        /// Count of corbel straight bars.
        /// </summary>
        public int TopBarCount { get; set; }

        /// <summary>
        /// Count of corbel stirrup bars.
        /// </summary>
        public int StirrupBarCount { get; set; }

        /// <summary>
        /// Constructor to initialize the fields.
        /// </summary>
        /// <param name="revitDoc">Revit DB Document</param>
        public CorbelReinforcementOptions(Document revitDoc)
        {
            RevitDoc = revitDoc;
            RebarBarTypes = new List<RebarBarType>();
            RebarBarTypeSet barset = RevitDoc.RebarBarTypes;            
            foreach (RebarBarType bartype in barset)
            {
                RebarBarTypes.Add(bartype);
            }
        }      
    }
}
