using App1.util;
using System;
using System.Collections.Generic;
using System.Data;

namespace App1.model
{
    // сами продажи
    public class Sales
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime datetime
        {
            get;
            set;
        }

        public int salesmanId
        {
            get;
            set;
        }

    }

    public class SalesManager
    {
        public static List<Sales> context;

        public static Sales lastAddedSale;

        public static DataSet ds;
        public static DataTable dt;
        public static string managerCommand = "SELECT * FROM Sales";

        public static void UpdateDBCollection()
        {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            context = new List<Sales>();
            foreach (DataRow item in dt.Rows)
            {
                var stuff = new Sales();
                stuff.Id = (int)item["id"];
                stuff.datetime = (DateTime)item["time"];
                if (!(item["salesmanId"] is System.DBNull))
                    stuff.salesmanId = (int)item["salesmanId"];
                context.Add(stuff);
            }
        }

        public static void Add(int salesmanId)
        {
            DataRow item = dt.NewRow();


            item["salesmanId"] = salesmanId;

            item["time"] = System.DateTime.Now;
            
            
            DB_Worker.AppendRowToDataSetInDB(managerCommand, ds, item);

            UpdateDBCollection();

            lastAddedSale = context[context.Count -1];
        }



    }

    // содержание продаж
    public class SalesContent
    {
        public int Id
        {
            get;
            set;
        }


        public int saleId
        {
            get;
            set;
        }

        public int goodId
        {
            get;
            set;
        }

        public decimal price
        {
            get;
            set;
        }

        public byte amount
        {
            get;
            set;
        }
    }

    public class SalesContentManager
    {
        public static List<SalesContent> context;

        public static DataSet ds;
        public static DataTable dt;
        public static string managerCommand = "SELECT * FROM SalesContent";

        public static void UpdateDBCollection()
        {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            context = new List<SalesContent>();
            foreach (DataRow item in dt.Rows)
            {
                var stuff = new SalesContent();
                stuff.Id = (int)item["id"];
                stuff.saleId = (int)item["saleId"];
                if (!(item["goodId"] is System.DBNull))
                    stuff.goodId = (int)item["goodId"];
                stuff.price = (decimal)item["price"];
                stuff.amount = (byte)item["amount"];
                context.Add(stuff);
            }
        }

        public static void Add(SalesContent stuff)
        {
            DataRow item = dt.NewRow();


            item["saleId"] = stuff.saleId;
            item["goodId"] = stuff.goodId;
            item["price"] = stuff.price;
            item["amount"] = stuff.amount;


            DB_Worker.AppendRowToDataSetInDB(managerCommand, ds, item);

            UpdateDBCollection();
        }



        public static void Remove(int id)
        {
            DataRow deletedRow = dt.Rows[0];
            foreach (DataRow row in dt.Rows)
            {

                if ((int)row["id"] == id)
                {
                    deletedRow = row;
                    break;
                }

            }

            DB_Worker.RemoveRowToDataSetInDB(managerCommand, ds, deletedRow);

            UpdateDBCollection();
        }

        public static void Edit(SalesContent oldRowVersion, SalesContent newRowVersion)
        {
            DataRow editedRow = dt.Rows[0];
            foreach (DataRow row in dt.Rows)
            {

                if ((int)row["id"] == oldRowVersion.Id)
                {
                    editedRow = row;
                    break;
                }

            }


            editedRow["saleId"] = newRowVersion.saleId;
            editedRow["goodId"] = newRowVersion.goodId;
            editedRow["price"] = newRowVersion.price;
            editedRow["amount"] = newRowVersion.amount;

            DB_Worker.UpdateDataSetInDB(managerCommand, ds);

            UpdateDBCollection();
        }


        public static List<SalesContent> GetListOfSaleStuff(int saleId)
        {
            UpdateDBCollection();
            List<SalesContent> saleStuff = new List<SalesContent>();

            foreach (var item in context)
            {
                if (item.saleId == saleId)
                    saleStuff.Add(item);
            }

            return saleStuff;
        }
    }

    // представления продаж
    public class SalesView
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime datetime
        {
            get;
            set;
        }

        public int salesmanId
        {
            get;
            set;
        }

        public string manName
        {
            get;
            set;
        }

        public decimal summa
        {
            get;
            set;
        }

        public List<SalesContent> saleStuff;

        public SalesView() 
        {
            
        }

        public decimal SetSum() 
        {
            saleStuff = SalesContentManager.GetListOfSaleStuff(Id);
            decimal summ = 0;
            foreach (var item in saleStuff)
            {
                summ += item.price * item.amount;
            }

           return summ;
        }
    }

    public class SalesViewManager
    {
        public static List<SalesView> sales;

        public static DataSet ds;
        public static DataTable dt;
        public static string managerCommand = "SELECT * FROM salesView";

        public static void UpdateDBCollection()
        {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            sales = new List<SalesView>();
            foreach (DataRow item in dt.Rows)
            {
                var sale = new SalesView();
                sale.Id = (int)item["id"];
                sale.datetime = (DateTime)item["datetime"];
                if (!(item["salesmanId"] is System.DBNull))
                    sale.salesmanId  = (int)item["salesmanId"];
                sale.manName = item["manName"] is System.DBNull ? "Продавец удален из системы" : (string)item["manName"];
                sale.summa =  sale.SetSum();
                sales.Add(sale);
            }
        }


        public static decimal GetGeneralIncome(List<SalesView> views)
        {
            decimal income = 0;

            foreach (var item in views)
            {
                income += item.summa;

            }

            return income;
        }

        public static List<SalesView> GetSalesPerDate(DateTime pic1, DateTime pic2) 
        {
            List<SalesView> stuff = new List<SalesView>();

            foreach (var item in sales) 
            {
                if (item.datetime >= pic1 && item.datetime <= pic2)
                {

                    stuff.Add(item);
                }
            
            }
            return stuff;
        }

    }

    // содержание продаж предствление
    public class SalesContentViewRow 
    {
        public string name
        {
            get;
            set;
        }

        public decimal sum
        {
            get;
            set;
        }


        public decimal price
        {
            get;
            set;
        }

        public byte amount
        {
            get;
            set;
        }
        public SalesContentViewRow() { }
        public SalesContentViewRow(string name, decimal price, byte amount) 
        { 
            this.name = name;
            this.price = price;
            this.amount = amount;
        }
    }

    public class SalesContentViewRowManager
    {
        public static List<SalesContentViewRow> context;

        public static DataSet ds;
        public static DataTable dt;
       

        public static void UpdateDBCollection(int saleId)
        {
            ds = DB_Worker.ExecuteTableCommand($"execute getSaleCont {saleId}");
            dt = ds.Tables[0];
            context = new List<SalesContentViewRow>();
            foreach (DataRow item in dt.Rows)
            {
                var stuff = new SalesContentViewRow();
               

                stuff.sum = (decimal)item["sum"];
                if (!(item["name"] is System.DBNull))
                {
                    stuff.name = (string)item["name"];
                }
                else
                {
                    stuff.name = "Товар удален из системы";
                }
                stuff.price = (decimal)item["price"];
                stuff.amount = (byte)item["amount"];
                context.Add(stuff);
            }
        }

        
    }
}
