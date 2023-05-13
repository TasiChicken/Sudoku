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
    public partial class SudokuPanel : UserControl
    {
        public SudokuPanel()
        {
            InitializeComponent();

            for (int i = 0; i < 9; i++)
                clonedSudoku[i] = new int[9];

            displayPanel = new SudokuDisplayPanel();
            controlPanel = new SudokuControlPanel(displayPanel);

            displayPanel.Parent = controlPanel.Parent = this;
            displayPanel.Left = displayPanel.Top = 0;
            controlPanel.Left = (this.Width - controlPanel.Width + displayPanel.Width) / 2;
            controlPanel.Top = (displayPanel.Height - controlPanel.Height) / 2;
        }

        private Puzzle puzzle;
        private int[][] clonedSudoku = new int[9][];
        private SudokuDisplayPanel displayPanel;
        private SudokuControlPanel controlPanel;
        public void Init(Puzzle puzzle)
        {
            this.puzzle = puzzle;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    clonedSudoku[i][j] = puzzle.Sudoku[i][j];

            displayPanel.DisplaySudoku(clonedSudoku);
        }

        public bool CoverPuzzleSudoku()
        {
            if (displayPanel.CheckNotRepeat())
            {
                puzzle.Sudoku = clonedSudoku;
                return true;
            }
            return false;
        }

        public void Lock()
        {
            displayPanel.Lock();
        }

        public bool CheckAnswer()
        {
            bool result = true;

            result = result && displayPanel.CheckNotRepeat();
            result = result && displayPanel.CheckFull();

            return result;
        }
    }
}
