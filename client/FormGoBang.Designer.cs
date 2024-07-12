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
            pictureBox_chessboard = new PictureBox();
            label_tip = new Label();
            textBox_position = new TextBox();
            button_moment = new Button();
            label_stand = new Label();
            pictureBox_stand = new PictureBox();
            label_turn = new Label();
            pictureBox_turn = new PictureBox();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_chessboard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_stand).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_turn).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_chessboard
            // 
            pictureBox_chessboard.BackgroundImage = Properties.Resources.棋盘;
            pictureBox_chessboard.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_chessboard.Location = new Point(12, 69);
            pictureBox_chessboard.Name = "pictureBox_chessboard";
            pictureBox_chessboard.Size = new Size(410, 410);
            pictureBox_chessboard.TabIndex = 0;
            pictureBox_chessboard.TabStop = false;
            // 
            // label_tip
            // 
            label_tip.AutoSize = true;
            label_tip.BackColor = Color.Transparent;
            label_tip.Location = new Point(32, 491);
            label_tip.Name = "label_tip";
            label_tip.Size = new Size(92, 17);
            label_tip.TabIndex = 1;
            label_tip.Text = "输入落子坐标：";
            // 
            // textBox_position
            // 
            textBox_position.AccessibleDescription = "";
            textBox_position.AccessibleName = "";
            textBox_position.Location = new Point(130, 485);
            textBox_position.Name = "textBox_position";
            textBox_position.PlaceholderText = "如：G7，以后改为鼠标落子";
            textBox_position.Size = new Size(188, 23);
            textBox_position.TabIndex = 2;
            // 
            // button_moment
            // 
            button_moment.Location = new Point(324, 485);
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
            label_stand.Location = new Point(12, 21);
            label_stand.Name = "label_stand";
            label_stand.Size = new Size(68, 17);
            label_stand.TabIndex = 4;
            label_stand.Text = "我方执棋：";
            // 
            // pictureBox_stand
            // 
            pictureBox_stand.BackgroundImage = Properties.Resources.黑子;
            pictureBox_stand.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_stand.Location = new Point(72, 12);
            pictureBox_stand.Name = "pictureBox_stand";
            pictureBox_stand.Size = new Size(30, 30);
            pictureBox_stand.TabIndex = 5;
            pictureBox_stand.TabStop = false;
            // 
            // label_turn
            // 
            label_turn.AutoSize = true;
            label_turn.BackColor = Color.Transparent;
            label_turn.Location = new Point(197, 21);
            label_turn.Name = "label_turn";
            label_turn.Size = new Size(68, 17);
            label_turn.TabIndex = 6;
            label_turn.Text = "当前走子：";
            // 
            // pictureBox_turn
            // 
            pictureBox_turn.BackgroundImage = Properties.Resources.黑子;
            pictureBox_turn.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_turn.Location = new Point(271, 12);
            pictureBox_turn.Name = "pictureBox_turn";
            pictureBox_turn.Size = new Size(30, 30);
            pictureBox_turn.TabIndex = 7;
            pictureBox_turn.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Location = new Point(12, 69);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(368, 410);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "五子棋";
            // 
            // FormGoBang
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(434, 517);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox_turn);
            Controls.Add(label_turn);
            Controls.Add(pictureBox_stand);
            Controls.Add(label_stand);
            Controls.Add(button_moment);
            Controls.Add(textBox_position);
            Controls.Add(label_tip);
            Controls.Add(pictureBox_chessboard);
            DoubleBuffered = true;
            Name = "FormGoBang";
            Text = "游戏标题";
            ((System.ComponentModel.ISupportInitialize)pictureBox_chessboard).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_stand).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_turn).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox_chessboard;
        private Label label_tip;
        private TextBox textBox_position;
        private Button button_moment;
        private Label label_stand;
        private PictureBox pictureBox_stand;
        private Label label_turn;
        private PictureBox pictureBox_turn;
        private GroupBox groupBox1;
    }
}