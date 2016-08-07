using System;
using System.Data.Common;
using System.Data.SQLite;

namespace Start_mpr.Sqldb
{
    public class MyDBConnection{

        public static DbConnection connect(string connetction_str){
            SQLiteFactory dbF = new SQLiteFactory();
            DbConnection cnn = dbF.CreateConnection();
            cnn.ConnectionString = connetction_str;
            cnn.Open();

            return cnn;
        }

        public static bool isConnected(DbConnection conn){
            return conn.State == System.Data.ConnectionState.Open;
        }
    }
}
