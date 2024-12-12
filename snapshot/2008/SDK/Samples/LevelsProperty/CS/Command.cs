//
// (C) Copyright 2003-2007 by Autodesk, Inc. 
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

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.LevelsProperty.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
	public class Command : IExternalCommand
	{
		#region GetDatum

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
		public IExternalCommand.Result Execute(ExternalCommandData revit, ref String message, ElementSet elements)
		{
			m_revit = revit;

            try
            {
                //Get every level by iterating through all elements
                systemLevelsDatum = new List<LevelsDataSource>();

                ElementIterator systemElementIterator = m_revit.Application.ActiveDocument.Elements;
                systemElementIterator.Reset();
                while (systemElementIterator.MoveNext())
                {
                    Level systemLevel = systemElementIterator.Current as Level;
                    if (null != systemLevel)
                    {
                        LevelsDataSource levelsDataSourceRow = new LevelsDataSource();

                        levelsDataSourceRow.LevelIDValue = systemLevel.Id.Value;
                        levelsDataSourceRow.Name = systemLevel.Name;

                        double temValue = systemLevel.Elevation * 304.8;
                        double temValue2 = double.Parse(temValue.ToString("#.0"));
                        levelsDataSourceRow.Elevation = temValue2;

                        systemLevelsDatum.Add(levelsDataSourceRow);
                    }
                }

                using (LevelsForm displayForm = new LevelsForm(this))
                {
                    displayForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return IExternalCommand.Result.Failed;
            }

			return IExternalCommand.Result.Succeeded;
		}

		ExternalCommandData m_revit;

		System.Collections.Generic.List<LevelsDataSource> systemLevelsDatum;
        /// <summary>
        /// Store all levels's datum in system
        /// </summary>
		public System.Collections.Generic.List<LevelsDataSource> SystemLevelsDatum
		{
			get
			{
				return systemLevelsDatum;
			}
			set
			{
				systemLevelsDatum = value;
			}
		}
		#endregion

		#region SetData
		/// <summary>
		/// Set Level
		/// </summary>
		/// <param name="levelIDValue">Pass a Level's ID value</param>
		/// <param name="levelName">Pass a Level's Name</param>
		/// <param name="levelElevation">Pass a Level's Elevation</param>
		public void SetLevel(int levelIDValue, String levelName, double levelElevation)
		{
			ElementIterator systemElementIterator = m_revit.Application.ActiveDocument.Elements;
			systemElementIterator.Reset();
			while (systemElementIterator.MoveNext())
			{
				Level systemLevel = systemElementIterator.Current as Autodesk.Revit.Elements.Level;
				if (null != systemLevel)
				{
					if (systemLevel.Id.Value == levelIDValue)
					{
						systemLevel.Name = levelName;
						systemLevel.Elevation = levelElevation / 304.8;
					}
				}
			}
		}
		#endregion

		#region CreateLevel
		/// <summary>
		/// Create a level
		/// </summary>
		/// <param name="levelName">Pass a Level's Name</param>
		/// <param name="levelElevation">Pass a Level's Elevation</param>
		public void CreateLevel(String levelName, double levelElevation)
		{
			Level newLevel = m_revit.Application.ActiveDocument.Create.NewLevel(levelElevation / 304.8);

			newLevel.Name = levelName;
		}
		#endregion

		#region DeleteLevel
		/// <summary>
		/// Delete a Level.
		/// </summary>
		/// <param name="IDValueOfLevel">A Level's ID value</param>
		public void DeleteLevel(int IDValueOfLevel)
		{
			ElementId IDOfLevel;
			IDOfLevel.Value = IDValueOfLevel;

			m_revit.Application.ActiveDocument.Delete(ref IDOfLevel);
		}
		#endregion
	}
}
