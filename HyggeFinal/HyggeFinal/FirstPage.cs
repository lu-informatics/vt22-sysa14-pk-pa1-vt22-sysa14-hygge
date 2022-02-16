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
    public partial class FirstPage : Form
    {
       
        public static FirstPage instance;
        public FirstPage()
        {
            InitializeComponent();
            instance = this;
          

        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            CreateProfile createProfile = new CreateProfile();
            createProfile.Show();

           // firstPage.Close(); //event, fixa så vi har en Close-metod till alla

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            YourProfile yourProfile = new YourProfile();
            yourProfile.Show();
        }
    }
}
