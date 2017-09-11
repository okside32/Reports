//using ClosedXML.Excel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Excel;
using Color = System.Drawing.Color;


namespace Report
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataTable yesterdayDataTable;
        private DataTable remainsDataTable;
        private DataTable revisionDataTable;
        private DataTable todayDataTable;
        private DataTable transferDataTable;
        private const int topCount = 6;
        private readonly List<string> filterArray = new List<string> {"чол", "жін", "підл", "дит", "юн"};

        private DataTable getTop(DataTable dt, string filter)
        {
            var searchTerm = new Regex(@"\s" + filter + ".?");
            IEnumerable<DataRow> res = (from d in dt.AsEnumerable()
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
                    if (i/(topCount*1) < 1.0)
                    {
                        salesDataGridView.Rows[i].Cells[j].Style.BackColor = Color.Aqua;
                        continue;
                    }
                    if (i/(topCount*2) < 1.0)
                    {
                        salesDataGridView.Rows[i].Cells[j].Style.BackColor = Color.DarkSalmon;
                        continue;
                    }
                    if (i/(topCount*3) < 1.0)
                    {
                        salesDataGridView.Rows[i].Cells[j].Style.BackColor = Color.DarkKhaki;
                        continue;
                    }

                    if (i/(topCount*4) < 1.0)
                    {
                        salesDataGridView.Rows[i].Cells[j].Style.BackColor = Color.ForestGreen;
                        continue;
                    }
                    if (i/(topCount*5) < 1.0)
                    {
                        salesDataGridView.Rows[i].Cells[j].Style.BackColor = Color.Gold;
                    }
                }
            }
        }

        private DataTable readFromExcel(string FileName)
        {
            FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader dataReader;
            if (FileName.EndsWith(".xls"))
            {
                dataReader = ExcelReaderFactory.CreateBinaryReader(fs);
            }
            else if (FileName.EndsWith(".xlsx"))
            {
                dataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            }
            else
            {
                throw new Exception("The file to be processed is not an Excel file");
            }
            dataReader.IsFirstRowAsColumnNames = false;
            DataTable table = dataReader.AsDataSet().Tables[0];
            dataReader.Close();
            return table;
        }

        private DataTable excelToRemainsTable(OpenFileDialog ofd)
        {
            DataTable dataTable = readFromExcel(ofd.FileName);
            //dataTable.Columns.RemoveAt(0);
            //dataTable.AcceptChanges();
            const int articleColNum = 1;
            const int remainsStartCol = 6;
            const int remainsEndCol = 9;
            dataTable.Columns[articleColNum].ColumnName = "article";
            dataTable.Columns[remainsStartCol].ColumnName = "initial";
            dataTable.Columns[remainsStartCol+1].ColumnName = "receipts";
            dataTable.Columns[remainsStartCol+2].ColumnName = "rate";
            dataTable.Columns[remainsEndCol].ColumnName = "final";
            dataTable.AcceptChanges();
            string[] existingcol = { "article", "initial","receipts","rate","final" };
            List<DataColumn> columnsToDelete = new List<DataColumn>();
            foreach(DataColumn col in dataTable.Columns)
            {
                if(!existingcol.Contains(col.ColumnName))
                    columnsToDelete.Add(col);       
            }

            foreach (DataColumn col in columnsToDelete)
            {
                dataTable.Columns.Remove(col);
            }
            
            DataTable resTable = dataTable.Clone();
            for (int i = 0; i < dataTable.Rows.Count; i++)
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
            foreach (DataRow row in resTable.Rows)
            {
                foreach (DataColumn col in resTable.Columns)
                {
                    if (row.IsNull(col) && col.DataType == typeof (double))
                        row.SetField(col, 0.0);
                }
            }
            return resTable;
        }

        private DataTable excelToRevisionTable(OpenFileDialog ofd)
        {
            DataTable dataTable = readFromExcel(ofd.FileName);
            DataTable copy = dataTable.Copy();
            IEnumerable<DataColumn> columns = copy.Columns.Cast<DataColumn>();
            EnumerableRowCollection<DataRow> rows = copy.AsEnumerable();
            List<DataColumn> nullColumns = columns.Where(col => rows.All(r => r.IsNull(col) ||
                                                                              String.CompareOrdinal(
                                                                                  (r.ToString()).Trim(),
                                                                                  string.Empty) == 0
                )).ToList();
            foreach (DataColumn colToRemove in nullColumns)
            {
                copy.Columns.Remove(colToRemove);
            }
            var searchTerm = new Regex(@"^[0-9]+\S.*?\s\S.*?");
            var resData = new DataTable();
            resData.Columns.Add("article");
            resData.Columns.Add("price");
            for (int rowNum = 0; rowNum < copy.Rows.Count; rowNum++)
            {
                for (int colNum = 0; colNum < copy.Columns.Count; colNum++)
                {
                    object cell = copy.Rows[rowNum][colNum];
                    if (searchTerm.IsMatch(cell.ToString()))
                    {
                        DataRow ravi = resData.NewRow();
                        ravi["article"] = copy.Rows[rowNum][colNum];
                        if (copy.Rows[rowNum][colNum + 1] == DBNull.Value)
                        {
                            ravi["price"] = "";
                        }
                        else
                        {
                            ravi["price"] = copy.Rows[rowNum][colNum + 1];
                        }
                        resData.Rows.Add(ravi);
                        break;
                    }
                }
            }
            return resData;
        }

        private DataTable getSingleRemainsDataTable(DataTable dataTable)
        {
            return (
                dataTable.AsEnumerable()
                    .Where(r => r.Field<double>("final") == 1.0)
                ).CopyToDataTable();
        }

        private DataTable FindRemains()
        {
            yesterdayDataTable = getSingleRemainsDataTable(yesterdayDataTable);
            todayDataTable = getSingleRemainsDataTable(todayDataTable);
            DataTable TableC = yesterdayDataTable.AsEnumerable()
                .Where(ra => todayDataTable.AsEnumerable()
                    .All(rb => rb.Field<string>("article") != ra.Field<string>("article"))
                )
                .CopyToDataTable();
            return TableC;
        }

        private DataTable FindRevision()
        {
            var searchTerm = new Regex(@"^[0-9]+.*?");
            var res = (from rem in remainsDataTable.AsEnumerable()
                join rev in revisionDataTable.AsEnumerable()
                    on searchTerm.Match(rem.Field<string>("article")).Value.ToLower() equals
                    searchTerm.Match(rev.Field<string>("article")).Value.ToLower()
                where rem.Field<double>("final") > 0.0
                select new
                {
                    article = rem.Field<string>("article"),
                    //initial=rem.Field<double>("initial"),
                    //receipts=rem.Field<double>("receipts"),
                    rate = rem.Field<double>("rate"),
                    final = rem.Field<double>("final"),
                    price = rev.Field<string>("price"),
                }
                ).Distinct();
            var newDataTable = new DataTable();
            newDataTable.Columns.AddRange(new[]
            {
                new DataColumn("article", typeof (string)),
                new DataColumn("rate", typeof (double)),
                new DataColumn("final", typeof (double)),
                new DataColumn("price", typeof (string))
            }
                );

            res.ToList().ForEach(x =>
            {
                DataRow row = newDataTable.NewRow();
                row["article"] = x.article;
                row["rate"] = x.rate;
                row["final"] = x.final;
                row["price"] = x.price;
                newDataTable.Rows.Add(row);
            });
            return newDataTable;
        }

        private void SaveToExcel(DataTable dt, string fileName)
        {
            var wb = new XLWorkbook();
            wb.Worksheets.Add(dt, "report");
            wb.SaveAs(fileName);
        }

        private void Open_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TopSalesPath.Text = ofd.FileName;
                        DataTable table = excelToRemainsTable(ofd);
                        DataTable resDataTable = table.Clone();
                        foreach (string filter in filterArray)
                        {
                            resDataTable.Merge(getTop(table, filter));
                        }
                        salesDataGridView.DataSource = resDataTable;
                        colorizeDataView(salesDataGridView.ColumnCount, salesDataGridView.RowCount);
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
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        YesterdaySalesPath.Text = ofd.FileName;
                        yesterdayDataTable = excelToRemainsTable(ofd);
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
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TodaySalesPath.Text = ofd.FileName;
                        todayDataTable = excelToRemainsTable(ofd);
                        salesDataGridView.DataSource = FindRemains();
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
                var saveFileDialog = new SaveFileDialog();
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
                        newFileName = Path.ChangeExtension(newFileName, null) + ".xlsx";
                    }
                    var dt = (DataTable) (salesDataGridView.DataSource);
                    SaveToExcel(dt, newFileName);
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

        private void RemainsButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        RemainTextBox.Text = ofd.FileName;
                        remainsDataTable = excelToRemainsTable(ofd);
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
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        RevisionTextBox.Text = ofd.FileName;
                        revisionDataTable = excelToRevisionTable(ofd);

                        salesDataGridView.DataSource = FindRevision();
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
            salesDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
        }

        private void TransferRemainbutton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TransferRemaintextBox.Text = ofd.FileName;
                        remainsDataTable = excelToRemainsTable(ofd);
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

        private void Transferbutton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        TransfertextBox.Text = ofd.FileName;
                         this.transferDataTable = stringToTransferDataTable(ReadAllTextFromDocx(ofd));
                        salesDataGridView.DataSource = FindTransfer();
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

        private DataTable FindTransfer()
        {
            var searchTerm = new Regex(@"^[0-9]+.*?");
            var qery = from rem in this.remainsDataTable.AsEnumerable()
                join tr in this.transferDataTable.AsEnumerable()
                    on searchTerm.Match(rem.Field<string>("article")).Value.ToLower() equals
                    searchTerm.Match(tr.Field<string>("article")).Value.ToLower()
                where rem.Field<double>("final") > 0.0
                select new
                {
                    article = rem.Field<string>("article"),
                    final = rem.Field<double>("final"),
                    count = tr.Field<string>("count"),
                    distanation = tr.Field<string>("distanation"),
                };
            return this.ToDataTable(qery.ToList());
        }

        private DataTable stringToTransferDataTable(string text)
        {
            var start = "Дніпро.*?\r\n";
            var end = "Загалом.+";
            string sOutput = Regex.Replace(text, start, "");
            sOutput = Regex.Replace(sOutput, end, "").Trim();
            var articleList = Regex.Split(sOutput, "\r\n");
            List<Transfer> transfers = new List<Transfer>();
            string dest = "";
            foreach (var article in articleList)
            {
                if (article.Contains("На "))
                {
                    dest = article.Replace("На", "").Trim();
                    continue;
                }
                var idCount = article.Split('-');
                transfers.Add(
                    new Transfer(
                        dest, idCount[0].Trim(), idCount[1].Trim()
                        )
                    );
            }

            return ToDataTable(transfers);
        }

        public  DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof (T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public  string ReadAllTextFromDocx(OpenFileDialog ofd)
        {
            StringBuilder stringBuilder;
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(ofd.FileName, false))
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                xmlNamespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                string wordprocessingDocumentText;
                using (StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream())
                    )
                {
                    wordprocessingDocumentText = streamReader.ReadToEnd();
                }

                stringBuilder = new StringBuilder(wordprocessingDocumentText.Length);

                XmlDocument xmlDocument = new XmlDocument(nameTable);
                xmlDocument.LoadXml(wordprocessingDocumentText);

                XmlNodeList paragraphNodes = xmlDocument.SelectNodes("//w:p", xmlNamespaceManager);
                foreach (XmlNode paragraphNode in paragraphNodes)
                {
                    XmlNodeList textNodes = paragraphNode.SelectNodes(".//w:t | .//w:tab | .//w:br", xmlNamespaceManager);
                    foreach (XmlNode textNode in textNodes)
                    {
                        switch (textNode.Name)
                        {
                            case "w:t":
                                stringBuilder.Append(textNode.InnerText);
                                break;

                            case "w:tab":
                                stringBuilder.Append("\t");
                                break;

                            case "w:br":
                                stringBuilder.Append("\v");
                                break;
                        }
                    }

                    stringBuilder.Append(Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
