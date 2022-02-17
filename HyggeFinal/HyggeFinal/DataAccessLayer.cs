using System;
using System.Data.SqlClient;
using System.Data;

namespace HyggeFinal
{
    public class DataAccessLayer {
        //Example method that will be called by the GUI controller:
        public static void AddLogin(string email, string password) => SendToDatabase("INSERT INTO Logins VALUES (@pw,@em)",new ParamArgs("@em",email), new ParamArgs("@pw",password));

        public static string Test(){ //this method should only be used to test out new features of the db. it does not test the functionality of the whole class.
            try {
                SendToDatabase("INSERT INTO Logins VALUES (@pw,@em)", new ParamArgs("@pw", "plapcom"), new ParamArgs("@em", "plip@plopmail.com"));
                DataSet ds = SendToDatabase("SELECT pword FROM Logins WHERE email = 'anabanana@hotmail.com'");
                return ds.Tables[0].Rows[0][0].ToString();
            } catch (SqlException) { return ("failed to connect."); } // returns an error message if the client couldn't connect to the db server (check the data source!)
        }
        public static DataSet SendToDatabase(string sqlQuery, params ParamArgs[] args ) {
            try {
                using (SqlConnection cnn = new SqlConnection("Data Source = SYST4DEV01; Initial Catalog = Hygge; User ID=hygge ; Password =hej123 ")) { //SQL Connection
                    cnn.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, cnn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter()) {
                        if (args != null) { foreach (ParamArgs param in args) command.Parameters.AddWithValue(param.ParamID, param.Value); } // fill each parameter using ParamArgs
                        string queryType = sqlQuery.Substring(0, 6);                                        //The initial command is separated into a substring
                        if ( queryType == "UPDATE" || queryType == "DELETE" || queryType == "INSERT" ) {        // if the query writes to the database...
                            switch (queryType) {// ... the adapter is associated with the proper command and executed.
                                case "UPDATE":
                                    adapter.UpdateCommand = command; //association
                                    adapter.UpdateCommand.ExecuteNonQuery(); //execution
                                    break;
                                case "DELETE":
                                    adapter.DeleteCommand = command;
                                    adapter.DeleteCommand.ExecuteNonQuery();
                                    break;
                                case "INSERT":
                                    adapter.InsertCommand = command;
                                    adapter.InsertCommand.ExecuteNonQuery();
                                    break;
                            }
                            return null; // returns null since this code path should only be reached by non-SELECT statements.
                        }
                        else {
                            DataSet dataSet = new DataSet(); // A new DataSet is initialized
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet); // The DataSet is filled with the results from the adapter's command
                            return dataSet;
                        }
                    }
                }
            }
            catch (SqlException) { Console.WriteLine("error"); return null; } // error handling here
        }
    }
    public class ParamArgs { // does this really deserve its own .cs file? is it a crime to keep it like this?
        public ParamArgs(string paramID, object val) { ParamID = paramID; Value = val; } //constructor
        public string ParamID { get; set; } //ParamID property
        public object Value { get; set; } //Value property
    }
}