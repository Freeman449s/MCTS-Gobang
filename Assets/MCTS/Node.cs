using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家层还是电脑层
/// 玩家层从子结点中选择UCB最小的一个，电脑层从子结点中选择UCB最大的一个
/// 若电脑层的得分取负，则统一选择子结点中UCB最大的一个
/// </summary>
public enum LevelType { Player, Computer };

public class Node : MonoBehaviour {
    int nSimulationTimes = 0;
    int nWinTimes = 0;
    float UCB = 0;
    readonly LevelType type; // readonly字段只能在声明时或构造函数内赋值
    MCTSGameState gameState; // 当前节点对应的游戏状态

    readonly Node parent = null;
    List<Node> children = new List<Node>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    
    public Node(LevelType type, Node parent) {
        this.type = type;
        this.parent = parent;
    }

    public void updateStatus(bool wins) {
        nSimulationTimes++;
        if (wins) {
            if (type == LevelType.Player) nWinTimes++;
            else nWinTimes--;
        }
        UCB = nWinTimes * 1.0f / nSimulationTimes + Mathf.Sqrt(Mathf.Log(parent.nSimulationTimes) / nSimulationTimes);
    }

    /// <summary>
    /// 依据最大最小算法选择子节点
    /// </summary>
    /// <returns></returns>
    public Node selectChild() {
        // TODO
        return null;
    }
}
