using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;


namespace client//改Client
{
    internal class GoBangPawn
    {
        public static void DrawPawn(Panel p, bool turn, int PlacementX, int PlacementY)
        {
            Graphics canva = p.CreateGraphics();// 创建面板画布

            // 消除棋子边缘的锯齿
            canva.SmoothingMode = SmoothingMode.HighQuality;
            canva.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //精确棋子的中心位置
            int AccurateX = PlacementX * GoBangStandard.CGap + 20 - 17;
            int AccurateY = PlacementY * GoBangStandard.CGap + 20 - 17;

            // 画棋子
            // 线性渐变，根据位置填颜色，从上至下渐变，使棋子具有真实感
            if (turn) //画黑子
                canva.FillEllipse(new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                    Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0)),
                    new Rectangle(
                        new Point(AccurateX, AccurateY),
                        new Size(GoBangStandard.CDiameter, GoBangStandard.CDiameter)));
            else //画白子
                canva.FillEllipse(new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                    Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204)), 
                    new Rectangle(
                        new Point(AccurateX, AccurateY),
                        new Size(GoBangStandard.CDiameter, GoBangStandard.CDiameter)));
        }

        // 界面重新聚焦时，重新加载棋子
        public static void LoadPawn(Panel p, byte[,] chessboard)
        {
            Graphics canva = p.CreateGraphics();// 创建面板画布
            // 消除棋子边缘的锯齿
            canva.SmoothingMode = SmoothingMode.HighQuality;
            canva.InterpolationMode = InterpolationMode.HighQualityBicubic;

            for (int i = 0; i < chessboard.GetLength(1); i++)
                for (int j = 0; j < chessboard.GetLength(0); j++)
                {
                    // 获取某位置棋子状态
                    // 0表示没有，1表示黑子，2表示白子
                    byte content = chessboard[j, i];
                    // 计算位置
                    int AccurateX = j * GoBangStandard.CGap + 20 - 17;
                    int AccurateY = i * GoBangStandard.CGap + 20 - 17;
                    switch (content)
                    {
                        case 0:
                            break;
                        case 1:
                            canva.FillEllipse(
                                new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                                Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0)),
                                new Rectangle(new Point(AccurateX, AccurateY),
                                new Size(GoBangStandard.CDiameter, GoBangStandard.CDiameter)));
                            break;
                        case 2:
                            canva.FillEllipse(
                                new LinearGradientBrush(new Point(20, 0), new Point(20, 40),
                                Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204)),
                                new Rectangle(new Point(AccurateX, AccurateY),
                                new Size(GoBangStandard.CDiameter, GoBangStandard.CDiameter)));
                            break;
                    }
                }
        }
    }
}
