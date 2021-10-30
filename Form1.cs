
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
        private const int coloms = 10;
        private const int num = 0;
        private const int rows = 10;
        private string str = "";

        private int currentRow;
        private int currentColumn;

        class MyCell
        {
            private DataGridView _dataGridView;
            private static MyCell _instance;
            public Cell CurrentCell { get; set; }
            public DataGridView DataGridView { set { _dataGridView = value; } }
            public static MyCell Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new MyCell();
                    }
                    return _instance;
                }
            }
        }

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
        private void InitializedDataGridView()
        {

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowCount = rows;
            dataGridView1.ColumnCount = coloms;
            FillHeader();
            dataGridView1.AutoResizeRows();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersWidth = dataGridView1.RowHeadersWidth + (7 * rows);
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



        public Form1()
        {
            InitializeComponent();
            InitializedDataGridView();
            AllCells();
            MyCell.Instance.DataGridView = dataGridView1;



        }


        private void button1_Click(object sender, EventArgs e)
        {

            EvaluateTable();

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
                FillHeader();
                DataGridViewRow addedRow = dataGridView1.Rows[dataGridView1.RowCount - 1];
                
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

                        var result = Calculator.Evaluate(textBox1.Text);
                        cell.Value = result;
                        result = cell.RowIndex;

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
            Stream stream;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                if ((stream = saveFileDialog1.OpenFile()) != null) {
                    StreamWriter writer = new StreamWriter(stream);
                    try
                    {
                        for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                writer.Write(dataGridView1.Rows[i].Cells[j].Value.ToString());
                            }
                            writer.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally {
                        writer.Close();
                    }
                    stream.Close();
                }
            }
            
        }
        
        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
 }

   


