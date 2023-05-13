using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class PuzzleForm : Form
    {
        #region field

        private Control[] textUI;

        #endregion

        #region init

        public PuzzleForm()
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
                this, button1, button2, button3
            };

            SaveHelper.setting.InitText(Forms.Puzzle, textUI);

            comboBox1.Items.AddRange(SaveHelper.difficultyStrings[(int)SaveHelper.setting.language]);
            comboBox1.SelectedIndex = 0;

            displayItems();
        }

        #endregion

        #region button

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON File (*.json) | *.json";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            SaveHelper.puzzles = JsonSerializer.Deserialize<List<Puzzle>[]>(File.ReadAllText(dialog.FileName));
            SaveHelper.Save(SaveHelper.DocsIndex.Puzzle);

            displayItems();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON File (*.json) | *.json";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            SaveHelper.Save(SaveHelper.DocsIndex.Puzzle, dialog.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveHelper.puzzles[comboBox1.SelectedIndex].Add(new Puzzle());
            SaveHelper.Save(SaveHelper.DocsIndex.Puzzle);
            displayItems();

            EditItem(SaveHelper.puzzles[comboBox1.SelectedIndex].Count - 1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayItems();
        }

        #endregion

        #region property

        public static PuzzleForm instance;

        #endregion

        #region function

        List<PuzzleEditItem> editItemPool = new List<PuzzleEditItem>();

        private void displayItems()
        {
            panel1.Controls.Clear();

            for(int i = 0; i < SaveHelper.puzzles[comboBox1.SelectedIndex].Count; i++)
            {
                if (editItemPool.Count <= i) editItemPool.Add(new PuzzleEditItem());

                editItemPool[i].Parent = panel1;
                editItemPool[i].Top = editItemPool[i].Height * i;
                editItemPool[i].index = i;
            }
        }

        #endregion

        #region api

        public void DeleteItem(int index)
        {
            SaveHelper.puzzles[comboBox1.SelectedIndex].RemoveAt(index);
            SaveHelper.Save(SaveHelper.DocsIndex.Puzzle);
            displayItems();
        }

        private static readonly string[] confirmText = { "Confirm", "確認" };
        private static readonly string[] cancelText = { "Cancel", "取消" };
        private Form editForm = null;
        private SudokuPanel sudokuPanel = null;
        private Button editFormConfirmBtn = null;
        private Button editFormCancelBtn = null;
        public void EditItem(int index)
        {
            initEditForm();

            editForm.Text = $"{comboBox1.Text}-{index + 1}/{SaveHelper.puzzles[comboBox1.SelectedIndex].Count}";

            sudokuPanel.Parent = null;
            sudokuPanel = new SudokuPanel();
            sudokuPanel.Parent = editForm;
            sudokuPanel.Init(SaveHelper.puzzles[comboBox1.SelectedIndex][index]);
            
            int languageIndex = (int)SaveHelper.setting.language;
            editFormConfirmBtn.Text = confirmText[languageIndex];
            editFormCancelBtn.Text = cancelText[languageIndex];

            this.Hide();
            editForm.ShowDialog();
            this.Show();
        }

        private void initEditForm()
        {
            if (editForm != null) return;

            editForm = new Form();
            editForm.Icon = Icon;
            editForm.Font = Font;
            editForm.AutoSize = true;
            editForm.StartPosition = FormStartPosition.CenterScreen;

            sudokuPanel = new SudokuPanel();
            sudokuPanel.Parent = editForm;

            editFormConfirmBtn = new Button();
            editFormCancelBtn = new Button();

            editFormConfirmBtn.Top = editFormCancelBtn.Top = sudokuPanel.Bottom;
            editFormConfirmBtn.Height = editFormCancelBtn.Height = 60;
            editFormConfirmBtn.Width = editFormCancelBtn.Width = 300;

            int margin = (sudokuPanel.Width - editFormConfirmBtn.Width * 2) / 4;
            editFormConfirmBtn.Left = margin;
            editFormCancelBtn.Left = margin * 3 + editFormConfirmBtn.Width;

            editFormConfirmBtn.Click += ConfirmBtn_Click;
            editFormCancelBtn.Click += CancelBtn_Click;

            editFormConfirmBtn.Parent = editFormCancelBtn.Parent = editForm;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            editForm.Close();
        }

        private static readonly string[] errorMessage = { "Wrong!", "錯誤!" };
        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (!sudokuPanel.CoverPuzzleSudoku())
            {
                MessageBox.Show(errorMessage[(int)SaveHelper.setting.language]);
                return;
            }
         
            SaveHelper.Save(SaveHelper.DocsIndex.Puzzle);
            editForm.Close();
        }

        public void ViewItem(int index)
        {
            Form form = new Form();

            form.Icon = Icon;
            form.Text = $"{comboBox1.Text}-{index + 1}/{SaveHelper.puzzles[comboBox1.SelectedIndex].Count}";
            form.StartPosition = FormStartPosition.CenterScreen;
            form.AutoSize = true;
            
            SudokuDisplayPanel displayPanel = new SudokuDisplayPanel();
            displayPanel.DisplaySudoku(SaveHelper.puzzles[comboBox1.SelectedIndex][index].Sudoku);
            displayPanel.Parent = form;
            displayPanel.Enabled = false;
            
            form.ShowDialog();
        }

        public void PrintItem(int index)
        {
            PrintDialog dialog = new PrintDialog();
            dialog.Document = new System.Drawing.Printing.PrintDocument();

            dialog.Document.PrintPage += (s, e) =>
            {
                int size = Math.Min(e.MarginBounds.Width, e.MarginBounds.Height);
                e.Graphics.DrawImage(SudokuDisplayPanel.CreateBitmap(SaveHelper.puzzles[comboBox1.SelectedIndex][index].Sudoku), (e.PageBounds.Width - size) / 2, (e.PageBounds.Height - size) / 2, size, size);
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            dialog.Document.Print();
        }

        #endregion
    }
}
