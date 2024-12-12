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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.CodeChecking.Engineering;

namespace CodeCheckingConcreteExample.Engine
{
   /// <summary>
   /// Base interface for the main loop class. Parameter of the constructor of the cref="Engine" class.
   /// </summary>
   public interface IEngineData
   {
      /// <summary>
      /// Gets the unit system
      /// </summary>
      /// <returns>cref="DisplayUnit"</returns>
      DisplayUnit GetInputDataUnitSystem();
      /// <summary>
      /// Gets number of available threads or processors.
      /// </summary>
      /// <param name="data">Service data.</param>
      /// <returns>Number of threads</returns>
      int GetNumberOfThreads(Autodesk.Revit.DB.CodeChecking.ServiceData data);

      /// <structural_toolkit_2015>
      
      /// <summary>
      /// Creates user imlementation of cref="ICalculationScenario" which consists list of calculation objects cref="ICalculatinObject".
      /// </summary>
      /// <param name="parameters">Instance of base class with common parameters.</param>
      /// <returns>The new instance of cref="ICalculationScenario"</returns>
      ICalculationScenario CreateCalculationScenario(CommonParametersBase parameters);
      /// </structural_toolkit_2015>
      
      /// <summary>
      /// Creates new instance of a class with common parameters.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="parameters">Instance of base class with common parameters.</param>
      /// <returns>New instance of user implementation of class derived from cref="CommonParametersBase".</returns>
      CommonParametersBase CreateCommonParameters(Autodesk.Revit.DB.CodeChecking.ServiceData data, CommonParametersBase parameters);
      /// <summary>
      /// Creates new instance of class with list of calculation points.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <param name="parameters">User object with common parameters.</param>
      /// <param name="elementId">Id of an element.</param>
      /// <returns>List of calculation points.</returns>
      List<CalcPoint> CreateCalcPointsForElement(Autodesk.Revit.DB.CodeChecking.ServiceData data, CommonParametersBase parameters, ElementId elementId);
      /// <summary>
      /// Creates new instance of user class which represents a section of a structure element.
      /// </summary>
      /// <param name="sectionDataBase">Instance of base class for the section.</param>
      /// <returns>New instance of user implementation of section class derived from cref="SectionDataBase".</returns>
      SectionDataBase CreateSectionData(SectionDataBase sectionDataBase);
      /// <summary>
      /// Creates new instance of user class which represents a structure element.
      /// </summary>
      /// <param name="elementDataBase">Instance of base class for the element.</param>
      /// <returns>New instance of user implementation of element class derived from cref="ElementDataBase".</returns>
      ElementDataBase CreateElementData(ElementDataBase elementDataBase);
      /// <summary>
      /// Creates new instance of class with results for the element.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <returns>User result schema object for the element.</returns>
      Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass CreateElementResult(Autodesk.Revit.DB.BuiltInCategory category, StructuralAssetClass material);
      /// <summary>
      /// Reads calculation parameters from revit data base.
      /// </summary>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>User calculation parameters schema object.</returns>
      Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass ReadCalculationParameter(Autodesk.Revit.DB.CodeChecking.ServiceData data);
      /// <summary>
      /// Reads parameters of user element label.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <param name="label">Acces to the Revit storage with labels."</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      /// <returns>User label of the element.</returns>
      Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass ReadElementLabel(Autodesk.Revit.DB.BuiltInCategory category, StructuralAssetClass material, Autodesk.Revit.DB.CodeChecking.Storage.Label label, Autodesk.Revit.DB.CodeChecking.ServiceData data);
      /// <summary>
      /// Verify parameters of user element label.
      /// </summary>
      /// <param name="category">Category of the element.</param>
      /// <param name="material">Material of the element.</param>
      /// <param name="label">Element label."</param>
      /// <param name="status">Reference to element's status".</param>
      void VerifyElementLabel(Autodesk.Revit.DB.BuiltInCategory category, StructuralAssetClass material, Autodesk.Revit.DB.ExtensibleStorage.Framework.SchemaClass label, ref Autodesk.Revit.DB.CodeChecking.Storage.ResultStatus status);
      /// <summary>
      /// Filters list of elements due to e.g. a result status for calculation purposes.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <returns>Filtered list of user element objects.</returns>
      List<ObjectDataBase> FilterElementForCalculation(List<ObjectDataBase> listElementData);
      /// <summary>
      /// Gives possibility to read data from Revit data base and puts them into user element's objects and into user section's objects and into user common parameters.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <param name="parameters">User common parameters.</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      void ReadFromRevitDB(List<ObjectDataBase> listElementData, CommonParametersBase parameters, Autodesk.Revit.DB.CodeChecking.ServiceData data);
      /// <summary>
      /// Gives possibility to write data to Revit data base.
      /// </summary>
      /// <param name="listElementData">List of user element objects.</param>
      /// <param name="parameters">User common parameters.</param>
      /// <param name="data">Acces to cref="ServiceData".</param>
      void SaveToRevitDB(List<ObjectDataBase> listElementData, CommonParametersBase parameters, Autodesk.Revit.DB.CodeChecking.ServiceData data);
   }
}
