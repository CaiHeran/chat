namespace Client
{
    partial class FormHome
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
            Label label_yourname;
            button_login = new Button();
            textBox_name = new TextBox();
            button_back = new Button();
            button_exit = new Button();
            textBox_id = new TextBox();
            label_yourID = new Label();
            label_yourname = new Label();
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
            // label_yourname
            // 
            label_yourname.AutoSize = true;
            label_yourname.Location = new Point(87, 118);
            label_yourname.Margin = new Padding(4, 0, 4, 0);
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
            textBox_name.Location = new Point(207, 115);
            textBox_name.Name = "textBox_name";
            textBox_name.Size = new Size(111, 29);
            textBox_name.TabIndex = 3;
            // 
            // button_back
            // 
            button_back.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_back.Location = new Point(353, 277);
            button_back.Name = "button_back";
            button_back.Size = new Size(99, 32);
            button_back.TabIndex = 4;
            button_back.Text = "返回上一步";
            button_back.UseVisualStyleBackColor = true;
            button_back.Click += button_back_Click;
            // 
            // button_exit
            // 
            button_exit.Font = new Font("微软雅黑", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_exit.Location = new Point(377, 12);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(75, 23);
            button_exit.TabIndex = 5;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // textBox_id
            // 
            textBox_id.Location = new Point(207, 66);
            textBox_id.Name = "textBox_id";
            textBox_id.Size = new Size(111, 29);
            textBox_id.TabIndex = 6;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(textBox_id);
            Controls.Add(button_exit);
            Controls.Add(button_back);
            Controls.Add(textBox_name);
            Controls.Add(label_yourname);
            Controls.Add(label_yourID);
            Controls.Add(button_login);
            Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = new Padding(4);
            Name = "FormHome";
            Text = "游戏标题";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_login;
        private Label label_yourID;
        private Label label_yourname;
        private TextBox textBox_name;
        private Button button_back;
        private Button button_exit;
        private TextBox textBox_id;
    }
}