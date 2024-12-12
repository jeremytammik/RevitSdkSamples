//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Resources;
using System.Reflection;
using System.Globalization;
using Autodesk.Revit;

namespace Revit.SDK.Samples.RDBLink.CS
{
    /// <summary>
    /// Provides static functions to get resource string from resource file
    /// </summary>
    public static class RDBResource
    {
        #region Fields
        /// <summary>
        /// ResourceManager
        /// </summary>
        static ResourceManager resource;

        /// <summary>
        /// Prefix of a table resource id
        /// </summary>
        public const string TableKeyPrefix = "TabN_";

        /// <summary>
        /// Suffix of a table resource id which contains instances
        /// </summary>
        public const string TableKeyInstanceSuffix = "_INSTANCE";

        /// <summary>
        /// Suffix of a table resource id which contains instances
        /// </summary>
        public const string TableKeySymbolSuffix = "_SYMBOL";

        #endregion

        #region Constructors
        /// <summary>
        /// Create ResourceManager
        /// </summary>
        static RDBResource()
        {
            resource = new ResourceManager(
                "Revit.SDK.Samples.RDBLink.CS.RDBLinkData",
                Assembly.GetExecutingAssembly());
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Get the localized resource value
        /// </summary>
        /// <param name="key">Resource id</param>
        /// <returns>Resource value</returns>
        public static string GetString(string key)
        {
            return resource.GetString(key);
        }

        /// <summary>
        /// Get database column name from resource and remove some invalid characters
        /// </summary>
        /// <param name="key">Resource id</param>
        /// <returns>database column name</returns>
        public static string GetColumnName(string key)
        {
            string value = GetString(key);
            foreach (char ch in DatabaseConfig.Instance().InvalidCharacters)
            {
                value = value.Replace(ch.ToString(), "");
            }
            return value;
        }

        /// <summary>
        /// Get database table name from resource and remove some invalid characters
        /// </summary>
        /// <param name="key">Resource id</param>
        /// <returns>database table name</returns>
        public static string GetTableName(string key)
        {
            if (key == null) return null;
            string value = GetString(key);
            if (value == null) 
                return null;

            string mapValue = null;
            DatabaseConfig.Instance().KeyWordsMap.TryGetValue(value, out mapValue);
            return mapValue == null ? value : mapValue;
        }

        /// <summary>
        /// Get table resource id from a category and its type (element or symbol)
        /// </summary>
        /// <param name="category">category</param>
        /// <param name="isSymbol">true if the table contains symbols, false if instances</param>
        /// <returns>table resource id</returns>
        public static string GetTableKey(Autodesk.Revit.Category category, bool isSymbol)
        {
            if (category == null) return null;
            BuiltInCategory buInCat = (BuiltInCategory)category.Id.Value;
            string tableKeySuffix = isSymbol ? TableKeySymbolSuffix : TableKeyInstanceSuffix;
            return TableKeyPrefix + buInCat.ToString() + tableKeySuffix;
        }

        #endregion
    }
}
