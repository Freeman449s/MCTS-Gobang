using System;

namespace Gobang
{
    public class GobangMove : MCTSGameMove, ICloneable
    {
        internal readonly GobangPoint point;

        public GobangMove(GobangPoint point)
        {
            this.point = point;
        }

        public object Clone()
        {
            return new GobangMove((GobangPoint) point.Clone());
        }
    }
}