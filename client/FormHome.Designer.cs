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
            button_CreateRoom = new Button();
            button_JoinRoom = new Button();
            button_exit = new Button();
            label_ID = new Label();
            label_name = new Label();
            textBox_id = new TextBox();
            SuspendLayout();
            // 
            // button_CreateRoom
            // 
            button_CreateRoom.Font = new Font("楷体", 14.25F);
            button_CreateRoom.Location = new Point(153, 62);
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
            button_JoinRoom.Location = new Point(153, 136);
            button_JoinRoom.Name = "button_JoinRoom";
            button_JoinRoom.Size = new Size(148, 44);
            button_JoinRoom.TabIndex = 1;
            button_JoinRoom.Text = "加入房间";
            button_JoinRoom.UseVisualStyleBackColor = true;
            button_JoinRoom.Click += button_JoinRoom_Click;
            // 
            // button_exit
            // 
            button_exit.Font = new Font("楷体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_exit.Location = new Point(180, 222);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(94, 41);
            button_exit.TabIndex = 2;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // label_ID
            // 
            label_ID.AutoSize = true;
            label_ID.Location = new Point(43, 9);
            label_ID.Name = "label_ID";
            label_ID.Size = new Size(21, 17);
            label_ID.TabIndex = 3;
            label_ID.Text = "ID";
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Location = new Point(43, 37);
            label_name.Name = "label_name";
            label_name.Size = new Size(43, 17);
            label_name.TabIndex = 4;
            label_name.Text = "Name";
            // 
            // textBox_id
            // 
            textBox_id.Location = new Point(325, 149);
            textBox_id.Margin = new Padding(0, 3, 0, 3);
            textBox_id.Name = "textBox_id";
            textBox_id.PlaceholderText = "请输入房间号";
            textBox_id.Size = new Size(111, 23);
            textBox_id.TabIndex = 5;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            ControlBox = false;
            Controls.Add(textBox_id);
            Controls.Add(label_name);
            Controls.Add(label_ID);
            Controls.Add(button_exit);
            Controls.Add(button_JoinRoom);
            Controls.Add(button_CreateRoom);
            Name = "FormHome";
            Text = "游戏标题";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_CreateRoom;
        private Button button_JoinRoom;
        private Button button_exit;
        private Label label_ID;
        private Label label_name;
        private TextBox textBox_id;
    }
}