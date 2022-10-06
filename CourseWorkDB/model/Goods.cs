using CourseWorkDB.util;
using System;
using System.Collections.Generic;
using System.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CourseWorkDB
{

    public class Good
    {
        public int Id
        {
            get;
            set;
        }
        public int GroupId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }

        public BitmapImage ImageBit
        {
            get;
            set;
        }
        public string Discribtion
        {
            get;
            set;
        }

        
    }
    public class GoodManager
    {
        public static List<Good> goods;

        public static string DEFAULT_IMAGE_PATH = "";
        public static DataSet ds;
        public static DataTable dt;
        private static string managerCommand = "SELECT * FROM Goods";

        public static void SetManagerCommandPerGroup(int groupId) {
            if (groupId > 0)
                managerCommand += $"where groupId = {groupId}";
        }

        public static void UpdateDBCollection() {
            ds = DB_Worker.ExecuteTableCommand(managerCommand);
            dt = ds.Tables[0];
            goods = new List<Good>();
            foreach (DataRow item in dt.Rows) {
                var newGood = new Good();
                newGood.Id = (int)item["id"];
                newGood.GroupId = (int)item["groupId"];
                newGood.Name = (string)item["name"];
                newGood.Discribtion = (item["discribtion"] is DBNull) ? "": (string)item["discribtion"];
                newGood.Image = (item["imageURL"] is DBNull) ? DEFAULT_IMAGE_PATH : (string)item["imageURL"];

                newGood.ImageBit = WindowShower.SetImage(newGood.Image);
                goods.Add(newGood);
            }
        }

        public static void Add(int groupId, string name, string discribtion="", string imageURL="")
        {
            //goods.Add(good);
            DataRow row = dt.NewRow();
            row["groupId"] = groupId;
            row["name"] = name;
            row["discribtion"] = discribtion;
            row["imageURL"] = imageURL;

           
            DB_Worker.AppendRowToDataSetInDB(managerCommand, ds, row);

            UpdateDBCollection();
        }

        public static void Remove(int id) {
            DataRow deletedRow = dt.Rows[0];
            foreach (DataRow row in dt.Rows)
            {

                if ((int) row["id"] == id)
                {
                    deletedRow = row;
                    break;
                }

            }

            DB_Worker.RemoveRowToDataSetInDB(managerCommand, ds, deletedRow);

            UpdateDBCollection();
        }

        public static void Edit(Good oldGoodVersion, Good newGoodVersion)
        {
            DataRow editedRow = dt.Rows[0];
            foreach (DataRow row in dt.Rows)
            {

                if ((int)row["groupId"] == oldGoodVersion.GroupId
                    && (string)row["name"] == oldGoodVersion.Name)
                {
                    editedRow = row;
                    break;
                }

            }

            editedRow["groupId"] = newGoodVersion.GroupId;
            editedRow["name"] = newGoodVersion.Name;
            editedRow["discribtion"] = newGoodVersion.Discribtion;
            editedRow["imageURL"] = newGoodVersion.Image;

            DB_Worker.UpdateDataSetInDB(managerCommand, ds);

            UpdateDBCollection();
        }

        public static List<string> GetListOfNames()
        {

            List<string> names = new List<string>();

            foreach (var item in goods)
            {
                names.Add(item.Name);
            }

            return names;
        }

        public static int GetGoodId(string name)
        {

            Good group = goods[0];

            foreach (var item in goods)
            {
                if (item.Name == name)
                {
                    group = item;
                    break;
                }

            }

            return group.Id;
        }

        public static string GetGoodName(int id)
        {

            Good group = goods[0];

            foreach (var item in goods)
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
