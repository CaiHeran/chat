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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label_ID = new Label();
            label_name = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("楷体", 14.25F);
            button1.Location = new Point(153, 62);
            button1.Name = "button1";
            button1.Size = new Size(148, 45);
            button1.TabIndex = 0;
            button1.Text = "创建房间";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("楷体", 14.25F);
            button2.Location = new Point(153, 136);
            button2.Name = "button2";
            button2.Size = new Size(148, 44);
            button2.TabIndex = 1;
            button2.Text = "加入房间";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Font = new Font("楷体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button3.Location = new Point(180, 222);
            button3.Name = "button3";
            button3.Size = new Size(94, 41);
            button3.TabIndex = 2;
            button3.Text = "退出";
            button3.UseVisualStyleBackColor = true;
            // 
            // label_ID
            // 
            label_ID.AutoSize = true;
            label_ID.Location = new Point(43, 9);
            label_ID.Name = "label_ID";
            label_ID.Size = new Size(43, 17);
            label_ID.TabIndex = 3;
            label_ID.Text = "label1";
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Location = new Point(43, 37);
            label_name.Name = "label_name";
            label_name.Size = new Size(43, 17);
            label_name.TabIndex = 4;
            label_name.Text = "label1";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 321);
            Controls.Add(label_name);
            Controls.Add(label_ID);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Label label_ID;
        private Label label_name;
    }
}