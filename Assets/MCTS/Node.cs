using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    /// <summary>
    /// 玩家层还是电脑层
    /// 玩家层从子结点中选择UCB最小的一个，电脑层从子结点中选择UCB最大的一个
    /// 若电脑层的得分取负，则统一选择子结点中UCB最大的一个
    /// </summary>
    internal enum LevelType { Player, Computer };

    int nSimulationTimes = 0;
    int nWinTimes = 0;
    float UCB = 0;
    LevelType type;

    Node parent = null;
    List<Node> children = new List<Node>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    internal Node(LevelType type, Node parent) {
        this.type = type;
        this.parent = parent;
    }

    internal void updateStatus(bool wins) {
        nSimulationTimes++;
        if (wins) {
            if (type == LevelType.Player) nWinTimes++;
            else nWinTimes--;
        }
        // TODO 计算UCB
    }
}