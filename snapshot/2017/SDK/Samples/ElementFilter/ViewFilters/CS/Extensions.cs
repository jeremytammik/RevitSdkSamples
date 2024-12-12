//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;

namespace Revit.SDK.Samples.ViewFilters.CS
{
    /// <summary>
    /// Generic utility class used to extend enum parse related methods
    /// </summary>
    /// <typeparam name="TEnum">Enum type for this class.</typeparam>
    public static class EnumParseUtility<TEnum>
    {
        /// <summary>
        /// Parse string to TEnum values
        /// </summary>
        /// <param name="strValue">Enum value in string.</param>
        /// <returns>TEnum value is parsed from input string.</returns>
        public static TEnum Parse(string strValue)
        {
            if (!typeof(TEnum).IsEnum) return default(TEnum);
            return (TEnum)Enum.Parse(typeof(TEnum), strValue);
        }

        /// <summary>
        /// Parse TEnum to string 
        /// </summary>
        /// <param name="enumVal">TEnum value to be parsed.</param>
        /// <returns>String parsed from input TEnum.</returns>
        public static String Parse(TEnum enumVal)
        {
            if (!typeof(TEnum).IsEnum) return String.Empty;
            return Enum.GetName(typeof(TEnum), enumVal);
        }

        /// <summary>
        /// Parse TEnum from integer value to string
        /// </summary>
        /// <param name="enumValInt">Integer value to be parsed.</param>
        /// <returns>String parsed from input TEnum(integer type)</returns>
        public static String Parse(int enumValInt)
        {
            if (!typeof(TEnum).IsEnum) return String.Empty;
            return Enum.GetName(typeof(TEnum), enumValInt);
        }
    }

    /// <summary>
    /// Generic utility class used to deal with List compare related methods.
    /// </summary>
    /// <typeparam name="T">Object type in list.</typeparam>
    public static class ListCompareUtility<T>
    {
        /// <summary>
        /// Check to see if two lists are equals
        /// When comparing, two list will be checked on amounts and contents
        /// </summary>
        /// <param name="coll1">1st list.</param>
        /// <param name="coll2">2nd list.</param>
        /// <returns>True if two list are identical on data.</returns>
        public static bool Equals(ICollection<T> coll1, ICollection<T> coll2)
        {
            if (coll1.Count != coll2.Count) return false;
            foreach (T val1 in coll1)
            {
                if (!coll2.Contains(val1)) return false;
            }
            foreach (T val2 in coll2)
            {
                if (!coll1.Contains(val2)) return false;
            }
            return true;
        }
    }
}
