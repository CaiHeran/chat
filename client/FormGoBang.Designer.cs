namespace client
{
    partial class FormGoBang
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
            label_tip = new Label();
            textBox_position = new TextBox();
            button_moment = new Button();
            label_stand = new Label();
            pictureBox_stand = new PictureBox();
            label_turn = new Label();
            pictureBox_turn = new PictureBox();
            panel_board = new Panel();
            panel_action = new Panel();
            panel_info = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox_stand).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_turn).BeginInit();
            panel_action.SuspendLayout();
            panel_info.SuspendLayout();
            SuspendLayout();
            // 
            // label_tip
            // 
            label_tip.AutoSize = true;
            label_tip.BackColor = Color.Transparent;
            label_tip.Location = new Point(3, 11);
            label_tip.Name = "label_tip";
            label_tip.Size = new Size(92, 17);
            label_tip.TabIndex = 1;
            label_tip.Text = "输入落子坐标：";
            // 
            // textBox_position
            // 
            textBox_position.AccessibleDescription = "";
            textBox_position.AccessibleName = "";
            textBox_position.Location = new Point(101, 5);
            textBox_position.Name = "textBox_position";
            textBox_position.PlaceholderText = "如：G7，以后改为鼠标落子";
            textBox_position.Size = new Size(188, 23);
            textBox_position.TabIndex = 2;
            // 
            // button_moment
            // 
            button_moment.Location = new Point(295, 5);
            button_moment.Name = "button_moment";
            button_moment.Size = new Size(75, 23);
            button_moment.TabIndex = 3;
            button_moment.Text = "落子";
            button_moment.UseVisualStyleBackColor = true;
            // 
            // label_stand
            // 
            label_stand.AutoSize = true;
            label_stand.BackColor = Color.Transparent;
            label_stand.Location = new Point(17, 21);
            label_stand.Name = "label_stand";
            label_stand.Size = new Size(68, 17);
            label_stand.TabIndex = 4;
            label_stand.Text = "我方执棋：";
            // 
            // pictureBox_stand
            // 
            pictureBox_stand.BackgroundImage = Properties.Resources.黑子;
            pictureBox_stand.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_stand.Location = new Point(91, 12);
            pictureBox_stand.Name = "pictureBox_stand";
            pictureBox_stand.Size = new Size(30, 30);
            pictureBox_stand.TabIndex = 5;
            pictureBox_stand.TabStop = false;
            // 
            // label_turn
            // 
            label_turn.AutoSize = true;
            label_turn.BackColor = Color.Transparent;
            label_turn.Location = new Point(202, 21);
            label_turn.Name = "label_turn";
            label_turn.Size = new Size(68, 17);
            label_turn.TabIndex = 6;
            label_turn.Text = "当前走子：";
            // 
            // pictureBox_turn
            // 
            pictureBox_turn.BackgroundImage = Properties.Resources.黑子;
            pictureBox_turn.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_turn.Location = new Point(276, 12);
            pictureBox_turn.Name = "pictureBox_turn";
            pictureBox_turn.Size = new Size(30, 30);
            pictureBox_turn.TabIndex = 7;
            pictureBox_turn.TabStop = false;
            // 
            // panel_board
            // 
            panel_board.BackColor = Color.Transparent;
            panel_board.Location = new Point(12, 69);
            panel_board.Name = "panel_board";
            panel_board.Size = new Size(410, 410);
            panel_board.TabIndex = 8;
            panel_board.Paint += panel_board_Paint;
            panel_board.MouseDown += panel_board_MouseDown;
            // 
            // panel_action
            // 
            panel_action.BackColor = Color.Transparent;
            panel_action.Controls.Add(label_tip);
            panel_action.Controls.Add(button_moment);
            panel_action.Controls.Add(textBox_position);
            panel_action.Location = new Point(12, 485);
            panel_action.Name = "panel_action";
            panel_action.Size = new Size(410, 32);
            panel_action.TabIndex = 0;
            // 
            // panel_info
            // 
            panel_info.BackColor = Color.Transparent;
            panel_info.Controls.Add(label_stand);
            panel_info.Controls.Add(pictureBox_stand);
            panel_info.Controls.Add(pictureBox_turn);
            panel_info.Controls.Add(label_turn);
            panel_info.Location = new Point(12, 12);
            panel_info.Name = "panel_info";
            panel_info.Size = new Size(410, 51);
            panel_info.TabIndex = 0;
            // 
            // FormGoBang
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(434, 521);
            Controls.Add(panel_action);
            Controls.Add(panel_info);
            Controls.Add(panel_board);
            DoubleBuffered = true;
            Name = "FormGoBang";
            Text = "游戏标题";
            Load += FormGoBang_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_stand).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_turn).EndInit();
            panel_action.ResumeLayout(false);
            panel_action.PerformLayout();
            panel_info.ResumeLayout(false);
            panel_info.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label_tip;
        private TextBox textBox_position;
        private Button button_moment;
        private Label label_stand;
        private PictureBox pictureBox_stand;
        private Label label_turn;
        private PictureBox pictureBox_turn;
        private Panel panel_board;
        private Panel panel_info;
        private Panel panel_action;
    }
}