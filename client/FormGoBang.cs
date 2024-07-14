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

namespace client//改Client
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
            //isplaying = false;
            isplaying = true;
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
            //绘制棋盘
            Graphics canva = panel_board.CreateGraphics();
            GoBangBoard.DrawBoard(canva);
            //加载棋子状态
            GoBangPawn.LoadPawn(panel_board, chessboard);
        }
        private void panel_board_MouseDown(object sender, MouseEventArgs e)
        {
            // 判断游戏是否开始
            if (!isplaying){ MessageBox.Show("游戏未开始", "游戏标题"); return; }
            int content = 0;//0无1黑2白
            //计算鼠标点击位置
            int PlacementX = e.X / GoBangStandard.CGap;
            int PlacementY = e.Y / GoBangStandard.CGap;

            try// 防止鼠标点击边界，导致数组越界
            {
                if (chessboard[PlacementX, PlacementY] != 0) return;                             
                if (!turn)
                {
                    chessboard[PlacementX, PlacementY] = 1;
                    content = 1;
                    //pictureBox_turn.BackgroundImage = "白子.png";// 改为白出子
                }
                else
                {
                    chessboard[PlacementX, PlacementY] = 2;
                    content = 2;                           
                    //pictureBox_turn.BackgroundImage = "黑子.png";//改为出黑子
                }
                GoBangPawn.DrawPawn(panel_board, turn, PlacementX, PlacementY);  // 画棋子

                // 一方获胜
                if (Judge(chessboard, content))
                {
                    string result = content == 1 ? "黑" : "白";
                    MessageBox.Show($"五连珠，{result}胜！", "游戏标题");
                    initchess();//重置游戏
                }

                turn = !turn;// 换方执子
            }
            catch (Exception) { }
            return;
        }

        // 棋子判断
        public static bool Judge(byte[,] chessboard, int content)
        {
            for (int i = 1; i < size+1; i++)
            for (int j = 1; j < size+1; j++)
            {
                if (chessboard[j, i] == content)
                {
                    if (j < 11)
                    if (chessboard[j + 1, i] == content
                    && chessboard[j + 2, i] == content
                    && chessboard[j + 3, i] == content
                    && chessboard[j + 4, i] == content) return true;

                    if (i < 11)
                    if (chessboard[j, i + 1] == content
                    && chessboard[j, i + 2] == content
                    && chessboard[j, i + 3] == content
                    && chessboard[j, i + 4] == content) return true;

                    if (j < 11 && i < 11)
                    if (chessboard[j + 1, i + 1] == content
                    && chessboard[j + 2, i + 2] == content
                    && chessboard[j + 3, i + 3] == content
                    && chessboard[j + 4, i + 4] == content) return true;

                    if (j > 3 && i < 11)
                    if (chessboard[j - 1, i + 1] == content
                    && chessboard[j - 2, i + 2] == content
                    && chessboard[j - 3, i + 3] == content
                    && chessboard[j - 4, i + 4] == content) return true;
                }
            }
            return false;
        }
    }
}
