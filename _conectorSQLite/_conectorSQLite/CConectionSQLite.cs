using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using _encryption;

namespace _conectorSQLite
{
    public class CConectionSQLite
    {
        private SQLiteConnection _connection;

        /// <summary>
        /// Fungsi ini digunakan untuk melakukan inisiasi koneksi/membuka koneksi ke database berbasis SQLite.
        /// </summary>
        /// <param name="configurationManager">
        /// Sediakan/deklarasikan variable bertipe data string.
        /// Nilai yang harus diisi pada variable configuration manager diperoleh melalui = Properties.Settings.Default.Setting   
        /// </param>
        /// <param name="errorMessage">Deklarasikan variable string yang akan digunakan untuk menampung pesan kesalahan</param>
        /// <param name="chiperPasswd">Masukkan nilai bertipe string untuk menchiper/encrypt conection string</param>
        /// <returns></returns>
        public SQLiteConnection Connect(string configurationManager, ref string errorMessage, string chiperPasswd)
        {
            string conString = configurationManager;

            try
            {
                _connection = new SQLiteConnection(CStringCipher.Decrypt(conString, chiperPasswd));
                _connection.Open();
            }
            catch (SQLiteException ex)
            {
                errorMessage = "Error connecting to server: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non SQLite error on connecting to server error: " + ex.Message;
            }

            return _connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQueryMasuk"></param>
        /// <param name="connection"></param>
        /// <param name="errorMessage"></param>
        public void Insertion(string sqlQueryMasuk, SQLiteConnection connection, ref string errorMessage)
        {
            try
            {
                SQLiteCommand insertCommand = new SQLiteCommand(sqlQueryMasuk, connection);
                insertCommand.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                errorMessage = "Error executing Insert Command. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non SQLite Error at Insert Command. Code: " + ex.Message;
            }
        }

        public void Insertion(string sqlQueryMasuk, SQLiteConnection sqLiteConnection,
            SQLiteTransaction sqLiteTransaction, ref string errorMessage)
        {
            try
            {
                SQLiteCommand insertCommand = new SQLiteCommand(sqlQueryMasuk, sqLiteConnection, sqLiteTransaction);
                insertCommand.ExecuteNonQuery();
            }
            catch (SQLiteException Ex)
            {
                errorMessage = "Error executing Insert Command (with transaction). Code: " + Ex.Message;
            }
            catch (Exception Ex)
            {
                errorMessage = "Non SQLite Error at Insert Command (with transaction). Code: " + Ex.Message;
            }
        }

        public SQLiteDataReader ReadingSqLiteDataReader(string queryBaca, SQLiteConnection connection,
            ref string errorMessage)
        {
            SQLiteDataReader pembaca = null;
            try
            {
                SQLiteCommand selectCommand = new SQLiteCommand(queryBaca, connection);
                pembaca = selectCommand.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                errorMessage = "Error reading record. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non SQLite Error at reading record. Code: " + ex.Message;
            }

            return pembaca;
        }

        public int getMaxID(SQLiteConnection connection, string tableName, string tableID, ref string errorMessage)
        {
            int maxID = 0;
            try
            {

            }
            catch (SQLiteException ex)
            {
                errorMessage = "Error get Max ID. Code: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = "Non SQLite Error at get Max ID. Code: " + ex.Message;
            }
            return maxID;
        }
    }
}
