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
        public CreateProfile() 
           
        {
            InitializeComponent();
            instance = this;
        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
          string email = txtCreateProfileEmail.Text;
          string password = txtCreateProfilePassword.Text;

            


         /*  switch (txtCreateProfileEmail.Text)
            {
                case null;
                case "":
                    break;

            }*/
           
        }
    }
}
