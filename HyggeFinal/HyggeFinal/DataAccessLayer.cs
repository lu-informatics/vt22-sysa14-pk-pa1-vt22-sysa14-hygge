using System;
using System.Data.SqlClient;
using System.Data;

namespace HyggeFinal
{
    public class DataAccessLayer {    
        //Create Login
        public static void CreateLogin(string email, string password) => SendToDatabase("INSERT INTO Logins VALUES (@password,@email)",
            new ParamArgs("@email",email), 
            new ParamArgs("@password",password));
        
        //Read Login
        public static DataSet ReadLogin(string email) => SendToDatabase("SELECT * FROM Login WHERE email = @email",
            new ParamArgs("@email",email));
        
        //Delete Login
        public static void DeleteLogin(string password) => SendToDatabase("DELETE Logins WHERE pword = @password",
            new ParamArgs("@password", password));

        //Create Industry
        public static void CreateIndustry(string industryName, string field) => SendToDatabase("INSERT INTO Industry VALUES (@industryName,@field)",
            new ParamArgs("@industryName",industryName),
            new ParamArgs("@field",field));



        //Create Education
        public static void CreateEducation(string educationName, string locale) => SendToDatabase("INSERT INTO Education VALUES (@educationName,@locale)", 
            new ParamArgs("@educationName", educationName),
            new ParamArgs("@locale",locale));
        
        //Read Education
        public static DataSet ReadEducation(string educationName) => SendToDatabase("SELECT * FROM Education WHERE educationName = @educationName",
            new ParamArgs("@educationName",educationName));
        
        //Delete Education
        public static void DeleteEducation(string educationName) => SendToDatabase("DELETE Education WHERE educationName = @educationName",
            new ParamArgs("@educationName", educationName));
        
        //Create Person
        public static void CreatePerson(// Create Person TODO: make method that reads in paramIDs and constructs a tailored SQL query based on that
            string personID, string username, int age, string gender,
            string email, string relationshipType, string industryName,
            string educationName, string preference) => SendToDatabase("");
        
        //Read Person
        public static DataSet ReadPerson(string personID) => SendToDatabase("SELECT * FROM Person WHERE personID = @pi",
            new ParamArgs("@pi",personID));

        //Delete Person
        public static void DeletePerson(string personID) => SendToDatabase("DELETE Person WHERE personID = @personID", new ParamArgs("@personID",personID));

        public static string Test(){ //this method should only be used to test out new features of the db. it does not test the functionality of the whole class.
            try {
                //SendToDatabase("INSERT INTO Logins VALUES (@pw,@em)", new ParamArgs("@pw", "plapcom"), new ParamArgs("@em", "plip@plopmail.com"));
                //DataSet ds = SendToDatabase("SELECT pword FROM Logins WHERE email = 'anabanana@hotmail.com'");
                DataSet ds = ReadLogin("anabanana@hotmail.com");
                return ds.Tables[0].Rows[0][0].ToString();
            } catch (SqlException) { return ("failed to connect."); } // returns an error message if the client couldn't connect to the db server (check the data source!)
        }
        private static DataSet SendToDatabase(string sqlQuery, params ParamArgs[] args ) {
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