using System;
using System.Data.SqlClient;
using System.Data;

namespace HyggeFinal
{
    public class DataAccessLayer {

        public enum Table
        {
            Logins,
            Person,
            Relationship,
            Interest,
            Industry,
            Education
        }


        private static class Utils {
            public static DataSet Read(Table table, ParamArgs primaryKey) //A table and ParamArgs object is passed to partially define search query, leaving parameter fill to SendToDatabase()
                => SendToDatabase($"SELECT * FROM {table} WHERE {primaryKey.ParamID.Substring(1)} = {primaryKey.ParamID}",primaryKey);

            public static void Delete(Table table, ParamArgs primaryKey)
                => SendToDatabase($"DELETE {table} WHERE {primaryKey.ParamID.Substring(1)} = {primaryKey.ParamID}",primaryKey);

            public static void Update(Table table, ParamArgs primaryKey, ParamArgs changedValue) // IMPORTANT: This method requires ParamID to match parameter name in the database!
                => SendToDatabase($"UPDATE {table} SET {changedValue.ParamID.Substring(1)} = {changedValue.ParamID} WHERE {primaryKey.ParamID.Substring(1)} = {primaryKey.ParamID}",
                    changedValue,
                    primaryKey);
        }


        public static class Login {
            //Create
            public static void CreateLogin(string email, string password) => SendToDatabase("INSERT INTO Logins VALUES (@password,@email)",
                new ParamArgs("@email", email),
                new ParamArgs("@password", password));
            //Update
            public static void UpdateLogin(string email, string newPassword) => Utils.Update(Table.Logins,new ParamArgs("@email",email),new ParamArgs("@pword",newPassword));
            //Read
            public static DataSet ReadLogin(string email) => Utils.Read(Table.Logins, new ParamArgs("@email", email));
            //Delete
            public static void DeleteLogin(string password) => Utils.Delete(Table.Logins,new ParamArgs("@pword",password));
        }


        public static class Industry {
            //Create
            public static void CreateIndustry(string industryName, string field) => SendToDatabase("INSERT INTO Industry VALUES (@industryName,@field)",
                new ParamArgs("@industryName", industryName),
                new ParamArgs("@field", field));
            //Update 
            public static void UpdateIndustry(string industryName, string newfield) => Utils.Update(Table.Industry,new ParamArgs("@industryName",industryName),new ParamArgs("@field",newfield));
            //Read
            public static DataSet ReadIndustry(string industryName) => Utils.Read(Table.Industry,new ParamArgs("@industryName",industryName));
            //Delete
            public static void DeleteIndustry(string industryName) => Utils.Delete(Table.Industry,new ParamArgs("@industryName",industryName));
        }


        public static class Education {
            //Create
            public static void CreateEducation(string educationName, string locale) => SendToDatabase("INSERT INTO Education VALUES (@educationName,@locale)",
                new ParamArgs("@educationName", educationName),
                new ParamArgs("@locale", locale));
            //Update
            public static void UpdateEducation(string educationName, string newLocale) => Utils.Update(Table.Education,new ParamArgs("@educationName",educationName),new ParamArgs("@locale",newLocale));
            //Read
            public static DataSet ReadEducation(string educationName) => Utils.Read(Table.Education,new ParamArgs("@educationName",educationName));
            //Delete
            public static void DeleteEducation(string educationName) => Utils.Delete(Table.Education,new ParamArgs("@educationName",educationName));
        }


        public static class Person {
            //Create Person (should only create necessary data and the use update to add nonessentials)
            //public static void CreatePerson(

            //Update
            public static void UpdatePerson(string personID, ParamArgs changedValue) => Utils.Update(Table.Person,new ParamArgs("@personID",personID),changedValue);

            //Read
            public static DataSet ReadPerson(string personID) => Utils.Read(Table.Person,new ParamArgs("@personID",personID));

            //Delete
            public static void DeletePerson(string personID) => Utils.Delete(Table.Person, new ParamArgs("@personID", personID));
        }


        public static class Relationship {
            //Create
            public static void CreateRelationship(string relationshipType, int lvlOfCommitment) => SendToDatabase("INSERT INTO Relationship VALUES (@relType,@lvlOfCommitment)",
                new ParamArgs("@relType",relationshipType),
                new ParamArgs("@lvlOfCommitment",lvlOfCommitment));
            //Update
            public static void UpdateRelationship(string relationshipType, int newLvlOfCommitment) => Utils.Update(Table.Relationship,
                new ParamArgs("@relationshipType",relationshipType), 
                new ParamArgs("@lvlOfCommitment",newLvlOfCommitment));
            //Read
            public static void ReadRelationship(string relationshipType) => Utils.Read(Table.Relationship,new ParamArgs("@relationshipType",relationshipType));
            //Delete
            public static void DeleteRelationship(string relationshipType) => SendToDatabase("DELETE Relationship WHERE relationshipType = @relationshipType",
                new ParamArgs("@relationshipType",relationshipType));
        }


        public static class Interest {
            //Create
            public static void CreateInterest(string category, string group) => SendToDatabase("INSERT INTO Interest VALUES (@category,@group)",
                new ParamArgs("@category",category),
                new ParamArgs("@group",group));
            //Update
            public static void UpdateInterest(string interestCategory, string newGroup) => SendToDatabase("UPDATE Interest SET interestGroup = @newGroup WHERE interestCategory = @category",
                new ParamArgs("@category",interestCategory),
                new ParamArgs("@newGroup",newGroup));
            //Read
            public static void ReadInterest(string interestCategory) => SendToDatabase("SELECT * FROM Interest WHERE interestCategory = @interestCategory", 
                new ParamArgs("@interestCategory",interestCategory));
            //Delete
            public static void DeleteInterest() => SendToDatabase("");
        }
        

        public static string Test() { //this method should only be used to test out new features of the db. it does not test the functionality of the whole class.
            try {
                DataSet ds = Login.ReadLogin("anabanana@hotmail.com");
                return ds.Tables[0].Rows[0][0].ToString(); // returns the value of the first column of the first row in string format
            } catch (SqlException) { return ("failed to connect."); } // returns an error message if the client couldn't connect to the db server (check the data source!)
        }


        private static DataSet SendToDatabase(string sqlQuery, params ParamArgs[] args ) {
            try {
                using (SqlConnection cnn = new SqlConnection("Data Source = SYST4DEV01; Initial Catalog = Hygge; User ID=hygge ; Password =hej123 ")) { //SQL Connection
                    cnn.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, cnn)) // "using" keyword ensures disposal when objects are no longer used
                    using (SqlDataAdapter adapter = new SqlDataAdapter()) {
                        if (args != null) { foreach (ParamArgs param in args) command.Parameters.AddWithValue(param.ParamID, param.Value); } // fill each parameter using ParamArgs
                        string queryType = sqlQuery.Substring(0, 6);                                        //The initial command is separated into a substring
                        if ( queryType == "UPDATE" || queryType == "DELETE" || queryType == "INSERT" ) {        // if the query writes to the database...
                            switch (queryType) {// ... the adapter is associated with the proper command and executed.
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
                        else {
                            DataSet dataSet = new DataSet(); // A new DataSet is initialized
                            adapter.SelectCommand = command; // adapter is associated with the identified SELECT command
                            adapter.Fill(dataSet); // The DataSet is filled with the results from the adapter's command
                            return dataSet; // the DataSet is returned
                        }
                    }
                }
            }
            catch (SqlException) { Console.WriteLine("error"); return null; } // error handling here
        }
    }
}