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
    public partial class Form1 : Form
    {

       public static Form1 instance;
        public TextBox tb1;
        public Form1() 

        {
            InitializeComponent();
            instance = this;
            tb1 = txtUserNameLogIn;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // This method is called when the windows form opens
            var contributor = "vido";

            //label1.Text = DataAccessLayer.Test();
        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {

        }
    }
    }


// Hej från My

//Hej fran Karinnnn

//Bajstolle


//Test2

//rebecca
