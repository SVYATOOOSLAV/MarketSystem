using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Kurs
{
    internal class DataBaseConn
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=SVYATBOOK;Initial Catalog=kurs;Integrated Security=True");

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
