using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace EXPORTSQLTOEXCEL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cnn;
            string connectionString = null;
            string sql = null;
            string data = null;
            int i = 0;
            int j = 0;
            int k = 0;
      
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            connectionString = "Data Source=192.168.1.3,1433;Network Library=DBMSSOCN;Initial Catalog = SQLEXPORT; User ID = sa; Password = pflukaaa; ";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = "SELECT * FROM Product";
            SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            // This for loop adds the column names to the first row of the Excel document.
            for (k = 0; k < ds.Tables[0].Columns.Count; k++)
            {
                xlWorkSheet.Cells[1, k + 1].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //xlWorkSheet.Cells[1, k + 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWorkSheet.Cells[1, k + 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignJustify;
                ((Excel.Range)xlWorkSheet.Cells[1, k + 1]).Interior.Color = ColorTranslator.ToOle(Color.LightGreen);
                xlWorkSheet.Cells[1, k + 1] = ds.Tables[0].Columns[k].ColumnName;
            }
              


            // This for loop populates the excel document with values.
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                    // +2 because we already inserted the header row.
                    xlWorkSheet.Cells[i + 2, j + 1].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //xlWorkSheet.Cells[i + 2, j + 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Cells[i + 2, j + 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignJustify;
                    xlWorkSheet.Cells[i + 2, j + 1] = data;
                    ((Excel.Range)xlWorkSheet.Cells[i + 2, j + 1]).Interior.Color = ColorTranslator.ToOle(Color.LightBlue);

                }
            }

            





            xlWorkBook.SaveAs(@"C:\Users\Admin\Desktop\TEST\ExportSQLDateien.xls",Excel.XlFileFormat.xlWorkbookNormal , misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
           // Excel.XlFileFormat.xlWorkbookNormal
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show("Excel file created");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
