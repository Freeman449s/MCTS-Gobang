using UnityEngine;

namespace Gobang
{
    /// <summary>
    /// 棋盘格子
    /// </summary>
    public class GobangPoint
    {
        internal PieceType pieceType;
        internal GameObject gameObj = null;

        public GobangPoint(PieceType pieceType, GameObject gameObj)
        {
            this.pieceType = pieceType;
            this.gameObj = gameObj;
        }

        public GobangPoint() : this(PieceType.Unplaced, null)
        {
        }
    }
}