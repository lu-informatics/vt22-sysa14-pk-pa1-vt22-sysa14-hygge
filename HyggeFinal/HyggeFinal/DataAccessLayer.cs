using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace HyggeFinal
{
    public class DataAccessLayer
    {
        private string connectionString = "Server=localhost;Database=Hygge;Intergrated Security = true";




        public SqlDataReader GetPersons()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "SELECT * FROM Person";
                    command.Connection = connection;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader;
                    }
                }
            }
        }
    }
}

        
            
    


