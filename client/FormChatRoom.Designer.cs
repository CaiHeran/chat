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
            textBox_input = new TextBox();
            button_send = new Button();
            richTextBox_view = new RichTextBox();
            label_content = new Label();
            button_exit = new Button();
            SuspendLayout();
            // 
            // listView_members
            // 
            listView_members.Location = new Point(47, 93);
            listView_members.Margin = new Padding(5, 4, 5, 4);
            listView_members.Name = "listView_members";
            listView_members.Size = new Size(395, 473);
            listView_members.TabIndex = 0;
            listView_members.UseCompatibleStateImageBehavior = false;
            // 
            // label_members
            // 
            label_members.AutoSize = true;
            label_members.Font = new Font("Microsoft YaHei UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_members.Location = new Point(47, 32);
            label_members.Margin = new Padding(5, 0, 5, 0);
            label_members.Name = "label_members";
            label_members.Size = new Size(245, 57);
            label_members.TabIndex = 1;
            label_members.Text = "房间成员：";
            // 
            // textBox_input
            // 
            textBox_input.Location = new Point(454, 534);
            textBox_input.Margin = new Padding(5, 4, 5, 4);
            textBox_input.Name = "textBox_input";
            textBox_input.PlaceholderText = "在这里键入消息";
            textBox_input.Size = new Size(655, 32);
            textBox_input.TabIndex = 2;
            // 
            // button_send
            // 
            button_send.Location = new Point(1120, 534);
            button_send.Margin = new Padding(5, 4, 5, 4);
            button_send.Name = "button_send";
            button_send.Size = new Size(118, 34);
            button_send.TabIndex = 3;
            button_send.Text = "发送";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // richTextBox_view
            // 
            richTextBox_view.Location = new Point(454, 93);
            richTextBox_view.Margin = new Padding(5, 4, 5, 4);
            richTextBox_view.Name = "richTextBox_view";
            richTextBox_view.Size = new Size(782, 430);
            richTextBox_view.TabIndex = 4;
            richTextBox_view.Text = "";
            // 
            // label_content
            // 
            label_content.AutoSize = true;
            label_content.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_content.Location = new Point(454, 32);
            label_content.Margin = new Padding(5, 0, 5, 0);
            label_content.Name = "label_content";
            label_content.Size = new Size(77, 39);
            label_content.TabIndex = 5;
            label_content.Text = "消息";
            // 
            // button_exit
            // 
            button_exit.Location = new Point(1120, 18);
            button_exit.Margin = new Padding(5, 4, 5, 4);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(118, 34);
            button_exit.TabIndex = 6;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // FormChatRoom
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1257, 662);
            ControlBox = false;
            Controls.Add(button_exit);
            Controls.Add(label_content);
            Controls.Add(richTextBox_view);
            Controls.Add(button_send);
            Controls.Add(textBox_input);
            Controls.Add(label_members);
            Controls.Add(listView_members);
            Margin = new Padding(5, 4, 5, 4);
            Name = "FormChatRoom";
            Text = "FormRoom";
            Load += this.FormChatRoom_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView_members;
        private Label label_members;
        private TextBox textBox_input;
        private Button button_send;
        private RichTextBox richTextBox_view;
        private Label label_content;
        private Button button_exit;
    }
}