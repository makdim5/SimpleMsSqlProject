using System.Data;
using System.Data.SqlClient;

namespace App1.util
{
    public class DB_Worker
    {
        private static string connectionString = "Data Source=MSIPYMAK\\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True";

        public static void ExecuteVoidCommand(string command)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {

                SqlCommand com1 = new SqlCommand();
                com1.CommandType = CommandType.Text;
                com1.CommandText = command;
                com1.Connection = Con;
                com1.ExecuteNonQuery();
            }

        }

        public static DataSet ExecuteTableCommand(string command)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command, Con);
                // Создание объекта Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);

                return ds;
            }

        }

        public static void AppendRowToDataSetInDB(string command, DataSet ds, DataRow dr)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command, Con);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                ds.Tables[0].Rows.Add(dr);

                adapter.Update(ds);
            }

        }

        public static void RemoveRowToDataSetInDB(string command, DataSet ds, DataRow dr)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command, Con);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                dr.Delete();

                adapter.Update(ds);
            }

        }

        public static void UpdateDataSetInDB(string command, DataSet ds)
        {

            using (SqlConnection Con = new SqlConnection(connectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command, Con);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);


                adapter.Update(ds);
            }

        }

    }
}
