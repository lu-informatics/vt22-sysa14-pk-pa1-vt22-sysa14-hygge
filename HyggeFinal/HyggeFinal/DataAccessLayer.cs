using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace HyggeFinal
{
    public class DataAccessLayer
    { //TODO error handling
        public delegate void errorHandler(string message);
        public static errorHandler onSqlError;
        public static string Test()
        { //this method should only be used to test out new features of the db. it does not test the functionality of the whole class.   
            try
            {
                DataSet ds = Login.ReadLogin("anabanana@hotmail.com");
                return ds.Tables[0].Rows[0][0].ToString(); // returns the value of the first column of the first row in string format
            }
            catch (SqlException) { return ("failed to connect."); } // returns a simple error message if the client couldn't connect to the db server (check the data source!)
        }
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
            // IMPORTANT: These methods requires ParamIDs to match parameter names in the database.

            public static DataSet ViewAll(Table table) => SendToDatabase($"SELECT * FROM {table}");

            public static void Create(Table table, ParamArgs value1, ParamArgs value2)//Generic SQL Create Method
                => SendToDatabase($"INSERT INTO {table}({value1.ParamID.Substring(1)},{value2.ParamID.Substring(1)}) VALUES ({value1.ParamID},{value2.ParamID})", value1, value2);
            public static void Update(Table table, ParamArgs primaryKey, ParamArgs changedValue)//Generic SQL Update Method

                //The Primarykey paramID is preceeded by 'pk_' in order to permit primary key modification. Example: @pk_email
                //if not, then @email would be declared twice in the SQL statement which throws an exception.
                //This is why PK's paramID is substringed at index 4 instead of 1, as it needs to cut off more than just the '@'.
                => SendToDatabase($"UPDATE {table} SET {changedValue.ParamID.Substring(1)} = {changedValue.ParamID} WHERE {primaryKey.ParamID.Substring(4)} = {primaryKey.ParamID}",
                    changedValue,
                    primaryKey);

            public static void Update(Table table, ParamArgs primaryKey1, ParamArgs primaryKey2, ParamArgs changedValue)
                //The Primarykey paramIDs are preceeded by 'pk_' in order to permit primary key modification. Example: @pk_email
                => SendToDatabase(
                    $"UPDATE {table} SET {changedValue.ParamID.Substring(1)} = {changedValue.ParamID} " +
                    $"WHERE {primaryKey1.ParamID.Substring(4)} = {primaryKey1.ParamID} " +
                    $"AND {primaryKey2.ParamID.Substring(4)} = {primaryKey2.ParamID}",
                    changedValue,
                    primaryKey1,
                    primaryKey2);

            public static DataSet Read(Table table, ParamArgs primaryKey)//Generic SQL Read method
                => SendToDatabase($"SELECT * FROM {table} WHERE {primaryKey.ParamID.Substring(1)} = {primaryKey.ParamID}", primaryKey);

            public static DataSet Read(Table table, ParamArgs primaryKey1,ParamArgs primaryKey2) //Many-to-many variant
                => SendToDatabase(
                    $"SELECT * FROM {table} " +
                    $"WHERE {primaryKey1.ParamID.Substring(1)} = {primaryKey1.ParamID} " +
                    $"AND {primaryKey2.ParamID.Substring(1)}={primaryKey2.ParamID}", 
                    primaryKey1,
                    primaryKey2);

           
            public static void Delete(Table table, ParamArgs primaryKey) //Generic SQL Delete Method
                => SendToDatabase($"DELETE {table} WHERE {primaryKey.ParamID.Substring(1)} = {primaryKey.ParamID}", primaryKey);
            public static void Delete(Table table, ParamArgs primaryKey1,ParamArgs primaryKey2) //Many-to-many variant
                => SendToDatabase(
                    $"DELETE {table} " +
                    $"WHERE {primaryKey1.ParamID.Substring(1)} = {primaryKey1.ParamID} " +
                    $"AND {primaryKey2.ParamID.Substring(1)} = {primaryKey2.ParamID}", 
                    primaryKey1,
                    primaryKey2);
        }

        public static class Login
        {
            //Create
            public static void CreateLogin(string email, string password) => Utils.Create(Table.Logins, new ParamArgs("@email", email), new ParamArgs("@pword", password));
            //Update
            public static void UpdateLogin(string email, string newPassword) => Utils.Update(Table.Logins, new ParamArgs("@pk_email", email), new ParamArgs("@pword", newPassword));
            //Read
            public static DataSet ReadLogin(string email) => Utils.Read(Table.Logins, new ParamArgs("@email", email));
            //Delete
            public static void DeleteLogin(string email) => Utils.Delete(Table.Logins, new ParamArgs("@email", email));
        }

        public static class Industry
        {
            //Create
            public static void CreateIndustry(string industryName, string field) => Utils.Create(Table.Industry, new ParamArgs("@industryName", industryName), new ParamArgs("@field", field));
            //Update 
            public static void UpdateIndustry(string industryName, string newfield) => Utils.Update(Table.Industry, new ParamArgs("@pk_industryName", industryName), new ParamArgs("@field", newfield));
            //Read
            public static DataSet ReadIndustry(string industryName) => Utils.Read(Table.Industry, new ParamArgs("@industryName", industryName));
            //Delete
            public static void DeleteIndustry(string industryName) => Utils.Delete(Table.Industry, new ParamArgs("@industryName", industryName));
        }

        public static class Education
        {
            //Create
            public static void CreateEducation(string educationName, string locale)
                => Utils.Create(Table.Education, new ParamArgs("@educationName", educationName), new ParamArgs("@locale", locale));
            //Update
            public static void UpdateEducation(string educationName, string newLocale)
                => Utils.Update(Table.Education, new ParamArgs("@pk_educationName", educationName), new ParamArgs("@locale", newLocale));
            //Read
            public static DataSet ReadEducation(string educationName) => Utils.Read(Table.Education, new ParamArgs("@educationName", educationName));
            //Delete
            public static void DeleteEducation(string educationName) => Utils.Delete(Table.Education, new ParamArgs("@educationName", educationName));
        }

        public static class Person
        {
            public static void CreatePerson(
                string personID, string username, int age, string gender,
                string email, string relationshipType, string industryName,
                string educationName, string preference)
                => SendToDatabase(
                    "INSERT INTO Person(personID,username,age,gender,preference,email,relationshipType,educationName,industryName) " +
                    "VALUES (@personID,@username,@age,@gender,@preference,@email,@relationshipType,@educationName,@industryName)",
                    new ParamArgs("@personID", personID),
                    new ParamArgs("@email", email),
                    new ParamArgs("@username", username),
                    new ParamArgs("@age", age),
                    new ParamArgs("@educationName", educationName),
                    new ParamArgs("@industryName", industryName),
                    new ParamArgs("@relationshipType", relationshipType),
                    new ParamArgs("@gender", gender),
                    new ParamArgs("@preference", preference));
            //Update
            public static void UpdatePerson(string personID, ParamArgs changedValue) => Utils.Update(Table.Person, new ParamArgs("@pk_personID", personID), changedValue);
            //Read
            public static DataSet ReadPerson(string personID) => Utils.Read(Table.Person, new ParamArgs("@personID", personID));
            //Delete
            public static void DeletePerson(string personID) => Utils.Delete(Table.Person, new ParamArgs("@personID", personID));
        }

        public static class Relationship
        {
            //Create
            public static void CreateRelationship(string relationshipType, int lvlOfCommitment)
                => Utils.Create(Table.Relationship, new ParamArgs("@relationshipType", relationshipType), new ParamArgs("@lvlOfCommitment", lvlOfCommitment));
            //Update
            public static void UpdateRelationship(string relationshipType, int newLvlOfCommitment)
                => Utils.Update(Table.Relationship, new ParamArgs("@pk_relationshipType", relationshipType), new ParamArgs("@lvlOfCommitment", newLvlOfCommitment));
            //Read
            public static DataSet ReadRelationship(string relationshipType) => Utils.Read(Table.Relationship, new ParamArgs("@relationshipType", relationshipType));
            //Delete
            public static void DeleteRelationship(string relationshipType) => Utils.Delete(Table.Relationship, new ParamArgs("@relationshipType", relationshipType));
        }

        public static class Interest
        {
            //Create
            public static void CreateInterest(string category, string group)
                => Utils.Create(Table.Interest, new ParamArgs("@interestCategory", category), new ParamArgs("@interestGroup", group));
            //Update
            public static void UpdateInterest(string interestCategory, string newGroup)
                => Utils.Update(Table.Interest, new ParamArgs("@pk_interestCategory", interestCategory), new ParamArgs("@interestGroup", newGroup));
            //Read
            public static DataSet ReadInterest(string interestCategory) => Utils.Read(Table.Interest, new ParamArgs("@interestCategory", interestCategory));
            //Delete
            public static void DeleteInterest(string interestCategory) => Utils.Delete(Table.Interest, new ParamArgs("@interestCategory", interestCategory));
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
                string message = "";
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
                }
                onSqlError?.Invoke(message);
                return null;
            } // error handling here
        }
    }
}