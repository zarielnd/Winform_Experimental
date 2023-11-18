using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ZarielMartApplication.ADO
{
    internal class DBContext
    {
        private SqlConnection _connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=ZarielMartDB;Integrated Security=True");

        public SqlConnection getConnection() { return _connection; }

        public void openConnection() {
            if (_connection.State != ConnectionState.Open) { 
                _connection.Open();
            }
        }

        public void closeConnection()
        {
            if (_connection.State != ConnectionState.Closed) { _connection.Close(); }
        }
    }
}
