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
    public partial class Delivery : Form
    {
        public struct ProductNew
        {
            public int idProduct;
            public string Name;
            public float weight;
        }

        public struct productBD
        {
            public int idProduct;
            public string Name;
        }

        public struct newIt
        {
            public ComboBox boxC;
            public TextBox boxT;
        }
        List<newIt> ListNewItem = new List<newIt>();
        WorkDB workDB = new WorkDB();
        List<productBD> listPR;
        public Delivery()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newIt newPr = new newIt();
            if (this.comboBox1.Text == "Выберите поставщика")
            {
                MessageBox.Show("Не выбран поставщик");
            }
            else
            {
                this.comboBox1.Enabled = false;

                //Создаем новую кнопку
                ComboBox tempCombo = new ComboBox();
                TextBox tempText = new TextBox();

                listPR = workDB.ReadProducts(this.comboBox1.SelectedIndex);

                if (listPR.Count > 0)
                {
                    for (int i = 0; i < listPR.Count; i++)
                    {
                        tempCombo.Items.Add(new SelectData(listPR[i].idProduct, listPR[i].Name));

                    }
                }

                tempCombo.Width = this.comboBox1.Width;
                tempCombo.Location = new Point(this.button1.Location.X, this.button1.Location.Y);
                

                tempText.Width = this.comboBox1.Width;
                tempText.Location = new Point(this.button1.Location.X + this.comboBox1.Width + 20, this.button1.Location.Y);

                this.button1.Location = new Point(this.button1.Location.X, this.button1.Location.Y + 30);

                //Добавляем элементы на форму
                this.Controls.Add(tempCombo);
                this.Controls.Add(tempText);

                newPr.boxC = tempCombo;
                newPr.boxT = tempText;
                ListNewItem.Add(newPr);
                this.Height += 30;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<ProductNew> newP = new List<ProductNew>();
            ProductNew prod = new ProductNew();
            if (ListNewItem.Count > 0)
            {
                int ik;
                for (int i = 0; i < ListNewItem.Count; i++)
                {
                    prod.Name = ListNewItem[i].boxC.Text;
                    prod.weight = (float)Convert.ToDouble(ListNewItem[i].boxT.Text);
                    ik = listPR.FindIndex(x => x.Name.Contains(prod.Name));
                    if (ik > 0)
                        prod.idProduct = listPR[ik].idProduct;
                    newP.Add(prod);
                }
            }
            workDB.SaveStock(newP);
            this.Close();
        }
    
    }
}
