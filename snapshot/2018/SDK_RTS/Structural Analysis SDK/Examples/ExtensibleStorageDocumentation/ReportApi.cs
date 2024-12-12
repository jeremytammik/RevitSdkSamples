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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;


namespace ExtensibleStorageDocumentation
{
  
    /// <summary>
    /// This class is an helper class to provide an overview of the documentation features using directly the API
    /// </summary>
    internal class ReportApi
    {
        /// <summary>
        /// the length of the beam. 
        /// </summary>
        private const int lengthBeam = 30;
        /// <summary>
        /// the intensity of the load.
        /// </summary>
        private const double loadIntensity = 3;

   
        private readonly Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document docReport;
        private readonly Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.DocumentLineBreak oneLineBreak;     
        private readonly Autodesk.Revit.DB.Document docRevit;

        /// <summary>
        /// Creates a ReportAPI class instance.
        /// </summary>
        /// <param name="activeDocRevit"></param>
        /// <param name="activeDocReport"></param>
        public ReportApi(Autodesk.Revit.DB.Document activeDocRevit, Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document activeDocReport)
        {
            this.docRevit = activeDocRevit;
            this.docReport = activeDocReport;
            this.oneLineBreak = new DocumentLineBreak(1);
        }
        
        /// <summary>
        /// Main method call to create the documentation using the API.
        /// Main functionnalities of the API are used on this example. 
        /// </summary>
        public void CreateDocumentApi()
        {
            // How to use different style of titles
            AddSection("All Title Styles Supported", 1);
            AddAllTitleType();
            
            // How to add an image 
            AddSection("Images", 1);
            AddImage();
            
            //  How to use different style of text
            AddSection("All Text Styles Supported ", 1);
            AddAllTextType();
           
            //  How to use different type od diagrams
            AddSection("All Diagram Type Supported", 1);
            AddBeamDiagrams(); 
            AddSinusXDiagrams();
            AddSupportedBeamDiagrams();
            AddCantileverDiagrams();
           
            //  How to use maps
            AddSection("Maps", 1);
            AddMap();
            AddMapWithHole();
           
            // How to create a table 
            AddSection("stringListValue", 1);
            AddTables();
            
            // How to create a status report
            AddSection("Status", 1);
            AddStatus();
          
            // How to add a simple text
            AddSection("Text In-Line Example", 1);
            AddTextInline();
        }
        
        /// <summary>
        /// This method will demonstrates how to create a title and add it to the report.
        /// 7 styles are supported. 
        /// </summary>
        private void AddAllTitleType()
        {
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 1", 1));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 2", 2));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 3", 3));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 4", 4));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 5", 5));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 6", 6));
            this.docReport.Main.Elements.Add(new DocumentTitle("This is a title size 7", 7));
        }
        
        /// <summary>
        /// This method demonstrates how to add an image embedded inside the component into the report
        /// </summary>
        private void AddImage()
        {
            var image = new DocumentImage(new Uri(@"pack://application:,,,/ExtensibleStorageDocumentation;component/Images/Image.png"));
            this.docReport.Main.Elements.Add(image);
        }

        /// <summary>
        /// This method demonstrates how to create a text and add it to the report.
        /// Text could be normal, with subscript, superscript and symbol characters
        /// </summary>
        private void AddAllTextType()
        {
            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentText("This is a normal text"));
            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentText("This is a text with{Subscript}"));
            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentText("This is a text with{^Superscript}"));
            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentText("This is a text with @a"));
            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentText("This is a normal text with break line ", true));
        }


        /// <summary>
        /// This method demonstrates how to create tables and add them in the report.
        /// </summary>
        private void AddTables()
        {
            // A 2 x 2 table 
            # region a first table 
           
            var tableTitle = new DocumentTitle { Size = 1, Text = "A 2 x 2 table" };
            var table = new DocumentTable(2, 2);

            // Set the table title       
            table.Title = tableTitle;
            // Number of rows and columns to take into consideration to build the table headers
            table.HeaderColumnsCount = 1;
            table.HeaderRowsCount = 1;
            // Specify dimension of columns
            table.PercentageColumnWidth[0] = 10;
            table.PercentageColumnWidth[1] = 50;
            // fill the table
            table[0, 0].Elements.Add(new DocumentText("columns/rows"));
            table[0, 1].Elements.Add(new DocumentText("header first column"));
            table[1, 0].Elements.Add(new DocumentText("header first row"));
            table[1, 1].Elements.Add(new DocumentText("A value"));
                
            // Add the table to the report 
            this.docReport.Main.Elements.Add(table);

            # endregion a first table

            // A 12 x 12 table 
            // This table is filled with moment values for a cantilever beam loaded by a concentrated load in different positions. 
            # region a second  table

            // Number of division for the beam 
            int numberDivisions = 10; 
            tableTitle = new DocumentTitle { Size = 1, Text = "A 12 x 12 table" };
            table = new DocumentTable(12, 12);
            // Set the table title   
            table.Title = tableTitle;
            // Number of rows and columns to take into consideration to build the table headers
            table.HeaderColumnsCount = 1;
            table.HeaderRowsCount = 1;
            // Specify dimension of columns
            for (int i = 0; i < 12; i++)
            {
                table.PercentageColumnWidth[i] = Convert.ToInt32(100 / table.ColumnsCount);
            }
           
            // fill the table
            // fill left top corner cell
            table[0, 0].Elements.Add(new DocumentText("Moment at (x) / Load at (x)"));
            // fill columns header with the position of the load
            for (int i = 0; i <= 10; i++)
            {
                table[0, i + 1].Elements.Add(new DocumentText(" Load at x = " + i *  lengthBeam / numberDivisions));
            }
            // fill rows header with the position of the moment 
            for (int i = 0; i <= 10; i++)
            {
                table[i + 1, 0].Elements.Add(new DocumentText("Moment at x = " + i * lengthBeam / numberDivisions));
            }
            // fill value cells
            for (int j = 0; j <= numberDivisions; j++)
            {
                for (int i = 0; i <= lengthBeam; i += 3)
                {
                    table[i / 3 + 1, j + 1].Elements.Add(new DocumentText(Math.Max(0, loadIntensity * (i - j * lengthBeam / numberDivisions)).ToString()));
                }
            }
            // Add the table to the report 
            this.docReport.Main.Elements.Add(table);
            # endregion a second  table
        }


        /// <summary>
        /// This method demonstrates how to create different type of diagrams
        /// Values used are the moment values for a beam loaded by an uniform load.
        ///  The formula used is M(x) = Q/12(6lx-l^2 -6x^2.
        /// Length unit is set to feet and force unit is set to Kips per foot. 
        /// </summary>
        private void AddBeamDiagrams()
        {
            // Set the main title   
            this.docReport.Main.Elements.Add(new DocumentTitle("Momemt M(x) = Q/12(6lx-l{^2} -6x{^2}), beam length = 30 feet,  load intensity = 3 Kips/feet", 1));
            
            # region Area Graph Representation
           
            // Set diagram title   
            this.docReport.Main.Elements.Add(new DocumentTitle("Area Graph Representation", 2));
            // Define title in image and default serie name
            var diagram = new DocumentDiagram("Feet/Moment", "Area Diagram Series"){ Height = 800, Width = 800};
            var serie = diagram.Series[0];
            // Set the type of diagram, here area
            serie.DiagramType = DiagramType.Area;
            // Set values along the beam, every foot.
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity/12*(6*lengthBeam*i - lengthBeam*lengthBeam - 6*i*i));
            }
            // Set the axis labels and specify units and how graduations will be shown  
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 5);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 500);
            // Add the legend 
            diagram.Legend = true;
            // Add the diagram to the report 
            this.docReport.Main.Elements.Add(diagram);

            # endregion Area Graph Representation

            # region Bar Graph Representation
            
            this.docReport.Main.Elements.Add(new DocumentTitle("Bar Graph Representation", 2));
            diagram = new DocumentDiagram("Moment/Feet", "Bar Diagram Series"){ Height = 800, Width = 800 };
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Bar;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity/12*(6*lengthBeam*i - lengthBeam*lengthBeam - 6*i*i));
            }
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 10);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 500);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
            
            # endregion Bar Graph Representation
         
            # region Column Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Column Graph Representation", 2));
            diagram = new DocumentDiagram("Feet/Moment", "Column Diagram Series"){Height = 800, Width = 800};
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Column;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity/12*(6*lengthBeam*i - lengthBeam*lengthBeam - 6*i*i));
            }
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 10);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 500);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
          
            # endregion Column Graph Representation

            # region Line Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Line Graph Representation", 2));
            diagram = new DocumentDiagram("Feet/Moment", "Line Diagram Series"){Height = 800, Width = 800 };
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Line;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity/12*(6*lengthBeam*i - lengthBeam*lengthBeam - 6*i*i));
            }
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 10);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 500);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
            # endregion Line Graph Representation

            # region Point Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Point Graph Representation", 2));
            diagram = new DocumentDiagram("Feet/Moment", "Point Diagram Series") {  Height = 800,   Width = 800 };
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Point;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity/12*(6*lengthBeam*i - lengthBeam*lengthBeam - 6*i*i));
            }
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 10);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 500);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);

            # endregion Point Graph Representation
        }


        /// <summary>
        /// This method demonstrates how to create different type of diagrams
        /// This method is really similar to  method AddBeamDiagrams
        /// Values used are based on Sin(x) with x in the range between -2 Pi and 2 Pi
        /// Angle unit is set to radian and sin(x) is set to a number 
        /// </summary>
        private void AddSinusXDiagrams()
        {
            this.docReport.Main.Elements.Add(new DocumentTitle("function Sin(x)  with x [-2@p;2@p] ", 1));

            # region Area Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Area Graph Representation", 2));
            var diagram = new DocumentDiagram("Sin(x)", "Area Diagram Series"){ Height = 400, Width = 800};
            var serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Area;
            for (double i = -2*Math.PI; i < 2*Math.PI; i += 0.05)
            {
                serie.AddXY(i, Math.Sin(i));
            }
            // Set the color
            serie.Color = new Color(0, 100, 0);
            diagram.SetLabelsX(UnitType.UT_Angle, DisplayUnitType.DUT_RADIANS, this.docRevit, 2);
            diagram.SetLabelsY(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL, this.docRevit, 2);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
           
            # endregion Area Graph Representation

            # region Bar Graph Representation

            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentTitle("Bar Graph Representation", 2));
            diagram = new DocumentDiagram("Sin(x)", "Bar Diagram Series") { Height = 400, Width = 800 };

            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Bar;
            for (double i = -2*Math.PI; i < 2*Math.PI; i += 0.05)
            {
                serie.AddXY(i, Math.Sin(i));
            }
            serie.Color = new Color(255, 0, 100);
            diagram.SetLabelsX(UnitType.UT_Angle, DisplayUnitType.DUT_RADIANS, this.docRevit, 2);
            diagram.SetLabelsY(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL, this.docRevit, 2);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);

            # endregion Bar Graph Representation

            # region Column Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Column Graph Representation", 2));
            diagram = new DocumentDiagram("Sin(x)", "Column Diagram Series"){ Height = 400, Width = 800};
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Column;
            for (double i = -2*Math.PI; i < 2*Math.PI; i += 0.05)
            {
                serie.AddXY(i, Math.Sin(i));
            }
            serie.Color = new Color(255, 0, 100);
            diagram.SetLabelsX(UnitType.UT_Angle, DisplayUnitType.DUT_RADIANS, this.docRevit, 2);
            diagram.SetLabelsY(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL, this.docRevit, 2);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);

            # endregion Column Graph Representation

            # region Line Graph Representation

            this.docReport.Main.Elements.Add(this.oneLineBreak);
            this.docReport.Main.Elements.Add(new DocumentTitle("Line Graph Representation", 2));
            diagram = new DocumentDiagram("Sin(x)", "Line Diagram Series"){ Height = 400, Width = 800};
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Line;
            for (double i = -2*Math.PI; i < 2*Math.PI; i += 0.05)
            {
                serie.AddXY(i, Math.Sin(i));
            }
            serie.Color = new Color(255, 0, 100);
            diagram.SetLabelsX(UnitType.UT_Angle, DisplayUnitType.DUT_RADIANS, this.docRevit, 2);
            diagram.SetLabelsY(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL, this.docRevit, 2);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
            
            # endregion Line Graph Representation

            # region Point Graph Representation

            this.docReport.Main.Elements.Add(new DocumentTitle("Point Graph Representation", 2));
            diagram = new DocumentDiagram("Sin(x)", "Point Diagram Series") { Height = 400, Width = 800 };
                           
            serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Point;
            for (double i = -2*Math.PI; i < 2*Math.PI; i += 0.05)
            {
                serie.AddXY(i, Math.Sin(i));
            }
            serie.Color = new Color(255, 0, 100);
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_RADIANS, this.docRevit, 2);
            diagram.SetLabelsY(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL, this.docRevit, 2);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);

            # endregion Point Graph Representation
        }


        /// <summary>
        /// This method demonstrates how to create three series with different represenatation inside the same diagram
        /// </summary>
        private void AddSupportedBeamDiagrams()
        {
            this.docReport.Main.Elements.Add(new DocumentTitle("Two Lines and a column reprensentations on the same diagram", 1));
            
            int scalingFactor = 20;
            var diagram = new DocumentDiagram("Supported beam", "Load Intensity") {Height = 800, Width = 800 };
            diagram.Legend = true;
            // first serie
            var serie = diagram.Series[0];
            serie.DiagramType = DiagramType.Column;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity * scalingFactor);
            }
            serie.Color = new Color(100, 0, 100);
            // second serie with name
            serie = new DocumentDiagramSeries("Shear") {  Color = new Color(255, 0, 100), DiagramType = DiagramType.Line };
            scalingFactor = 5;
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity * (lengthBeam / 2 - i) * scalingFactor);
            }
            diagram.Series.Add(serie);
            // third serie with name
            serie = new DocumentDiagramSeries("Momemt"){Color = new Color(255, 0, 255),DiagramType = DiagramType.Line };
            for (int i = 0; i <= lengthBeam; i++)
            {
                serie.AddXY(i, loadIntensity*i/2*(lengthBeam - i));
            }
            diagram.Series.Add(serie);
            // Add the diagram with 3 series and different representation inside the report 
            this.docReport.Main.Elements.Add(diagram);
        }


        /// <summary>
        /// This method demonstrates how to create few series and superposed them on the same diagram
        /// </summary>
        private void AddCantileverDiagrams()
        {
            this.docReport.Main.Elements.Add(new DocumentTitle("Many series ", 1));
            var diagram = new DocumentDiagram("Cantilever Beam - Concentrated at any point"){ Height = 800,Width = 800};
            
            // creation of 10 series
            // Formula use M(x) = P(x-a) with a the position of the load
            // Values use are similar as in the table creation example
            for (int j = 0; j <= 10; j++)
            {
                var series = new DocumentDiagramSeries("load position:" + j + " x L/10"){DiagramType = DiagramType.Area};
                for (int i = 0; i <= lengthBeam; i++)
                {
                    series.AddXY(i, Math.Max(0, loadIntensity*(i - j*lengthBeam/10)));
                }
                series.Color = new Color(Convert.ToByte(j*25), Convert.ToByte(j*5), Convert.ToByte(j*10));
                diagram.Series.Add(series);
            }
            diagram.SetLabelsX(UnitType.UT_Length, DisplayUnitType.DUT_DECIMAL_FEET, this.docRevit, 5);
            diagram.SetLabelsY(UnitType.UT_Moment, DisplayUnitType.DUT_KIP_FEET, this.docRevit, 100);
            diagram.Legend = true;
            this.docReport.Main.Elements.Add(diagram);
        }
        /// <summary>
        /// This method demonstrates how to use the map object and add in inside the report
        /// </summary>
        private void AddMap()
        {
            // Definition of the units
            var documentMap = new DocumentMap(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL);
            
            double originX = 0;
            double originY = 0;
            double lengthX = Math.PI*2;
            double lengthY = Math.PI*2;
            
            //Definition of the external contour 
            documentMap.AddPoint(originX, originY, 0, true);
            documentMap.AddPoint(originX + lengthX, originY, 0, true);
            documentMap.AddPoint(originX + lengthX, originY + lengthY, 0, true);
            documentMap.AddPoint(originX, originY + lengthY, 0, true);
            // Values generation using Sinus(x)
            double incrementX = (lengthX) / 50;
            double incrementY = (lengthY);
            for (double x = 0; x <= 50; x++)
            {
                for (double y = 0; y <= 1; y++)
                {
                    double positionX = originX + x * incrementX;
                    double positionY = originY + y * incrementY;
                    documentMap.AddPoint(positionX, positionY, Math.Sin(positionX));
                }
            }
            //Set the title 
            documentMap.Title = "Sin(x) ";
            documentMap.Legend = true;
            // define the dimensions in pixels ?
            documentMap.Width = 500;
            // Set the number of color to used
            documentMap.SetColors(15);
            this.docReport.Main.Elements.Add(documentMap);
        }

        private void AddMapWithHole()
        {
            var documentMap = new DocumentMap(UnitType.UT_Number, DisplayUnitType.DUT_GENERAL);
            double originX = 0;
            double originY = 0;
            double lengthX = Math.PI;
            double lengthY = Math.PI;
            //External contour
            documentMap.AddPoint(originX, originY, 0, true);
            documentMap.AddPoint(originX + lengthX, originY, 0, true);
            documentMap.AddPoint(originX + lengthX, originY + lengthY, 0, true);
            documentMap.AddPoint(originX, originY + lengthY, 0, true);
            //holes
            var hole = new List<int>
                           {
                               documentMap.AddPoint(originX + lengthX/4, originY + lengthY/4, Math.Sin(originX + lengthX/4)),
                               documentMap.AddPoint(originX + 3*lengthX/4, originY + lengthY/4, Math.Sin(originX + 3*lengthX/4)),
                               documentMap.AddPoint(originX + 3*lengthX/4, originY + 3*lengthY/4, Math.Sin(originX + 3*lengthX/4)),
                               documentMap.AddPoint(originX + lengthX/4, originY + 3*lengthY/4, Math.Sin(originX + lengthX/4))
                           };
            documentMap.AddHole(hole);
            // value
            double incrementX = (lengthX)/50;
            double incrementY = (lengthY)/50;
            for (double x = 0; x <= 50; x++)
            {
                for (double y = 0; y <= 50; y++)
                {
                    double positionX = originX + x*incrementX;
                    double positionY = originY + y*incrementY;
                    documentMap.AddPoint(positionX, positionY, Math.Sin(positionX));
                }
            }
            //Title
            documentMap.Title = "Sin(x) ";
            documentMap.Legend = true;
            documentMap.Width = 500;
            documentMap.SetColors(10);
            this.docReport.Main.Elements.Add(documentMap);
        }
        /// <summary>
        /// This method demonstrates how to add a status, verified or not, inside the report. 
        /// </summary>
        private void AddStatus()
        {
            var documentStatus = new DocumentStatus("M{sd} = 24", "< M{rd} = 25", "Verified", true, "[6.6.6]");
            this.docReport.Main.Elements.Add(documentStatus);
            documentStatus = new DocumentStatus("M{sd} = 24", "> M{rd} = 25", "Not Verified", false, "[6.6.6]");
            this.docReport.Main.Elements.Add(documentStatus);
            documentStatus = new DocumentStatus("M{sd} = 24", "= M{rd} = 24", "Verified", true, "[6.6.6]");
            this.docReport.Main.Elements.Add(documentStatus);
        }

        private void AddTextInline()
        {
            this.docReport.Main.Elements.Add(new DocumentValue(2, DisplayUnitType.DUT_CENTIMETERS, UnitType.UT_Length, this.docRevit.GetUnits(), true));
            this.docReport.Main.Elements.Add(new DocumentValueWithName("First field", 2, DisplayUnitType.DUT_CENTIMETERS, UnitType.UT_Length, this.docRevit.GetUnits(), true));
            this.docReport.Main.Elements.Add(new DocumentValueWithDescription("Second field", "Description", "Note", 2, DisplayUnitType.DUT_CENTIMETERS, UnitType.UT_Length, this.docRevit.GetUnits(), true));
        }

        public void AddSection(string textTitle, short sizeTitle)
        {
            var documentSection = new DocumentSection(textTitle, sizeTitle);
            this.docReport.Main.Elements.Add(documentSection);
        }
    }
}