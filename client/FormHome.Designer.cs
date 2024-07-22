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
            components = new System.ComponentModel.Container();
            button_CreateRoom = new Button();
            button_JoinRoom = new Button();
            label_ID = new Label();
            label_name = new Label();
            textBox_roomid = new TextBox();
            errorProvider_join = new ErrorProvider(components);
            label_tip = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider_join).BeginInit();
            SuspendLayout();
            // 
            // button_CreateRoom
            // 
            button_CreateRoom.Font = new Font("楷体", 14.25F);
            button_CreateRoom.Location = new Point(153, 75);
            button_CreateRoom.Name = "button_CreateRoom";
            button_CreateRoom.Size = new Size(148, 45);
            button_CreateRoom.TabIndex = 0;
            button_CreateRoom.Text = "创建房间";
            button_CreateRoom.UseVisualStyleBackColor = true;
            button_CreateRoom.Click += button_CreateRoom_Click;
            // 
            // button_JoinRoom
            // 
            button_JoinRoom.Font = new Font("楷体", 14.25F);
            button_JoinRoom.Location = new Point(153, 200);
            button_JoinRoom.Name = "button_JoinRoom";
            button_JoinRoom.Size = new Size(148, 44);
            button_JoinRoom.TabIndex = 1;
            button_JoinRoom.Text = "加入房间";
            button_JoinRoom.UseVisualStyleBackColor = true;
            button_JoinRoom.Click += button_JoinRoom_Click;
            // 
            // label_ID
            // 
            label_ID.AutoSize = true;
            label_ID.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_ID.Location = new Point(37, 14);
            label_ID.Margin = new Padding(5);
            label_ID.Name = "label_ID";
            label_ID.Size = new Size(24, 20);
            label_ID.TabIndex = 3;
            label_ID.Text = "ID";
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_name.Location = new Point(28, 44);
            label_name.Margin = new Padding(5);
            label_name.Name = "label_name";
            label_name.Size = new Size(49, 20);
            label_name.TabIndex = 4;
            label_name.Text = "Name";
            // 
            // textBox_roomid
            // 
            textBox_roomid.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox_roomid.Location = new Point(194, 163);
            textBox_roomid.Margin = new Padding(0, 3, 0, 3);
            textBox_roomid.Name = "textBox_roomid";
            textBox_roomid.PlaceholderText = "房间号";
            textBox_roomid.Size = new Size(66, 28);
            textBox_roomid.TabIndex = 5;
            // 
            // errorProvider_join
            // 
            errorProvider_join.ContainerControl = this;
            // 
            // label_tip
            // 
            label_tip.AutoSize = true;
            label_tip.ForeColor = Color.Red;
            label_tip.Location = new Point(275, 170);
            label_tip.Margin = new Padding(2, 0, 2, 0);
            label_tip.Name = "label_tip";
            label_tip.Size = new Size(43, 17);
            label_tip.TabIndex = 6;
            label_tip.Text = "label1";
            label_tip.Visible = false;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(label_tip);
            Controls.Add(textBox_roomid);
            Controls.Add(label_name);
            Controls.Add(label_ID);
            Controls.Add(button_JoinRoom);
            Controls.Add(button_CreateRoom);
            MaximizeBox = false;
            Name = "FormHome";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Home Menu";
            FormClosing += Form_Closing;
            ((System.ComponentModel.ISupportInitialize)errorProvider_join).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_CreateRoom;
        private Button button_JoinRoom;
        private Label label_ID;
        private Label label_name;
        private TextBox textBox_roomid;
        private ErrorProvider errorProvider_join;
        private Label label_tip;
    }
}