//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
namespace Revit.SDK.Samples.SlabProperties.CS
{
    /// <summary>
    /// Show some properties of a slab in Revit Structure 5, including Level, Type name, Span direction,
    /// Material name, Thickness, and Young Modulus for each layer of the slab's material. 
    /// </summary>
    public partial class SlabPropertiesForm : System.Windows.Forms.Form
    {
        // To store the data
        private Command m_dataBuffer;

        private SlabPropertiesForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// overload the constructor
        /// </summary>
        /// <param name="dataBuffer">To store the data of a slab</param>
        public SlabPropertiesForm(Command dataBuffer)
        {
            InitializeComponent();
            
            // get all the data
            m_dataBuffer = dataBuffer;
        }

        /// <summary>
        /// Close the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Display the properties on the form when the form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlabPropertiesForm_Load(object sender, System.EventArgs e)
        {
            this.levelTextBox.Text = m_dataBuffer.Level;
            this.typeNameTextBox.Text = m_dataBuffer.TypeName;
            this.spanDirectionTextBox.Text = m_dataBuffer.SpanDirection;

            int numberOfLayers = m_dataBuffer.NumberOfLayers;

            this.layerRichTextBox.Text = "";

            for (int i = 0; i < numberOfLayers; i++)
            {
                // Get each layer's Material name and Young Modulus properties
                m_dataBuffer.SetLayer(i);

                this.layerRichTextBox.Text += "Layer " + (i + 1).ToString() + "\r\n";
                this.layerRichTextBox.Text += "Material name:  " + m_dataBuffer.LayerMaterialName + "\r\n";
                this.layerRichTextBox.Text += "Thickness: " + m_dataBuffer.LayerThickness + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus X:  " + m_dataBuffer.LayerYoungModulusX + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus Y:  " + m_dataBuffer.LayerYoungModulusY + "\r\n";
                this.layerRichTextBox.Text += "YoungModulus Z:  " + m_dataBuffer.LayerYoungModulusZ + "\r\n";
                this.layerRichTextBox.Text += "-----------------------------------------------------------" + "\r\n";
            }
        }
    }
}
