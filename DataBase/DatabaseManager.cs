using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.DataBase
{
    public class DatabaseManager
    {
        private string connectionString = "";
        private SqlConnection connection;
        private SqlTransaction transaction;

        public DatabaseManager(string connectionString)
        {
           connection = new SqlConnection(connectionString);
        }

        public SqlConnection getConnection() => connection;

        public void openConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Метод для выполнения запроса
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            openConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                DataTable dataTable = new DataTable();
                dataTable.Load(command.ExecuteReader());
                closeConnection();

                return dataTable;
            }
        }

        public void BeginTransaction()
        {
            openConnection();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
            closeConnection();
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
            closeConnection();
        }
    }
}
