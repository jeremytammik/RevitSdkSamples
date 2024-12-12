using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ScheduleToHTML.CS
{
    /// <summary>
    /// A class that can export a schedule to HTML.
    /// </summary>
    class ScheduleHTMLExporter
    {
        /// <summary>
        /// Constructs a new instance of the schedule exporter operating on the input schedule.
        /// </summary>
        /// <param name="input">The schedule to be exported.</param>
        public ScheduleHTMLExporter(ViewSchedule input)
        {
            theSchedule = input;
        }

        /// <summary>
        /// Exports the schedule to formatted HTML.
        /// </summary>
        public void ExportToHTML()
        {
            // Setup file location in temp directory
            string folder = Environment.GetEnvironmentVariable("TEMP");
            string htmlFile = System.IO.Path.Combine(folder, ReplaceIllegalCharacters(theSchedule.Name) + ".html");

            // Initialize StringWriter instance.
            StreamWriter stringWriter = new StreamWriter(htmlFile);
            try
            {
                // Put HtmlTextWriter in using block because it needs to call Dispose.
                using (writer = new HtmlTextWriter(stringWriter))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    //Write schedule header
                    WriteHeader();

                    //Write schedule body
                    WriteBody();

                    writer.RenderEndTag();
                }
            }
            finally
            {
                stringWriter.Close();
            }

            // Show the created file
            System.Diagnostics.Process.Start(htmlFile);
        }

        /// <summary>
        /// Writes the header section of the table to the HTML file.
        /// </summary>
        private void WriteHeader()
        {
            // Clear written cells
            writtenCells.Clear();

            // Start table to represent the header
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Get header section and write each cell
            headerSection = theSchedule.GetTableData().GetSectionData(SectionType.Header);
            int numberOfRows = headerSection.NumberOfRows;
            int numberOfColumns = headerSection.NumberOfColumns;

            for (int iRow = headerSection.FirstRowNumber; iRow < numberOfRows; iRow++)
            {
                WriteHeaderSectionRow(iRow, numberOfColumns);
            }

            // Close header table
            writer.RenderEndTag();
        }

        /// <summary>
        /// Writes the body section of the table to the HTML file.
        /// </summary>
        private void WriteBody()
        {
            // Clear written cells
            writtenCells.Clear();

            // Write the start of the body table
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Get body section and write contents
            bodySection = theSchedule.GetTableData().GetSectionData(SectionType.Body);
            int numberOfRows = bodySection.NumberOfRows;
            int numberOfColumns = bodySection.NumberOfColumns;

            for (int iRow = bodySection.FirstRowNumber; iRow < numberOfRows; iRow++)
            {
                WriteBodySectionRow(iRow, numberOfColumns);
            }

            // Close the table
            writer.RenderEndTag();
        }

        /// <summary>
        /// Gets the Color value formatted for HTML (#XXXXXX) output.
        /// </summary>
        /// <param name="color">he color.</param>
        /// <returns>The color string.</returns>
        private static String GetColorHtmlString(Color color)
        {
            return String.Format("#{0}{1}{2}", color.Red.ToString("X"), color.Green.ToString("X"), color.Blue.ToString("X"));
        }

        /// <summary>
        /// A predefined color value used for comparison.
        /// </summary>
        private static Color Black
        {
            get
            {
                return new Color(0, 0, 0);
            }
        }

        /// <summary>
        /// A predefined color value used for comparison.
        /// </summary>
        private static Color White
        {
            get
            {
                return new Color(255, 255, 255);
            }
        }

        /// <summary>
        /// Compares two colors.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <returns>True if the colors are equal, false otherwise.</returns>
        private bool ColorsEqual(Color color1, Color color2)
        {
            return color1.Red == color2.Red && color1.Green == color2.Green && color1.Blue == color2.Blue;
        }

        /// <summary>
        /// Gets the HTML string representing this horizontal alignment.
        /// </summary>
        /// <param name="style">The horizontal alignment.</param>
        /// <returns>The related string.</returns>
        private static String GetAlignString(HorizontalAlignmentStyle style)
        {
            switch (style)
            {
                case HorizontalAlignmentStyle.Left:
                    return "left";
                case HorizontalAlignmentStyle.Center:
                    return "center";
                case HorizontalAlignmentStyle.Right:
                    return "right";
            }
            return "";
        }

        /// <summary>
        /// Writes a row of the header.
        /// </summary>
        /// <param name="iRow">The row number.</param>
        /// <param name="numberOfColumns">The number of columns to write.</param>
        private void WriteHeaderSectionRow(int iRow, int numberOfColumns)
        {
            WriteSectionRow(SectionType.Header, headerSection, iRow, numberOfColumns);
        }

        /// <summary>
        /// Writes a row of the body.
        /// </summary>
        /// <param name="iRow">The row number.</param>
        /// <param name="numberOfColumns">The number of columns to write.</param>
        private void WriteBodySectionRow(int iRow, int numberOfColumns)
        {
            WriteSectionRow(SectionType.Body, bodySection, iRow, numberOfColumns);
        }


        /// <summary>
        /// Writes a row of a table section.
        /// </summary>
        /// <param name="iRow">The row number.</param>
        /// <param name="numberOfColumns">The number of columns to write.</param>
        /// <param name="secType">The section type.</param>
        /// <param name="data">The table section data.</param>
        private void WriteSectionRow(SectionType secType, TableSectionData data, int iRow, int numberOfColumns)
        {
            // Start the table row tag.
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            // Loop over the table section row.
            for (int iCol = data.FirstColumnNumber; iCol < numberOfColumns; iCol++)
            {
                // Skip already written cells
                if (writtenCells.Contains(new Tuple<int, int>(iRow, iCol)))
                    continue;

                // Get style
                TableCellStyle style = data.GetTableCellStyle(iRow, iCol);
                int numberOfStyleTags = 1;



                // Merged cells
                TableMergedCell mergedCell = data.GetMergedCell(iRow, iCol);

                // If merged cell spans multiple columns
                if (mergedCell.Left != mergedCell.Right)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, (mergedCell.Right - mergedCell.Left + 1).ToString());
                }

                // If merged cell spans multiple rows
                if (mergedCell.Top != mergedCell.Bottom)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, (mergedCell.Bottom - mergedCell.Top + 1).ToString());
                }

                // Remember all written cells related to the merge 
                for (int iMergedRow = mergedCell.Top; iMergedRow <= mergedCell.Bottom; iMergedRow++)
                {
                    for (int iMergedCol = mergedCell.Left; iMergedCol <= mergedCell.Right; iMergedCol++)
                    {
                        writtenCells.Add(new Tuple<int, int>(iMergedRow, iMergedCol));
                    }
                }

                // Write formatting attributes for the upcoming cell
                // Background color
                if (!ColorsEqual(style.BackgroundColor, White))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Bgcolor, GetColorHtmlString(style.BackgroundColor));
                }

                // Horizontal alignment
                writer.AddAttribute(HtmlTextWriterAttribute.Align, GetAlignString(style.FontHorizontalAlignment));

                // Write cell tag
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                // Write subtags for the cell
                // Underline
                if (style.IsFontUnderline)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.U);
                    numberOfStyleTags++;
                }

                //Italic
                if (style.IsFontItalic)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.I);
                    numberOfStyleTags++;
                }

                //Bold
                if (style.IsFontBold)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    numberOfStyleTags++;
                }

                // Write cell text
                String cellText = theSchedule.GetCellText(secType, iRow, iCol);
                if (cellText.Length > 0)
                {
                    writer.Write(cellText);
                }
                else
                {
                    writer.Write("&nbsp;");
                }

                // Close open style tags & cell tag
                for (int i = 0; i < numberOfStyleTags; i++)
                {
                    writer.RenderEndTag();
                }
            }
            // Close row tag
            writer.RenderEndTag();
        }

        /// <summary>
        /// An utility method to replace illegal characters of the Schedule name when creating the HTML file name.
        /// </summary>
        /// <param name="stringWithIllegalChar">The Schedule name.</param>
        /// <returns>The updated string without illegal characters.</returns>
        private static string ReplaceIllegalCharacters(string stringWithIllegalChar)
        {
            char[] illegalChars = System.IO.Path.GetInvalidFileNameChars();

            string updated = stringWithIllegalChar;
            foreach (char ch in illegalChars)
            {
                updated = updated.Replace(ch, '_');
            }

            return updated;
        }


        /// <summary>
        /// The writer for the HTML file.
        /// </summary>
        private HtmlTextWriter writer;

        /// <summary>
        /// The schedule being exported.
        /// </summary>
        private ViewSchedule theSchedule;

        /// <summary>
        /// The body section of the table.
        /// </summary>
        private TableSectionData bodySection;

        /// <summary>
        /// The header section of the table.
        /// </summary>
        private TableSectionData headerSection;

        /// <summary>
        /// A collection of cells which have already been output.  This is needed to deal with
        /// cell merging - each cell should be written only once even as all the cells are iterated in
        /// order.
        /// </summary>
        List<Tuple<int, int>> writtenCells = new List<Tuple<int, int>>();

    }
}

