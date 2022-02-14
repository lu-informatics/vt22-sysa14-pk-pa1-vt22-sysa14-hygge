namespace HyggeFinal
{
    partial class Form2
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtCreatingEmail = new System.Windows.Forms.TextBox();
            this.txtCreatingProfilePassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreatingNewProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter your email *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter a password *";
            // 
            // txtCreatingEmail
            // 
            this.txtCreatingEmail.Location = new System.Drawing.Point(181, 112);
            this.txtCreatingEmail.Name = "txtCreatingEmail";
            this.txtCreatingEmail.Size = new System.Drawing.Size(178, 20);
            this.txtCreatingEmail.TabIndex = 2;
            // 
            // txtCreatingProfilePassword
            // 
            this.txtCreatingProfilePassword.Location = new System.Drawing.Point(181, 168);
            this.txtCreatingProfilePassword.Name = "txtCreatingProfilePassword";
            this.txtCreatingProfilePassword.Size = new System.Drawing.Size(178, 20);
            this.txtCreatingProfilePassword.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(462, 42);
            this.label3.TabIndex = 4;
            this.label3.Text = "Creating a profile for Hygge";
            // 
            // btnCreatingNewProfile
            // 
            this.btnCreatingNewProfile.Location = new System.Drawing.Point(389, 168);
            this.btnCreatingNewProfile.Name = "btnCreatingNewProfile";
            this.btnCreatingNewProfile.Size = new System.Drawing.Size(96, 20);
            this.btnCreatingNewProfile.TabIndex = 5;
            this.btnCreatingNewProfile.Text = "Create a profile";
            this.btnCreatingNewProfile.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 450);
            this.Controls.Add(this.btnCreatingNewProfile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCreatingProfilePassword);
            this.Controls.Add(this.txtCreatingEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCreatingEmail;
        private System.Windows.Forms.TextBox txtCreatingProfilePassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreatingNewProfile;
    }
}