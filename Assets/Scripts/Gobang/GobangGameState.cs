using System;
using System.Collections.Generic;

namespace Gobang
{
    /// <summary>
    /// 落子类型
    /// </summary>
    public enum PieceType
    {
        Unplaced = 0, // 定义默认值
        Player,
        Computer
    };

    public class GobangGameState : MCTSGameState
    {
        private readonly int boardWidth;
        private readonly int boardHeight;
        internal GobangPoint[,] boardState; // 棋盘，使用C#二维数组
        private readonly GobangMove lastMove = null;

        /// <summary>
        /// 从已有的游戏状态构建对象
        /// </summary>
        /// <param name="boardState">已有游戏状态</param>
        /// <param name="lastMove">最后一步何者落子</param>
        public GobangGameState(GobangPoint[,] boardState, GobangMove lastMove)
        {
            this.boardState = boardState;
            boardWidth = boardState.GetLength(0);
            boardHeight = boardState.GetLength(1);
            this.lastMove = lastMove;
        }

        /// <summary>
        /// 创建全新的棋盘
        /// </summary>
        /// <param name="boardWidth">棋盘宽度</param>
        /// <param name="boardHeight">棋盘高度</param>
        public GobangGameState(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            boardState = new GobangPoint[boardWidth, boardHeight];
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    boardState[x, y] = new GobangPoint();
                }
            }

            lastMove = null;
        }

        public MCTSGameMove returnLastMove()
        {
            return lastMove;
        }

        public bool existsUnexpandedChild(List<MCTSGameState> children)
        {
            throw new NotImplementedException();
        }

        public MCTSGameState expand(List<MCTSGameState> existingChildren)
        {
            throw new NotImplementedException();
        }

        public bool simulate()
        {
            throw new NotImplementedException();
        }

        public GameResult judgeLastMove()
        {
            throw new NotImplementedException();
        }

        // ========================================== 工具函数或读取器 ==========================================
        public PieceType getPieceType(int x, int y)
        {
            return boardState[x, y].pieceType;
        }

        public PieceType getPieceType(int[] coord)
        {
            return getPieceType(coord[0], coord[1]);
        }
    }
}