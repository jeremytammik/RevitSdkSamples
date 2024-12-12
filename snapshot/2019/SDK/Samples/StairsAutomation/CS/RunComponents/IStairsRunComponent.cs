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

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.StairsAutomation.CS
{
	/// <summary>
	/// The base interface for a single stairs run.
	/// </summary>
	public interface IStairsRunComponent
	{
        /// <summary>
        /// Causes the run to be created.
        /// </summary>
        /// <remarks>The stairs must be properly set in the stairs edit mode, with an open transaction.</remarks>
        /// <param name="document">The document.</param>
        /// <param name="stairsId">The stairs id.</param>
        /// <returns>The new stairs run.</returns>
		StairsRun CreateStairsRun(Document document, ElementId stairsId);
		
        /// <summary>
        /// The bottom elevation of the run.
        /// </summary>
		double RunElevation
		{
			get;
		}
		
        /// <summary>
        /// The top elevation of the run.
        /// </summary>
        /// <throws cref="NotSupportedException">Thrown if the stairs run has not been created, and this cannot be calculated without it.</throws>
		double TopElevation
		{
			get;
		}
		
        /// <summary>
        /// The width of the run.
        /// </summary>
		double Width
		{
			get;
			set;
		}
		
        /// <summary>
        /// Gets the path of the stairs run. 
        /// </summary>
        /// <remarks>This is not guaranteed to succeed if the run has not been created.</remarks>
        /// <returns>The stairs path.</returns>
        /// <throws cref="NotSupportedException">Thrown if the stairs run has not been created, and this cannot be calculated without it.</throws>
		IList<Curve> GetStairsPath();

        /// <summary>
        /// Gets the first tread line for the run.
        /// </summary>
        /// <remarks>This is not guaranteed to succeed if the run has not been created.</remarks>
        /// <returns>The first curve.</returns>
        /// <throws cref="NotSupportedException">Thrown if the stairs run has not been created, and this cannot be calculated without it.</throws>
		Curve GetFirstCurve();

        /// <summary>
        /// Gets the last tread line for the run.
        /// </summary>
        /// <remarks>This is not guaranteed to succeed if the run has not been created.</remarks>
        /// <returns>The last curve.</returns>
        /// <throws cref="NotSupportedException">Thrown if the stairs run has not been created, and this cannot be calculated without it.</throws>
		Curve GetLastCurve();

        /// <summary>
        /// Gets the endpoint of the run for purposes of locating the next run.
        /// </summary>
        /// <remarks>If the run "start point" is the lower left corner of the two dimensional projection of the run, this is the "upper right". 
        /// This is not guaranteed to succeed if the run has not been created.</remarks>
        /// <returns>The endpoint.</returns>
        /// <throws cref="NotSupportedException">Thrown if the stairs run has not been created, and this cannot be calculated without it.</throws>
		XYZ GetRunEndpoint();
	}
}
