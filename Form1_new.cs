
using LabCalculator;
using LabCalculator1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabCalculator1
{

    public partial class Form1 : Form
    {

        private Grid GR = new Grid();
        private _26BasedSystem sys26 = new _26BasedSystem();


        private int currentRow;
        private int currentColumn;



        public int CurrentRow
        {
            get { return currentRow; }
            set { currentRow = value; }
        }

        public int CurrentColumn
        {
            get { return currentColumn; }
            set { currentColumn = value; }
        }





        public Form1()
        {
            InitializeComponent();
            InitTable(GR.RowCount, GR.ColCount);



        }
        private void InitTable(int row, int col)
        {
            dataGridView1.AllowUserToAddRows = false;
            for (int i = 0; i < col; i++)
            {
                string colname = sys26.To26Sys(i);
                dataGridView1.Columns.Add(colname, colname);
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.RowCount = row;
            for (int i = 0; i < row; i++)
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            GR.SetGrid(row, col);

            dataGridView1.AllowUserToAddRows = false;
        }
        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GridFile|*.grd";
            saveFileDialog.Title = "Save Grid File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                GR.Save(sw);
                sw.Close();
                fs.Close();
            }
        }
        void FillHeader()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                //col.CellTemplate = new Cell();
                col.HeaderText = "A" + (col.Index + 1);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = "0" + (row.Index + 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int col = dataGridView1.SelectedCells[0].ColumnIndex;
            int row = dataGridView1.SelectedCells[0].RowIndex;
            string expr = textBox1.Text;
            if (expr == "")
                return;
            GR.ChangeCellWithAllPointers(row, col, expr, dataGridView1);
            dataGridView1[col, row].Value = GR.grid[row][col].Value;

        }





        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddColumn();
            AddRow();

        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }




        private void SingleCell(DataGridViewRow row, DataGridViewCell cell)
        {
            string cellName = "R" + (row.Index + 1).ToString() + "C" + (cell.ColumnIndex + 1).ToString();


        }

        private void AllCells()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                foreach (DataGridViewCell cell in row.Cells)
                {
                    SingleCell(row, cell);

                }
            }
        }
        private void AddRow()
        {
            try
            {
                dataGridView1.Rows.Add(new DataGridViewRow());

                DataGridViewRow addedRow = dataGridView1.Rows[dataGridView1.RowCount - 1];
                FillHeader();
                foreach (DataGridViewCell cell in addedRow.Cells)
                {
                    SingleCell(addedRow, cell);
                }
            }
            catch
            {
                MessageBox.Show("Error Column or Rows!");
            }
        }

        private void AddColumn()
        {
            try
            {
                dataGridView1.Columns.Add(new DataGridViewColumn(dataGridView1.Rows[0].Cells[0]));

                FillHeader();
                foreach (DataGridViewRow cl in dataGridView1.Rows)
                {
                    SingleCell(cl, cl.Cells[dataGridView1.ColumnCount - 1]);
                }
            }
            catch
            {
                MessageBox.Show("Error Column or Rows!");
            }
        }
        private void DeletedRow()
        {
            int last = dataGridView1.RowCount - 1;
            dataGridView1.Rows.RemoveAt(last);
            int last2 = dataGridView1.ColumnCount - 1;
            dataGridView1.Columns.RemoveAt(last2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeletedRow();
        }

        private void EvaluateTable()
        {



            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {

                        string result;
                        result = Convert.ToString(Calculator.Evaluate(textBox1.Text));

                        cell.Value = result;

                        currentRow = cell.RowIndex;
                        currentColumn = cell.ColumnIndex;
                        Convert.ToString(result);

                    }
                }
                break;

            }
        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddColumn();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddRow();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream str = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((str = openFileDialog1.OpenFile()) != null)
                {
                    StreamReader myread = new StreamReader(str);
                    string[] mystr;
                    int num = 0;
                    try
                    {
                        string[] str1 = myread.ReadToEnd().Split('\n');
                        num = str1.Count();
                        dataGridView1.RowCount = num;
                        for (int i = 0; i < num; i++)
                        {
                            mystr = str1[i].Split('^');
                            for (int j = 0; j < dataGridView1.ColumnCount - 1; j++)
                            {
                                try
                                {
                                    dataGridView1.Rows[i].Cells[j].Value = mystr[j];
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        myread.Close();
                    }
                }
            }
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GridFile|*.grd";
            saveFileDialog.Title = "Save Grid File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                GR.Save(sw);
                sw.Close();
                fs.Close();
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }






    }

}

   


