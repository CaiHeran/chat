namespace client
{
    partial class FormGobang
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
            button_exit = new Button();
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
            label_tip.Location = new Point(5, 16);
            label_tip.Margin = new Padding(5, 0, 5, 0);
            label_tip.Name = "label_tip";
            label_tip.Size = new Size(145, 25);
            label_tip.TabIndex = 1;
            label_tip.Text = "输入落子坐标：";
            // 
            // textBox_position
            // 
            textBox_position.AccessibleDescription = "";
            textBox_position.AccessibleName = "";
            textBox_position.Location = new Point(159, 7);
            textBox_position.Margin = new Padding(5, 4, 5, 4);
            textBox_position.Name = "textBox_position";
            textBox_position.PlaceholderText = "如：G7，以后改为鼠标落子";
            textBox_position.Size = new Size(293, 32);
            textBox_position.TabIndex = 2;
            // 
            // button_moment
            // 
            button_moment.Location = new Point(464, 7);
            button_moment.Margin = new Padding(5, 4, 5, 4);
            button_moment.Name = "button_moment";
            button_moment.Size = new Size(118, 34);
            button_moment.TabIndex = 3;
            button_moment.Text = "落子";
            button_moment.UseVisualStyleBackColor = true;
            // 
            // label_stand
            // 
            label_stand.AutoSize = true;
            label_stand.BackColor = Color.Transparent;
            label_stand.Location = new Point(27, 31);
            label_stand.Margin = new Padding(5, 0, 5, 0);
            label_stand.Name = "label_stand";
            label_stand.Size = new Size(107, 25);
            label_stand.TabIndex = 4;
            label_stand.Text = "我方执棋：";
            // 
            // pictureBox_stand
            // 
            pictureBox_stand.BackgroundImage = Properties.Resources.黑子;
            pictureBox_stand.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_stand.Location = new Point(143, 18);
            pictureBox_stand.Margin = new Padding(5, 4, 5, 4);
            pictureBox_stand.Name = "pictureBox_stand";
            pictureBox_stand.Size = new Size(47, 44);
            pictureBox_stand.TabIndex = 5;
            pictureBox_stand.TabStop = false;
            // 
            // label_turn
            // 
            label_turn.AutoSize = true;
            label_turn.BackColor = Color.Transparent;
            label_turn.Location = new Point(317, 31);
            label_turn.Margin = new Padding(5, 0, 5, 0);
            label_turn.Name = "label_turn";
            label_turn.Size = new Size(107, 25);
            label_turn.TabIndex = 6;
            label_turn.Text = "当前走子：";
            // 
            // pictureBox_turn
            // 
            pictureBox_turn.BackgroundImage = Properties.Resources.黑子;
            pictureBox_turn.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_turn.Location = new Point(434, 18);
            pictureBox_turn.Margin = new Padding(5, 4, 5, 4);
            pictureBox_turn.Name = "pictureBox_turn";
            pictureBox_turn.Size = new Size(47, 44);
            pictureBox_turn.TabIndex = 7;
            pictureBox_turn.TabStop = false;
            // 
            // panel_board
            // 
            panel_board.BackColor = Color.Transparent;
            panel_board.Location = new Point(19, 101);
            panel_board.Margin = new Padding(5, 4, 5, 4);
            panel_board.Name = "panel_board";
            panel_board.Size = new Size(644, 603);
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
            panel_action.Location = new Point(19, 713);
            panel_action.Margin = new Padding(5, 4, 5, 4);
            panel_action.Name = "panel_action";
            panel_action.Size = new Size(644, 47);
            panel_action.TabIndex = 0;
            // 
            // panel_info
            // 
            panel_info.BackColor = Color.Transparent;
            panel_info.Controls.Add(button_exit);
            panel_info.Controls.Add(label_stand);
            panel_info.Controls.Add(pictureBox_stand);
            panel_info.Controls.Add(pictureBox_turn);
            panel_info.Controls.Add(label_turn);
            panel_info.Location = new Point(19, 18);
            panel_info.Margin = new Padding(5, 4, 5, 4);
            panel_info.Name = "panel_info";
            panel_info.Size = new Size(644, 75);
            panel_info.TabIndex = 0;
            // 
            // button_exit
            // 
            button_exit.Location = new Point(521, 22);
            button_exit.Margin = new Padding(5, 4, 5, 4);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(118, 34);
            button_exit.TabIndex = 6;
            button_exit.Text = "退出";
            button_exit.UseVisualStyleBackColor = true;
            button_exit.Click += button_exit_Click;
            // 
            // FormGobang
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(682, 766);
            ControlBox = false;
            Controls.Add(panel_action);
            Controls.Add(panel_info);
            Controls.Add(panel_board);
            DoubleBuffered = true;
            Margin = new Padding(5, 4, 5, 4);
            Name = "FormGobang";
            Text = "游戏标题";
            Load += FormGobang_Load;
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
        private Button button_exit;
    }
}