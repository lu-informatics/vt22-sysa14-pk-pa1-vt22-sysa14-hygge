using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace HyggeFinal
{
    internal class DataAccessLayer
    {
        private static SqlConnection cnn = new SqlConnection("Data Source = SYST4DEV01; Initial Catalog = Hygge; User ID=hygge ; Password =hej123 ");
        
        public static string Test(){
        try {
                cnn.Open();
                cnn.Close();
                return ("Successfully connected and closed server.");
        } catch (SqlException){
                return ("failed to connect.");
            }
        }
        
    }

}
