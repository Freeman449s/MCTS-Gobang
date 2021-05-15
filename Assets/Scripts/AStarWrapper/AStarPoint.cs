
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// AStar 中点的定义
    /// </summary>
    public class AStarPoint
    {
        // 父亲节点
        public AStarPoint Parent { get; set; }

        // F G H 值
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        // 坐标值
        public int X { get; set; }
        public int Y { get; set; }

        // 是否是障碍物（例如墙）
        public bool IsWall { get; set; }

        // 该点的游戏物体（根据需要可不用可删除）
        public GameObject gameObject;
        // 该点的空间位置（根据需要可不用可删除）
        public Vector3 position;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="go"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        public AStarPoint(int x, int y, GameObject go = null, AStarPoint parent = null, Vector3 position = default)
        {
            this.X = x;
            this.Y = y;
            this.gameObject = go;
            this.position = position;
            this.Parent = parent;
            IsWall = false;
        }

        /// <summary>
        /// 更新G，F 值，和父亲节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="g"></param>
        public void UpdateParent(AStarPoint parent, float g)
        {
            this.Parent = parent;
            this.G = g;
            F = G + H;
        }
    }
}
