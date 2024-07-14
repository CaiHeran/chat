using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client//改成Client
{
    internal class GoBangBoard
    {
        // 画棋盘方法
        public static void DrawBoard(Graphics canva)  //参数是画布对象
        {
            int GapWidth = GoBangStandard.CGap;                   // 棋格宽度
            int GapNum = GoBangStandard.CWidth / GapWidth - 1;    // 棋格数量
            /*
            Bitmap image = new Bitmap("棋盘.png"); // 棋盘位图路径
            canva.DrawImage(image, new Point[] {
                new Point(0, 0),
                new Point(GapWidth * GapNum + 44, 0),
                new Point(0, GapWidth * GapNum + 44)
            }); // 画棋盘图片
            */

            // 用“画笔”画棋盘
            canva.Clear(Color.Transparent);// 清除画布、并设置画布透明——Requring fixing
            Pen pen = new Pen(Color.FromArgb(192, 166, 107));// 设置画笔棕色
            for (int i = 0; i < GapNum + 1; i++)//绘制棋盘
            {
                canva.DrawLine(pen, new Point(20, i * GapWidth + 20), new Point(GapWidth * GapNum + 20, i * GapWidth + 20));
                canva.DrawLine(pen, new Point(i * GapWidth + 20, 20), new Point(i * GapWidth + 20, GapWidth * GapNum + 20));
            }
        }
    }
}
