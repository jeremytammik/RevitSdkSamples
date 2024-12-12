namespace REX.Serialization.Resources.Dialogs
{
    partial class SubControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.rexEditBox1 = new REX.Controls.Forms.REXEditBox();
            this.rexEditBox2 = new REX.Controls.Forms.REXEditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rexEditBox1
            // 
            this.rexEditBox1.BackColor = System.Drawing.SystemColors.Window;
            this.rexEditBox1.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.rexEditBox1.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.rexEditBox1.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.rexEditBox1.Complex = false;
            this.rexEditBox1.ComplexSpaceSeparator = false;
            this.rexEditBox1.ComplexUnitOnEnd = false;
            this.rexEditBox1.CorrectValue = "";
            this.rexEditBox1.DataType = REX.Controls.Common.EControlType.TEXT;
            this.rexEditBox1.Epsilon = 0;
            this.rexEditBox1.InvalidCharacters = "";
            this.rexEditBox1.Location = new System.Drawing.Point(128, 26);
            this.rexEditBox1.MaxTokens = 0;
            this.rexEditBox1.MinTokens = 0;
            this.rexEditBox1.Name = "rexEditBox1";
            this.rexEditBox1.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.rexEditBox1.OriginalTextOnEnter = false;
            this.rexEditBox1.RangeMax = 0;
            this.rexEditBox1.RangeMin = 0;
            this.rexEditBox1.RememberCorrect = false;
            this.rexEditBox1.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.rexEditBox1.RoundingIncrement = 1E-08;
            this.rexEditBox1.Separator = ".";
            this.rexEditBox1.Size = new System.Drawing.Size(100, 20);
            this.rexEditBox1.TabIndex = 0;
            // 
            // rexEditBox2
            // 
            this.rexEditBox2.BackColor = System.Drawing.SystemColors.Window;
            this.rexEditBox2.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.rexEditBox2.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.rexEditBox2.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.rexEditBox2.Complex = false;
            this.rexEditBox2.ComplexSpaceSeparator = false;
            this.rexEditBox2.ComplexUnitOnEnd = false;
            this.rexEditBox2.CorrectValue = null;
            this.rexEditBox2.DataType = REX.Controls.Common.EControlType.TEXT;
            this.rexEditBox2.Epsilon = 0;
            this.rexEditBox2.InvalidCharacters = "";
            this.rexEditBox2.Location = new System.Drawing.Point(129, 68);
            this.rexEditBox2.MaxTokens = 0;
            this.rexEditBox2.MinTokens = 0;
            this.rexEditBox2.Name = "rexEditBox2";
            this.rexEditBox2.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.rexEditBox2.OriginalTextOnEnter = false;
            this.rexEditBox2.RangeMax = 0;
            this.rexEditBox2.RangeMin = 0;
            this.rexEditBox2.RememberCorrect = false;
            this.rexEditBox2.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.rexEditBox2.RoundingIncrement = 1E-08;
            this.rexEditBox2.Separator = ".";
            this.rexEditBox2.Size = new System.Drawing.Size(100, 20);
            this.rexEditBox2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Value A:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Value B:";
            // 
            // SubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rexEditBox2);
            this.Controls.Add(this.rexEditBox1);
            this.Name = "SubControl";
            this.Size = new System.Drawing.Size(305, 151);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private REX.Controls.Forms.REXEditBox rexEditBox1;
        private REX.Controls.Forms.REXEditBox rexEditBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        }
    }
