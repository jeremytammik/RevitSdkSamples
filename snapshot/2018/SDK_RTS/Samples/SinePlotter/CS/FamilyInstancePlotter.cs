//
// (C) Copyright 2003-2017 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.SinePlotter.CS
{
    /// <summary>
    /// This class plots a number of instances of a given family object along a sine curve.
    /// </summary>
    class FamilyInstancePlotter
    {
        private FamilySymbol familySymbol;
        private Document document;
 
       
        /// <summary>
        /// The constructor for the FamilyInstancePlotter Class.
        /// </summary>
        /// <param name="fs">A Revit family symbol.</param>
        /// <param name="doc">The active Revit document.</param>
        public FamilyInstancePlotter(FamilySymbol fs, Document doc)
        {
            familySymbol = fs;
            document = doc;
        }


        /// <summary>
        /// Places a family instance at the given location.
        /// </summary>
        /// <param name="location">A point XYZ signifying the location for a family instance to be placed.</param>
        private void PlaceAtLocation(XYZ location)
        {
            Transaction t = new Transaction(document, "Place family instance");
            t.Start();
            FamilyInstance prism = document.Create.NewFamilyInstance(location, familySymbol,
                Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
            t.Commit();
        }


        /// <summary>
        /// Computes a sine curve taking into account input values for the sine curve period, amplitude and number 
        /// of circles and places a number of instances defined by the partitions input value of the same family 
        /// object along this curve.
        /// </summary>
        /// <param name="partitions">An integer value denoting the number of partitions per curve period.</param>
        /// <param name="period">A double value denoting the period of the curve, i.e. how often the curve
        /// goes a full repition around the unit circle.</param>
        /// <param name="amplitude">A double value denoting how far the curve gets away from the x-axis.</param>
        /// <param name="numOfCircles">A double value denoting the number of circles the curve makes.</param>
        public void PlaceInstancesOnCurve(int partitions, double period, double amplitude, double numOfCircles)
        {
            //Given the number of partitions compute the angle increment. 
            double theta = (2 * Math.PI) / partitions;

            TransactionGroup transGroup = new TransactionGroup(document, "Place All Instances");
            transGroup.Start();
            //Calculates the sine of an angle. This function expects the values of the angle parameter to be
            //provided in radians (values from 0 to 6.28). Values are returned in the range -1 to 1. The theta 
            //increment will give us at every iteration the point on the curve x for which we need to evaluate y.
            for (int i = 0; i <= partitions * numOfCircles; i++)
            {
                double x = i * theta;
                //function used:  y = a*sin(b*x)
                double y = (Math.Sin(period * x)) * amplitude;              
                XYZ temp = new XYZ(x, y, 0);
                
                PlaceAtLocation(temp);
            }
            transGroup.Assimilate();
        }

    }
}
