using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Windows.Forms;

namespace Start_mpr.Sqldb
{
    public sealed class SqlManager {
        private static readonly Object s_lock = new Object();
        private static SqlManager instance = null;

        private SqlManager() { }

        public static SqlManager Instance {
            get {	
                if (instance != null) return instance;
                Monitor.Enter(s_lock);
                SqlManager temp = new SqlManager();
                Interlocked.Exchange(ref instance, temp);
                Monitor.Exit(s_lock);
                return instance; 
            }
        }

        #region Fields
        private DbConnection connection;
        private string str_connection { get; set; }
        private bool isConnected {
            get {
                return MyDBConnection.isConnected(this.connection);
            }
        }
        private string command_str;
        #endregion

        #region Methods
        private void connect() {
            this.connection = MyDBConnection.connect(this.str_connection);
        }

        private void disconnect() {
            this.connection.Close();
            this.connection.Dispose();
        }

        public void setConnectionString(string str){
            this.str_connection = str;
        }

        public string getConnectionString(){
            return this.str_connection;
        }

        public ArrayList GET() {
            ArrayList list = new ArrayList();
			
            this.connect();
			
            if (isConnected)
            {	
                this.command_str = "select * from TestResult where Name='" + Auth.FirstName + " " + Auth.LastName + "'";
                DbCommand command = this.connection.CreateCommand();
                command.CommandText = this.command_str;
                DbDataReader reader = command.ExecuteReader();
				
                while(reader.Read()){		
                    MyResult res = new MyResult();
                    res.id = reader.GetInt32(0);
                    res.fio = reader.GetString(1);
						
                    byte[] bytBLOB = new byte[reader.GetBytes(2, 0, null, 0, int.MaxValue) - 1];  // Arihmetic overflow exception	
	
                    reader.GetBytes(2, 0, bytBLOB, 0, bytBLOB.Length);
					
                    res.result = bytBLOB;
                    res.date = reader.GetDateTime(3);
                    list.Add(res);
                }
				
                reader.Close();
            }
			
            this.disconnect();
            return list;
        }

        public void POST(byte[] mprOUT) {
            this.connect();
			
            if (isConnected){
                this.command_str = "INSERT INTO TestResult(Name,Result,Date) VALUES(@name,@result,@date)";
                DbCommand command = connection.CreateCommand();
                command.CommandText = command_str;

                this.addParams(command, new string[] { "name", "result", "date" }, new object[] { Auth.FirstName + " " + Auth.LastName, mprOUT, DateTime.Now });
                command.ExecuteNonQuery();
            }
            else { System.Windows.Forms.MessageBox.Show("Ошибка подключения к базе данных"); }

            this.disconnect();
        }

        public void DELETE_ALL()
        {
            this.connect();
            if (isConnected){
                this.command_str = "DELETE FROM TestResult WHERE Name='" + Auth.FirstName + " " + Auth.LastName + "'";
                DbCommand command = this.connection.CreateCommand();
                command.CommandText = this.command_str;

                command.ExecuteNonQuery();
            }
            else { System.Windows.Forms.MessageBox.Show("Ошибка подключения к базе данных"); }

            this.disconnect();
        }

        public void DELETE_BY_ID(int id) {
            this.connect();
            if (isConnected){
                this.command_str = "delete from TestResult where Name='" + Auth.FirstName + " " + Auth.LastName + "' and id='" + id.ToString() + "';";
                DbCommand command = this.connection.CreateCommand();
                command.CommandText = this.command_str;
                command.ExecuteNonQuery();
            }
            else { System.Windows.Forms.MessageBox.Show("Ошибка подключения к базе данных"); }
            this.disconnect();
        }

        private void addParams(DbCommand command, string[] params_array, object[] values){
            DbParameterCollection collection = command.Parameters;
            for(int i = 0; i < params_array.Length; i++) {
                DbParameter parameter = command.CreateParameter();
                switch (params_array[i]){
                    case "result":
                        parameter.DbType = DbType.Binary;
                        break;
                    case "name":
                        parameter.DbType = DbType.String;
                        break;
                    case "date":
                        parameter.DbType = DbType.DateTime;
                        break;
                    default:
                        parameter.DbType = DbType.String;
                        break;
                }
                parameter.ParameterName = "@" + params_array[i];
                parameter.Value = values[i];

                collection.Add(parameter);
            }
        }
        #endregion

    }
}
