using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryOperations.Contexts
{
    public class DBContext
    {
        private readonly IDbConnection _connection;
        public DBContext(string connectionstring)
        {
            _connection = new SqlConnection(connectionstring);
        }
        public IDbConnection Connection
        {
            get { return _connection; }
        }
    }
}
