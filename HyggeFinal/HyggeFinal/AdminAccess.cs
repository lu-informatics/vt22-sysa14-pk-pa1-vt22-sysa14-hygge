using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyggeFinal
{
    public partial class AdminAccess : Form
    {
        private delegate void updateValue(DataAccessLayer.Table table, ParamArgs primaryKey, ParamArgs changedValue);
        private TextBox[] inputFields;
        private Label[] inputLabels;
        public AdminAccess()
        {
            InitializeComponent();
        }
        private void AdminAccess_Load(object sender, EventArgs e)
        {
            inputFields = new TextBox[] { textBox1,textBox2,textBox3,textBox4,textBox5, textBox6, textBox7, textBox8, textBox9 };
            inputLabels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9 };
            //this form will be used for the demonstration of CURD
        }
        private void DisableAndClearAllInputFields() {
            textBoxSearchIdentifier.Text = "";
            foreach (TextBox tbx in inputFields) { tbx.Text=""; tbx.Enabled = false; }
            foreach (Label lbl in inputLabels) lbl.Text = "";
        }

        //Clicking a Menu item changes the datasource used for the DataGridView dgvTable
        private void ChangeDataSource(DataAccessLayer.Table chosenTable) {
            DisableAndClearAllInputFields();
            DataSet ds = DataAccessLayer.Utils.ViewAll(chosenTable);
            dgvTable.DataSource = ds.Tables[0]; //datasource change
            lblTable.Text = chosenTable.ToString();
            lblSearch.Text = $"Search in {chosenTable}";
                for (int i = 0; i < dgvTable.Columns.Count; i++) {
                inputFields[i].Enabled = true;
                inputLabels[i].Text = ds.Tables[0].Columns[i].ToString();
            }
            }
        private void LoginsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Logins);
        private void PersonsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Person);
        private void RelationshipsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Relationship);
        private void IndustriesToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Industry);
        private void EducationsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Education);
        private void InterestsToolStripMenuItem_Click(object sender, EventArgs e) => ChangeDataSource(DataAccessLayer.Table.Interest);
        private void SearchButton_Click(object sender, EventArgs e)
        {
            switch (lblTable.Text) {
                case "Industry":
                    FillInputFields(DataAccessLayer.Industry.ReadIndustry(textBoxSearchIdentifier.Text));
                    break;
                case "Interest":
                    FillInputFields(DataAccessLayer.Interest.ReadInterest(textBoxSearchIdentifier.Text));
                    break;
                case "Logins":
                    FillInputFields(DataAccessLayer.Login.ReadLogin(textBoxSearchIdentifier.Text));
                    break;
                case "Person":
                    FillInputFields(DataAccessLayer.Person.ReadPerson(textBoxSearchIdentifier.Text));
                    break;
                case "Relationship":
                    FillInputFields(DataAccessLayer.Relationship.ReadRelationship(textBoxSearchIdentifier.Text));
                    break;
                case "Education":
                    FillInputFields(DataAccessLayer.Education.ReadEducation(textBoxSearchIdentifier.Text));
                    break;
                default:
                    //no table selected or bug
                    break;
            }
        }
        private void FillInputFields(DataSet ds) {
            for (int i = 0; i < ds.Tables[0].Columns.Count;i++) {
                //inputFields[i].Enabled = true; //enable the inputfield
                inputFields[i].Text = ds.Tables[0].Rows[0][i].ToString(); //fill the inputfield with data from the search
                inputLabels[i].Text = ds.Tables[0].Columns[i].ToString();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e) { // This method calls on the DAL to update each column of a selected row
            for (int i = 0; i < inputFields.Length; i++) {
                if (inputFields[i].Enabled) {
                    DataAccessLayer.Utils.Update(FindTable(),
                        new ParamArgs($"@{inputLabels[0].Text}",inputFields[0].Text),
                        new ParamArgs($"@{inputLabels[i].Text}",inputFields[i].Text));
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (FindTable() == DataAccessLayer.Table.Person) {
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
                new ParamArgs($"@{inputLabels[0].Text}",inputFields[0].Text),
                new ParamArgs($"@{inputLabels[1].Text}",inputFields[1].Text));
        }
        private DataAccessLayer.Table FindTable() {
            switch (lblTable.Text) {
                case "Industry": return DataAccessLayer.Table.Industry;
                case "Person": return DataAccessLayer.Table.Person;
                case "Relationship": return DataAccessLayer.Table.Relationship;
                case "Interest": return DataAccessLayer.Table.Interest;
                case "Education": return DataAccessLayer.Table.Education;
                case "Logins": return DataAccessLayer.Table.Logins;
                default: return DataAccessLayer.Table.Error; //placeholder solution for non-nullable
            }

        }
    }
}
