namespace Client
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
            Label label_yourID;
            Label label2;
            button_login = new Button();
            textBox_name = new TextBox();
            label_yourID = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // label_yourID
            // 
            label_yourID.AutoSize = true;
            label_yourID.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_yourID.Location = new Point(118, 69);
            label_yourID.Margin = new Padding(4, 0, 4, 0);
            label_yourID.Name = "label_yourID";
            label_yourID.Size = new Size(91, 21);
            label_yourID.TabIndex = 1;
            label_yourID.Text = "你的ID是：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(87, 118);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(122, 21);
            label2.TabIndex = 2;
            label2.Text = "请设置用户名：";
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
            textBox_name.Location = new Point(207, 115);
            textBox_name.Name = "textBox_name";
            textBox_name.Size = new Size(111, 29);
            textBox_name.TabIndex = 3;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(textBox_name);
            Controls.Add(label2);
            Controls.Add(label_yourID);
            Controls.Add(button_login);
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = new Padding(4);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_login;
        private Label label_yourID;
        private Label label2;
        private TextBox textBox_name;
    }
}