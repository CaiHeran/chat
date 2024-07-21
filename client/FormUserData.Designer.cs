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
            label_ip = new Label();
            SuspendLayout();
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_name.Location = new Point(12, 9);
            label_name.Name = "label_name";
            label_name.Size = new Size(82, 41);
            label_name.TabIndex = 0;
            label_name.Text = "姓名";
            // 
            // label_id
            // 
            label_id.AutoSize = true;
            label_id.Location = new Point(204, 25);
            label_id.Name = "label_id";
            label_id.Size = new Size(68, 25);
            label_id.TabIndex = 1;
            label_id.Text = "id : 11";
            // 
            // label_ip
            // 
            label_ip.AutoSize = true;
            label_ip.Location = new Point(12, 77);
            label_ip.Name = "label_ip";
            label_ip.Size = new Size(327, 25);
            label_ip.TabIndex = 2;
            label_ip.Text = "ip : fe80::9dec:8110:e008:XXXXXXX";
            // 
            // FormUserData
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 125);
            ControlBox = false;
            Controls.Add(label_ip);
            Controls.Add(label_id);
            Controls.Add(label_name);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormUserData";
            Text = "游戏标题";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_name;
        private Label label_id;
        private Label label_ip;
    }
}