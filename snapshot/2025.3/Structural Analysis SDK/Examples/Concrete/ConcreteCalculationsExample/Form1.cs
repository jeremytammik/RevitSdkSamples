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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConcreteCalculationsExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
          

            InitializeComponent();
            ResultTextBox.Text = "Example described in the Concrete validation manual ";
            ExampleSelection.Items.Add("All Cases");
            ExampleSelection.Items.Add("Case1");
            ExampleSelection.Items.Add("Case2");
            ExampleSelection.Items.Add("Case3");
            ExampleSelection.Items.Add("Case4");
            ExampleSelection.Items.Add("Case5");
            ExampleSelection.Items.Add("Case6");
            ExampleSelection.Items.Add("Case7");
            ExampleSelection.Items.Add("Case8");
            ExampleSelection.Items.Add("Case9");
            ExampleSelection.Items.Add("Case10");
            ExampleSelection.Items.Add("Case11");
            ExampleSelection.Items.Add("Case12");
            ExampleSelection.Items.Add("Case13");
            ExampleSelection.Items.Add("Case14");
            ExampleSelection.Items.Add("Case15");
            ExampleSelection.Items.Add("Case16");
            ExampleSelection.Items.Add("Case17");
            ExampleSelection.Items.Add("Case18");
            ExampleSelection.SelectedItem = "All Cases"; 

        }

        private void RunExample_Click(object sender, EventArgs e)
        {
           
            ResultTextBox.Text = "Example described in the Concrete validation manual ";
            Example example = new Example(ResultTextBox);
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[1]))
                example.Case1();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[2]))
                example.Case2();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[3]))
                example.Case3();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[4]))
                example.Case4();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[5]))
                example.Case5();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[6]))
                example.Case6();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[7]))
                example.Case7();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[8]))
                example.Case8();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[9]))
                example.Case9();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[10]))
                example.Case10();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[11]))
                example.Case11();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[12]))
                example.Case12();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[13]))
                example.Case13();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[14]))
                example.Case14();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[15]))
                example.Case15();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[16]))
                example.Case16();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[17]))
                example.Case17();
            if (ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[0]) || ExampleSelection.SelectedItem.Equals(ExampleSelection.Items[18]))
                example.Case18();

            example.output.Text = example.sb.ToString();    
        }


    }
    partial class Example
    {
        public RichTextBox output;
        public StringBuilder sb = new StringBuilder();
        public string decoration = "************************************************************"; 
        public Example(RichTextBox ResultTextBox)
        {
            this.sb = new StringBuilder();
            this.output = ResultTextBox;
        }

        
        public string FormatOutput(String label, double value, int maxDigit)
        {

            string outString = "";  

            switch (maxDigit)
            {
                case 1:
                    outString = "\t" + label + " =\t" +String.Format("{0:0.#}", value);
                    break;
                case 2:
                    outString = "\t" + label + " =\t " + String.Format("{0:0.##}", value);
                    break;
               case 3:
                    outString = "\t" + label + " =\t " + String.Format("{0:0.###}", value);
                    break;
               case 4:
                    outString = "\t" + label + " =\t " + String.Format("{0:0.####}", value);
                    break;
               case 5:
                    outString = "\t" + label + " =\t " + String.Format("{0:0.#####}", value);
                    break;
               case 6:
                    outString = "\t" + label + " =\t " + String.Format("{0:0.######}", value);
                    break;
                default:
                    outString = "\t" + label + " =\t " + value;
                    break; 
            }

            return outString;     
        }


    }
}
