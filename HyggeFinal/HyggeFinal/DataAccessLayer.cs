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
        private static Dictionary<string, string> queries = new Dictionary<string,string>() {};
                                                                          /*SERVER NAME*/
        private static SqlConnection cnn = new SqlConnection("Data Source = SYST4DEV01; Initial Catalog = Hygge; User ID=hygge ; Password =hej123 "); // CHECK THIS IF CONNECT FAIL
        
        /* update methods can open and close the method in one go, whilst find methods need to send the connection to be closed after saving the value-
        the reason that we don't handle the conversion inside the method is because it decreases flexibility and modularity; the data access layer should only return datareaders.
        */

        public static string Test(){
        try {
                cnn.Open();
                cnn.Close();
                return ("Successfully connected and closed server.");
        } catch (SqlException){
                return ("failed to connect.");
            }
        }


        public static SqlDataReader FetchFromDatabase()
        {
            try { return null; } catch (SqlException) { return null; }
        }
    }

}
