
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// AStar 中点的定义
    /// </summary>
    public class Point
    {
        // 父亲节点
        public Point Parent { get; set; }

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
        public Point(int x, int y, GameObject go = null, Point parent = null, Vector3 position = default)
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
        public void UpdateParent(Point parent, float g)
        {
            this.Parent = parent;
            this.G = g;
            F = G + H;
        }
    }
}
