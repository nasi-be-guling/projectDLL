using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
//using System.Linq;
//using System.Text;
using _encryption;

namespace _connection
{
    public class CConnect
    {
        public SqlConnection KonekDb(string configurationManager)
        {
            // ===================================== OLDIES WITH PLAIN STRING READER =======================================================
            //string dbName;
            //string instance;
            //string port;
            //string username;
            //string pass;

            //TextReader tR = new StreamReader("settings.ini");
            //string reader = null;

            //List<settingList> grupSetting = new List<settingList>();
            //while ((reader = tR.ReadLine()) != null)
            //{
            //    settingList itemSettingList = new settingList();
            //    itemSettingList.iniFile = reader.ToString();
            //    grupSetting.Add(itemSettingList);
            //}
            //dbName = grupSetting[0].iniFile.ToString();
            //instance = grupSetting[1].iniFile.ToString();
            //port = grupSetting[2].iniFile.ToString();
            //username = grupSetting[3].iniFile.ToString();
            //pass = grupSetting[4].iniFile.ToString();
            //tR.Close();

            //string koneksi = @"Initial Catalog=" + dbName + ";" +
            //    @"Data Source=" + instance + "," + port + ";" +
            //    @"User ID='" + username + "';" +
            //    @"Password='" + pass + "'";

            // =================================== With encrypted connection string !!!!! =========================================
            string conString = configurationManager;
            SqlConnection koneksiAktip = new SqlConnection(CStringCipher.Decrypt(conString, "123"));

            return koneksiAktip;
        }

        public void MasukkanData(string sqlQueryMasuk, SqlConnection koneksiAktip)
        {
            SqlCommand insertCommand = new SqlCommand(sqlQueryMasuk, koneksiAktip);
            insertCommand.ExecuteNonQuery();
            //insertCommand.Dispose();
        }

        public void MasukkanData(string sqlQueryMasuk, SqlConnection koneksiAktip, SqlTransaction trans)
        {
            // this overriding moethod is including a transaction
            SqlCommand insertCommand = new SqlCommand(sqlQueryMasuk, koneksiAktip, trans);
            insertCommand.ExecuteNonQuery();
        }

    //    public bool BulkCommand(SqlConnection connection, SqlTransaction trans, string query, string paramName, string paramValue)
    //    {
    //        using (SqlCommand cmd = connection.CreateCommand())
    //        {
    //            cmd.Connection = connection;
    //            cmd.Transaction = trans;
    //            cmd.CommandType = CommandType.Text;
    //            try
    //            {
    //                cmd.CommandText = query;
    //                cmd.Parameters.AddWithValue(paramName, paramValue);
    //                cmd.ExecuteNonQuery();
    //                return true;
    //            }
    //            catch (Exception ex)
    //            {
    //                //MessageBox.Show(string.Format("Terjadi Kesalahan dengan tipe : {0}, Pesan : {1}", ex.GetType(),
    //                //    ex.Message));
    //                try
    //                {
    //                    trans.Rollback();
    //                }
    //                catch (Exception ex2)
    //                {
    //                    //MessageBox.Show(string.Format("Terjadi Kesalahan dengan tipe : {0}, Pesan : {1}", ex2.GetType(),
    //                    //    ex2.Message));
    //                    throw;
    //                }
    //                throw;
    //            }
    //        }
    ////      string connString = "your connectionstring";
    ////      try
    ////      {
    ////          using (var conn = new SqlConnection(connString))
    ////          {
    ////              using (var cmd = new SqlCommand())
    ////              {
    ////                  cmd.Connection = conn;
    ////                  cmd.CommandType = CommandType.Text;
    ////                  cmd.CommandText = "DELETE FROM tbl_Users WHERE userID = @id";
    ////                  cmd.Parameters.AddWithValue("@id", userId);
    ////                  conn.Open();
    ////                  cmd.ExecuteNonQuery();
    ////                  return true;
    ////              }
    ////          }
    ////      }
    ////      catch(Exception ex)
    ////      {
    ////          //Log the Error here for Debugging
    //        //return false;
    //    }

        public DataTable DTabel(string queryShowTable, SqlConnection koneksiAktip)
        {
            SqlDataAdapter dAdapter = new SqlDataAdapter(queryShowTable, koneksiAktip);

            DataTable dTabel = new DataTable();
            dAdapter.Fill(dTabel);

            return dTabel;
        }

        public SqlDataReader MembacaData(string queryBaca, SqlConnection koneksiAktip, ref string exceptionMessage)
        {
            SqlCommand selectCommand = new SqlCommand(queryBaca, koneksiAktip);
            SqlDataReader pembaca = null;
            try
            {
                pembaca = selectCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                if (pembaca != null)
                pembaca.Close();
                exceptionMessage = ex.Message;
                return null;
            }
            return pembaca;
        }

        public SqlDataReader MembacaData(string queryBaca, SqlConnection koneksiAktip, SqlTransaction transaction)
        {
            SqlCommand selectCommand = new SqlCommand(queryBaca, koneksiAktip, transaction);
            SqlDataReader pembaca = null;
            try
            {
                pembaca = selectCommand.ExecuteReader();
            }
            catch
            {
                if (pembaca != null)
                    pembaca.Close();
            }
            return pembaca;
        }

        public string PengambilPKey(string qbaca, string configurationManager)
        {
            SqlConnection koneksiAktip = KonekDb(configurationManager);
            koneksiAktip.Open();
            SqlDataAdapter aBaca = new SqlDataAdapter(qbaca, koneksiAktip);
            DataTable dTabel = new DataTable();
            aBaca.Fill(dTabel);
            DataTableReader pembaca = dTabel.CreateDataReader();
            pembaca.Read();
            string pKey = pembaca.HasRows ? pembaca[0].ToString().Trim() : "NO";
            pembaca.Close();
            return pKey;
        }

        //public string pengecekData(string queryCek, SqlConnection koneksiAktip, TextBox namaTxt)
        //{
        //    SqlDataAdapter sAdap = new SqlDataAdapter(queryCek, koneksiAktip);
        //    string hasilCek = null;
        //    DataTable dTabel = new DataTable();
        //    sAdap.Fill(dTabel);
        //    while (dTabel.Rows[0].ToString().Trim() != namaTxt.Text.Trim())
        //    {
        //        dTabel.n
        //        //if dTabel.Rows .ToString().Trim() == namaTxt.Text.Trim())
        //        //{
        //        //    hasilCek = "ADA";
        //        //}
        //        //else
        //        //{
        //        //    hasilCek = "TIDAK";
        //        //}
        //        //hasilCek = dRow[1].ToString();
        //    }
        //    return hasilCek;
        //}
        public int DataReaderRowCount(string query, SqlConnection koneksi)
        {
            int rowCount = 0;
            SqlCommand perintah = new SqlCommand(query, koneksi);
            koneksi.Open();
            SqlDataReader pembaca = perintah.ExecuteReader();
            if (pembaca.HasRows)
            {
                while (pembaca.Read())
                {
                    rowCount++;
                }
                pembaca.Close();
            }
            koneksi.Close();
            return rowCount;
        }

        public bool CekKoneksi(string configurationManager, ref string exceptionMessage)
        {
            SqlConnection connection = KonekDb(configurationManager);
            try
            {
                connection.Open();
            }
            catch (Exception exception)
            {
                exceptionMessage = exception.Message;
                return false;
            }
            connection.Close();
            return true;
        }

        public bool CekFieldUnik(string namaTabel, string namaField, string kondisi, string configurationManager)
        {
            bool status = false;
            SqlConnection koneksi = KonekDb(configurationManager);
            koneksi.Open();
            SqlDataReader pembaca = MembacaData("SELECT " + namaField + " FROM " + namaTabel + " WHERE " + namaField + " = '" +
                kondisi + "' ", koneksi, null);
            if (pembaca.HasRows)
            {
                status = true;
            }
            koneksi.Close();
            return status;
        }

        public bool CekFieldUnikNoKoneksi(string namaTabel, string namaField, string kondisi, SqlConnection koneksi)
        {
            bool status = false;
            SqlDataReader pembaca = MembacaData("SELECT " + namaField + " FROM " + namaTabel + " WHERE " + namaField + " = '" +
                kondisi + "' ", koneksi, null);
            if (pembaca.HasRows)
            {
                status = true;
            }
            return status;
        }

        public string GetServer(string configurationManager)
        {
            //TextReader tR = new StreamReader("settings.ini");
            //string reader = null;

            //List<settingList> grupSetting = new List<settingList>();
            //while ((reader = tR.ReadLine()) != null)
            //{
            //    settingList itemSettingList = new settingList();
            //    itemSettingList.iniFile = reader.ToString();
            //    grupSetting.Add(itemSettingList);
            //}
            ////dbName = grupSetting[0].iniFile.ToString();
            //namaServer = grupSetting[1].iniFile.ToString();
            ////port = grupSetting[2].iniFile.ToString();
            ////username = grupSetting[3].iniFile.ToString();
            ////pass = grupSetting[4].iniFile.ToString();
            //tR.Close();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            string conString = configurationManager;
            //namaServer = 
            builder.ConnectionString = CStringCipher.Decrypt(conString, "123");
            //namaServer = builder.UserInstance;
            return builder.DataSource.Remove(builder.DataSource.ToString(CultureInfo.InvariantCulture).Length - 5);
        }
    }
}
