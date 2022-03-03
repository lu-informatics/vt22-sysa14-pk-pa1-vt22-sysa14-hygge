using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HyggeFinal
{
    public partial class AdminAccess : Form
    { //this form will be used for the demonstration of CURD
        private TextBox[] inputFields;
        private Label[] inputLabels;
        private bool tableIsReady;
        public AdminAccess()
        {
            InitializeComponent();
        }

        private void AdminAccess_Load(object sender, EventArgs e)
        {
            //some configuration for visual appeal
            dgvTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTable.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dgvTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTable.EnableHeadersVisualStyles = false;
            dgvTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTable.BackgroundColor = dgvTable.DefaultCellStyle.BackColor;

            inputFields = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9 };
            inputLabels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9 };

        }
        private void DisableInputFields()
        {
            textBoxSearchIdentifier.Text = "";
            foreach (TextBox tbx in inputFields) { tbx.Text = ""; tbx.Enabled = false; }
            foreach (Label lbl in inputLabels) lbl.Text = "";
        }

        private void UpdateDataSource(DataAccessLayer.Table chosenTable)
        {
            tableIsReady = false; //tableIsReady is set to false to prevent premature trigger of selectionChanged-event. It is set to true in cellclick- and keydown-events below 
            DataSet ds = DataAccessLayer.Utils.ViewAll(chosenTable);
            dgvTable.DataSource = ds.Tables[0]; //datasource change
            lblTable.Text = chosenTable.ToString();
            lblSearch.Text = $"Search in {chosenTable}";
            for (int i = 0; i < dgvTable.Columns.Count; i++)
            {
                inputFields[i].Enabled = true;
                inputLabels[i].Text = ds.Tables[0].Columns[i].ToString();
            }
            lblIdentifier.Text = $"{inputLabels[0].Text}";
        }
        //Clicking a Menu item changes the datasource used for the DataGridView dgvTable
        private void ChangeDataSource(DataAccessLayer.Table chosenTable)
        {
            DisableInputFields();
            UpdateDataSource(chosenTable);
        } //Below are calls to ChangeDataSource for each menu item 
        private void LoginsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Logins);
        private void PersonsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Person);
        private void RelationshipsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Relationship);
        private void IndustriesToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Industry);
        private void EducationsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Education);
        private void InterestsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Interest);
        private void EducationIndustryToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.EducationIndustry);
        private void PersonInterestConnectionsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.PersonInterest);

        private void GoToRowOf(string primaryKey)
        {
            foreach (DataGridViewRow row in dgvTable.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(primaryKey))
                {
                    row.Selected = true;
                    break;
                }
            }
        }
        private void GoToRowOf(string primaryKey1, string primaryKey2)
        {
            foreach (DataGridViewRow row in dgvTable.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(primaryKey1) && row.Cells[1].Value.ToString().Equals(primaryKey2))
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)// searches for specified Identifier using the currently viewed table
        {
            if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest))
            {
                DataAccessLayer.Utils.Read(FindTable(),
                    new ParamArgs($"@{inputLabels[0].Text}", TrueDataType(inputFields[0].Text)),
                    new ParamArgs($"@{inputLabels[1].Text}", TrueDataType(inputFields[1].Text)));
                string primaryKey1 = inputLabels[0].Text;
                string primaryKey2 = inputLabels[1].Text;
                FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{inputLabels[0].Text}", primaryKey1), new ParamArgs($"@{inputLabels[1].Text}", primaryKey2)));
                GoToRowOf(primaryKey1, primaryKey2);

            }
            else
            {
                string primaryKey = textBoxSearchIdentifier.Text;
                FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{lblIdentifier.Text}", primaryKey)));
                GoToRowOf(primaryKey);
            }

            textBoxSearchIdentifier.Clear();
        }
        private void FillInputFields(DataSet ds)
        {
            try
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    inputFields[i].Text = ds.Tables[0].Rows[0][i].ToString(); //fill the inputfield with data from the search
                    inputLabels[i].Text = ds.Tables[0].Columns[i].ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void SaveButton_Click(object sender, EventArgs e) //Update values for entry with a primary key matching that of inputFields[0]
        {
            if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest))
            {
                for (int i = 0; i < inputFields.Length; i++)
                {
                    if (inputFields[i].Enabled)
                    {
                        DataAccessLayer.Utils.Update(FindTable(),
                            new ParamArgs($"@pk_{dgvTable.Columns[0].Name}", TrueDataType(dgvTable.SelectedRows[0].Cells[0].Value.ToString())),
                            new ParamArgs($"@pk_{dgvTable.Columns[1].Name }", TrueDataType(dgvTable.SelectedRows[0].Cells[1].Value.ToString())),
                            new ParamArgs($"@{inputLabels[i].Text}", TrueDataType(inputFields[i].Text)));
                    }
                }
                string primaryKey1 = inputFields[0].Text;
                string primaryKey2 = inputFields[1].Text;
                UpdateDataSource(FindTable());
                GoToRowOf(primaryKey1, primaryKey2);
            }
            else
            {
                for (int i = 0; i < inputFields.Length; i++)
                {
                    if (inputFields[i].Enabled)
                        try
                        {
                            DataAccessLayer.Utils.Update(FindTable(),
                                new ParamArgs($"@pk_{dgvTable.Columns[0].Name}", TrueDataType(dgvTable.SelectedRows[0].Cells[0].Value.ToString())),
                                new ParamArgs($"@{inputLabels[i].Text}", TrueDataType(inputFields[i].Text)));
                        }
                        catch (Exception ex)
                        {
                            //Index out of range: when you havent selected a table
                        }
                }
                string key = inputFields[0].Text;
                UpdateDataSource(FindTable());
                GoToRowOf(key);
            }


        }

        private void BtnCreate_Click(object sender, EventArgs e) //Create a new row with user input provided in inputFields
        {
            if (FindTable() == DataAccessLayer.Table.Person)
            {
                DataAccessLayer.Person.CreatePerson(
                    inputFields[0].Text,
                    inputFields[1].Text,
                    Int32.Parse(inputFields[2].Text), //error handling required here 
                    inputFields[3].Text,
                    inputFields[4].Text,
                    inputFields[5].Text,
                    inputFields[6].Text,
                    inputFields[7].Text,
                    inputFields[8].Text);
            }
            else DataAccessLayer.Utils.Create(FindTable(),
                new ParamArgs($"@{inputLabels[0].Text}", TrueDataType(inputFields[0].Text)),
                new ParamArgs($"@{inputLabels[1].Text}", TrueDataType(inputFields[1].Text)));
            string primaryKey = inputFields[0].Text;
            UpdateDataSource(FindTable()); //updates the table display
            GoToRowOf(primaryKey);

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
            if (FindTable().Equals(DataAccessLayer.Table.EducationIndustry) || FindTable().Equals(DataAccessLayer.Table.PersonInterest))
            {
                DataAccessLayer.Utils.Delete(FindTable(),
                    new ParamArgs($"@{dgvTable.Columns[0].Name}", TrueDataType(inputFields[0].Text)),
                    new ParamArgs($"@{dgvTable.Columns[1].Name}", TrueDataType(inputFields[1].Text)));
                ChangeDataSource(FindTable());
            }
            else
            {
                DataAccessLayer.Utils.Delete(FindTable(), new ParamArgs($"@{dgvTable.Columns[0].Name}", TrueDataType(inputFields[0].Text)));
                ChangeDataSource(FindTable());
            }
        }
        private object TrueDataType(string str)
        {try
            { //converts input string into either sql ready string with '' or into integer
                if (str.All(Char.IsDigit)) return long.Parse(str);
                else return str;
            }catch(Exception ex) { return null; }//leaving input strings empty when trying to create new
        }
        
        private void DgvTable_SelectionChanged(object sender, EventArgs e)
        { // Fills user input fields with data respective to table selection
            if (sender is DataGridView dgv && dgv.SelectedRows.Count > 0 && tableIsReady)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    string colname = row.Cells[0].OwningColumn.Name.ToString();
                    FillInputFields(DataAccessLayer.Utils.Read(FindTable(), new ParamArgs($"@{colname}", TrueDataType(row.Cells[0].Value.ToString()))));
                }
            }
        }
        private void DgvTable_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        { // asserts that the arrowkeys should be recognised for the dgvTable_KeyDown event
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                    e.IsInputKey = true;
                    break;
            }
        }
        private void DgvTable_KeyDown(object sender, KeyEventArgs e) => AssertTableReadiness();
        private void DgvTable_CellClick(object sender, DataGridViewCellEventArgs e) => AssertTableReadiness();
        private void AssertTableReadiness()
        {
            tableIsReady = true; DgvTable_SelectionChanged(dgvTable, EventArgs.Empty);
        }

        private void BtnClearSelection_Click(object sender, EventArgs e)
        {
            ChangeDataSource(FindTable());
        }

        private void DisplayErrorMessage(Exception ex)
        {
            string message;
            if (ex is SqlException)
            {
                SqlException se = ex as SqlException;
                switch (se.Number)
                {
                    case -2: //connection timed out
                        break;
                    case 208: //invalid object name (no table selected when searching/clearing selection)
                        break;
                    case 8178: //expected a parameter that was not supplied, or an int was too long
                        break;
                    case 245: //conversion error from string to int
                        break;
                    case 547: //foreign key violation: no such foreign key
                        break;
                    case 2628: //string too long
                        break;
                    case 2627: //primary key violation
                        break;
                }
            }
            else if (ex is IndexOutOfRangeException) //likely a search for a value that doesnt exist, or a search thru the normal bar when in many2many
            {

            }
        }
    }
}
