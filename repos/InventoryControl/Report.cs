using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventoryControl
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        public struct FileRes
        {
            public string NameProvider;
            public string NameProduct;
            public float weight;
            public float price;
            public DateTime date;
        }
        WorkDB workDB = new WorkDB();
        Excel.Application app = null;
        Excel.Workbook workbook = null;
        Excel.Worksheet worksheet = null;


        public void button1_Click(object sender, EventArgs e)
        {
            DateTime day1, day2;
            day1 = dateTimePicker1.Value;
            day2 = dateTimePicker2.Value;

            List<FileRes> files = new List<FileRes>();

            if (day1.Year == day2.Year && day1.Month == day2.Month && day1.Day == day2.Day)
            {//отчет за один день 
                files = workDB.ReadOneDayRes(day1);
            }
            else if (day1.Year <= day2.Year && day1.Month <= day2.Month && day1.Day < day2.Day)
            {//отчет за несколько дней 
                files = workDB.ReadManyRes(day1, day2);
            }
            else
            {
                MessageBox.Show("Не правильно указаны даты");
            }
            if (files.Count > 0)
            {
                SaveExcelDate(files);
            }
            else
            {
                MessageBox.Show("Нет данных");
            }
            this.Close();
        }

        public void SaveExcelDate(List<FileRes> resultS)
        {
          
            app = new Excel.Application();
            app.Visible = true;
            workbook = app.Workbooks.Add(1);
            worksheet = (Excel.Worksheet)workbook.Sheets[1];

           

            //создадим заголовки у столбцов
            worksheet.get_Range("A" + 1).Value = "Поставщик";
             worksheet.get_Range("B" + 1).Value = "Наименование товара";
             worksheet.get_Range("C" + 1).Value = "Вес";
             worksheet.get_Range("D" + 1).Value = "Цена";
             worksheet.get_Range("E" + 1).Value = "Расход";
             worksheet.get_Range("F" + 1).Value = "Дата";
             string str;
             for (int i = 0; i < resultS.Count; i++)
             {
                 worksheet.get_Range("A" + (i+2)).Value = resultS[i].NameProvider;
                 worksheet.get_Range("B" + (i + 2)).Value = resultS[i].NameProduct;
                 worksheet.get_Range("C" + (i + 2)).Value = resultS[i].weight + " кг";
                 worksheet.get_Range("D" + (i + 2)).Value = resultS[i].price + " руб";
                 worksheet.get_Range("E" + (i + 2)).Value = resultS[i].weight * resultS[i].price + " руб";
                 str = resultS[i].date.Day.ToString() + "." + resultS[i].date.Month.ToString() + "." + resultS[i].date.Year.ToString();
                 worksheet.get_Range("F" + (i + 2)).Value = str;

             }
            
            //string strF = "C:\\Report" + DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString() + ".xlsx";
            //workbook.SaveAs(strF);//сохранить в эксель файл

        }
    }
}
