using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ClosedXML.Excel;
using Excel;
using GemBox.Spreadsheet;

namespace Report
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private DataTable yesterdayDataTable;
        private DataTable remainDataTable;
        private DataTable revisionDataTable;
        private DataTable todayDataTable;
        private const int topCount = 6;
        private List<string>filterArray = new List<string> { "чол", "жін" ,"підл", "дит","юн" };
        private DataTable cleanData(DataTable dataTable)
        {
            dataTable.Columns.RemoveAt(0);
            dataTable.Columns[0].ColumnName = "article";
            dataTable.Columns[1].ColumnName = "initial";
            dataTable.Columns[2].ColumnName = "receipts";
            dataTable.Columns[3].ColumnName = "rate";
            dataTable.Columns[4].ColumnName = "final";
            dataTable.AcceptChanges();
            DataTable resTable = dataTable.Clone();
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dataTable.Rows[i];
                if (Regex.IsMatch(dr["article"].ToString(), @"^\d"))
                {
                    if (dr["rate"] == DBNull.Value)
                    {
                        dr["rate"] = 0.0;
                    }
                    if (dr["final"] == DBNull.Value)
                    {
                        dr["final"] = 0.0;
                    }
                    resTable.Rows.Add(dr.ItemArray);
                    resTable.AcceptChanges();

                }
            }
            return resTable;
        }
        private DataTable getTop(DataTable dt, string filter)
        {
            var searchTerm =
           new Regex(@"\s" + filter + ".?");
            //var res = (from d in dt.AsEnumerable()
            //           from c in notContains
            //           where !(d.Field<string>("article").Contains(c))
            //           where (d.Field<string>("article").Contains(filter))
            //           orderby d.Field<double>("rate") descending
            //           select d).Distinct().Take(5);
            var res = (from d in dt.AsEnumerable()
                       let matches = searchTerm.Matches(d.Field<string>("article"))
                       where matches.Count > 0
                       orderby d.Field<double>("rate") descending
                       select d
                      ).Distinct().Take(topCount);
            return res.CopyToDataTable();
        }
        private void colorizeDataView(int columnCount, int rowCount)
        {
            for (int j = 0; j < columnCount; j++)
            {
                for (int i = 0; i < rowCount - 1; i++)
                {
                    if ((double)(i / (topCount*1)) < 1.0)
                    {
                        sales.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Aqua;
                        continue;
                    }
                    if ((double)(i / (topCount * 2)) < 1.0)
                    {
                        sales.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.DarkSalmon;
                        continue;
                    }
                    if ((double)(i / (topCount * 3)) < 1.0)
                    {
                        sales.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.DarkKhaki;
                        continue;
                    }

                    if ((double)(i / (topCount * 4)) < 1.0)
                    {
                        sales.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.ForestGreen;
                        continue;
                    }
                    if ((double)(i / (topCount * 5)) < 1.0)
                    {
                        sales.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Gold;
                        continue;
                    }
                }
            }
        }
        private DataTable excelToRemainTable(OpenFileDialog ofd)
        {
            FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
            reader.IsFirstRowAsColumnNames = false;
            var table = reader.AsDataSet().Tables[0];
            reader.Close();
            return this.cleanData(table);
        }
        private DataTable cleanRevisionData(DataTable dataTable)
        {
            //dataTable.Columns.RemoveAt(0);
            dataTable.Columns[0].ColumnName = "article";
            dataTable.Columns[1].ColumnName = "initial";
            dataTable.Columns[2].ColumnName = "receipts";
            dataTable.Columns[3].ColumnName = "rate";
            dataTable.Columns[4].ColumnName = "final";
            dataTable.AcceptChanges();
            DataTable resTable = dataTable.Clone();
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dataTable.Rows[i];
                if (Regex.IsMatch(dr["article"].ToString(), @"^\d"))
                {
                    if (dr["rate"] == DBNull.Value)
                    {
                        dr["rate"] = 0.0;
                    }
                    if (dr["final"] == DBNull.Value)
                    {
                        dr["final"] = 0.0;
                    }
                    resTable.Rows.Add(dr.ItemArray);
                    resTable.AcceptChanges();

                }
            }
            return resTable;
        }
        private DataTable excelToRevisionTable(OpenFileDialog ofd)
        {
            FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(fs);
            reader.IsFirstRowAsColumnNames = false;
            var table = reader.AsDataSet().Tables[0];
            reader.Close();
            return this.cleanRevisionData(table);
        }
        private DataTable getSingleReminsDataTable(DataTable dataTable)
        {
            return (
                dataTable.AsEnumerable()
                .Where(r => r.Field<double>("final")==1.0)
                ).CopyToDataTable();
        }
        private DataTable FindRemains()
        {
            this.yesterdayDataTable = this.getSingleReminsDataTable(this.yesterdayDataTable);       
            this.todayDataTable = this.getSingleReminsDataTable(this.todayDataTable);
            DataTable TableC = this.yesterdayDataTable.AsEnumerable()
            .Where(ra => this.todayDataTable.AsEnumerable()
            .All(rb => rb.Field<string>("article") != ra.Field<string>("article"))
        )
    .CopyToDataTable();
            return TableC;
        }
        private void SaveToExcel(DataTable dt,string fileName)
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt,"report");
            wb.SaveAs(fileName);
        }
        private void Open_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TopSalesPath.Text = ofd.FileName;
                        var table = this.excelToRemainTable(ofd);
                        var resDataTable = table.Clone();
                        foreach (var filter in this.filterArray)
                        {
                            resDataTable.Merge(this.getTop(table, filter));
                        }
                        sales.DataSource = resDataTable;
                        //this.sales.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                        this.colorizeDataView(sales.ColumnCount, sales.RowCount);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }

           
        }
        private void YesterdaySales_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        YesterdaySalesPath.Text = ofd.FileName;
                        this.yesterdayDataTable = this.excelToRemainTable(ofd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            
        }
        private void TodaySales_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TodaySalesPath.Text = ofd.FileName;
                        this.todayDataTable = this.excelToRemainTable(ofd);
                        sales.DataSource = this.FindRemains();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            
        }

        private void SaveTable_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.SupportMultiDottedExtensions = false;
                saveFileDialog.ValidateNames = true;
                saveFileDialog.DereferenceLinks = false; // Will return .lnk in shortcuts.
                saveFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "")
                {
                    string newFileName = saveFileDialog.FileName;
                    if (!newFileName.Contains(".xlsx"))
                    {
                        newFileName = newFileName + ".xlsx";
                    }
                    var dt = (DataTable)(sales.DataSource);
                    this.SaveToExcel(dt, newFileName);
                    MessageBox.Show("Saved " + newFileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
            
        }

        private void RemainButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        RemainTextBox.Text = ofd.FileName;
                        this.remainDataTable = this.excelToRemainTable(ofd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
        }

        private void RevisionButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        RevisionTextBox.Text = ofd.FileName;
                        this.revisionDataTable = this.excelToRevisionTable(ofd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
        }

        private void sales_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.sales.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
        }
    }
}
