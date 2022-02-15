using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyggeFinal
{
    public partial class Form2 : Form
    {
        DataAccessLayer dal = new DataAccessLayer();
        public static Form2 instance;
        public Form2()
        {
            InitializeComponent();
            instance= this;
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.instance.tb1.Text = "set by form2";
        }

        private void btnCreatingNewProfile1(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            SqlDataReader reader = dal.GetPersons();
            Console.WriteLine(reader);
        }
    }
}
