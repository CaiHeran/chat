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
            textBox_host = new TextBox();
            textBox_port = new TextBox();
            button_connect = new Button();
            label_server = new Label();
            label_port = new Label();
            SuspendLayout();
            // 
            // label_server
            // 
            label_server.AutoSize = true;
            label_server.BackColor = Color.Transparent;
            label_server.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_server.Location = new Point(66, 75);
            label_server.Margin = new Padding(3, 0, 0, 0);
            label_server.Name = "label_server";
            label_server.Size = new Size(58, 21);
            label_server.TabIndex = 0;
            label_server.Text = "服务器";
            // 
            // label_port
            // 
            label_port.AutoSize = true;
            label_port.BackColor = Color.Transparent;
            label_port.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_port.Location = new Point(82, 121);
            label_port.Margin = new Padding(3, 0, 0, 0);
            label_port.Name = "label_port";
            label_port.Size = new Size(42, 21);
            label_port.TabIndex = 2;
            label_port.Text = "端口";
            // 
            // textBox_host
            // 
            textBox_host.Cursor = Cursors.IBeam;
            textBox_host.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_host.Location = new Point(124, 73);
            textBox_host.Margin = new Padding(0, 3, 3, 3);
            textBox_host.Name = "textBox_host";
            textBox_host.Size = new Size(282, 25);
            textBox_host.TabIndex = 1;
            // 
            // textBox_port
            // 
            textBox_port.Cursor = Cursors.IBeam;
            textBox_port.Font = new Font("Microsoft YaHei UI", 10.5F);
            textBox_port.Location = new Point(124, 119);
            textBox_port.Margin = new Padding(0, 3, 3, 3);
            textBox_port.Name = "textBox_port";
            textBox_port.PlaceholderText = "0~65535";
            textBox_port.Size = new Size(69, 25);
            textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            button_connect.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_connect.Location = new Point(157, 194);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(146, 41);
            button_connect.TabIndex = 4;
            button_connect.Text = "连接";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // FormConnect
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(464, 321);
            Controls.Add(button_connect);
            Controls.Add(textBox_port);
            Controls.Add(label_port);
            Controls.Add(textBox_host);
            Controls.Add(label_server);
            MaximizeBox = false;
            Name = "FormConnect";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Connect";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_server;
        private TextBox textBox_host;
        private Label label_port;
        private TextBox textBox_port;
        private Button button_connect;
    }
}
