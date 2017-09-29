using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Social.BestShirtUSA
{
    class ConnectMySQL
    {
        public class DBConnection
        {
            private DBConnection()
            {
            }
            public string Password { get; set; }
            private MySqlConnection connection = null;
            public MySqlConnection Connection
            {
                get { return connection; }
            }

            private static DBConnection _instance = null;
            public static DBConnection Instance()
            {
                if (_instance == null)
                    _instance = new DBConnection();
                return _instance;
            }

            public bool IsConnect()
            {
                bool result = true;
                if (Connection == null || connection.State == ConnectionState.Closed)
                {
                    //string connstring = "Server=127.0.0.1; database=besttshi_ss_dbname27c; UID=root; password= ";
                    string connstring = "Server=50.116.101.32;Port=3306;Database=besttshi_ss_dbname27c;UID=besttshi_ss_d27c;Pwd=It@dmin092017";
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                    result = true;
                }
                return result;
            }

            public void Close()
            {
                connection.Close();
            }
        }
    }
}
