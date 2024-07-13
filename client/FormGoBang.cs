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
            Graphics canva = panel_board.CreateGraphics();// 创建画布
            GoBangBoard.DrawBoard(canva);                 // 画棋盘
            GoBangPawn.LoadPawn(panel_board, chessboard); // 加载棋子
        }

        private void panel_board_MouseDown(object sender, MouseEventArgs e)
        {
            // 判断游戏是否开始
            if (!isplaying)
            {
                MessageBox.Show("游戏未开始", "游戏标题");
                return;
            }

            byte content = 0;// 获取棋盘某格状态，0无1黑2白
            //计算鼠标点击点位
            int PlacementX = e.X / GoBangStandard.CGap;
            int PlacementY = e.Y / GoBangStandard.CGap;
            try // 防止鼠标点击边界，数组越界
            {
                // 有棋子不可落子
                if (chessboard[PlacementX, PlacementY] != 0) return;
                // 落子
                if (turn)
                {
                    chessboard[PlacementX, PlacementY] = 2;
                    content = 2;
                    //pictureBox_turn.Image = "黑子.png";// 提示接下来"黑子回合"
                }
                else
                {
                    chessboard[PlacementX, PlacementY] = 1;
                    content = 1; 
                    //pictureBox_turn.Image = "白子.png";// 提示文本改为"白子回合"
                }

                GoBangPawn.DrawPawn(panel_board, turn, PlacementX, PlacementY);
                if (GoBangStandard.WinJudge(chessboard, content))
                {
                    string result = (content == 1) ? "黑" : "白";
                    MessageBox.Show($"{result}胜！", "游戏标题");
                    initchess();// 初始化游戏
                }
                turn = !turn; // 换方执子
            }
            catch (Exception) { }
        }
    }
}
