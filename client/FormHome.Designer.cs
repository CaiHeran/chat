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
            button_exit = new Button();
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
            button_CreateRoom.Location = new Point(240, 91);
            button_CreateRoom.Margin = new Padding(5, 4, 5, 4);
            button_CreateRoom.Name = "button_CreateRoom";
            button_CreateRoom.Size = new Size(233, 66);
            button_CreateRoom.TabIndex = 0;
            button_CreateRoom.Text = "创建房间";
            button_CreateRoom.UseVisualStyleBackColor = true;
            button_CreateRoom.Click += button_CreateRoom_Click;
            // 
            // button_JoinRoom
            // 
            button_JoinRoom.Font = new Font("楷体", 14.25F);
            button_JoinRoom.Location = new Point(240, 200);
            button_JoinRoom.Margin = new Padding(5, 4, 5, 4);
            button_JoinRoom.Name = "button_JoinRoom";
            button_JoinRoom.Size = new Size(233, 65);
            button_JoinRoom.TabIndex = 1;
            button_JoinRoom.Text = "加入房间";
            button_JoinRoom.UseVisualStyleBackColor = true;
            button_JoinRoom.Click += button_JoinRoom_Click;
            // 
            // button_exit
            // 
            button_exit.Font = new Font("楷体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_exit.Location = new Point(283, 326);
            button_exit.Margin = new Padding(5, 4, 5, 4);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(148, 60);
            button_exit.TabIndex = 2;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // label_ID
            // 
            label_ID.AutoSize = true;
            label_ID.Location = new Point(68, 13);
            label_ID.Margin = new Padding(5, 0, 5, 0);
            label_ID.Name = "label_ID";
            label_ID.Size = new Size(32, 25);
            label_ID.TabIndex = 3;
            label_ID.Text = "ID";
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Location = new Point(68, 54);
            label_name.Margin = new Padding(5, 0, 5, 0);
            label_name.Name = "label_name";
            label_name.Size = new Size(67, 25);
            label_name.TabIndex = 4;
            label_name.Text = "Name";
            // 
            // textBox_roomid
            // 
            textBox_roomid.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox_roomid.Location = new Point(103, 211);
            textBox_roomid.Margin = new Padding(0, 4, 0, 4);
            textBox_roomid.Name = "textBox_roomid";
            textBox_roomid.PlaceholderText = "房间号";
            textBox_roomid.Size = new Size(101, 43);
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
            label_tip.Location = new Point(502, 217);
            label_tip.Name = "label_tip";
            label_tip.Size = new Size(67, 25);
            label_tip.TabIndex = 6;
            label_tip.Text = "label1";
            label_tip.Visible = false;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(729, 472);
            ControlBox = false;
            Controls.Add(label_tip);
            Controls.Add(textBox_roomid);
            Controls.Add(label_name);
            Controls.Add(label_ID);
            Controls.Add(button_exit);
            Controls.Add(button_JoinRoom);
            Controls.Add(button_CreateRoom);
            Margin = new Padding(5, 4, 5, 4);
            Name = "FormHome";
            Text = "游戏标题";
            ((System.ComponentModel.ISupportInitialize)errorProvider_join).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_CreateRoom;
        private Button button_JoinRoom;
        private Button button_exit;
        private Label label_ID;
        private Label label_name;
        private TextBox textBox_roomid;
        private ErrorProvider errorProvider_join;
        private Label label_tip;
    }
}