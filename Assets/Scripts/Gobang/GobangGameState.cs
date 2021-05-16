using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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

    public class GobangGameState : MCTSGameState, ICloneable
    {
        private readonly int boardSideLength;
        internal readonly GobangPoint[,] boardState; // 棋盘，使用C#二维数组
        private readonly HashSet<GobangPoint> unplacedPoints = null;
        private GobangMove lastMove = null;

        /// <summary>
        /// 从已有的游戏状态构建对象
        /// </summary>
        /// <param name="boardState">已有游戏状态</param>
        /// <param name="lastMove">最后一步何者落子</param>
        public GobangGameState(GobangPoint[,] boardState, GobangMove lastMove)
        {
            this.boardState = boardState;
            boardSideLength = boardState.GetLength(0);
            this.lastMove = lastMove;
            unplacedPoints = findUnplacedPoints(this.boardState);
        }

        /// <summary>
        /// 创建全新的棋盘
        /// </summary>
        /// <param name="boardSideLength">棋盘边长</param>
        public GobangGameState(int boardSideLength)
        {
            this.boardSideLength = boardSideLength;
            boardState = new GobangPoint[boardSideLength, boardSideLength];
            for (int x = 0; x < boardSideLength; x++)
            {
                for (int y = 0; y < boardSideLength; y++)
                {
                    int[] coord = {x, y};
                    boardState[x, y] = new GobangPoint(coord);
                }
            }

            lastMove = null;
            unplacedPoints = findUnplacedPoints(boardState);
        }

        public bool existsUnexpandedChild(List<MCTSGameState> existingChildren)
        {
            return findUnexpandedPoints(existingChildren).Count > 0;
        }

        public MCTSGameState expand(List<MCTSGameState> existingChildren)
        {
            // 随机选择落子位置
            HashSet<GobangPoint> unexpandedPoints = findUnexpandedPoints(existingChildren);
            GobangPoint point = randomlySelectPoint(unexpandedPoints);

            GobangGameState nextState = (GobangGameState) this.Clone();
            nextState.placePiece(new GobangMove(new GobangPoint(point.coord, PieceType.Computer, point.gameObj)));
            return nextState;
        }

        public bool simulate()
        {
            GameResult gameResult = judgeLastMove();
            if (gameResult == GameResult.ComputerWon) return true;

            // 建立模拟所需数据结构
            GobangGameState localGameState = (GobangGameState) this.Clone();
            /*GobangPoint[,] localBoardState = gameState.boardState; // 棋盘
            HashSet<GobangPoint> localUnplacedPoints = findUnplacedPoints(localBoardState);*/
            PieceType curPieceType = PieceType.Computer;
            // 模拟
            do
            {
                curPieceType = (curPieceType == PieceType.Computer ? PieceType.Player : PieceType.Computer);
                GobangPoint point = randomlySelectPoint(localGameState.unplacedPoints);
                point.pieceType = curPieceType;
                GobangMove move = new GobangMove(point);
                localGameState.placePiece(move);
                gameResult = localGameState.judgeLastMove();
            } while (gameResult == GameResult.NoOutcome);

            return gameResult == GameResult.ComputerWon;
        }
        
        /// <summary>
        /// 落子时调用的函数
        /// 改变落子位置的PieceType，将落子点从未落子点集中移除，并更新lastMove
        /// </summary>
        /// <param name="move">代表本次落子的对象</param>
        internal void placePiece(GobangMove move)
        {
            int[] coord = move.point.coord;
            boardState[coord[0], coord[1]].pieceType = move.point.pieceType;
            unplacedPoints.Remove(move.point);
            lastMove = move;
        }

        public GameResult judgeLastMove()
        {
            return judgeMove(boardState, lastMove);
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

        public MCTSGameMove returnLastMove()
        {
            return lastMove;
        }

        /// <summary>
        /// 找出棋盘上为空的位置
        /// </summary>
        /// <param name="curBoardState">当前棋盘状况</param>
        /// <returns>未落子位置组成的集合</returns>
        private static HashSet<GobangPoint> findUnplacedPoints(GobangPoint[,] curBoardState)
        {
            int sideLength = curBoardState.GetLength(0);
            HashSet<GobangPoint> unplacedPointSet = new HashSet<GobangPoint>();
            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    if (curBoardState[x, y].pieceType == PieceType.Unplaced) unplacedPointSet.Add(curBoardState[x, y]);
                }
            }

            return unplacedPointSet;
        }

        /// <summary>
        /// 依据传入的子节点，找出可展开的位置
        /// </summary>
        /// <param name="existingChildren">已经存在的子节点</param>
        /// <returns>可展开位置的集合</returns>
        private HashSet<GobangPoint> findUnexpandedPoints(List<MCTSGameState> existingChildren)
        {
            HashSet<GobangPoint> unplacedPointsCopy = new HashSet<GobangPoint>(unplacedPoints);
            foreach (var mctsChild in existingChildren)
            {
                GobangGameState child = (GobangGameState) mctsChild;
                unplacedPointsCopy.Remove(child.lastMove.point);
            }

            return unplacedPointsCopy;
        }

        public object Clone()
        {
            GobangPoint[,] stateClone = new GobangPoint[boardSideLength, boardSideLength];
            for (int x = 0; x < boardSideLength; x++)
            {
                for (int y = 0; y < boardSideLength; y++)
                {
                    stateClone[x, y] = (GobangPoint) boardState[x, y].Clone();
                }
            }

            GobangMove lastMoveClone = (GobangMove) lastMove.Clone();
            return new GobangGameState(stateClone, lastMoveClone);
        }

        /// <summary>
        /// 判断此次落子是否导致一方胜利，以及是哪一方胜利
        /// 函数不会检查在此次落子之前是否已经有一方胜利
        /// </summary>
        /// <param name="boardState">落子后的棋盘状态</param>
        /// <param name="move">此次落子</param>
        /// <returns>游戏结果：电脑胜利，玩家胜利，胜负未分或平局</returns>
        static GameResult judgeMove(GobangPoint[,] boardState, GobangMove move)
        {
            int[] lastPieceCoord = move.point.coord;
            int lastPieceX = lastPieceCoord[0];
            int lastPieceY = lastPieceCoord[1];
            PieceType lastPieceType = move.point.pieceType;
            bool wins = false;
            // 检查所需的一些边界量
            int startXMin = Mathf.Max(0, lastPieceX - 4);
            int startXMax = Mathf.Min(boardState.GetLength(0) - 1 - 4, lastPieceX);
            int startYMin = Mathf.Max(0, lastPieceY - 4);
            int startYMax = Mathf.Min(boardState.GetLength(0) - 1 - 4, lastPieceY);

            // 横向检查（忽略横纵坐标的转换）
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!wins)
            {
                for (int startX = startXMin; startX <= startXMax; startX++)
                {
                    if (boardState[startX, lastPieceY].pieceType == lastPieceType &&
                        boardState[startX + 1, lastPieceY].pieceType == lastPieceType &&
                        boardState[startX + 2, lastPieceY].pieceType == lastPieceType &&
                        boardState[startX + 3, lastPieceY].pieceType == lastPieceType &&
                        boardState[startX + 4, lastPieceY].pieceType == lastPieceType
                    )
                    {
                        wins = true;
                        break;
                    }
                }
            }

            // 纵向检查（忽略横纵坐标的转换）
            if (!wins)
            {
                for (int startY = startYMin; startY <= startYMax; startY++)
                {
                    if (boardState[lastPieceY, startY].pieceType == lastPieceType &&
                        boardState[lastPieceY, startY + 1].pieceType == lastPieceType &&
                        boardState[lastPieceY, startY + 2].pieceType == lastPieceType &&
                        boardState[lastPieceY, startY + 3].pieceType == lastPieceType &&
                        boardState[lastPieceY, startY + 4].pieceType == lastPieceType
                    )
                    {
                        wins = true;
                        break;
                    }
                }
            }

            // 左上向右下检查（假设原点位于左上角，忽略横纵坐标的对换）
            if (!wins)
            {
                int startPointMaxOffset = Mathf.Min(lastPieceX - startXMin, lastPieceY - startYMin); // 起点距离当前位置的最大偏移量
                int startPointMinOffset = Mathf.Max(lastPieceX - startXMax, lastPieceY - startYMax); // 起点距离当前位置的最小偏移量

                for (int delta = startPointMinOffset; delta <= startPointMaxOffset; delta++)
                {
                    int startX = lastPieceX - delta;
                    int startY = lastPieceY - delta;
                    if (boardState[startX, startY].pieceType == lastPieceType &&
                        boardState[startX + 1, startY + 1].pieceType == lastPieceType &&
                        boardState[startX + 2, startY + 2].pieceType == lastPieceType &&
                        boardState[startX + 3, startY + 3].pieceType == lastPieceType &&
                        boardState[startX + 4, startY + 4].pieceType == lastPieceType
                    )
                    {
                        wins = true;
                        break;
                    }
                }
            }

            // 左下向右上检查（假设原点位于左上角，忽略横纵坐标的对换）
            if (!wins)
            {
                // 特殊的检查方式，需要更新边界量
                startYMin = Mathf.Max(4, lastPieceY);
                startYMax = Mathf.Min(boardState.GetLength(0) - 1, lastPieceY + 4);
                int startPointMaxOffset = Mathf.Min(lastPieceX - startXMin, startYMax - lastPieceY); // 起点距离当前位置的最大偏移量
                int startPointMinOffset = Mathf.Max(lastPieceX - startXMax, startYMin - lastPieceY); // 起点距离当前位置的最小偏移量

                for (int delta = startPointMinOffset; delta <= startPointMaxOffset; delta++)
                {
                    int startX = lastPieceX - delta;
                    int startY = lastPieceY + delta;

                    if (boardState[startX, startY].pieceType == lastPieceType &&
                        boardState[startX + 1, startY - 1].pieceType == lastPieceType &&
                        boardState[startX + 2, startY - 2].pieceType == lastPieceType &&
                        boardState[startX + 3, startY - 3].pieceType == lastPieceType &&
                        boardState[startX + 4, startY - 4].pieceType == lastPieceType
                    )
                    {
                        wins = true;
                        break;
                    }
                }
            }

            if (wins) return lastPieceType == PieceType.Computer ? GameResult.ComputerWon : GameResult.PlayerWon;
            else
            {
                HashSet<GobangPoint> unplacedPoints = findUnplacedPoints(boardState);
                if (unplacedPoints.Count == 0) return GameResult.Draw;
                else return GameResult.NoOutcome;
            }
        }

        /// <summary>
        /// 从传入点集中随机选择一个点
        /// </summary>
        /// <param name="points">选择范围（点集）</param>
        /// <returns>代表被选中点的对象</returns>
        static GobangPoint randomlySelectPoint(ISet<GobangPoint> points)
        {
            List<GobangPoint> pointList = new List<GobangPoint>(points);
            Random random = new Random();
            int index = random.Next(pointList.Count);
            GobangPoint selected = pointList[index];
            return selected;
        }
    }
}