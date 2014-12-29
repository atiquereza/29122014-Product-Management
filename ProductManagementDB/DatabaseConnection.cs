using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagementSystem
{
    class DatabaseConnection
    {
        public string connectionString = @"Server=(local)\SQLEXPRESS; Database=ProductManagement; Integrated Security=true;";
        public string sqlQuery;

        public void connectDB(out SqlCommand command, out SqlConnection conn)
        {
            conn = new SqlConnection(connectionString);
            command = new SqlCommand(sqlQuery, conn);
            conn.Open();
        }

        public SqlDataReader sqlSelect(SqlCommand command)
        {
            SqlDataReader newDataReader = command.ExecuteReader();
            return newDataReader;
        }



        public void connectionClose(SqlConnection conn)
        {
            conn.Close();
        }

        public int sqlDML(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        
    }
}
