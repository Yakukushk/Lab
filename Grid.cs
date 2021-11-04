using LabCalculator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabCalculator1
{
    class Grid
    {
        private const int initColCount = 10;
        private const int initRowCount = 10;
        public int ColCount;
        public int RowCount;

        private _26BasedSystem sys26 = new _26BasedSystem();
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public List<List<Cell>> grid = new List<List<Cell>>();
        public Grid()
        {
            ColCount = initColCount;
            RowCount = initRowCount;

            for (int i = 0; i < initRowCount; i++)
            {
                List<Cell> row = new List<Cell>();
                for (int j = 0; j < initColCount; j++)
                {
                    string name = sys26.To26Sys(j) + i.ToString();
                    row.Add(new Cell(name, i, j));
                    dictionary.Add(name, "");
                }
                grid.Add(row);
            }
        }
        public void SetGrid(int row, int col)
        {
            Clear();

            ColCount = col;
            RowCount = row;
            for (int i = 0; i < RowCount; ++i)
            {
                List<Cell> newRow = new List<Cell>();
                for (int j = 0; j < ColCount; j++)
                {
                    string name = sys26.To26Sys(j) + i.ToString();
                    newRow.Add(new Cell(name, i, j));
                    dictionary.Add(name, "");
                }
                grid.Add(newRow);
            }


        }

        public void ChangeCellWithAllPointers(int row, int col, string expression, DataGridView dataGridView)
        {
            grid[row][col].new_referencesFromThis.Clear();
            grid[row][col].Expression = expression;

            grid[row][col].new_referencesFromThis.Clear();//?
            string value = expression;

            if (expression != "")
                if (expression[0] != '=')
                {
                    grid[row][col].Value = expression;
                    dictionary[FullName(col, row)] = expression;
                    foreach (Cell cell in grid[row][col].pointersToThis)
                    {
                        RefreshCellAndPointers(cell, dataGridView);
                    }
                    return;
                }
            string new_expression = ConvertReferences(row, col, expression);
            if (new_expression != "")
                new_expression = new_expression.Remove(0, 1);

            //loops_check
            if (!grid[row][col].CheckForLoop(grid[row][col].new_referencesFromThis))
            {
                MessageBox.Show("There ia a loop! Change the expression.");
                return;
            }

            grid[row][col].AddPointersAndReferences();

            value = Calculate(new_expression);
            if (value == "error")
            {
                MessageBox.Show("Error in cell" + FullName(col, row) + '!');
                return;
            }
            grid[row][col].Value = value;
            dictionary[FullName(col, row)] = value;
            foreach (Cell cell in grid[row][col].pointersToThis)
            {
                RefreshCellAndPointers(cell, dataGridView);
            }
        }

        public bool RefreshCellAndPointers(Cell cell, DataGridView dataGridView)
        {
            cell.new_referencesFromThis.Clear();

            string new_expression = ConvertReferences(cell.RowIndex, cell.ColIndex, cell.Expression);
            new_expression = new_expression.Remove(0, 1);
            string value = Calculate(new_expression);
            if (value == "error")
            {
                MessageBox.Show("Error in cell" + cell.Index + '!');
                return false;
            }
            grid[cell.RowIndex][cell.ColIndex].Value = value;
            dictionary[FullName(cell.ColIndex, cell.RowIndex)] = value;
            dataGridView[cell.ColIndex, cell.RowIndex].Value = value;
            foreach (Cell point in cell.pointersToThis)
                if (!RefreshCellAndPointers(point, dataGridView))
                    return false;

            return true;
        }
        public string FullName(int col, int row)
        {
            return sys26.To26Sys(col) + row;
        }

        public string ConvertReferences(int row, int col, string expression)
        {
            string cellPattern = @"[A-Z]+[0-9]+";
            Regex regex = new Regex(cellPattern, RegexOptions.IgnoreCase);
            int[] nums;
            foreach (Match match in regex.Matches(expression))
                if (dictionary.ContainsKey(match.Value))
                {
                    nums = sys26.From26Sys(match.Value);
                    grid[row][col].new_referencesFromThis.Add(grid[nums[1]][nums[0]]);
                }
            MatchEvaluator myEvaluator = new MatchEvaluator(RefToValue);
            string new_expression = regex.Replace(expression, myEvaluator);
            return new_expression;
        }

        public string RefToValue(Match m)
        {
            if (dictionary.ContainsKey(m.Value))
                if (dictionary[m.Value] == "")
                    return "0";
                else
                    return dictionary[m.Value];
            return m.Value;
        }
        public void Clear()
        {
            foreach (List<Cell> list in grid)
                list.Clear();
            grid.Clear();

            dictionary.Clear();
            RowCount = 0;
            ColCount = 0;
        }


        public string Calculate(string expression)
        {
            string res;
            try
            {

                res = Convert.ToString(Calculator.Evaluate(expression));
                if (res == "∞")
                    res = "Division by zero error.";
                return res;
            }
            catch
            {
                return "Error";
            }

        }
        public void AddRow(DataGridView dataGridView)
        {
            RowCount++;
            List<Cell> new_row = new List<Cell>();
            for (int i = 0; i < ColCount; i++)
            {
                string name = FullName(i, RowCount - 1);
                new_row.Add(new Cell(name, RowCount - 1, i));
                dictionary.Add(name, " ");
            }
            grid.Add(new_row);

            RefreshReferences();
            foreach (List<Cell> list in grid)
                foreach (Cell cell in list)
                    if (cell.referencesFromThis != null)
                        foreach (Cell cell_in_ref in cell.referencesFromThis)
                            if (cell_in_ref.RowIndex == RowCount - 1)
                                if (!cell_in_ref.pointersToThis.Contains(cell))
                                    cell_in_ref.pointersToThis.Add(cell);
            for (int i = 0; i < ColCount; i++)
                ChangeCellWithAllPointers(RowCount - 1, i, "", dataGridView);
        }
        public void AddCol(DataGridView dataGridView)
        {
            ColCount++;
            for (int i = 0; i < RowCount; i++)
            {
                string name = FullName(ColCount, i);
                grid[i].Add(new Cell(name, i, ColCount));
                dictionary.Add(name, " ");
            }

            RefreshReferences();
            foreach (List<Cell> list in grid)
                foreach (Cell cell in list)
                    if (cell.referencesFromThis != null)
                        foreach (Cell cell_in_ref in cell.referencesFromThis)
                            if (cell_in_ref.ColIndex == ColCount - 1)
                                if (!cell_in_ref.pointersToThis.Contains(cell))
                                    cell_in_ref.pointersToThis.Add(cell);
            for (int i = 0; i < RowCount; i++)
                ChangeCellWithAllPointers(i, ColCount - 1, "", dataGridView);
        }

        public void RefreshReferences()
        {
            foreach (List<Cell> list in grid)
                foreach (Cell cell in list)
                {
                    if (cell.referencesFromThis != null)
                        cell.referencesFromThis.Clear();
                    if (cell.new_referencesFromThis != null)
                        cell.new_referencesFromThis.Clear();
                    if (cell.Expression == "")
                        continue;
                    string new_expression = cell.Expression;
                    if (cell.Expression[0] == '=')
                    {
                        new_expression = ConvertReferences(cell.RowIndex, cell.ColIndex, cell.Expression);
                        cell.referencesFromThis.AddRange(cell.new_referencesFromThis);
                    }
                }
        }

        public bool DeleteRow(DataGridView dataGridView)
        {
            List<Cell> sufferedCells = new List<Cell>();
            List<string> notEmptyCells = new List<string>();
            if (RowCount == 0)
                return false;
            int curRow = RowCount - 1;
            for (int i = 0; i < ColCount; i++)
            {
                string name = FullName(i, curRow);
                if (dictionary[name] != "0" && dictionary[name] != "" && dictionary[name] != " ")
                    notEmptyCells.Add(name);
                if (grid[curRow][i].pointersToThis.Count() != 0)
                    sufferedCells.AddRange(grid[curRow][i].pointersToThis);
            }
            if ((sufferedCells.Count() != 0) || (notEmptyCells.Count() != 0))
            {
                string errorMessage = "";
                if (notEmptyCells.Count() != 0)
                {
                    errorMessage = "There are not empty cells: ";
                    errorMessage += string.Join("; ", notEmptyCells.ToArray());
                    errorMessage += Environment.NewLine;
                }
                if (sufferedCells.Count() != 0)
                {
                    errorMessage += "There are other cells that point to cells from the following row: ";
                    foreach (Cell cell in sufferedCells)
                        errorMessage += string.Join(";", cell.Index);
                    errorMessage += Environment.NewLine;
                }
                errorMessage += "Are you sure you want to delete this row?";
                DialogResult result = MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                    return false;

            }
            // удаляем клетки из словаря
            for (int i = 0; i < ColCount; i++)
            {
                string name = FullName(i, curRow);
                dictionary.Remove(name);
            }
            //  обновляем поврежденные клетки
            foreach (Cell cell in sufferedCells)
                RefreshCellAndPointers(cell, dataGridView);
            // удаляем последнюю строку таблицы
            grid.RemoveAt(curRow);

            RowCount--;
            return true;
        }
        public bool DeleteCol(DataGridView dataGridView)
        {
            List<Cell> sufferedCells = new List<Cell>();
            List<string> notEmptyCells = new List<string>();
            if (ColCount == 0)
                return false;
            int curCol = ColCount - 1;
            for (int i = 0; i < RowCount; i++)
            {
                string name = FullName(curCol, i);
                if (dictionary[name] != "0" && dictionary[name] != "" && dictionary[name] != " ")
                    notEmptyCells.Add(name);
                if (grid[i][curCol].pointersToThis.Count() != 0)
                    sufferedCells.AddRange(grid[i][curCol].pointersToThis);
            }
            if ((sufferedCells.Count() != 0) || (notEmptyCells.Count() != 0))
            {
                string errorMessage = "";
                if (notEmptyCells.Count() != 0)
                {
                    errorMessage = "There are not empty cells: ";
                    errorMessage += string.Join("; ", notEmptyCells.ToArray());
                    errorMessage += Environment.NewLine;
                }
                if (sufferedCells.Count() != 0)
                {
                    errorMessage += "There are other cells that point to cells from the following column: ";
                    foreach (Cell cell in sufferedCells)
                        errorMessage += string.Join(";", cell.Index);
                    errorMessage += Environment.NewLine;
                }
                errorMessage += "Are you sure you want to delete this column?";
                DialogResult result = MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                    return false;

            }
            // удаляем клетки из словаря
            for (int i = 0; i < RowCount; i++)
            {
                string name = FullName(curCol, i);
                dictionary.Remove(name);
                grid[i].RemoveAt(curCol);
            }
            //  обновляем поврежденные клетки
            foreach (Cell cell in sufferedCells)
                RefreshCellAndPointers(cell, dataGridView);
            // удаляем последний столбик таблицы

            ColCount--;
            return true;
        }

        public void Save(StreamWriter sw)
        {
            sw.WriteLine(RowCount);
            sw.WriteLine(ColCount);
            foreach (List<Cell> list in grid)
                foreach (Cell cell in list)
                {
                    sw.WriteLine(cell.Index);
                    sw.WriteLine(cell.Expression);
                    sw.WriteLine(cell.Value);
                    if (cell.referencesFromThis == null)
                        sw.WriteLine();
                    else
                    {
                        sw.WriteLine(cell.referencesFromThis.Count);
                        foreach (Cell refCell in cell.referencesFromThis)
                            sw.WriteLine(refCell.Index);
                    }
                    if (cell.pointersToThis == null)
                        sw.WriteLine(0);
                    else
                    {
                        sw.WriteLine(cell.pointersToThis.Count);
                        foreach (Cell ptrCell in cell.pointersToThis)
                            sw.WriteLine(ptrCell.Index);
                    }



                }
        }

        public void Open(int row, int col, StreamReader sr, DataGridView dataGridView)
        {
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    string index = sr.ReadLine();
                    string expression = sr.ReadLine();
                    string value = sr.ReadLine();
                    if (expression != "")
                        dictionary[index] = value;
                    else
                        dictionary[index] = "";
                    int refCount = Convert.ToInt32(sr.ReadLine());
                    List<Cell> newRef = new List<Cell>();
                    string refer;
                    for (int i = 0; i < refCount; i++)
                    {
                        refer = sr.ReadLine();
                        newRef.Add(grid[sys26.From26Sys(refer)[1]][sys26.From26Sys(refer)[0]]);
                    }

                    int ptrCount = Convert.ToInt32(sr.ReadLine());
                    List<Cell> newPtr = new List<Cell>();
                    string point;
                    for (int i = 0; i < ptrCount; i++)
                    {
                        point = sr.ReadLine();
                        newPtr.Add(grid[sys26.From26Sys(point)[1]][sys26.From26Sys(point)[0]]);
                    }
                    grid[r][c].SetCell(value, expression, newRef, newPtr);
                    int icol = grid[r][c].ColIndex;
                    int irow = grid[r][c].RowIndex;
                    dataGridView[icol, irow].Value = dictionary[index];


                }
            }
        }
    }
    }

