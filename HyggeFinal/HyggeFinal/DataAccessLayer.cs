using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
namespace HyggeFinal
{
    public class DataAccessLayer
    {
        public delegate void errorHandler(string message);
        //to receive error information about the DAL's SqlExceptions, a class must bind a fitting method to this delegate instance.
        public static errorHandler onSqlError;
        public enum Table
        { // Enumeration of valid Tables
            Logins,
            Person,
            Relationship,
            Interest,
            Industry,
            Education,
            EducationIndustry,
            PersonInterest,
            Error
        }
        public static class Utils
        {
            // These methods partially define the SQL query, leaving parameter fill for the SendToDatabes method.
            // IMPORTANT: These methods require that ParamIDs match their respective parameter names in the database.
            public static void Create(Table table, params ParamArgs[] values)
            {
                string paramNames = "";
                string paramIDs = "";
                for(int i = 0; i<values.Length;i++)
                {
                    paramNames  +=$"{(i<0?",":"")}{values[i].ParamID.Substring(1)}";
                    paramIDs    +=$"{(i<0?",":"")}{values[i].ParamID}";
                }
                SendToDatabase($"INSERT INTO {table}({paramNames}) VALUES ({paramIDs})");
            }
            
            public static void Update(Table table, ParamArgs changedValue, params ParamArgs[] keys)
            { //line below prevents duplicate declaration at pk modification without needing any pk_ prefix
                ParamArgs valuePA = new ParamArgs("@val_" + changedValue.ParamID.Substring(1), changedValue);
                BuildQuery($"UPDATE {table} SET {valuePA.ParamID.Substring(5)} = {valuePA.ParamID}",keys); 
            }
            
            public static DataSet Read(Table table, params ParamArgs[] keys) => BuildQuery($"SELECT * FROM {table}",keys);
            
            public static void Delete(Table table, params ParamArgs[] keys) => BuildQuery($"DELETE {table}", keys);
            
            public static DataSet BuildQuery(string queryStart, params ParamArgs[] keys)
            {//BuildQuery creates any amount of 'equals' conditions to append to a query.
                if (keys != null && keys.Length > 0) 
                    for (int i = 0; i < keys.Length; i++) queryStart += $"{(i > 0 ? " AND" : " WHERE")} {keys[i].ParamID.Substring(1)} = {keys[i].ParamID}";
                return SendToDatabase(queryStart, keys);
            }
        }
        public static class Person
        {
            public static void CreatePerson(
                string personID, string username, int age, string gender,
                string email, string relationshipType, string industryName,
                string educationName, string preference)
                => Utils.Create(Table.Person, 
                    new ParamArgs("@personID", personID),
                    new ParamArgs("@email", email),
                    new ParamArgs("@username", username),
                    new ParamArgs("@age", age),
                    new ParamArgs("@educationName", educationName),
                    new ParamArgs("@industryName", industryName),
                    new ParamArgs("@relationshipType", relationshipType),
                    new ParamArgs("@gender", gender),
                    new ParamArgs("@preference", preference));
        }
        private static DataSet SendToDatabase(string sqlQuery, params ParamArgs[] args)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Data Source = SYST4DEV01; Initial Catalog = Hygge; User ID=hygge ; Password =hej123 "))
                { //SQL Connection
                    cnn.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, cnn)) // "using" keyword ensures disposal when objects are no longer used
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        if (args != null) { foreach (ParamArgs param in args) command.Parameters.AddWithValue(param.ParamID, param.Value); } // fill each parameter using ParamArgs
                        string queryType = sqlQuery.Substring(0, 6);                                        //The initial command is separated into a substring
                        List<string> wca = new List<string> { "UPDATE", "INSERT", "DELETE" };
                        if (wca.Contains(queryType))
                        {        // if the query contains a command that writes to the database...
                            switch (queryType)
                            {// ... then the adapter will be associated with the proper command and executed.
                                case "UPDATE":
                                    adapter.UpdateCommand = command; //association...
                                    adapter.UpdateCommand.ExecuteNonQuery(); //... and execution
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
                            return null; // returns null in place of a DataSet. Shouldn't cause any issues since this code path should only be reached by non-SELECT statements.
                        }
                        else
                        {
                            DataSet dataSet = new DataSet(); // A new DataSet is initialized
                            adapter.SelectCommand = command; // adapter is associated with the identified SELECT command
                            adapter.Fill(dataSet); // The DataSet is filled with the results from the adapter's command
                            return dataSet; // the DataSet is returned
                        }
                    }
                }
            }
            catch (SqlException e) {
                string message;
                switch (e.Number)
                {
                    case -2: //connection timed out
                        message = "Connection timed out. Please Check your connection and try again later.";
                        break;
                    case 208: //invalid object name (no table selected when searching/clearing selection)
                        message = "An error occurred. Please select a table in the 'View' menu and try again.";
                        break;
                    case 8178: //expected a parameter that was not supplied, or an int was too long - trying to create with empty table
                        message = "An error has occurred. Please make sure that all fields are filled in correctly.";
                        break;
                    case 245: //conversion error from string to int
                        message = "An error has occured. Please make sure that lvlOfCommitment/age is an appropriate number.";
                        break;
                    case 547: //foreign key violation: no such foreign key
                        message = "Error: Education, Industry, Interest and Relationship fields must use an existing entry. Add them first and try again.";
                        break;
                    case 2628: //string too long
                        message = "Error: Please make sure that all fields are appropriately sized.";
                        break;
                    case 2627: //primary key violation
                        message = "Error: There already exists an entry with that identifier.";
                        break;
                    case 18456: //login failed for database
                        message = "Error: Login failed.";
                        break;
                    case 67: //network name could not be found
                        message = "Error: The Network could not be found!";
                        break;
                    default:
                        message = "Error";
                        break;
                }
                onSqlError?.Invoke(message); //if the onSqlError has any delegates, invoke it.
                return null;
            }
        }
    }
}