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

        /// <summary>
        /// Function ini digunakan untuk melakukan inisiasi koneksi/membuka koneksi ke database berbasis mysql
        /// </summary>
        /// <param name="configurationManager">Sediakan variable bertipe data string. 
        /// Nilai ini didapat dari string _configurationManager = Properties.Settings.Default.Setting;</param>
        /// <param name="errorMessage">Sediakan variable ref bertipe data string, untuk menampung pesan error</param>
        /// <param name="cipherPasswd">Masukkan password untuk mendecipher enkripsi dari configurationManager</param>
        /// <returns>Mengembalikan object MySqlConnection</returns>
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
                errorMessage = "Error connecting to the server. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non MySQL related error on connecting to server. Code: " + ex.Message;
            }

            return _connection;
        }

        /// <summary>
        /// Function untuk melakukan sqlcommand seperti insert, update, delete pada tabel database berbasis MySql
        /// </summary>
        /// <param name="sqlQueryMasuk">Sediakan variable bertipe data string, untuk sql query</param>
        /// <param name="connection">Sediakan object MySqlConnection yg sebelumnya telah diinisiasi</param>
        /// <param name="errorMessage">Sediakan variable ref bertipe data string, untuk menampung pesan error</param>
        public void Insertion(string sqlQueryMasuk, MySqlConnection connection, ref string errorMessage)
        {
            try
            {
                MySqlCommand insertCommand = new MySqlCommand(sqlQueryMasuk, connection);
                insertCommand.ExecuteNonQuery();
                //insertCommand.Dispose();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error executing Insert Command. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non MySQL Error at Insert Command. Code: " + ex.Message;                
            }
        }

        /// <summary>
        /// Function untuk melakukan sqlcommand seperti insert, update, delete pada tabel database berbasis MySql. Dilengkapi dengan MySqlTransaction
        /// </summary>
        /// <param name="sqlQueryMasuk">Sediakan variable bertipe data string, untuk sql query</param>
        /// <param name="connection">Sediakan object MySqlConnection yang sebelumnya telah diinisiasi</param>
        /// <param name="trans">Sediakan Object MySqlTranction yang sebelumnya telah diinisiasi</param>
        /// <param name="errorMessage">Sediakan variable ref bertipe data string, untuk menampung pesan error</param>
        public void Insertion(string sqlQueryMasuk, MySqlConnection connection, MySqlTransaction trans, ref string errorMessage)
        {
            // this overriding moethod is including a transaction
            try
            {
                MySqlCommand insertCommand = new MySqlCommand(sqlQueryMasuk, connection, trans);
                insertCommand.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error executing Insert Command (with transaction). Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non MySQL Error at Insert Command (with transaction). Code: " + ex.Message;
            }
        }

        /// <summary>
        /// Function ini digunakan untuk membaca/select record pada tabel database berbasis MySql
        /// </summary>
        /// <param name="queryBaca">Sediakan variable bertipe data string, untuk sql query</param>
        /// <param name="connection">Sediakan object MySqlConnection yang sebelumnya telah diinisiasi</param>
        /// <param name="errorMessage">Sediakan variable ref bertipe data string, untuk menampung pesan error</param>
        /// <returns>Mengembalikan object MySqlDataReader</returns>
        public MySqlDataReader Reading(string queryBaca, MySqlConnection connection, ref string errorMessage)
        {
            MySqlDataReader pembaca = null;
            try
            {
                MySqlCommand selectCommand = new MySqlCommand(queryBaca, connection);
                pembaca = selectCommand.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error reading record. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non MySQL Error at reading record. Code: " + ex.Message;
            }
            return pembaca;
        }

        /// <summary>
        /// Function ini digunakan untuk mengambil primary key terakhir pada tabel tertentu, yang nantikan akan dipergunakan untuk menentukan id incremental pada proses insert, pada tabel tersebut
        /// </summary>
        /// <param name="connection">Sediakan object MySqlConnection yang sebelumnya telah diinisiasi</param>
        /// <param name="tableName">Sediakan variable bertipe data string, yaitu nama tabel yang akan dicheck</param>
        /// <param name="tableId">Sediakan variable bertipe data string, yaitu nama id field pada tabel yang akan dicheck</param>
        /// <param name="errorMessage">Sediakan variable ref bertipe data string, untuk menampung pesan error</param>
        /// <returns>Mengembalikan tipe data integer</returns>
        public int GetMaxId(MySqlConnection connection, string tableName, string tableId, ref string errorMessage)
        {
            int maxid = 0;
            try
            {
                MySqlCommand selectCommand = new MySqlCommand("select COALESCE(MAX(" + tableId + "),0) from " + tableName + "", connection);
                MySqlDataReader pembaca = selectCommand.ExecuteReader();

                if (!pembaca.HasRows) return maxid;
                pembaca.Read();
                maxid = pembaca.GetInt16(0);
                pembaca.Close();
            }
            catch (MySqlException ex)
            {
                errorMessage = "Error get Max ID. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non MySQL Error at get Max ID. Code: " + ex.Message;
            }
            return maxid;
        }
    }
}
