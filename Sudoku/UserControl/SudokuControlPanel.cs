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
    public partial class SudokuControlPanel : UserControl
    {
        public SudokuControlPanel(SudokuDisplayPanel displayPanel)
        {
            this.displayPanel = displayPanel;
            InitializeComponent();
        }

        private SudokuDisplayPanel displayPanel;
        private const int margin = 3;
        static readonly string[] clearText = { "Clear", "清除" };
        private void SudokuControlPanel_Load(object sender, EventArgs e)
        {
            int sizeValue = (this.Width - margin * 4) / 3;
            Size size = new Size(sizeValue, sizeValue);

            Button button;
            for (int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                {
                    button = new Button();
                    button.Name = button.Text = (i * 3 + j + 1).ToString();
                    button.Size = size;
                    button.Left = margin * (j + 1) + sizeValue * j;
                    button.Top = margin * (i + 1) + sizeValue * i;
                    button.Click += Button_Click;
                    button.KeyPress += Button_KeyPress;
                    button.Parent = this;
                }

            button = new Button();
            button.Text = clearText[(int)SaveHelper.setting.language];
            button.Height = sizeValue;
            button.Width = this.Width - margin * 2;
            button.Left = margin;
            button.Top = margin * 4 + sizeValue * 3;
            button.Name = "0";
            button.Click += Button_Click;
            button.KeyPress += Button_KeyPress;
            button.Parent = this;
        }

        private void Button_KeyPress(object sender, KeyPressEventArgs e)
        {
            int value = e.KeyChar - '0';
            if (value < 0 || value > 9) return;

            displayPanel.SetGrid(value);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            displayPanel.SetGrid((sender as Button).Name[0] - '0');
        }
    }
}
