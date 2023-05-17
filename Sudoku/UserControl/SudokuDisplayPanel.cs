using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class SudokuDisplayPanel : UserControl
    {
        public static Bitmap CreateBitmap(int[][] sudoku)
        {
            SudokuDisplayPanel instance = new SudokuDisplayPanel();
            Bitmap bitmap = new Bitmap(instance.Width, instance.Height);
            instance.DisplaySudoku(sudoku);
            
            instance.DrawToBitmap(bitmap, new Rectangle(0, 0, instance.Width, instance.Height));
            return bitmap;
        }

        public SudokuDisplayPanel()
        {
            InitializeComponent();

            origin = new int[9][];
            for (int i = 0; i < 9; i++)
                origin[i] = new int[9];

            int gridSize = (this.Width - margin * 12) / 9;
            Size size = new Size(gridSize, gridSize);

            grids = new Button[9][];
            for (int i = 0; i < 9; i++)
            {
                grids[i] = new Button[9];

                for (int j = 0; j < 9; j++)
                {
                    grids[i][j] = new Button();
                    grids[i][j].BackColor = Color.White;
                    grids[i][j].Name = "" + i + j;
                    grids[i][j].Click += Grid_Click;
                    grids[i][j].KeyPress += Grid_KeyPress;
                    grids[i][j].KeyDown += Grid_KeyDown;
                    grids[i][j].Parent = this;
                    grids[i][j].Size = size;
                    grids[i][j].Left = margin * (i / 3 + i + 1) + gridSize * i;
                    grids[i][j].Top = margin * (j / 3 + j + 1) + gridSize * j;
                }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                SetGrid(0);
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            int number = e.KeyChar - '0';
            if (number < 0 || number > 9) return;

            SetGrid(number);
        }

        private int[][] sudoku;
        private int[][] origin;
        private Button[][] grids;
        private const int margin = 3;

        private void Grid_Click(object sender, EventArgs e)
        {
            if (focusX != -1) grids[focusX][focusY].BackColor = Color.White;
            string name = (sender as Button).Name;
            focusX = name[0] - '0';
            focusY = name[1] - '0';
            grids[focusX][focusY].BackColor = Color.LightCyan;
        }

        private int focusX = -1;
        private int focusY = -1;
        public void SetGrid(int value)
        {
            if (focusX == -1) return;

            sudoku[focusX][focusY] = value;
            grids[focusX][focusY].Text = value == 0 ? "" : value.ToString();
        }

        public void DisplaySudoku(int[][] sudoku)
        {
            this.sudoku = sudoku;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    origin[i][j] = sudoku[i][j];

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grids[i][j].Text = sudoku[i][j] == 0 ? "" : sudoku[i][j].ToString();
        }

        public void Lock()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (origin[i][j] != 0)
                    {
                        grids[i][j].BackColor = Color.LightGoldenrodYellow;
                        grids[i][j].Enabled = false;
                    }
        }

        public bool CheckNotRepeat()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grids[i][j].BackColor = Color.White;

            Lock();

            bool notRepeat = true;

            int[] numbers = new int[10];

            for(int i = 0; i < 9; i++)
            {
                newIntArray(numbers);
             
                for (int j = 0; j < 9; j++)
                    numbers[sudoku[i][j]]++;
                
                for (int j = 0; j < 9; j++)
                    if(sudoku[i][j] != 0 && numbers[sudoku[i][j]] > 1)
                    {
                        notRepeat = false;
                        grids[i][j].BackColor = Color.PaleVioletRed;
                    }
            }


            for (int i = 0; i < 9; i++)
            {
                newIntArray(numbers);
                
                for (int j = 0; j < 9; j++)
                    numbers[sudoku[j][i]]++;

                for (int j = 0; j < 9; j++)
                    if (sudoku[j][i] != 0 && numbers[sudoku[j][i]] > 1)
                    {
                        notRepeat = false;
                        grids[j][i].BackColor = Color.PaleVioletRed;
                    }
            }
            
            for (int i = 0; i < 9; i += 3)
                for (int j = 0; j < 9; j += 3)
                    notRepeat = notRepeat && checkNotRepeat_Jiugongge(i, j, numbers);
   
            return notRepeat;
        }

        private bool checkNotRepeat_Jiugongge(int x, int y, int[] array)
        {
            bool notRepeat = true;

            newIntArray(array);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    array[sudoku[x + i][y + j]]++;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (sudoku[x + i][y + j] != 0 && array[sudoku[x + i][y + j]] > 1)
                    {
                        notRepeat = false;
                        grids[x + i][y + j].BackColor = Color.PaleVioletRed;
                    }

            return notRepeat;
        }

        private void newIntArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = 0;
        }

        public bool CheckFull()
        {
            bool full = true;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (sudoku[i][j] == 0)
                    {
                        full = false;
                        grids[i][j].BackColor = Color.PaleVioletRed;
                    }

            return full;
        }
    }
}
