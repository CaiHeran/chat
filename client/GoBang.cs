using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace client// 改Client
{
    internal class Gobang
    {
        internal static bool isplaying = false;                                       // 棋局状态（未开始）
        internal static bool turn = false;                                            // 着手状态（黑先手）
        internal static bool myturn = true;                                           // 是否自己着手
        internal const int size = 13;                                                 // 棋盘每行每列格数
        internal static byte[,] ChessBoard = new byte[size + 1, size + 1];            // 棋盘数组 0无1黑2白
        internal class Board // 关于棋盘
        {
            public static void DrawBoard(Graphics canva)  // 绘制棋盘，参数为画布
            {
                int GapWidth = Standard.CGap;                                 // 棋格宽度
                int GapNum = Standard.CWidth / GapWidth - 1;                  // 棋格数量
                /*
                // 画棋盘图片
                Bitmap image = new Bitmap("棋盘.png"); // 棋盘路径
                canva.DrawImage(image, new Point[] {
                    new Point(0, 0),
                    new Point(GapWidth * GapNum + 44, 0),
                    new Point(0, GapWidth * GapNum + 44)
                });
                */
                // 用“画笔”画棋盘
                canva.Clear(Color.Transparent);                              // 清除画布、并设置画布透明    !!!Requring fixing
                Pen pen = new Pen(Color.FromArgb(192, 166, 107));            // 设置画笔棕色
                for (int i = 0; i < GapNum + 1; i++)                         // 绘制棋盘
                {
                    canva.DrawLine(pen, new Point(20, i * GapWidth + 20),
                                        new Point(GapWidth * GapNum + 20, i * GapWidth + 20));
                    canva.DrawLine(pen, new Point(i * GapWidth + 20, 20),
                                        new Point(i * GapWidth + 20, GapWidth * GapNum + 20));
                }
            }
        }
        internal class Pawn //关于棋子
        {
            public static void DrawPawn(Panel p, bool turn, int PlacementX, int PlacementY)//绘制棋子，参数为颜色和位置
            {
                Graphics canva = p.CreateGraphics();                            // 创建面板画布
                canva.SmoothingMode = SmoothingMode.HighQuality;
                canva.InterpolationMode = InterpolationMode.HighQualityBicubic; // 消除棋子边缘的锯齿

                int AccurateX = PlacementX * Standard.CGap + 10;
                int AccurateY = PlacementY * Standard.CGap + 10;                // 计算其在屏幕的坐标

                // 画棋子
                // 线性渐变，根据位置填颜色，从上至下渐变，使棋子具有真实感
                if (!turn)                                                      // 画黑子
                    canva.FillEllipse(new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                        Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0)),
                        new Rectangle(
                            new Point(AccurateX, AccurateY),
                            new Size(Standard.CDiameter, Standard.CDiameter)));
                else                                                            // 画白子
                    canva.FillEllipse(new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                        Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204)),
                        new Rectangle(
                            new Point(AccurateX, AccurateY),
                            new Size(Standard.CDiameter, Standard.CDiameter)));
            }
            public static void LoadPawn(Panel p, byte[,] ChessBoard)            // 重新加载棋子,参数为画布和棋盘数组
            {
                Graphics canva = p.CreateGraphics();                            // 创建面板画布
                canva.SmoothingMode = SmoothingMode.HighQuality;
                canva.InterpolationMode = InterpolationMode.HighQualityBicubic; // 消除棋子边缘的锯齿

                for (int i = 1; i <= size; i++)
                    for (int j = 1; j <= size; j++)
                    {
                        byte content = ChessBoard[i, j];                        // 获取(i,j)位置棋子状态
                        int AccurateX = j * Standard.CGap + 10;
                        int AccurateY = i * Standard.CGap + 10;                 // 计算其在屏幕的坐标
                        switch (content)
                        {
                            case 0:
                                break;
                            case 1:
                                canva.FillEllipse(
                                    new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                                    Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0)),
                                    new Rectangle(new Point(AccurateX, AccurateY),
                                    new Size(Standard.CDiameter, Standard.CDiameter)));
                                break;
                            case 2:
                                canva.FillEllipse(
                                    new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                                    Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204)),
                                    new Rectangle(new Point(AccurateX, AccurateY),
                                    new Size(Standard.CDiameter, Standard.CDiameter)));
                                break;
                        }
                    }
            }
        }

        internal class Standard //关于参数
        {
            //窗体数据
            public const int Width = 450, Height = 560;
            public const int PosX = 400, PosY = 75;
            //棋盘数据
            public const int CWidth = 410, CHeight = 410;
            public const int CPosX = 12, CPosY = 69;
            public const int CGap = 30;                                 // 棋格宽度
            public const int CDiameter = 20;                            // 棋子宽度（直径）
        }
        internal static void InitChess()
        {
            //isplaying = false;
            isplaying = true;
            turn = false;
            myturn = true;//myturn = 服务器获取
            for (int i = 0; i <= size; i++)
                for (int j = 0; j <= size; j++) ChessBoard[i, j] = 0;
            //pictureBox_stand.BackgroundImage = "黑子.png";
            //pictureBox_turn.BackgroundImage = "黑子.png";
        }
        internal static bool WinJudge(byte[,] ChessBoard, byte content)   // 输赢判断,参数为棋盘数组和新子颜色
        {
            for (int i = 1; i <= size; i++)
                for (int j = 1; j <= size; j++)
                {
                    if (ChessBoard[j, i] == content)
                    {
                        if (j < 11)
                            if (ChessBoard[j + 1, i] == content
                            && ChessBoard[j + 2, i] == content
                            && ChessBoard[j + 3, i] == content
                            && ChessBoard[j + 4, i] == content) return true;

                        if (i < 11)
                            if (ChessBoard[j, i + 1] == content
                            && ChessBoard[j, i + 2] == content
                            && ChessBoard[j, i + 3] == content
                            && ChessBoard[j, i + 4] == content) return true;

                        if (j < 11 && i < 11)
                            if (ChessBoard[j + 1, i + 1] == content
                            && ChessBoard[j + 2, i + 2] == content
                            && ChessBoard[j + 3, i + 3] == content
                            && ChessBoard[j + 4, i + 4] == content) return true;

                        if (j > 3 && i < 11)
                            if (ChessBoard[j - 1, i + 1] == content
                            && ChessBoard[j - 2, i + 2] == content
                            && ChessBoard[j - 3, i + 3] == content
                            && ChessBoard[j - 4, i + 4] == content) return true;
                    }
                }
            return false;
        }
    }
}
