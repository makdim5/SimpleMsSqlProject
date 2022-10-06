using App1.util;
using System;
using System.Collections.Generic;
using System.Data;
namespace App1
{
    public class GoodGroups
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
    public class GoodGroupsManager
    {
        public static List<GoodGroups> goodsGroups;

        public static DataSet ds;
        public static DataTable dt;
        public static string managerCommand = "SELECT * FROM GoodsGroups";

        public static void UpdateDBCollection()
        {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            goodsGroups = new List<GoodGroups>();
            foreach (DataRow item in dt.Rows)
            {
                var newGroup = new GoodGroups();
                newGroup.Id = (int)item["id"];
                newGroup.Name = (string)item["name"];
                goodsGroups.Add(newGroup);
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

        public static void Edit(GoodGroups oldRowVersion, GoodGroups newRowVersion)
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

            foreach (var item in goodsGroups) 
            {
                names.Add(item.Name);
            }

            return names;
        }

        public static int GetGroupId(string name)
        {

            GoodGroups group = goodsGroups[0];

            foreach (var item in goodsGroups)
            {
                if (item.Name == name)
                {
                    group = item;
                    break;
                }

            }

            return group.Id;
        }

        public static string GetGroupName(int id)
        {

            GoodGroups group = goodsGroups[0];

            foreach (var item in goodsGroups)
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
