using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace App.DAL
{
    public class DBManager
    {
        private readonly string ConnectionString;

        public DBManager()
        {
            ConnectionString = ConfigurationManager.AppSettings["DBConnectionString_Dev"];
        }

        public SqlCommand GetSqlCommand(string sqlCommand)
        {
            var con = new SqlConnection(ConnectionString);
            return new SqlCommand(sqlCommand, con);
        }

        public void AddParameter(SqlCommand cmd,string parameterName,SqlDbType sqlDbType)
        {
            cmd.Parameters.Add(parameterName, sqlDbType);
        }

        public DataTable GetDataTableResult(SqlCommand sqlCommand)
        {
            try
            {
                var da = new SqlDataAdapter(sqlCommand);
                var dt = new DataTable();

                sqlCommand.Connection.Open();
                da.Fill(dt);

                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
        }
    }
}
