namespace REX.Unit.Resources.Dialogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubControl));
            this.labelA = new System.Windows.Forms.Label();
            this.labelB = new System.Windows.Forms.Label();
            this.labelC = new System.Windows.Forms.Label();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.rexEditBoxA = new REX.Controls.Forms.REXEditBox();
            this.rexEditBoxB = new REX.Controls.Forms.REXEditBox();
            this.rexEditBoxC = new REX.Controls.Forms.REXEditBox();
            this.groupBoxBase = new System.Windows.Forms.GroupBox();
            this.textBoxA = new REX.Controls.Forms.REXEditBox();
            this.textBoxB = new REX.Controls.Forms.REXEditBox();
            this.textBoxC = new REX.Controls.Forms.REXEditBox();
            this.groupBoxInterface = new System.Windows.Forms.GroupBox();
            this.textBoxIC = new REX.Controls.Forms.REXEditBox();
            this.textBoxIA = new REX.Controls.Forms.REXEditBox();
            this.textBoxIB = new REX.Controls.Forms.REXEditBox();
            this.groupBoxDisplay.SuspendLayout();
            this.groupBoxBase.SuspendLayout();
            this.groupBoxInterface.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelA
            // 
            resources.ApplyResources(this.labelA, "labelA");
            this.labelA.Name = "labelA";
            // 
            // labelB
            // 
            resources.ApplyResources(this.labelB, "labelB");
            this.labelB.Name = "labelB";
            // 
            // labelC
            // 
            resources.ApplyResources(this.labelC, "labelC");
            this.labelC.Name = "labelC";
            // 
            // groupBoxDisplay
            // 
            this.groupBoxDisplay.Controls.Add(this.labelA);
            this.groupBoxDisplay.Controls.Add(this.labelB);
            this.groupBoxDisplay.Controls.Add(this.labelC);
            this.groupBoxDisplay.Controls.Add(this.rexEditBoxA);
            this.groupBoxDisplay.Controls.Add(this.rexEditBoxB);
            this.groupBoxDisplay.Controls.Add(this.rexEditBoxC);
            resources.ApplyResources(this.groupBoxDisplay, "groupBoxDisplay");
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.TabStop = false;
            // 
            // rexEditBoxA
            // 
            this.rexEditBoxA.BackColor = System.Drawing.SystemColors.Window;
            this.rexEditBoxA.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.rexEditBoxA.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.rexEditBoxA.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.rexEditBoxA.Complex = false;
            this.rexEditBoxA.ComplexSeparator = ",";
            this.rexEditBoxA.ComplexSpaceSeparator = false;
            this.rexEditBoxA.ComplexUnitOnEnd = false;
            this.rexEditBoxA.CorrectValue = "";
            this.rexEditBoxA.DataType = REX.Controls.Common.EControlType.TEXT;
            this.rexEditBoxA.Epsilon = 0D;
            this.rexEditBoxA.InvalidCharacters = "";
            resources.ApplyResources(this.rexEditBoxA, "rexEditBoxA");
            this.rexEditBoxA.MaxTokens = 0;
            this.rexEditBoxA.MinTokens = 0;
            this.rexEditBoxA.Name = "rexEditBoxA";
            this.rexEditBoxA.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.rexEditBoxA.OriginalTextOnEnter = false;
            this.rexEditBoxA.RangeMax = 0D;
            this.rexEditBoxA.RangeMin = 0D;
            this.rexEditBoxA.RememberCorrect = false;
            this.rexEditBoxA.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.rexEditBoxA.RoundingIncrement = 1E-08D;
            this.rexEditBoxA.Separator = ".";
            this.rexEditBoxA.TextChanged += new System.EventHandler(this.rexEditBoxA_TextChanged);
            this.rexEditBoxA.Leave += new System.EventHandler(this.rexEditBoxA_Leave);
            // 
            // rexEditBoxB
            // 
            this.rexEditBoxB.BackColor = System.Drawing.SystemColors.Window;
            this.rexEditBoxB.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.rexEditBoxB.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.rexEditBoxB.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.rexEditBoxB.Complex = false;
            this.rexEditBoxB.ComplexSeparator = ",";
            this.rexEditBoxB.ComplexSpaceSeparator = false;
            this.rexEditBoxB.ComplexUnitOnEnd = false;
            this.rexEditBoxB.CorrectValue = "";
            this.rexEditBoxB.DataType = REX.Controls.Common.EControlType.TEXT;
            this.rexEditBoxB.Epsilon = 0D;
            this.rexEditBoxB.InvalidCharacters = "";
            resources.ApplyResources(this.rexEditBoxB, "rexEditBoxB");
            this.rexEditBoxB.MaxTokens = 0;
            this.rexEditBoxB.MinTokens = 0;
            this.rexEditBoxB.Name = "rexEditBoxB";
            this.rexEditBoxB.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.rexEditBoxB.OriginalTextOnEnter = false;
            this.rexEditBoxB.RangeMax = 0D;
            this.rexEditBoxB.RangeMin = 0D;
            this.rexEditBoxB.RememberCorrect = false;
            this.rexEditBoxB.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.rexEditBoxB.RoundingIncrement = 1E-08D;
            this.rexEditBoxB.Separator = ".";
            this.rexEditBoxB.TextChanged += new System.EventHandler(this.rexEditBoxB_TextChanged);
            this.rexEditBoxB.Leave += new System.EventHandler(this.rexEditBoxB_TextChanged);
            // 
            // rexEditBoxC
            // 
            this.rexEditBoxC.BackColor = System.Drawing.SystemColors.Window;
            this.rexEditBoxC.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.rexEditBoxC.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.rexEditBoxC.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.rexEditBoxC.Complex = false;
            this.rexEditBoxC.ComplexSeparator = ",";
            this.rexEditBoxC.ComplexSpaceSeparator = false;
            this.rexEditBoxC.ComplexUnitOnEnd = false;
            this.rexEditBoxC.CorrectValue = "";
            this.rexEditBoxC.DataType = REX.Controls.Common.EControlType.TEXT;
            this.rexEditBoxC.Epsilon = 0D;
            this.rexEditBoxC.InvalidCharacters = "";
            resources.ApplyResources(this.rexEditBoxC, "rexEditBoxC");
            this.rexEditBoxC.MaxTokens = 0;
            this.rexEditBoxC.MinTokens = 0;
            this.rexEditBoxC.Name = "rexEditBoxC";
            this.rexEditBoxC.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.rexEditBoxC.OriginalTextOnEnter = false;
            this.rexEditBoxC.RangeMax = 0D;
            this.rexEditBoxC.RangeMin = 0D;
            this.rexEditBoxC.RememberCorrect = false;
            this.rexEditBoxC.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.rexEditBoxC.RoundingIncrement = 1E-08D;
            this.rexEditBoxC.Separator = ".";
            this.rexEditBoxC.TextChanged += new System.EventHandler(this.rexEditBoxC_TextChanged);
            this.rexEditBoxC.Leave += new System.EventHandler(this.rexEditBoxC_Leave);
            // 
            // groupBoxBase
            // 
            this.groupBoxBase.Controls.Add(this.textBoxA);
            this.groupBoxBase.Controls.Add(this.textBoxB);
            this.groupBoxBase.Controls.Add(this.textBoxC);
            resources.ApplyResources(this.groupBoxBase, "groupBoxBase");
            this.groupBoxBase.Name = "groupBoxBase";
            this.groupBoxBase.TabStop = false;
            // 
            // textBoxA
            // 
            this.textBoxA.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxA.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxA.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxA.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxA.Complex = false;
            this.textBoxA.ComplexSeparator = ",";
            this.textBoxA.ComplexSpaceSeparator = false;
            this.textBoxA.ComplexUnitOnEnd = false;
            this.textBoxA.CorrectValue = "";
            this.textBoxA.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxA.Epsilon = 0D;
            this.textBoxA.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxA, "textBoxA");
            this.textBoxA.MaxTokens = 0;
            this.textBoxA.MinTokens = 0;
            this.textBoxA.Name = "textBoxA";
            this.textBoxA.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxA.OriginalTextOnEnter = false;
            this.textBoxA.RangeMax = 0D;
            this.textBoxA.RangeMin = 0D;
            this.textBoxA.RememberCorrect = false;
            this.textBoxA.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxA.RoundingIncrement = 1E-08D;
            this.textBoxA.Separator = ".";
            // 
            // textBoxB
            // 
            this.textBoxB.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxB.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxB.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxB.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxB.Complex = false;
            this.textBoxB.ComplexSeparator = ",";
            this.textBoxB.ComplexSpaceSeparator = false;
            this.textBoxB.ComplexUnitOnEnd = false;
            this.textBoxB.CorrectValue = "";
            this.textBoxB.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxB.Epsilon = 0D;
            this.textBoxB.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxB, "textBoxB");
            this.textBoxB.MaxTokens = 0;
            this.textBoxB.MinTokens = 0;
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxB.OriginalTextOnEnter = false;
            this.textBoxB.RangeMax = 0D;
            this.textBoxB.RangeMin = 0D;
            this.textBoxB.RememberCorrect = false;
            this.textBoxB.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxB.RoundingIncrement = 1E-08D;
            this.textBoxB.Separator = ".";
            // 
            // textBoxC
            // 
            this.textBoxC.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxC.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxC.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxC.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxC.Complex = false;
            this.textBoxC.ComplexSeparator = ",";
            this.textBoxC.ComplexSpaceSeparator = false;
            this.textBoxC.ComplexUnitOnEnd = false;
            this.textBoxC.CorrectValue = "";
            this.textBoxC.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxC.Epsilon = 0D;
            this.textBoxC.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxC, "textBoxC");
            this.textBoxC.MaxTokens = 0;
            this.textBoxC.MinTokens = 0;
            this.textBoxC.Name = "textBoxC";
            this.textBoxC.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxC.OriginalTextOnEnter = false;
            this.textBoxC.RangeMax = 0D;
            this.textBoxC.RangeMin = 0D;
            this.textBoxC.RememberCorrect = false;
            this.textBoxC.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxC.RoundingIncrement = 1E-08D;
            this.textBoxC.Separator = ".";
            // 
            // groupBoxInterface
            // 
            this.groupBoxInterface.Controls.Add(this.textBoxIC);
            this.groupBoxInterface.Controls.Add(this.textBoxIA);
            this.groupBoxInterface.Controls.Add(this.textBoxIB);
            resources.ApplyResources(this.groupBoxInterface, "groupBoxInterface");
            this.groupBoxInterface.Name = "groupBoxInterface";
            this.groupBoxInterface.TabStop = false;
            // 
            // textBoxIC
            // 
            this.textBoxIC.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIC.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxIC.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxIC.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxIC.Complex = false;
            this.textBoxIC.ComplexSeparator = ",";
            this.textBoxIC.ComplexSpaceSeparator = false;
            this.textBoxIC.ComplexUnitOnEnd = false;
            this.textBoxIC.CorrectValue = "";
            this.textBoxIC.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxIC.Epsilon = 0D;
            this.textBoxIC.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxIC, "textBoxIC");
            this.textBoxIC.MaxTokens = 0;
            this.textBoxIC.MinTokens = 0;
            this.textBoxIC.Name = "textBoxIC";
            this.textBoxIC.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxIC.OriginalTextOnEnter = false;
            this.textBoxIC.RangeMax = 0D;
            this.textBoxIC.RangeMin = 0D;
            this.textBoxIC.RememberCorrect = false;
            this.textBoxIC.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxIC.RoundingIncrement = 1E-08D;
            this.textBoxIC.Separator = ".";
            // 
            // textBoxIA
            // 
            this.textBoxIA.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIA.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxIA.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxIA.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxIA.Complex = false;
            this.textBoxIA.ComplexSeparator = ",";
            this.textBoxIA.ComplexSpaceSeparator = false;
            this.textBoxIA.ComplexUnitOnEnd = false;
            this.textBoxIA.CorrectValue = "";
            this.textBoxIA.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxIA.Epsilon = 0D;
            this.textBoxIA.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxIA, "textBoxIA");
            this.textBoxIA.MaxTokens = 0;
            this.textBoxIA.MinTokens = 0;
            this.textBoxIA.Name = "textBoxIA";
            this.textBoxIA.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxIA.OriginalTextOnEnter = false;
            this.textBoxIA.RangeMax = 0D;
            this.textBoxIA.RangeMin = 0D;
            this.textBoxIA.RememberCorrect = false;
            this.textBoxIA.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxIA.RoundingIncrement = 1E-08D;
            this.textBoxIA.Separator = ".";
            // 
            // textBoxIB
            // 
            this.textBoxIB.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIB.ColorBackgroundDisabled = System.Drawing.SystemColors.Control;
            this.textBoxIB.ColorBackgroundNeutral = System.Drawing.SystemColors.Window;
            this.textBoxIB.ColorBackgroundStandard = System.Drawing.SystemColors.Control;
            this.textBoxIB.Complex = false;
            this.textBoxIB.ComplexSeparator = ",";
            this.textBoxIB.ComplexSpaceSeparator = false;
            this.textBoxIB.ComplexUnitOnEnd = false;
            this.textBoxIB.CorrectValue = "";
            this.textBoxIB.DataType = REX.Controls.Common.EControlType.TEXT;
            this.textBoxIB.Epsilon = 0D;
            this.textBoxIB.InvalidCharacters = "";
            resources.ApplyResources(this.textBoxIB, "textBoxIB");
            this.textBoxIB.MaxTokens = 0;
            this.textBoxIB.MinTokens = 0;
            this.textBoxIB.Name = "textBoxIB";
            this.textBoxIB.OnDisableAction = REX.Controls.Common.EOnDisable.eSTANDARD;
            this.textBoxIB.OriginalTextOnEnter = false;
            this.textBoxIB.RangeMax = 0D;
            this.textBoxIB.RangeMin = 0D;
            this.textBoxIB.RememberCorrect = false;
            this.textBoxIB.RoundingFractional = REX.Controls.Common.EFractialRound.ROUNDING_1;
            this.textBoxIB.RoundingIncrement = 1E-08D;
            this.textBoxIB.Separator = ".";
            // 
            // SubControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxInterface);
            this.Controls.Add(this.groupBoxBase);
            this.Controls.Add(this.groupBoxDisplay);
            this.Name = "SubControl";
            this.groupBoxDisplay.ResumeLayout(false);
            this.groupBoxDisplay.PerformLayout();
            this.groupBoxBase.ResumeLayout(false);
            this.groupBoxBase.PerformLayout();
            this.groupBoxInterface.ResumeLayout(false);
            this.groupBoxInterface.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelA;
        private System.Windows.Forms.Label labelB;
        private System.Windows.Forms.Label labelC;
        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private System.Windows.Forms.GroupBox groupBoxBase;
        private System.Windows.Forms.GroupBox groupBoxInterface;
        private REX.Controls.Forms.REXEditBox rexEditBoxA;
        private REX.Controls.Forms.REXEditBox rexEditBoxB;
        private REX.Controls.Forms.REXEditBox rexEditBoxC;
        private REX.Controls.Forms.REXEditBox textBoxA;
        private REX.Controls.Forms.REXEditBox textBoxB;
        private REX.Controls.Forms.REXEditBox textBoxC;
        private REX.Controls.Forms.REXEditBox textBoxIA;
        private REX.Controls.Forms.REXEditBox textBoxIB;
        private REX.Controls.Forms.REXEditBox textBoxIC;


    }
}
