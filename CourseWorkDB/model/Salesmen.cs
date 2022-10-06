using CourseWorkDB.util;
using System;
using System.Collections.Generic;
using System.Data;

namespace CourseWorkDB.model
{
    public class Salesmen
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

    }
    public class SalesmenManager
    {
        public static List<Salesmen> salesmen;

        public static DataSet ds;
        public static DataTable dt;
        public static string managerCommand = "SELECT * FROM Salesmen";

        public static void UpdateDBCollection()
        {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            salesmen = new List<Salesmen>();
            foreach (DataRow item in dt.Rows)
            {
                var salesman = new Salesmen();
                salesman.Id = (int)item["id"];
                salesman.Name = (string)item["name"];
                salesmen.Add(salesman);
            }
        }

        public static void Add(string name)
        {
            
            DataRow row = dt.NewRow();

            row["name"] = name;


            DB_Worker.AppendRowToDataSetInDB(managerCommand, ds, row);

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

        public static void Edit(Salesmen oldRowVersion, Salesmen newRowVersion)
        {
            DataRow editedRow = dt.Rows[0];
            foreach (DataRow row in dt.Rows)
            {

                if ((int)row["id"] == oldRowVersion.Id
                    && (string)row["name"] == oldRowVersion.Name)
                {
                    editedRow = row;
                    break;
                }

            }

            editedRow["name"] = newRowVersion.Name;
            
            DB_Worker.UpdateDataSetInDB(managerCommand, ds);

            UpdateDBCollection();
        }

        public static List<string> GetListOfNames()
        {

            List<string> names = new List<string>();

            foreach (var item in salesmen)
            {
                names.Add(item.Name);
            }

            return names;
        }

        public static int GetSalesmenId(string name)
        {

            Salesmen group = salesmen[0];

            foreach (var item in salesmen)
            {
                if (item.Name == name)
                {
                    group = item;
                    break;
                }

            }

            return group.Id;
        }

        public static string GetSalesmenName(int id)
        {

            Salesmen group = salesmen[0];

            foreach (var item in salesmen)
            {
                if (item.Id == id)
                {
                    group = item;
                    break;
                }

            }

            return group.Name;
        }


    }
}
