namespace Client
{
    partial class FormTest
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
            richTextBox_recv = new RichTextBox();
            richTextBox_send = new RichTextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // richTextBox_recv
            // 
            richTextBox_recv.Font = new Font("JetBrains Mono", 11.9999981F);
            richTextBox_recv.Location = new Point(12, 12);
            richTextBox_recv.Name = "richTextBox_recv";
            richTextBox_recv.Size = new Size(328, 426);
            richTextBox_recv.TabIndex = 0;
            richTextBox_recv.Text = "";
            // 
            // richTextBox_send
            // 
            richTextBox_send.Font = new Font("JetBrains Mono", 11.9999981F);
            richTextBox_send.Location = new Point(393, 12);
            richTextBox_send.Name = "richTextBox_send";
            richTextBox_send.Size = new Size(324, 242);
            richTextBox_send.TabIndex = 1;
            richTextBox_send.Text = "";
            // 
            // button1
            // 
            button1.Font = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button1.Location = new Point(505, 325);
            button1.Name = "button1";
            button1.Size = new Size(77, 38);
            button1.TabIndex = 2;
            button1.Text = "发送";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // FormTest
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(729, 450);
            Controls.Add(button1);
            Controls.Add(richTextBox_send);
            Controls.Add(richTextBox_recv);
            Name = "FormTest";
            Text = "FormTest";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox_recv;
        private RichTextBox richTextBox_send;
        private Button button1;
    }
}