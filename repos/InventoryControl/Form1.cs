using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InventoryControl
{
    class SelectData
    {
        public readonly int Value;
        public readonly string Text;
        
        public SelectData(int Value, string Text)
        {
            this.Value = Value;
            this.Text = Text;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        WorkDB workDB = new WorkDB();
        private void button1_Click(object sender, EventArgs e)
        {
            Delivery newDelivery = new Delivery();

            List<SelectData> selectDatas = workDB.ReadProvider();
            newDelivery.comboBox1.Items.Add(new SelectData(0, "Выберите поставщика"));
            if (selectDatas.Count > 0)
            {
                for (int i = 0; i < selectDatas.Count; i++)
                {
                    newDelivery.comboBox1.Items.Add(new SelectData(selectDatas[i].Value, selectDatas[i].Text));
                }
            }
            newDelivery.comboBox1.SelectedIndex = 0;

            newDelivery.Show();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Report newReport = new Report();
            newReport.Show();
        }
    }
}
