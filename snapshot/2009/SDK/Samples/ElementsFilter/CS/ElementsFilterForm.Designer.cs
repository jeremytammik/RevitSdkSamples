namespace Revit.SDK.Samples.ElementsFilter.CS
{
    partial class ElementsFilterForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementsFilterForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.booleanAreaLabel = new System.Windows.Forms.Label();
            this.notButton = new System.Windows.Forms.Button();
            this.orButton = new System.Windows.Forms.Button();
            this.andButton = new System.Windows.Forms.Button();
            this.addExecutingFilterButton = new System.Windows.Forms.Button();
            this.reSetButton = new System.Windows.Forms.Button();
            this.booleanExpressionTextBox = new System.Windows.Forms.TextBox();
            this.applyFilterButton = new System.Windows.Forms.Button();
            this.m_filtersTabControl = new System.Windows.Forms.TabControl();
            this.categoryFilterTabPage = new System.Windows.Forms.TabPage();
            this.m_categoryListBox = new System.Windows.Forms.ListBox();
            this.m_ModeComboBox = new System.Windows.Forms.ComboBox();
            this.familyFilterTabPage = new System.Windows.Forms.TabPage();
            this.familyNameListBox = new System.Windows.Forms.ListBox();
            this.symbolFilterTabPage = new System.Windows.Forms.TabPage();
            this.symbolListBox = new System.Windows.Forms.ListBox();
            this.parameterFilterTabPage = new System.Windows.Forms.TabPage();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.valueTypeComboBox = new System.Windows.Forms.ComboBox();
            this.criteriaListBox = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.builtInParasListBox = new System.Windows.Forms.ListBox();
            this.typeFilterTabPage = new System.Windows.Forms.TabPage();
            this.typeListBox = new System.Windows.Forms.ListBox();
            this.structureFilterTabPage = new System.Windows.Forms.TabPage();
            this.structureEnumListBox = new System.Windows.Forms.ListBox();
            this.classificationComboBox = new System.Windows.Forms.ComboBox();
            this.newFilterButton = new System.Windows.Forms.Button();
            this.deleteFilterButton = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listView3 = new System.Windows.Forms.ListView();
            this.filtersListBox = new System.Windows.Forms.ListBox();
            this.filterNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tipMessageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.filterButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.filterInfoLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.m_filtersTabControl.SuspendLayout();
            this.categoryFilterTabPage.SuspendLayout();
            this.familyFilterTabPage.SuspendLayout();
            this.symbolFilterTabPage.SuspendLayout();
            this.parameterFilterTabPage.SuspendLayout();
            this.typeFilterTabPage.SuspendLayout();
            this.structureFilterTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.booleanAreaLabel);
            this.groupBox1.Controls.Add(this.notButton);
            this.groupBox1.Controls.Add(this.orButton);
            this.groupBox1.Controls.Add(this.andButton);
            this.groupBox1.Controls.Add(this.addExecutingFilterButton);
            this.groupBox1.Controls.Add(this.reSetButton);
            this.groupBox1.Controls.Add(this.booleanExpressionTextBox);
            this.groupBox1.Controls.Add(this.applyFilterButton);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // booleanAreaLabel
            // 
            resources.ApplyResources(this.booleanAreaLabel, "booleanAreaLabel");
            this.booleanAreaLabel.Name = "booleanAreaLabel";
            // 
            // notButton
            // 
            resources.ApplyResources(this.notButton, "notButton");
            this.notButton.Name = "notButton";
            this.notButton.UseVisualStyleBackColor = true;
            this.notButton.Click += new System.EventHandler(this.notButton_Click);
            // 
            // orButton
            // 
            resources.ApplyResources(this.orButton, "orButton");
            this.orButton.Name = "orButton";
            this.orButton.UseVisualStyleBackColor = true;
            this.orButton.Click += new System.EventHandler(this.orButton_Click);
            // 
            // andButton
            // 
            resources.ApplyResources(this.andButton, "andButton");
            this.andButton.Name = "andButton";
            this.andButton.UseVisualStyleBackColor = true;
            this.andButton.Click += new System.EventHandler(this.andButton_Click);
            // 
            // addExecutingFilterButton
            // 
            resources.ApplyResources(this.addExecutingFilterButton, "addExecutingFilterButton");
            this.addExecutingFilterButton.Name = "addExecutingFilterButton";
            this.addExecutingFilterButton.UseVisualStyleBackColor = true;
            this.addExecutingFilterButton.Click += new System.EventHandler(this.addExecutingFilterButton_Click);
            // 
            // reSetButton
            // 
            resources.ApplyResources(this.reSetButton, "reSetButton");
            this.reSetButton.Name = "reSetButton";
            this.reSetButton.UseVisualStyleBackColor = true;
            this.reSetButton.Click += new System.EventHandler(this.reSetButton_Click);
            // 
            // booleanExpressionTextBox
            // 
            resources.ApplyResources(this.booleanExpressionTextBox, "booleanExpressionTextBox");
            this.booleanExpressionTextBox.Name = "booleanExpressionTextBox";
            this.booleanExpressionTextBox.ReadOnly = true;
            // 
            // applyFilterButton
            // 
            resources.ApplyResources(this.applyFilterButton, "applyFilterButton");
            this.applyFilterButton.Name = "applyFilterButton";
            this.applyFilterButton.UseVisualStyleBackColor = true;
            this.applyFilterButton.Click += new System.EventHandler(this.applyFilterButton_Click);
            // 
            // m_filtersTabControl
            // 
            this.m_filtersTabControl.Controls.Add(this.categoryFilterTabPage);
            this.m_filtersTabControl.Controls.Add(this.familyFilterTabPage);
            this.m_filtersTabControl.Controls.Add(this.symbolFilterTabPage);
            this.m_filtersTabControl.Controls.Add(this.parameterFilterTabPage);
            this.m_filtersTabControl.Controls.Add(this.typeFilterTabPage);
            this.m_filtersTabControl.Controls.Add(this.structureFilterTabPage);
            resources.ApplyResources(this.m_filtersTabControl, "m_filtersTabControl");
            this.m_filtersTabControl.Name = "m_filtersTabControl";
            this.m_filtersTabControl.SelectedIndex = 0;
            // 
            // categoryFilterTabPage
            // 
            this.categoryFilterTabPage.Controls.Add(this.m_categoryListBox);
            this.categoryFilterTabPage.Controls.Add(this.m_ModeComboBox);
            resources.ApplyResources(this.categoryFilterTabPage, "categoryFilterTabPage");
            this.categoryFilterTabPage.Name = "categoryFilterTabPage";
            this.categoryFilterTabPage.UseVisualStyleBackColor = true;
            // 
            // m_categoryListBox
            // 
            this.m_categoryListBox.FormattingEnabled = true;
            resources.ApplyResources(this.m_categoryListBox, "m_categoryListBox");
            this.m_categoryListBox.Name = "m_categoryListBox";
            this.m_categoryListBox.Sorted = true;
            this.m_categoryListBox.DoubleClick += new System.EventHandler(this.newFilterButton_Click);
            this.m_categoryListBox.SelectedValueChanged += new System.EventHandler(this.m_categoryListBox_SelectedValueChanged);
            // 
            // m_ModeComboBox
            // 
            this.m_ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ModeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.m_ModeComboBox, "m_ModeComboBox");
            this.m_ModeComboBox.Name = "m_ModeComboBox";
            this.m_ModeComboBox.SelectedValueChanged += new System.EventHandler(this.m_ModeComboBox_SelectedValueChanged);
            // 
            // familyFilterTabPage
            // 
            this.familyFilterTabPage.Controls.Add(this.familyNameListBox);
            resources.ApplyResources(this.familyFilterTabPage, "familyFilterTabPage");
            this.familyFilterTabPage.Name = "familyFilterTabPage";
            this.familyFilterTabPage.UseVisualStyleBackColor = true;
            this.familyFilterTabPage.Enter += new System.EventHandler(this.familyFilterTabPage_Enter);
            // 
            // familyNameListBox
            // 
            this.familyNameListBox.FormattingEnabled = true;
            resources.ApplyResources(this.familyNameListBox, "familyNameListBox");
            this.familyNameListBox.Name = "familyNameListBox";
            this.familyNameListBox.Sorted = true;
            this.familyNameListBox.DoubleClick += new System.EventHandler(this.newFilterButton_Click);
            this.familyNameListBox.SelectedValueChanged += new System.EventHandler(this.familyNameListBox_SelectedValueChanged);
            // 
            // symbolFilterTabPage
            // 
            this.symbolFilterTabPage.Controls.Add(this.symbolListBox);
            resources.ApplyResources(this.symbolFilterTabPage, "symbolFilterTabPage");
            this.symbolFilterTabPage.Name = "symbolFilterTabPage";
            this.symbolFilterTabPage.UseVisualStyleBackColor = true;
            this.symbolFilterTabPage.Enter += new System.EventHandler(this.symbolFilterTabPage_Enter);
            // 
            // symbolListBox
            // 
            this.symbolListBox.FormattingEnabled = true;
            resources.ApplyResources(this.symbolListBox, "symbolListBox");
            this.symbolListBox.Name = "symbolListBox";
            this.symbolListBox.Sorted = true;
            this.symbolListBox.DoubleClick += new System.EventHandler(this.newFilterButton_Click);
            this.symbolListBox.SelectedValueChanged += new System.EventHandler(this.symbolListBox_SelectedValueChanged);
            // 
            // parameterFilterTabPage
            // 
            this.parameterFilterTabPage.Controls.Add(this.valueTextBox);
            this.parameterFilterTabPage.Controls.Add(this.valueTypeComboBox);
            this.parameterFilterTabPage.Controls.Add(this.criteriaListBox);
            this.parameterFilterTabPage.Controls.Add(this.label7);
            this.parameterFilterTabPage.Controls.Add(this.label8);
            this.parameterFilterTabPage.Controls.Add(this.label6);
            this.parameterFilterTabPage.Controls.Add(this.label5);
            this.parameterFilterTabPage.Controls.Add(this.builtInParasListBox);
            resources.ApplyResources(this.parameterFilterTabPage, "parameterFilterTabPage");
            this.parameterFilterTabPage.Name = "parameterFilterTabPage";
            this.parameterFilterTabPage.UseVisualStyleBackColor = true;
            this.parameterFilterTabPage.Enter += new System.EventHandler(this.parameterFilterTabPage_Enter);
            // 
            // valueTextBox
            // 
            resources.ApplyResources(this.valueTextBox, "valueTextBox");
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // valueTypeComboBox
            // 
            this.valueTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.valueTypeComboBox, "valueTypeComboBox");
            this.valueTypeComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.valueTypeComboBox.FormattingEnabled = true;
            this.valueTypeComboBox.Name = "valueTypeComboBox";
            this.valueTypeComboBox.SelectedValueChanged += new System.EventHandler(this.valueTypeComboBox_SelectedValueChanged);
            // 
            // criteriaListBox
            // 
            this.criteriaListBox.FormattingEnabled = true;
            resources.ApplyResources(this.criteriaListBox, "criteriaListBox");
            this.criteriaListBox.Name = "criteriaListBox";
            this.criteriaListBox.Sorted = true;
            this.criteriaListBox.SelectedValueChanged += new System.EventHandler(this.criteriaListBox_SelectedValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // builtInParasListBox
            // 
            this.builtInParasListBox.FormattingEnabled = true;
            resources.ApplyResources(this.builtInParasListBox, "builtInParasListBox");
            this.builtInParasListBox.Name = "builtInParasListBox";
            this.builtInParasListBox.Sorted = true;
            this.builtInParasListBox.SelectedIndexChanged += new System.EventHandler(this.builtInParasListBox_SelectedIndexChanged);
            // 
            // typeFilterTabPage
            // 
            this.typeFilterTabPage.Controls.Add(this.typeListBox);
            resources.ApplyResources(this.typeFilterTabPage, "typeFilterTabPage");
            this.typeFilterTabPage.Name = "typeFilterTabPage";
            this.typeFilterTabPage.UseVisualStyleBackColor = true;
            this.typeFilterTabPage.Enter += new System.EventHandler(this.typeFilterTabPage_Enter);
            // 
            // typeListBox
            // 
            this.typeListBox.FormattingEnabled = true;
            resources.ApplyResources(this.typeListBox, "typeListBox");
            this.typeListBox.Name = "typeListBox";
            this.typeListBox.Sorted = true;
            this.typeListBox.DoubleClick += new System.EventHandler(this.newFilterButton_Click);
            this.typeListBox.SelectedValueChanged += new System.EventHandler(this.typeListBox_SelectedValueChanged);
            // 
            // structureFilterTabPage
            // 
            this.structureFilterTabPage.Controls.Add(this.structureEnumListBox);
            this.structureFilterTabPage.Controls.Add(this.classificationComboBox);
            resources.ApplyResources(this.structureFilterTabPage, "structureFilterTabPage");
            this.structureFilterTabPage.Name = "structureFilterTabPage";
            this.structureFilterTabPage.UseVisualStyleBackColor = true;
            this.structureFilterTabPage.Enter += new System.EventHandler(this.structureFilterTabPage_Enter);
            // 
            // structureEnumListBox
            // 
            this.structureEnumListBox.FormattingEnabled = true;
            resources.ApplyResources(this.structureEnumListBox, "structureEnumListBox");
            this.structureEnumListBox.Name = "structureEnumListBox";
            this.structureEnumListBox.Sorted = true;
            this.structureEnumListBox.DoubleClick += new System.EventHandler(this.newFilterButton_Click);
            this.structureEnumListBox.SelectedValueChanged += new System.EventHandler(this.structureEnumListBox_SelectedValueChanged);
            // 
            // classificationComboBox
            // 
            this.classificationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classificationComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.classificationComboBox, "classificationComboBox");
            this.classificationComboBox.Name = "classificationComboBox";
            this.classificationComboBox.SelectedValueChanged += new System.EventHandler(this.classificationComboBox_SelectedValueChanged);
            // 
            // newFilterButton
            // 
            resources.ApplyResources(this.newFilterButton, "newFilterButton");
            this.newFilterButton.Name = "newFilterButton";
            this.newFilterButton.UseVisualStyleBackColor = true;
            this.newFilterButton.Click += new System.EventHandler(this.newFilterButton_Click);
            // 
            // deleteFilterButton
            // 
            resources.ApplyResources(this.deleteFilterButton, "deleteFilterButton");
            this.deleteFilterButton.Name = "deleteFilterButton";
            this.deleteFilterButton.UseVisualStyleBackColor = true;
            this.deleteFilterButton.Click += new System.EventHandler(this.deleteFilterButton_Click);
            // 
            // textBox4
            // 
            resources.ApplyResources(this.textBox4, "textBox4");
            this.textBox4.Name = "textBox4";
            // 
            // textBox5
            // 
            resources.ApplyResources(this.textBox5, "textBox5");
            this.textBox5.Name = "textBox5";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // listView3
            // 
            resources.ApplyResources(this.listView3, "listView3");
            this.listView3.Name = "listView3";
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // filtersListBox
            // 
            this.filtersListBox.FormattingEnabled = true;
            resources.ApplyResources(this.filtersListBox, "filtersListBox");
            this.filtersListBox.Name = "filtersListBox";
            this.filtersListBox.DoubleClick += new System.EventHandler(this.filtersListBox_DoubleClick);
            this.filtersListBox.SelectedValueChanged += new System.EventHandler(this.filtersListBox_SelectedValueChanged);
            // 
            // filterNameTextBox
            // 
            resources.ApplyResources(this.filterNameTextBox, "filterNameTextBox");
            this.filterNameTextBox.Name = "filterNameTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tipMessageToolTip
            // 
            this.tipMessageToolTip.IsBalloon = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // filterButton
            // 
            resources.ApplyResources(this.filterButton, "filterButton");
            this.filterButton.Name = "filterButton";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // filterInfoLabel
            // 
            resources.ApplyResources(this.filterInfoLabel, "filterInfoLabel");
            this.filterInfoLabel.Name = "filterInfoLabel";
            // 
            // ElementsFilterForm
            // 
            this.AcceptButton = this.cancelButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.filterInfoLabel);
            this.Controls.Add(this.filterNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filtersListBox);
            this.Controls.Add(this.m_filtersTabControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.filterButton);
            this.Controls.Add(this.deleteFilterButton);
            this.Controls.Add(this.newFilterButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ElementsFilterForm";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.m_filtersTabControl.ResumeLayout(false);
            this.categoryFilterTabPage.ResumeLayout(false);
            this.familyFilterTabPage.ResumeLayout(false);
            this.symbolFilterTabPage.ResumeLayout(false);
            this.parameterFilterTabPage.ResumeLayout(false);
            this.parameterFilterTabPage.PerformLayout();
            this.typeFilterTabPage.ResumeLayout(false);
            this.structureFilterTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox booleanExpressionTextBox;
        private System.Windows.Forms.TabControl m_filtersTabControl;
        private System.Windows.Forms.TabPage categoryFilterTabPage;
        private System.Windows.Forms.TabPage familyFilterTabPage;
        private System.Windows.Forms.TabPage symbolFilterTabPage;
        private System.Windows.Forms.TabPage parameterFilterTabPage;
        private System.Windows.Forms.TabPage typeFilterTabPage;
        private System.Windows.Forms.TabPage structureFilterTabPage;
        private System.Windows.Forms.Button newFilterButton;
        private System.Windows.Forms.Button deleteFilterButton;
        private System.Windows.Forms.Button reSetButton;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.Button addExecutingFilterButton;
        private System.Windows.Forms.Button applyFilterButton;
        private System.Windows.Forms.ComboBox m_ModeComboBox;
        private System.Windows.Forms.ListBox m_categoryListBox;
        private System.Windows.Forms.ListBox filtersListBox;
        private System.Windows.Forms.ListBox familyNameListBox;
        private System.Windows.Forms.ListBox symbolListBox;
        private System.Windows.Forms.ListBox typeListBox;
        private System.Windows.Forms.TextBox filterNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip tipMessageToolTip;
        private System.Windows.Forms.ListBox criteriaListBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox builtInParasListBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.ComboBox valueTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox structureEnumListBox;
        private System.Windows.Forms.ComboBox classificationComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button notButton;
        private System.Windows.Forms.Button orButton;
        private System.Windows.Forms.Button andButton;
        private System.Windows.Forms.Label filterInfoLabel;
        private System.Windows.Forms.Label booleanAreaLabel;
    }
}