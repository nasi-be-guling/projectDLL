//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System;
using MySql.Data.MySqlClient;
using _encryption;

namespace _connectMySQL
{
    public class CConnection
    {
        private MySqlConnection _connection;

        public MySqlConnection Connect(string configurationManager, ref string errorMessage, string cipherPasswd)
        {
            // server=;port=3306;user id=root; password=123; database=mysql; pooling=false; sqlservermode=True
            string conString = configurationManager;

            try
            {
                _connection = new MySqlConnection(CStringCipher.Decrypt(conString, cipherPasswd));
                _connection.Open();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error connecting to the server: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Error in common issues : " + ex.Message;
            }

            return _connection;
        }

        public void Insertion(string sqlQueryMasuk, MySqlConnection connection)
        {
            MySqlCommand insertCommand = new MySqlCommand(sqlQueryMasuk, connection);
            insertCommand.ExecuteNonQuery();
            //insertCommand.Dispose();
        }

        public void Insertion(string sqlQueryMasuk, MySqlConnection connection, MySqlTransaction trans)
        {
            // this overriding moethod is including a transaction
            MySqlCommand insertCommand = new MySqlCommand(sqlQueryMasuk, connection, trans);
            insertCommand.ExecuteNonQuery();
        }

        public MySqlDataReader Reading(string queryBaca, MySqlConnection connection)
        {
            MySqlCommand selectCommand = new MySqlCommand(queryBaca, connection);
            MySqlDataReader pembaca = selectCommand.ExecuteReader();
            return pembaca;
        }

        public int GetMaxId(MySqlConnection connection, string tableName, string tableId)
        {
            int maxid = 0;
            MySqlCommand selectCommand = new MySqlCommand("select COALESCE(MAX(" + tableId + "),0) from " + tableName + "", connection);
            MySqlDataReader pembaca = selectCommand.ExecuteReader();

            if (!pembaca.HasRows) return maxid;
            pembaca.Read();
            maxid = pembaca.GetInt16(0);
            pembaca.Close();

            return maxid;
           
        }
    }
}
