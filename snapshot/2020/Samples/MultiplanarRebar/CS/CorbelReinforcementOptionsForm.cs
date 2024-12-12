using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.MultiplanarRebar.CS
{
    partial class CorbelReinforcementOptionsForm : System.Windows.Forms.Form
    {
        private CorbelReinforcementOptions CorbelReinforcementOptions;

        public CorbelReinforcementOptionsForm(CorbelReinforcementOptions options)
        {
            CorbelReinforcementOptions = options;

            InitializeComponent();

            Initialize();
        }

        void Initialize()
        {
            List<RebarBarType> bartypes4 = new List<RebarBarType>(CorbelReinforcementOptions.RebarBarTypes);
            columnBarTypeComboBox.DataSource = bartypes4;
            columnBarTypeComboBox.ValueMember = "Name";

            List<RebarBarType> bartypes1 = new List<RebarBarType>(CorbelReinforcementOptions.RebarBarTypes);
            topBarTypeComboBox.DataSource = bartypes1;
            topBarTypeComboBox.ValueMember = "Name";

            List<RebarBarType> bartypes2 = new List<RebarBarType>(CorbelReinforcementOptions.RebarBarTypes);
            stirrupBarTypeComboBox.DataSource = bartypes2;
            stirrupBarTypeComboBox.ValueMember = "Name";

            List<RebarBarType> bartypes3 = new List<RebarBarType>(CorbelReinforcementOptions.RebarBarTypes);
            multiplanarBarTypeComboBox.DataSource = bartypes3;
            multiplanarBarTypeComboBox.ValueMember = "Name";

            topBarCountTextBox.Text = "3";
            stirrupBarCountTextBox.Text = "3";
        }

        private void topBarTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CorbelReinforcementOptions.TopBarType = topBarTypeComboBox.SelectedItem as RebarBarType;
        }

        private void stirrupBarTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CorbelReinforcementOptions.StirrupBarType = stirrupBarTypeComboBox.SelectedItem as RebarBarType;
        }

        private void multiplanarBarTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CorbelReinforcementOptions.MultiplanarBarType = multiplanarBarTypeComboBox.SelectedItem as RebarBarType;
        }

        private void columnBarTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CorbelReinforcementOptions.HostStraightBarType = columnBarTypeComboBox.SelectedItem as RebarBarType;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CorbelReinforcementOptions.TopBarCount = int.Parse(topBarCountTextBox.Text);
            CorbelReinforcementOptions.StirrupBarCount = int.Parse(stirrupBarCountTextBox.Text);

            DialogResult = DialogResult.OK;
        }

        private void topBarCountTextBox_Validating(object sender, CancelEventArgs e)
        {
            int count = 0;
            if (!int.TryParse(topBarCountTextBox.Text, out count) || count < 2)
            {
                e.Cancel = true;
            }
        }

        private void stirrupBarCountTextBox_Validating(object sender, CancelEventArgs e)
        {
            int count = 0;
            if (!int.TryParse(stirrupBarCountTextBox.Text, out count) || count < 2)
            {
                e.Cancel = true;
            }
        }
    }
}
