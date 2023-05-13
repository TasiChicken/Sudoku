using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Sudoku
{
    public partial class ModeChooseForm : Form
    {
        #region field

        private Control[] textUI;

        #endregion


        public ModeChooseForm(int difficulty)
        {
            this.difficulty = difficulty;
            InitializeComponent();
            init();
        }

        int difficulty;
        private void init()
        {
            Icon = Properties.Resources.SudokuIcon64;
            textUI = new Control[]
            {
                label1, button1, button4
            };

            SaveHelper.setting.InitText(Forms.Choose, textUI);

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = SaveHelper.puzzles[difficulty].Count;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            chooseSudoku((int)numericUpDown1.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            chooseSudoku(random.Next(1, (int)numericUpDown1.Maximum + 1));
        }

        private void chooseSudoku(int index)
        {
            this.Hide();

            PlayForm playForm = new PlayForm(SaveHelper.puzzles[difficulty][index - 1]);
            playForm.Text = this.Text + $"{index}/{numericUpDown1.Maximum}";
            playForm.ShowDialog();

            this.Close();
        }

    }
}
