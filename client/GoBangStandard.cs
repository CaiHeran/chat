using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client//改Client
{
    //数据
    internal class GoBangStandard
    {
        //窗体数据
        public const int Width = 450, Height = 560;
        public const int PosX = 400, PosY = 75;

        //棋盘数据
        public const int CWidth = 410, CHeight = 410;
        public const int CPosX = 12, CPosY = 69;

        public const int CGap = 30;//棋格宽度
        public const int CDiameter = 20;//棋子宽度（直径）

        //胜利判断
        public static bool WinJudge(byte[,] chessboard, int content)
        {
            for (int i = 1; i <= FormGoBang.size+1; i++)
                for (int j = 1; j <= FormGoBang.size+1; j++)
                {
                    if (chessboard[i, j] == content)
                    {
                        // 水平
                        if (j < 11)
                        if (chessboard[j + 1, i] == content
                            && chessboard[j + 2, i] == content
                            && chessboard[j + 3, i] == content
                            && chessboard[j + 4, i] == content) return true;
                        // 垂直
                        if (i < 11)
                        if (chessboard[j, i + 1] == content
                            && chessboard[j, i + 2] == content
                            && chessboard[j, i + 3] == content
                            && chessboard[j, i + 4] == content) return true;
                        // 斜向右下
                        if (j < 11 && i < 11)
                        if (chessboard[j + 1, i + 1] == content
                            && chessboard[j + 2, i + 2] == content
                            && chessboard[j + 3, i + 3] == content
                            && chessboard[j + 4, i + 4] == content) return true;
                    }
                        // 斜向左下
                        if (j > 3 && i < 11)
                        if (chessboard[j - 1, i + 1] == content
                            && chessboard[j - 2, i + 2] == content
                            && chessboard[j - 3, i + 3] == content
                            && chessboard[j - 4, i + 4] == content) return true;
                }
            return false;
        }
    }
}
