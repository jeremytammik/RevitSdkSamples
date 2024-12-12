//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;

    /// <summary>
    /// data manager take charge of FamilySymbol object in current document
    /// </summary>
    public class FrameTypesMgr
    {
        // map list pairs FamilySymbol object and its Name 
        private Dictionary<String, FamilySymbol> m_symbolMaps;    
        // list of FamilySymbol objects
        private List<FamilySymbol> m_symbols;                    
        private ExternalCommandData m_commandData;

        /// <summary>
        /// command data pass from entry point
        /// </summary>
        public ExternalCommandData CommandData
        {
            get
            {
                return m_commandData;
            }
        }

        /// <summary>
        /// size of FamilySymbol objects in current Revit document
        /// </summary>
        public int Size
        {
            get
            {
                return m_symbolMaps.Count;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="commandData"></param>
        public FrameTypesMgr(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_symbolMaps = new Dictionary<String, FamilySymbol>();
            m_symbols = new List<FamilySymbol>();
        }

        /// <summary>
        /// constructor without parameters is forbidden
        /// </summary>
        private FrameTypesMgr()
        {
        }

        /// <summary>
        /// get list of FamilySymbol objects in current Revit document
        /// </summary>
        public ReadOnlyCollection<FamilySymbol> FramingSymbols
        {
            get
            {
                return new ReadOnlyCollection<FamilySymbol>(m_symbols);
            }
        }

        /// <summary>
        /// add one FamilySymbol object to the lists
        /// </summary>
        /// <param name="framingSymbol"></param>
        /// <returns></returns>
        public bool AddSymbol(Autodesk.Revit.DB.FamilySymbol framingSymbol)
        {
            if (ContainsSymbolName(framingSymbol.Name))
            {
                return false;
            }
            m_symbolMaps.Add(framingSymbol.Name, framingSymbol);
            m_symbols.Add(framingSymbol);
            return true;
        }

        /// delete one FamilySymbol both in Revit and lists here
        /// </summary>
        /// <param name="symbol">FamilySymbol to be deleted</param>
        /// <returns>successful to delete</returns>
        public bool DeleteSymbol(FamilySymbol symbol)
        {
            try
            {              
                // remove from the lists
                m_symbolMaps.Remove(symbol.Name);
                m_symbols.Remove(symbol);
                // delete from Revit
                List<ElementId> ids = m_commandData.Application.ActiveUIDocument.Document.Delete(symbol.Id) as List<ElementId>;
                if (ids.Count == 0)
                {
                    return false;
                }               
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// duplicate one FamilySymbol and add to lists
        /// </summary>
        /// <param name="framingSymbol">FamilySymbol to be copied</param>
        /// <param name="symbolName">duplicate FamilySymbol's Name</param>
        /// <returns>new FamilySymbol</returns>
        public FamilySymbol DuplicateSymbol(ElementType framingSymbol, string symbolName)
        {
            // duplicate a FamilySymbol
            ElementType symbol = framingSymbol.Duplicate(GenerateSymbolName(symbolName));
            FamilySymbol result = symbol as FamilySymbol;
            if (null != result)
            {
                // add to lists
                m_symbolMaps.Add(result.Name, result);
                m_symbols.Add(result);
            }
            return result;
        }

        /// <summary>
        /// inquire whether the FamilySymbol's Name already exists in the list
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public bool ContainsSymbolName(string symbolName)
        {
            return m_symbolMaps.ContainsKey(symbolName);
        }

        /// <summary>
        /// generate a new FamilySymbol's Name according to given name
        /// </summary>
        /// <param name="symbolName">original name</param>
        /// <returns>generated name</returns>
        public string GenerateSymbolName(string symbolName)
        {
            int suffix = 2;
            string result = symbolName;
            while (ContainsSymbolName(result))
            {
                result = symbolName + " " + suffix.ToString();
                suffix++;
            }
            return result;
        }
    }
}
