namespace Client
{
    partial class FormUserData
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
            form = null;
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
            label_name = new Label();
            label_id = new Label();
            SuspendLayout();
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_name.Location = new Point(8, 6);
            label_name.Margin = new Padding(2, 0, 2, 0);
            label_name.Name = "label_name";
            label_name.Size = new Size(57, 30);
            label_name.TabIndex = 0;
            label_name.Text = "姓名";
            // 
            // label_id
            // 
            label_id.AutoSize = true;
            label_id.Location = new Point(130, 17);
            label_id.Margin = new Padding(2, 0, 2, 0);
            label_id.Name = "label_id";
            label_id.Size = new Size(44, 17);
            label_id.TabIndex = 1;
            label_id.Text = "id : 11";
            // 
            // FormUserData
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(230, 85);
            ControlBox = false;
            Controls.Add(label_id);
            Controls.Add(label_name);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2, 2, 2, 2);
            Name = "FormUserData";
            StartPosition = FormStartPosition.Manual;
            Text = "游戏标题";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_name;
        private Label label_id;
    }
}