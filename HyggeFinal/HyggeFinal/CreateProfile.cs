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
    public partial class CreateProfile : Form
    {
        public static CreateProfile instance;
        public TextBox tb1;
        public TextBox tb2;

        public CreateProfile()

        {
            InitializeComponent();
            instance = this;
            tb1 = txtCreateProfileEmail;
            tb2 = txtCreateProfilePassword;

        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            YourProfile yourProfile = new YourProfile();
            yourProfile.Show();


            //Errorhantering!! 

            /* switch (txtCreateProfileEmail.Text)
             {
                 case null;
                 case "":
                     break;

             }

         } */
        }
    }
}

