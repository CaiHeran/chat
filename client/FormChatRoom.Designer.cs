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
            listView_members = new ListView();
            label_members = new Label();
            button_send = new Button();
            richTextBox_view = new RichTextBox();
            button_exit = new Button();
            richTextBox_input = new RichTextBox();
            SuspendLayout();
            // 
            // listView_members
            // 
            listView_members.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            listView_members.Location = new Point(14, 110);
            listView_members.Margin = new Padding(5);
            listView_members.Name = "listView_members";
            listView_members.Size = new Size(220, 377);
            listView_members.TabIndex = 0;
            listView_members.UseCompatibleStateImageBehavior = false;
            // 
            // label_members
            // 
            label_members.AutoSize = true;
            label_members.Font = new Font("微软雅黑", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_members.Location = new Point(14, 72);
            label_members.Margin = new Padding(5);
            label_members.Name = "label_members";
            label_members.Size = new Size(117, 28);
            label_members.TabIndex = 1;
            label_members.Text = "房间成员：";
            // 
            // button_send
            // 
            button_send.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_send.Location = new Point(718, 412);
            button_send.Name = "button_send";
            button_send.Size = new Size(54, 33);
            button_send.TabIndex = 3;
            button_send.Text = "发送";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // richTextBox_view
            // 
            richTextBox_view.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_view.Location = new Point(262, 46);
            richTextBox_view.Margin = new Padding(5);
            richTextBox_view.Name = "richTextBox_view";
            richTextBox_view.Size = new Size(500, 350);
            richTextBox_view.TabIndex = 4;
            richTextBox_view.Text = "";
            // 
            // button_exit
            // 
            button_exit.Location = new Point(718, 12);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(54, 26);
            button_exit.TabIndex = 6;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // richTextBox_input
            // 
            richTextBox_input.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_input.Location = new Point(260, 412);
            richTextBox_input.Margin = new Padding(5);
            richTextBox_input.Name = "richTextBox_input";
            richTextBox_input.Size = new Size(450, 75);
            richTextBox_input.TabIndex = 7;
            richTextBox_input.Text = "";
            // 
            // FormChatRoom
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 501);
            ControlBox = false;
            Controls.Add(richTextBox_input);
            Controls.Add(button_exit);
            Controls.Add(richTextBox_view);
            Controls.Add(button_send);
            Controls.Add(label_members);
            Controls.Add(listView_members);
            Name = "FormChatRoom";
            Text = "FormRoom";
            Load += FormChatRoom_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView_members;
        private Label label_members;
        private Button button_send;
        private static RichTextBox richTextBox_view;
        private Button button_exit;
        private RichTextBox richTextBox_input;
    }
}