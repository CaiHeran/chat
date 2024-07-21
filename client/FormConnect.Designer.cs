namespace Client
{
    partial class FormConnect
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label label_server;
            Label label_port;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnect));
            textBox_host = new TextBox();
            textBox_port = new TextBox();
            button_connect = new Button();
            button_exit = new Button();
            button_mini = new Button();
            label_server = new Label();
            label_port = new Label();
            SuspendLayout();
            // 
            // label_server
            // 
            label_server.AutoSize = true;
            label_server.BackColor = Color.Transparent;
            label_server.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_server.Location = new Point(57, 129);
            label_server.Margin = new Padding(5, 0, 0, 0);
            label_server.Name = "label_server";
            label_server.Size = new Size(86, 31);
            label_server.TabIndex = 0;
            label_server.Text = "服务器";
            // 
            // label_port
            // 
            label_port.AutoSize = true;
            label_port.BackColor = Color.Transparent;
            label_port.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_port.Location = new Point(451, 129);
            label_port.Margin = new Padding(5, 0, 0, 0);
            label_port.Name = "label_port";
            label_port.Size = new Size(62, 31);
            label_port.TabIndex = 2;
            label_port.Text = "端口";
            // 
            // textBox_host
            // 
            textBox_host.Cursor = Cursors.IBeam;
            textBox_host.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_host.Location = new Point(148, 126);
            textBox_host.Margin = new Padding(0, 4, 5, 4);
            textBox_host.Name = "textBox_host";
            textBox_host.Size = new Size(281, 34);
            textBox_host.TabIndex = 1;
            // 
            // textBox_port
            // 
            textBox_port.Cursor = Cursors.IBeam;
            textBox_port.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_port.Location = new Point(517, 126);
            textBox_port.Margin = new Padding(0, 4, 5, 4);
            textBox_port.Name = "textBox_port";
            textBox_port.PlaceholderText = "0~65535";
            textBox_port.Size = new Size(106, 34);
            textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            button_connect.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_connect.Location = new Point(250, 265);
            button_connect.Margin = new Padding(5, 4, 5, 4);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(229, 60);
            button_connect.TabIndex = 4;
            button_connect.Text = "连接";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // button_exit
            // 
            button_exit.Location = new Point(575, 381);
            button_exit.Margin = new Padding(5, 4, 5, 4);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(118, 34);
            button_exit.TabIndex = 5;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // button_mini
            // 
            button_mini.Location = new Point(418, 381);
            button_mini.Margin = new Padding(5, 4, 5, 4);
            button_mini.Name = "button_mini";
            button_mini.Size = new Size(118, 34);
            button_mini.TabIndex = 6;
            button_mini.Text = "最小化";
            button_mini.UseVisualStyleBackColor = true;
            button_mini.Click += button_mini_Click;
            // 
            // FormConnect
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(729, 472);
            ControlBox = false;
            Controls.Add(button_mini);
            Controls.Add(button_exit);
            Controls.Add(button_connect);
            Controls.Add(textBox_port);
            Controls.Add(label_port);
            Controls.Add(textBox_host);
            Controls.Add(label_server);
            Margin = new Padding(5, 4, 5, 4);
            MaximizeBox = false;
            Name = "FormConnect";
            Text = "游戏标题";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_server;
        private TextBox textBox_host;
        private Label label_port;
        private TextBox textBox_port;
        private Button button_connect;
        private Button button_exit;
        private Button button_mini;
    }
}
