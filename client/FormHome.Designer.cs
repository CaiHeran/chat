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
            button_Exist = new Button();
            label_ID = new Label();
            label_name = new Label();
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
            // button_Exist
            // 
            button_Exist.Font = new Font("楷体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_Exist.Location = new Point(180, 222);
            button_Exist.Name = "button_Exist";
            button_Exist.Size = new Size(94, 41);
            button_Exist.TabIndex = 2;
            button_Exist.Text = "退出";
            button_Exist.UseVisualStyleBackColor = true;
            button_Exist.Click += button_Exist_Click;
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
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(label_name);
            Controls.Add(label_ID);
            Controls.Add(button_Exist);
            Controls.Add(button_JoinRoom);
            Controls.Add(button_CreateRoom);
            Name = "FormHome";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_CreateRoom;
        private Button button_JoinRoom;
        private Button button_Exist;
        private Label label_ID;
        private Label label_name;
    }
}