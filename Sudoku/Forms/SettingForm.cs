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
        #region field

        private Control[] textUI;

        #endregion

        #region init

        public SettingForm()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Icon = Properties.Resources.SudokuIcon64;
            textUI = new Control[]
            {
                this, label1, button1, button2
            };
            comboBox1.Items.AddRange(Enum.GetNames(typeof(Language)));
            comboBox1.SelectedIndex = (int)SaveHelper.setting.language;

            SaveHelper.setting.InitText(Forms.Setting, textUI);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveHelper.setting.language = (Language)comboBox1.SelectedIndex;
            Form1.instance.refreshText();

            SaveHelper.Save(SaveHelper.DocsIndex.Setting);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
