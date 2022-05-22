using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InventoryControl
{
    internal class WorkDB
    {
        String connStr = "Server=localhost;port=3308;Database=dbproduct;Uid=Anastasiya;password=Anastasiya";

        public List<SelectData> ReadProvider()
        {
            List<SelectData> provider = new List<SelectData>();

            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "Select * from Provider";

            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;
            command.CommandText = sql;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    provider.Add(new SelectData(Int32.Parse(reader.GetString(0)), reader.GetString(1)));
                    
                }
            }
           
            conn.Close();
            return provider;
        }

        public List<Delivery.productBD> ReadProducts(int id)
        { 
            List<Delivery.productBD> products = new List<Delivery.productBD>();
            Delivery.productBD product = new Delivery.productBD();
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "Select idProduct, Name from Product where idProvider=" + id;


            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;
            command.CommandText = sql;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    product.idProduct = Int32.Parse(reader.GetString(0));
                    product.Name = reader.GetString(1);
                    products.Add(product);

                }
            }

            conn.Close();
            return products;
        }

        public void SaveStock(List<Delivery.ProductNew> products)
        {
            string date = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
            MySqlConnection conn = new MySqlConnection(connStr);

            MySqlCommand command;

            if (products.Count > 0)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].idProduct > 0 && products[i].weight > 0)
                    {
                        string upSql = "insert stock (idProduct, Stockcol, dateStock) values " +
                            "(" + products[i].idProduct + ", " + products[i].weight + ", cast('" + date + "' as datetime))";
                        conn.Open();

                        command = conn.CreateCommand();

                        command.CommandText = upSql;
                        int resn = command.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            }
            

        }

        public List<Report.FileRes> ReadOneDayRes(DateTime date)
        { 
            List<Report.FileRes> res = new List<Report.FileRes>();
            Report.FileRes fileRes = new Report.FileRes();
            string dsrt;
            dsrt = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "SELECT dbproduct.provider.name, dbproduct.product.name, dbproduct.product.price, dbproduct.stock.stockcol, dbproduct.stock.dateStock " +
                "FROM dbproduct.stock inner join dbproduct.product on dbproduct.product.idProduct = dbproduct.stock.idProduct " +
                "inner join dbproduct.provider on dbproduct.product.idProvider = dbproduct.provider.idProvider " +
                "where dateStock='" + dsrt + "'";


            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;
            command.CommandText = sql;
            int ik;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    fileRes.NameProvider = reader.GetString(0);
                    fileRes.NameProduct = reader.GetString(1);
                    fileRes.price = (float)Convert.ToDouble(reader.GetString(2));
                    fileRes.weight = (float)Convert.ToDouble(reader.GetString(3));
                    string ss = reader.GetString(4);
                    fileRes.date = DateTime.Parse(ss);

                    ik = res.FindIndex(x => x.NameProduct.Contains(fileRes.NameProduct));
                    bool ok = false;
                    for (int j = 0; j < res.Count; j++)
                    {
                        if (res[j].NameProduct == fileRes.NameProduct) //такой товар в списке уже есть
                        {
                            if (fileRes.date.Year == res[j].date.Year && fileRes.date.Month == res[j].date.Month && fileRes.date.Day == res[j].date.Day)
                            {//такая дата уже есть
                                ok = true;
                                fileRes.weight += res[j].weight;
                                res[j] = fileRes;
                            }
                        }
                    }
                    if (!ok) 
                    {//такого товара в списке еще нет 
                        res.Add(fileRes);
                    }
                }
            }

            conn.Close();

            res = res.OrderBy(x => x.date).ToList();

            return res;
        }

        public List<Report.FileRes> ReadManyRes(DateTime date1, DateTime date2)
        {
            List<Report.FileRes> res = new List<Report.FileRes>();
            Report.FileRes fileRes = new Report.FileRes();
            string dsrt1, dsrt2;
            dsrt1 = date1.Year.ToString() + "-" + date1.Month.ToString() + "-" + date1.Day.ToString();
            dsrt2 = date2.Year.ToString() + "-" + date2.Month.ToString() + "-" + date2.Day.ToString();
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "SELECT dbproduct.provider.name, dbproduct.product.name, dbproduct.product.price, dbproduct.stock.stockcol, dbproduct.stock.dateStock " +
                "FROM dbproduct.stock inner join dbproduct.product on dbproduct.product.idProduct = dbproduct.stock.idProduct " +
                "inner join dbproduct.provider on dbproduct.product.idProvider = dbproduct.provider.idProvider " +
                "where dateStock between '" + dsrt1 + "' and '" + dsrt2 + "'";


            MySqlCommand command = new MySqlCommand();

            command.Connection = conn;
            command.CommandText = sql;
            int ik;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    fileRes.NameProvider = reader.GetString(0);
                    fileRes.NameProduct = reader.GetString(1);
                    fileRes.price = (float)Convert.ToDouble(reader.GetString(2));
                    fileRes.weight = (float)Convert.ToDouble(reader.GetString(3));
                    string ss = reader.GetString(4);
                    fileRes.date = DateTime.Parse(ss);

                    ik = res.FindIndex(x => x.NameProduct.Contains(fileRes.NameProduct));
                    bool ok = false;
                    for (int j = 0; j < res.Count; j++)
                    {
                        if (res[j].NameProduct == fileRes.NameProduct) //такой товар в списке уже есть
                        {
                            if (fileRes.date.Year == res[j].date.Year && fileRes.date.Month == res[j].date.Month && fileRes.date.Day == res[j].date.Day)
                            {//такая дата уже есть
                                ok = true;
                                fileRes.weight += res[j].weight;
                                res[j] = fileRes;
                            }
                        }
                    }
                    if (!ok)
                    {//такого товара в списке еще нет 
                        res.Add(fileRes);
                    }
                }
            }

            conn.Close();

            res = res.OrderBy(x => x.date).ToList();

            return res;
        }

    }
}
