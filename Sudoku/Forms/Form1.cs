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

        private Control[] textUI;

        #endregion

        #region init

        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            instance = this;

            Icon = Properties.Resources.SudokuIcon64;
            textUI = new Control[]
            {
                this, groupBox1, groupBox2,
                button1, button2, button3,
                button4, button5,
            };

            refreshText();
        }
        
        public void refreshText()
        {
            SaveHelper.setting.InitText(Forms.Menu, textUI);
        }

        #endregion

        #region property

        public static Form1 instance;

        #endregion

        #region play

        private void button1_Click(object sender, EventArgs e)
        {
            showChooseForm(Difficulty.Easy, sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            showChooseForm(Difficulty.Normal, sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showChooseForm(Difficulty.Hard, sender);
        }

        private static readonly string[] noSudokuMsg =
        {
            "There is no Sudoku!", "沒有題目!"
        };
        private void showChooseForm(Difficulty difficulty, object btn)
        {
            int d = (int)difficulty;
            string text = (btn as Button).Text;

            if (SaveHelper.puzzles[d].Count == 0)
            {
                MessageBox.Show(noSudokuMsg[(int)SaveHelper.setting.language], text);
                return;
            }

            ModeChooseForm form = new ModeChooseForm(d);
            form.Text = text;
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        #endregion

        #region function

        private void button4_Click(object sender, EventArgs e)
        {
            new SettingForm().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            new PuzzleForm().ShowDialog();
            this.Show();
        }

        #endregion
    }
}
