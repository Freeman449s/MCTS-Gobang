namespace Gobang
{
    public class GobangMove : MCTSGameMove
    {
        internal int[,] gameBoardCoord; // 棋盘坐标
        internal PieceType pieceType; // 何者落子

        public GobangMove(int[,] gameBoardCoord, PieceType pieceType)
        {
            this.gameBoardCoord = gameBoardCoord;
            this.pieceType = pieceType;
        }
    }
}