namespace Client
{
    partial class FormRoom
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
            SuspendLayout();
            // 
            // listView_members
            // 
            listView_members.Location = new Point(30, 63);
            listView_members.Name = "listView_members";
            listView_members.Size = new Size(253, 323);
            listView_members.TabIndex = 0;
            listView_members.UseCompatibleStateImageBehavior = false;
            // 
            // label_members
            // 
            label_members.AutoSize = true;
            label_members.Font = new Font("Microsoft YaHei UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_members.Location = new Point(30, 22);
            label_members.Name = "label_members";
            label_members.Size = new Size(162, 38);
            label_members.TabIndex = 1;
            label_members.Text = "房间成员：";
            // 
            // textBox_input
            // 
            textBox_input.Location = new Point(289, 363);
            textBox_input.Name = "textBox_input";
            textBox_input.Size = new Size(418, 23);
            textBox_input.TabIndex = 2;
            textBox_input.Text = "在这里键入消息";
            // 
            // button_send
            // 
            button_send.Location = new Point(713, 363);
            button_send.Name = "button_send";
            button_send.Size = new Size(75, 23);
            button_send.TabIndex = 3;
            button_send.Text = "发送";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // richTextBox_view
            // 
            richTextBox_view.Location = new Point(289, 63);
            richTextBox_view.Name = "richTextBox_view";
            richTextBox_view.Size = new Size(499, 294);
            richTextBox_view.TabIndex = 4;
            richTextBox_view.Text = "";
            // 
            // label_content
            // 
            label_content.AutoSize = true;
            label_content.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_content.Location = new Point(289, 22);
            label_content.Name = "label_content";
            label_content.Size = new Size(52, 27);
            label_content.TabIndex = 5;
            label_content.Text = "消息";
            // 
            // FormRoom
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(label_content);
            Controls.Add(richTextBox_view);
            Controls.Add(button_send);
            Controls.Add(textBox_input);
            Controls.Add(label_members);
            Controls.Add(listView_members);
            Name = "FormRoom";
            Text = "游戏标题";
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
    }
}