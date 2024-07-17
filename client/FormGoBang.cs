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
    public partial class FormGobang : Form
    {
        public static FormGobang formGobang = new FormGobang();
        public FormGobang()
        {
            InitializeComponent();
        }
        private void FormGobang_Load(object sender, EventArgs e)
        {
            Gobang.InitChess();
            this.Width = Gobang.Standard.Width;
            this.Height = Gobang.Standard.Height;
            this.Location = new Point(Gobang.Standard.PosX, Gobang.Standard.PosY);
        }
        private void panel_board_Paint(object sender, PaintEventArgs e)
        {
            Graphics canva = panel_board.CreateGraphics();                                // 获取画布
            Gobang.Board.DrawBoard(canva);                                                // 绘制棋盘
            Gobang.Pawn.LoadPawn(panel_board, Gobang.ChessBoard);                         // 加载棋子状态
        }
        private void panel_board_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Gobang.isplaying) { MessageBox.Show("游戏未开始", "游戏标题"); return; }  // 判断游戏是否开始
            byte content = 0;                                                             // 0无1黑2白
            int PlacementX = e.X / Gobang.Standard.CGap;                                  // 将鼠标点击屏幕位置转换为格子位置
            int PlacementY = e.Y / Gobang.Standard.CGap;

            try                                                                           // 防止鼠标点击边界，导致数组越界
            {
                if (Gobang.ChessBoard[PlacementX, PlacementY] != 0) return;
                if (!Gobang.turn)
                {
                    Gobang.ChessBoard[PlacementX, PlacementY] = 1;
                    content = 1;
                    //pictureBox_turn.BackgroundImage = "白子.png";                      // 改为白出子
                }
                else
                {
                    Gobang.ChessBoard[PlacementX, PlacementY] = 2;
                    content = 2;
                    //pictureBox_turn.BackgroundImage = "黑子.png";                      // 改为出黑子
                }
                Gobang.Pawn.DrawPawn(panel_board, Gobang.turn, PlacementX, PlacementY);  // 画棋子

                // 一方获胜
                if (Gobang.WinJudge(Gobang.ChessBoard, content))
                {
                    string result = content == 1 ? "黑" : "白";
                    MessageBox.Show($"五连珠，{result}胜！", "游戏标题");
                    Gobang.InitChess();//重置游戏
                }

                Gobang.turn = !Gobang.turn;// 换方执子
            }
            catch (Exception) { }
            return;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("真的要退出吗？", "游戏标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
            this.Close();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}
