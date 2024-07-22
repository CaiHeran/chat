namespace Client
{
    partial class FormLogin
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
            Label label_yourname;
            button_login = new Button();
            textBox_name = new TextBox();
            label_yourname = new Label();
            SuspendLayout();
            // 
            // label_yourname
            // 
            label_yourname.AutoSize = true;
            label_yourname.Location = new Point(99, 99);
            label_yourname.Margin = new Padding(0);
            label_yourname.Name = "label_yourname";
            label_yourname.Size = new Size(122, 21);
            label_yourname.TabIndex = 2;
            label_yourname.Text = "请设置用户名：";
            // 
            // button_login
            // 
            button_login.Location = new Point(157, 193);
            button_login.Margin = new Padding(4);
            button_login.Name = "button_login";
            button_login.Size = new Size(145, 40);
            button_login.TabIndex = 0;
            button_login.Text = "登录";
            button_login.UseVisualStyleBackColor = true;
            button_login.Click += button_login_Click;
            // 
            // textBox_name
            // 
            textBox_name.Location = new Point(221, 96);
            textBox_name.Margin = new Padding(0, 3, 0, 3);
            textBox_name.Name = "textBox_name";
            textBox_name.Size = new Size(111, 29);
            textBox_name.TabIndex = 3;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(textBox_name);
            Controls.Add(label_yourname);
            Controls.Add(button_login);
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            this.FormClosing += this.Form_Closing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_login;
        private Label label_yourname;
        private TextBox textBox_name;
    }
}