using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        #region field

        #endregion

        #region init

        public Form1()
        {
            instance = this;

            InitializeComponent();
            Icon = Properties.Resources.SudokuIcon64;
            RefreshText();
        }
        public static Form1 instance;
        public void RefreshText()
        {
            SettingHelper.InitText(Forms.Menu, new Control[]
            {
                this, groupBox1, groupBox2,
                button1, button2, button3,
                button4, button5, button6,
            });
        }

        #endregion

        #region play

        private void button1_Click(object sender, EventArgs e)
        {
            new MenuForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region function

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SettingForm().ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
