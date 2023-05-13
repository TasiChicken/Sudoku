using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class SettingForm : Form
    {
        #region init

        public SettingForm()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Icon = Properties.Resources.SudokuIcon64;
            SettingHelper.InitText(Forms.Setting, new Control[]
            {
                this, label1, button1, button2
            });

            comboBox1.Items.AddRange(Enum.GetNames(typeof(SettingHelper.Language)));
            comboBox1.SelectedIndex = (int)SettingHelper.language;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SettingHelper.language = (SettingHelper.Language)comboBox1.SelectedIndex;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
