using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05.FourInARow
{
    public partial class GameSettingsMenu : Form
    {
        public GameSettingsMenu()
        {
            InitializeComponent();
        }

        private void Player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Player2CheckBox.Checked)
            {
                this.Player2TextBox.Enabled = true;
                this.Player2TextBox.Text = string.Empty;
            }
            else
            {
                this.Player2TextBox.Enabled = false;
                this.Player2TextBox.Text = "[Computer]";
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            GameBoardForm gameBoardForm = new GameBoardForm((int)this.numericUpDownRows.Value, (int)this.numericUpDownCols.Value);
            gameBoardForm.ShowDialog();
            this.Close();
        }
    }
}
