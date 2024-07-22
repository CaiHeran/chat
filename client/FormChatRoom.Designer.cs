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
            components = new System.ComponentModel.Container();
            errorProvider_send = new ErrorProvider(components);
            button_exit = new Button();
            richTextBox_view = new RichTextBox();
            richTextBox_input = new RichTextBox();
            button_send = new Button();
            dataGridView_list = new DataGridView();
            label_roomid = new Label();
            panel_background = new Panel();
            ((System.ComponentModel.ISupportInitialize)errorProvider_send).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_list).BeginInit();
            panel_background.SuspendLayout();
            SuspendLayout();
            // 
            // errorProvider_send
            // 
            errorProvider_send.ContainerControl = this;
            // 
            // button_exit
            // 
            button_exit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_exit.Location = new Point(796, 12);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(54, 26);
            button_exit.TabIndex = 6;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // richTextBox_view
            // 
            richTextBox_view.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox_view.BackColor = SystemColors.Window;
            richTextBox_view.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_view.Location = new Point(224, 48);
            richTextBox_view.Margin = new Padding(5);
            richTextBox_view.Name = "richTextBox_view";
            richTextBox_view.ReadOnly = true;
            richTextBox_view.Size = new Size(625, 391);
            richTextBox_view.TabIndex = 4;
            richTextBox_view.Text = "";
            // 
            // richTextBox_input
            // 
            richTextBox_input.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTextBox_input.Location = new Point(224, 449);
            richTextBox_input.Margin = new Padding(5);
            richTextBox_input.Name = "richTextBox_input";
            richTextBox_input.Size = new Size(563, 94);
            richTextBox_input.TabIndex = 7;
            richTextBox_input.Text = "";
            // 
            // button_send
            // 
            button_send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_send.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_send.Location = new Point(796, 449);
            button_send.Name = "button_send";
            button_send.Size = new Size(54, 33);
            button_send.TabIndex = 3;
            button_send.Text = "发送";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // dataGridView_list
            // 
            dataGridView_list.AllowUserToAddRows = false;
            dataGridView_list.AllowUserToDeleteRows = false;
            dataGridView_list.AllowUserToOrderColumns = true;
            dataGridView_list.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dataGridView_list.BackgroundColor = SystemColors.Window;
            dataGridView_list.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_list.Location = new Point(13, 91);
            dataGridView_list.Margin = new Padding(5);
            dataGridView_list.Name = "dataGridView_list";
            dataGridView_list.ReadOnly = true;
            dataGridView_list.RowHeadersWidth = 62;
            dataGridView_list.Size = new Size(201, 452);
            dataGridView_list.TabIndex = 9;
            dataGridView_list.CellMouseLeave += dataGridView_list_CellMouseLeave;
            dataGridView_list.CellMouseMove += dataGridView_list_CellMouseMove;
            // 
            // label_roomid
            // 
            label_roomid.AutoSize = true;
            label_roomid.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label_roomid.Location = new Point(13, 16);
            label_roomid.Margin = new Padding(2, 0, 2, 0);
            label_roomid.Name = "label_roomid";
            label_roomid.Size = new Size(88, 25);
            label_roomid.TabIndex = 8;
            label_roomid.Text = "房间号：";
            // 
            // panel_background
            // 
            panel_background.Controls.Add(label_roomid);
            panel_background.Controls.Add(dataGridView_list);
            panel_background.Controls.Add(button_send);
            panel_background.Controls.Add(richTextBox_input);
            panel_background.Controls.Add(richTextBox_view);
            panel_background.Controls.Add(button_exit);
            panel_background.Dock = DockStyle.Fill;
            panel_background.Location = new Point(0, 0);
            panel_background.Margin = new Padding(2);
            panel_background.Name = "panel_background";
            panel_background.Size = new Size(862, 557);
            panel_background.TabIndex = 11;
            // 
            // FormChatRoom
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 557);
            ControlBox = false;
            Controls.Add(panel_background);
            Name = "FormChatRoom";
            Text = "ChatRoom";
            Load += FormChatRoom_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider_send).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView_list).EndInit();
            panel_background.ResumeLayout(false);
            panel_background.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ErrorProvider errorProvider_send;
        private Panel panel_background;
        private Label label_roomid;
        private DataGridView dataGridView_list;
        private Button button_send;
        private RichTextBox richTextBox_input;
        private RichTextBox richTextBox_view;
        private Button button_exit;
    }
}