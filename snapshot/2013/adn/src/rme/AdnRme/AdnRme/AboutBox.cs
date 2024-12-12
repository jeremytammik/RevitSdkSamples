#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Windows.Forms;
using System.Reflection;
#endregion // Namespaces

namespace AdnRme
{
  partial class AboutBox : Form
  {
    public AboutBox()
    {
      InitializeComponent();

      // Initialize the AboutBox to display the product 
      // information from the assembly information.
      // Change assembly information settings for your 
      // application through either Project > Properties 
      // > Application > Assembly Information or
      // by editing AssemblyInfo.cs

      Text = "About " + AssemblyTitle;
      labelProductName.Text = AssemblyProduct;
      labelVersion.Text = "Version " + AssemblyVersion;
      labelCopyright.Text = AssemblyCopyright;
      labelCompanyName.Text = AssemblyCompany;
      textBoxDescription.Text = AssemblyDescription;
    }

    #region Assembly Attribute Accessors

    /// <summary>
    /// Short cut to get executing assembly
    /// </summary>
    Assembly ExecutingAssembly
    {
      get
      {
        return Assembly.GetExecutingAssembly();
      }
    }

    object GetFirstCustomAttribute( Type t )
    {
      Assembly a = ExecutingAssembly;
      object[] attributes 
        = a.GetCustomAttributes( t, false );
      return ( 0 < attributes.Length )
        ? attributes[0]
        : null;
    }

    public string AssemblyTitle
    {
      get
      {
        // Get all Title attributes on this assembly
        //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyTitleAttribute ), false );
        //object[] attributes = GetCustomAttributes( typeof( AssemblyTitleAttribute ) );
        object a = GetFirstCustomAttribute( typeof( AssemblyTitleAttribute ) );
        // If there is at least one Title attribute
        if( null != a )
        {
          // Select the first one
          AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute) a;
          // If it is not an empty string, return it
          if( titleAttribute.Title != "" )
            return titleAttribute.Title;
        }
        // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
        return System.IO.Path.GetFileNameWithoutExtension( ExecutingAssembly.CodeBase );
      }
    }

    public string AssemblyVersion
    {
      get
      {
        return ExecutingAssembly.GetName().Version.ToString();
      }
    }

    public string AssemblyDescription
    {
      get
      {
        // Get all Description attributes on this assembly
        //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyDescriptionAttribute ), false );
        //object[] attributes = GetCustomAttributes( typeof( AssemblyDescriptionAttribute ) );
        object a = GetFirstCustomAttribute( typeof( AssemblyDescriptionAttribute ) );
        // If there aren't any Description attributes, return an empty string
        //if( attributes.Length == 0 )
        //  return "";
        // If there is a Description attribute, return its value
        //return ( (AssemblyDescriptionAttribute) attributes[0] ).Description;
        return (null == a) 
          ? string.Empty 
          : ( (AssemblyDescriptionAttribute) a ).Description;
      }
    }

    public string AssemblyProduct
    {
      get
      {
        // Get all Product attributes on this assembly
        //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyProductAttribute ), false );
        //object[] attributes = GetCustomAttributes( typeof( AssemblyProductAttribute ) );
        object a = GetFirstCustomAttribute( typeof( AssemblyProductAttribute ) );
        // If there aren't any Product attributes, return an empty string
        //if( attributes.Length == 0 )
        //  return "";
        // If there is a Product attribute, return its value
        //return ( (AssemblyProductAttribute) attributes[0] ).Product;
        return ( null == a )
          ? string.Empty
          : ( ( AssemblyProductAttribute ) a ).Product;
      }
    }

    public string AssemblyCopyright
    {
      get
      {
        // Get all Copyright attributes on this assembly
        //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false );
        //object[] attributes = GetCustomAttributes( typeof( AssemblyCopyrightAttribute ) );
        object a = GetFirstCustomAttribute( typeof( AssemblyCopyrightAttribute ) );
        // If there aren't any Copyright attributes, return an empty string
        //if( attributes.Length == 0 )
        //  return "";
        // If there is a Copyright attribute, return its value
        //return ( (AssemblyCopyrightAttribute) attributes[0] ).Copyright;
        return ( null == a )
          ? string.Empty
          : ( ( AssemblyCopyrightAttribute ) a ).Copyright;
      }
    }

    public string AssemblyCompany
    {
      get
      {
        // Get all Company attributes on this assembly
        //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyCompanyAttribute ), false );
        //object[] attributes = GetCustomAttributes( typeof( AssemblyCompanyAttribute ) );
        object a = GetFirstCustomAttribute( typeof( AssemblyCompanyAttribute ) );
        // If there aren't any Company attributes, return an empty string
        //if( attributes.Length == 0 )
        //  return "";
        // If there is a Company attribute, return its value
        //return ( (AssemblyCompanyAttribute) attributes[0] ).Company;
        return ( null == a )
          ? string.Empty
          : ( ( AssemblyCompanyAttribute ) a ).Company;
      }
    }
    #endregion
  }
}
