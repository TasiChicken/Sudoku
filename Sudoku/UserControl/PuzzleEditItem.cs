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
    public partial class PuzzleEditItem : UserControl
    {
        public PuzzleEditItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PuzzleForm.instance.DeleteItem(index);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PuzzleForm.instance.ViewItem(index);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PuzzleForm.instance.EditItem(index);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PuzzleForm.instance.PrintItem(index);
        }


        private int _index;
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                label1.Text = value + 1 + "";
            }
        }

    }
}
