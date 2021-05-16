using System;
using UnityEngine;

namespace Gobang
{
    /// <summary>
    /// 棋盘格子
    /// </summary>
    public class GobangPoint : ICloneable
    {
        internal readonly int[] coord = null;
        internal PieceType pieceType = PieceType.Unplaced;
        internal GameObject gameObj = null;

        public GobangPoint(int[] coord, PieceType pieceType = PieceType.Unplaced, GameObject gameObj = null)
        {
            this.coord = coord;
            this.pieceType = pieceType;
            this.gameObj = gameObj;
        }

        // ======================================== 重写的函数 ========================================
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is GobangPoint)) return false;
            GobangPoint other = (GobangPoint) obj;
            return this.coord[0] == other.coord[0] && this.coord[1] == other.coord[1];
        }

        public override int GetHashCode()
        {
            return coord[0] + coord[1];
        }

        public object Clone()
        {
            int[] coordClone = {coord[0], coord[1]};
            return new GobangPoint(coordClone, pieceType, gameObj);
        }
    }
}