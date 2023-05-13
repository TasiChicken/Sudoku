using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Sudoku
{
    public partial class PlayForm : Form
    {
        private SudokuPanel sudokuPanel;
        public PlayForm(Puzzle puzzle)
        {
            InitializeComponent();
            language = (int)SaveHelper.setting.language;

            sudokuPanel = new SudokuPanel();
            sudokuPanel.Init(puzzle);
            sudokuPanel.Parent = this;
            sudokuPanel.Lock();
            sudokuPanel.Top = 80;

            sudokuPanel.Visible = button2.Visible = false;

            Icon = Properties.Resources.SudokuIcon64;
            SaveHelper.setting.InitText(Forms.Play, new Control[]
            {
                button1, button2
            });

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private int time = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            label1.Text = $"{time / 60 / 60:00}:{time / 60 % 60:00}:{time % 60:00}";
        }

        private int language;
        private Timer timer;
        private bool playing = false;
        private void button1_Click(object sender, EventArgs e)
        {
            playing = !playing;

            if (playing)
                timer.Start();
            else
                timer.Stop();

            button1.Text = btn1Texts[playing ? 0 : 1][language];
            sudokuPanel.Visible = button2.Visible = playing;
        }

        private static readonly string[][] btn1Texts = {
            new string[] { "Pause", "暫停" },
            new string[] { "Resume", "繼續" }
        };
        private static readonly string[] correctStrings = { "Correct!", "正確!" };
        private void button2_Click(object sender, EventArgs e)
        {
            if (sudokuPanel.CheckAnswer())
            {
                timer.Stop();
                MessageBox.Show(correctStrings[language]);

                button1.Visible = button2.Visible = false;
                sudokuPanel.Enabled = false;
            }
        }
    }
}
