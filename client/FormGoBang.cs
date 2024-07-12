using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class FormGoBang : Form
    {
        public static FormGoBang formgobang = new FormGoBang();

        private bool isplaying = false;//棋局未开始
        private bool turn = false;//黑先手
        private bool myturn = true;//是否自己先手
        public const int size = 13;//棋盘长宽
        public byte[,] chessboard = new byte[size + 1, size + 1];

        private void initchess()
        {
            isplaying = false;
            turn = false;
            myturn = true;//myturn = 服务器获取
            for (int i = 0; i <= size; i++)
                for (int j = 0; j <= size; j++) chessboard[i, j] = 0;
            //pictureBox_stand.BackgroundImage = "黑子.png";
            //pictureBox_turn.BackgroundImage = "黑子.png";
        }

        public FormGoBang()
        {
            InitializeComponent();
        }



        private void FormGoBang_Load(object sender, EventArgs e)
        {
            initchess();
            this.Width = GoBangStandard.Width;
            this.Height = GoBangStandard.Height;
            this.Location = new Point(GoBangStandard.PosX, GoBangStandard.PosY);
        }

        private void panel_board_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel_board.CreateGraphics();      // 创建面板画布
            GoBangChessBoard.DrawCB(g);                      // 调用画棋盘方法
            //Chess.ReDrawC(panel_board, CheckBoard);         // 调用重新加载棋子方法
        }
    }
}
