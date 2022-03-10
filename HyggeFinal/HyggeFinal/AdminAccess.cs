using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HyggeFinal
{
    public partial class AdminAccess : Form
    { //this form will be used for the demonstration of CRUD
        private TextBox[] inputFields; // array of user-input TextBoxes to be iterated through for enabling/disabling in bulk
        private Label[] inputLabels; // same purpose as inputFields
        private bool tableIsReady; // bool used to prevent premature inputField loading,
        public AdminAccess()
        {
            InitializeComponent();
        }

        private void AdminAccess_Load(object sender, EventArgs e) //code that runs when the form first loads
        {
            DataAccessLayer.onSqlError += new DataAccessLayer.errorHandler(DisplayErrorMessage);

            //some configuration for visual appeal
            dgvTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //select fullrows and not individual cells in the DataGridView
            dgvTable.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray; //header row background color
            dgvTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // dgv header row text color
            dgvTable.EnableHeadersVisualStyles = false; // if this is not set to false, visual styles will override configurations made here
            dgvTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //stretch columns across dgv
            dgvTable.BackgroundColor = dgvTable.DefaultCellStyle.BackColor; //dgv background color

            inputFields = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9 }; //initialize arrays with form items
            inputLabels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9 };
        }
        private void DisableInputFields() //clears all user-input fields and labels and disables them (prevents editing contents)
        {
            textBoxSearchIdentifier.Text = "";
            foreach (TextBox tbx in inputFields) { tbx.Text = ""; tbx.Enabled = false; } //inputfields are cleared and disabled
            foreach (Label lbl in inputLabels) lbl.Text = ""; //labels are cleared
        }

        private void UpdateDataSource(DataAccessLayer.Table chosenTable) //updates the currently viewed dgv table. This is called after changes are made to an entry.
        {
            tableIsReady = false; //tableIsReady is set to false to prevent premature trigger of selectionChanged-event. It is set to true in cellclick- and keydown-events below 
            DataSet ds = DataAccessLayer.Utils.Read(chosenTable);// a DAL method is called and returns a DataSet
            if (ds != null)
            { //if a DataSet was returned...
                dgvTable.DataSource = ds.Tables[0]; //... change the datasource for DataGridView to the fetched DataSet
                lblTable.Text = chosenTable.ToString();
                lblSearch.Text = $"Search in {chosenTable}";
                for (int i = 0; i < dgvTable.Columns.Count; i++) //enable an amount of inputfields from top to bottom based on the column count of the fetched dataset.
                {
                    inputFields[i].Enabled = true; //enable the input field
                    inputLabels[i].Text = ds.Tables[0].Columns[i].ToString(); //change the label next to the input field to the name of the corresponding column
                }
                lblIdentifier.Text = $"{inputLabels[0].Text}"; //change the the label next to the search bar to the appropriate primary key
            }
        }

        //Clicking a Menu item changes the datasource used for the DataGridView dgvTable
        private void ChangeDataSource(DataAccessLayer.Table chosenTable)
        {
            DisableInputFields(); //disables all inputfields...
            UpdateDataSource(chosenTable); //... and reenables an appropriate amount in UpdateDataSource whilst updating the DataGridView

            //search bar should only be enabled for tables with a primary key consisting of a single value; many-to-many entries like EducationIndustry are searched for in the inputFields.
            textBoxSearchIdentifier.Enabled = !chosenTable.Equals(DataAccessLayer.Table.EducationIndustry) && !chosenTable.Equals(DataAccessLayer.Table.PersonInterest);
        } //Below are calls to ChangeDataSource for each menu item 
        private void LoginsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Logins);
        private void PersonsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Person);
        private void RelationshipsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Relationship);
        private void IndustriesToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Industry);
        private void EducationsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Education);
        private void InterestsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Interest);
        private void EducationIndustryToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.EducationIndustry);
        private void PersonInterestConnectionsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.PersonInterest);

        private void SelectRowOf(string primaryKey) // highlights a row in the DataGridView based on a given primary key
        {
            foreach (DataGridViewRow row in dgvTable.Rows) //for each row in the datagridview...
            {
                if (row.Cells[0].Value.ToString().Equals(primaryKey)) //... if the row's first cell matches the primarykey... 
                {
                    row.Selected = true; // ... select the row. (this deselects any prior selection since MultiSelect is disabled in the dgv properties)
                    break;//break the loop since the desired row has been found
                }
            }
        }
        private void SelectRowOf(string primaryKey1, string primaryKey2) //many-to-many variant overload
        {
            foreach (DataGridViewRow row in dgvTable.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(primaryKey1) && row.Cells[1].Value.ToString().Equals(primaryKey2)) //two values are checked for a primary key match instead of one.
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)// searches for specified Identifier using the currently viewed table
        {
            if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest)) //if in a many-to-many table...
            {//... perform the search as follows:
                DataAccessLayer.Utils.Read(FindTable(), //make a DAL call using the two primary key values provided
                    new ParamArgs($"@{inputLabels[0].Text}", TrueDataType(inputFields[0].Text)),
                    new ParamArgs($"@{inputLabels[1].Text}", TrueDataType(inputFields[1].Text)));
                string primaryKey1 = inputFields[0].Text; //values are saved before inputfields clear
                string primaryKey2 = inputFields[1].Text; //
                FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{inputLabels[0].Text}", primaryKey1), new ParamArgs($"@{inputLabels[1].Text}", primaryKey2)));
                SelectRowOf(primaryKey1, primaryKey2);
            }
            else // if it's not a many-to-many table...
            {
                string primaryKey = textBoxSearchIdentifier.Text; //... use the provided input in the dedicated search bar for the primary key
                FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{lblIdentifier.Text}", primaryKey)));
                SelectRowOf(primaryKey);
            }

            textBoxSearchIdentifier.Clear();
        }
        private void FillInputFields(DataSet ds) // takes in a DataSet and uses its values to fill the input fields.
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    inputFields[i].Text = ds.Tables[0].Rows[0][i].ToString(); //fill the inputfield with data from the search
                    inputLabels[i].Text = ds.Tables[0].Columns[i].ToString(); //change names of labels to appropriate column names
                }
            }
            else DisplayErrorMessage("Error: No values for for input fields.");//nothing found via DisplayErrorMessage();
        }
        private void SaveButton_Click(object sender, EventArgs e) //Update values for entry with a primary key matching that of inputFields[0]
        {
            if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest)) //if it is a many-to-many table...
            {
                for (int i = 0; i < inputFields.Length; i++)
                {
                    if (inputFields[i].Enabled)
                    {
                        DataAccessLayer.Utils.Update(FindTable(), //update the value of each column if both primary key cells match
                            new ParamArgs($"@{inputLabels[i].Text}", TrueDataType(inputFields[i].Text)),
                            new ParamArgs($"@pk_{dgvTable.Columns[0].Name}", TrueDataType(dgvTable.SelectedRows[0].Cells[0].Value.ToString())),
                            new ParamArgs($"@pk_{dgvTable.Columns[1].Name }", TrueDataType(dgvTable.SelectedRows[0].Cells[1].Value.ToString()))
                            );
                    }
                }
                string primaryKey1 = inputFields[0].Text;
                string primaryKey2 = inputFields[1].Text;
                UpdateDataSource(FindTable());
                SelectRowOf(primaryKey1, primaryKey2);
            }
            else //if it is a single-cell primary key
            {
                for (int i = 0; i < inputFields.Length; i++)
                {
                    if (inputFields[i].Enabled)
                        try
                        {
                            DataAccessLayer.Utils.Update(FindTable(), //calls on DAL method to update entry in SQL database
                                new ParamArgs($"@{inputLabels[i].Text}", TrueDataType(inputFields[i].Text)),
                                new ParamArgs($"@pk_{dgvTable.Columns[0].Name}", TrueDataType(dgvTable.SelectedRows[0].Cells[0].Value.ToString()))
                                );
                        }
                        catch (Exception)
                        {
                            DisplayErrorMessage("Error: Please fill in all fields correctly.");
                            //Index out of range: when you havent selected a table or try to save changes to a person with no fields filled
                            break;
                        }
                }
                string key = inputFields[0].Text;
                UpdateDataSource(FindTable());
                SelectRowOf(key);
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e) //Create a new row with user input provided in inputFields
        {
            if (FindTable().Equals(DataAccessLayer.Table.Person) && IsFilledOut()) //if the Person table is being viewed and all input fields are filled out.
                    DataAccessLayer.Person.CreatePerson(
                        inputFields[0].Text,
                        inputFields[1].Text,
                        int.Parse(inputFields[2].Text), //error handling required here 
                        inputFields[3].Text,
                        inputFields[4].Text,
                        inputFields[5].Text,
                        inputFields[6].Text,
                        inputFields[7].Text,
                        inputFields[8].Text);
            else DataAccessLayer.Utils.Create(FindTable(),
                new ParamArgs($"@{inputLabels[0].Text}", TrueDataType(inputFields[0].Text)),
                new ParamArgs($"@{inputLabels[1].Text}", TrueDataType(inputFields[1].Text)));
            string primaryKey = inputFields[0].Text;
            UpdateDataSource(FindTable()); //updates the table display
            SelectRowOf(primaryKey);
        }
        private DataAccessLayer.Table FindTable()
        {
            switch (lblTable.Text)
            {
                case "Industry": return DataAccessLayer.Table.Industry;
                case "Person": return DataAccessLayer.Table.Person;
                case "Relationship": return DataAccessLayer.Table.Relationship;
                case "Interest": return DataAccessLayer.Table.Interest;
                case "Education": return DataAccessLayer.Table.Education;
                case "Logins": return DataAccessLayer.Table.Logins;
                case "EducationIndustry": return DataAccessLayer.Table.EducationIndustry;
                default: return DataAccessLayer.Table.Error; //placeholder solution for non-nullable
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e) // Delete a Row using primary key in inputFields[0]
        {
            if (dgvTable != null && dgvTable.Columns.Count > 0) //if there is a dgv that is populated with at least column headers; has a table been selected?
            {
                if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest)) //if it is a many-to-many entry...
                {//... do as follows:
                    DataAccessLayer.Utils.Delete(FindTable(),
                        new ParamArgs($"@{dgvTable.Columns[0].Name}", TrueDataType(inputFields[0].Text)),
                        new ParamArgs($"@{dgvTable.Columns[1].Name}", TrueDataType(inputFields[1].Text)));
                    ChangeDataSource(FindTable());
                }
                else //the primary key is comprised of a single column
                {
                    DataAccessLayer.Utils.Delete(FindTable(), new ParamArgs($"@{dgvTable.Columns[0].Name}", TrueDataType(inputFields[0].Text)));
                    ChangeDataSource(FindTable());
                }
            }
            else DisplayErrorMessage("Error: No table selected. Select a table in the 'View' menu and try again.");
        }
        private object TrueDataType(string str) //simple method for determining the proper datatype of a value input in the input fields
        {
            try
            { //converts input string into either sql ready string with '' or into integer
                if (str.All(char.IsDigit)) return long.Parse(str);
                else return str;
            }
            catch (Exception) { return null; }//leaving input strings empty when trying to create new. error feedback is handled by delegate cast, so no need to handle it further here.
        }

        private void DgvTable_SelectionChanged(object sender, EventArgs e) // Triggers whenever the row selection in the DataGridView changes
        { // Fills user input fields with data respective to table selection
            if (sender is DataGridView dgv && dgv.SelectedRows.Count > 0 && tableIsReady) //tableIsReady makes sure that the table wasn't just recently loaded
            {
                DataGridViewRow row = dgv.SelectedRows[0]; //store the selected row in a DataGridViewRow variable
                if (row != null)
                {
                    string colname = row.Cells[0].OwningColumn.Name.ToString(); //get the name of the primary key column
                    //fill input fields with new selection:
                    FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{colname}", TrueDataType(row.Cells[0].Value.ToString()))));
                }
            }
        }
        private void DgvTable_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        { // asserts that the arrowkeys should be recognised for the dgvTable_KeyDown event. This method is called right before the KeyDown event.
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                    e.IsInputKey = true;
                    break;
            }
        }
        private void DgvTable_KeyDown(object sender, KeyEventArgs e) => AssertTableReadiness(); //call AssertTableReadiness() whenever a key is first pressed
        private void DgvTable_CellClick(object sender, DataGridViewCellEventArgs e) => AssertTableReadiness(); //call AssertTableReadiness() whenever a dgv cell is clicked
        private void AssertTableReadiness() //sets tableIsReady = true and trigger DgvTable_SelectionChanged event
        {
            tableIsReady = true; DgvTable_SelectionChanged(dgvTable, EventArgs.Empty);
        }
        private bool IsFilledOut() //returns false if any input fields are empty, otherwise returns true.
        {
            foreach (TextBox inputField in inputFields)
            {
                if (inputField.Text.Equals("")) return false;
            }
            return true;
        }

        private void BtnClearSelection_Click(object sender, EventArgs e) //redraws the dgv table by 'changing' datasource to current table which clears the selection as a result.
        {
            ChangeDataSource(FindTable());
        }

        private void DisplayErrorMessage(string message) //displays a messagebox detailing the error that has occurred.
        {
            Console.WriteLine(message);
            MessageBox.Show(message, "Error", MessageBoxButtons.OK);
        }

    }
}
