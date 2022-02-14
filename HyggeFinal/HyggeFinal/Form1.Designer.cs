namespace HyggeFinal
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreateProfile = new System.Windows.Forms.Button();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.btnForgotPassword = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserNameLogIn = new System.Windows.Forms.TextBox();
            this.txtPasswordLogIn = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 69);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hygge";
            // 
            // btnCreateProfile
            // 
            this.btnCreateProfile.BackColor = System.Drawing.Color.White;
            this.btnCreateProfile.Location = new System.Drawing.Point(107, 291);
            this.btnCreateProfile.Name = "btnCreateProfile";
            this.btnCreateProfile.Size = new System.Drawing.Size(121, 25);
            this.btnCreateProfile.TabIndex = 1;
            this.btnCreateProfile.Text = "Create a profile";
            this.btnCreateProfile.UseVisualStyleBackColor = false;
            this.btnCreateProfile.Click += new System.EventHandler(this.btnCreateProfile_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLogIn.Location = new System.Drawing.Point(348, 206);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(93, 23);
            this.btnLogIn.TabIndex = 2;
            this.btnLogIn.Text = "log in";
            this.btnLogIn.UseVisualStyleBackColor = false;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // btnForgotPassword
            // 
            this.btnForgotPassword.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnForgotPassword.Location = new System.Drawing.Point(252, 291);
            this.btnForgotPassword.Name = "btnForgotPassword";
            this.btnForgotPassword.Size = new System.Drawing.Size(121, 25);
            this.btnForgotPassword.TabIndex = 3;
            this.btnForgotPassword.Text = "Forgot password?";
            this.btnForgotPassword.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User name : ";
            // 
            // txtUserNameLogIn
            // 
            this.txtUserNameLogIn.Location = new System.Drawing.Point(136, 140);
            this.txtUserNameLogIn.Name = "txtUserNameLogIn";
            this.txtUserNameLogIn.Size = new System.Drawing.Size(194, 20);
            this.txtUserNameLogIn.TabIndex = 6;
            // 
            // txtPasswordLogIn
            // 
            this.txtPasswordLogIn.Location = new System.Drawing.Point(136, 208);
            this.txtPasswordLogIn.Name = "txtPasswordLogIn";
            this.txtPasswordLogIn.Size = new System.Drawing.Size(194, 20);
            this.txtPasswordLogIn.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 454);
            this.Controls.Add(this.txtPasswordLogIn);
            this.Controls.Add(this.txtUserNameLogIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnForgotPassword);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.btnCreateProfile);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreateProfile;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Button btnForgotPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserNameLogIn;
        private System.Windows.Forms.TextBox txtPasswordLogIn;
    }
}

