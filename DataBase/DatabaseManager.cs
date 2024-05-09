using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.DataBase
{
    public class DataBaseManager
    {
        private readonly string connectionString;

        public DataBaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(command.ExecuteReader());
                    return dataTable;
                }
            }
        }
    }
}
