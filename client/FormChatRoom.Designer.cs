namespace Client
{
    partial class FormChatRoom
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
            label_members = new Label();
            button_send = new Button();
            richTextBox_view = new RichTextBox();
            button_exit = new Button();
            richTextBox_input = new RichTextBox();
            label_roomid = new Label();
            dataGridView_list = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView_list).BeginInit();
            SuspendLayout();
            // 
            // label_members
            // 
            label_members.AutoSize = true;
            label_members.Font = new Font("微软雅黑", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_members.Location = new Point(22, 106);
            label_members.Margin = new Padding(8, 7, 8, 7);
            label_members.Name = "label_members";
            label_members.Size = new Size(178, 41);
            label_members.TabIndex = 1;
            label_members.Text = "房间成员：";
            // 
            // button_send
            // 
            button_send.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_send.Location = new Point(1128, 606);
            button_send.Margin = new Padding(5, 4, 5, 4);
            button_send.Name = "button_send";
            button_send.Size = new Size(85, 49);
            button_send.TabIndex = 3;
            button_send.Text = "发送";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // richTextBox_view
            // 
            richTextBox_view.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_view.Location = new Point(412, 68);
            richTextBox_view.Margin = new Padding(8, 7, 8, 7);
            richTextBox_view.Name = "richTextBox_view";
            richTextBox_view.Size = new Size(783, 513);
            richTextBox_view.TabIndex = 4;
            richTextBox_view.Text = "";
            // 
            // button_exit
            // 
            button_exit.Location = new Point(1128, 18);
            button_exit.Margin = new Padding(5, 4, 5, 4);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(85, 38);
            button_exit.TabIndex = 6;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // richTextBox_input
            // 
            richTextBox_input.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_input.Location = new Point(409, 606);
            richTextBox_input.Margin = new Padding(8, 7, 8, 7);
            richTextBox_input.Name = "richTextBox_input";
            richTextBox_input.Size = new Size(705, 108);
            richTextBox_input.TabIndex = 7;
            richTextBox_input.Text = "";
            // 
            // label_roomid
            // 
            label_roomid.AutoSize = true;
            label_roomid.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_roomid.Location = new Point(22, 20);
            label_roomid.Name = "label_roomid";
            label_roomid.Size = new Size(127, 36);
            label_roomid.TabIndex = 8;
            label_roomid.Text = "房间号：";
            // 
            // dataGridView_list
            // 
            dataGridView_list.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_list.Location = new Point(22, 157);
            dataGridView_list.Name = "dataGridView_list";
            dataGridView_list.RowHeadersWidth = 62;
            dataGridView_list.Size = new Size(360, 540);
            dataGridView_list.TabIndex = 9;
            // 
            // FormChatRoom
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 737);
            ControlBox = false;
            Controls.Add(dataGridView_list);
            Controls.Add(label_roomid);
            Controls.Add(richTextBox_input);
            Controls.Add(button_exit);
            Controls.Add(richTextBox_view);
            Controls.Add(button_send);
            Controls.Add(label_members);
            Margin = new Padding(5, 4, 5, 4);
            Name = "FormChatRoom";
            Text = "FormRoom";
            Load += FormChatRoom_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView_list).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label_members;
        private Button button_send;
        private RichTextBox richTextBox_view;
        private Button button_exit;
        private RichTextBox richTextBox_input;
        private Label label_roomid;
        private static DataGridView dataGridView_list;
    }
}