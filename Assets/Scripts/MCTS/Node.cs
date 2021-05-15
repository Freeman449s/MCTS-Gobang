using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家层还是电脑层
/// 玩家层从子结点中选择UCB最小的一个，电脑层从子结点中选择UCB最大的一个
/// 若电脑层的得分取负，则统一选择子结点中UCB最大的一个
/// </summary>
public enum LevelType
{
    Player,
    Computer
};

public class Node
{
    int nSimulationTimes = 0;
    int nWinTimes = 0;
    float UCB = 0;
    readonly LevelType type; // readonly字段只能在声明时或构造函数内赋值，但对对象而言，仍然能够调用它本身提供的方法
    internal readonly MCTSGameState gameState = null; // 当前节点对应的游戏状态

    internal readonly Node parent = null;
    readonly List<Node> children = new List<Node>();

    public Node(LevelType type, MCTSGameState gameState, Node parent)
    {
        this.type = type;
        this.parent = parent;
        this.gameState = gameState;
    }

    /// <summary>
    /// 依据最近一次模拟是否胜利，更新节点状态
    /// </summary>
    /// <param name="won">最近一次模拟是否胜利</param>
    public void updateStatus(bool won)
    {
        nSimulationTimes++;
        if (won)
        {
            if (type == LevelType.Player) nWinTimes++;
            else nWinTimes--;
        }

        UCB = nWinTimes * 1.0f / nSimulationTimes + Mathf.Sqrt(Mathf.Log(parent.nSimulationTimes) / nSimulationTimes);
    }

    /// <summary>
    /// 依据最大最小算法选择子节点
    /// 由于电脑层的得分取负，统一选择UCB最大的子节点
    /// </summary>
    /// <returns>返回UCB最大的子节点。没有子节点将返回null</returns>
    public Node selectChildWithMaxUCB()
    {
        if (children.Count == 0) return null;
        Node childWithMaxUCB = children[0];
        foreach (Node child in children)
        {
            if (child.UCB > childWithMaxUCB.UCB) childWithMaxUCB = child;
        }

        return childWithMaxUCB;
    }

    /// <summary>
    /// 检查当前节点是否还存在未展开的子节点
    /// </summary>
    /// <returns>true, 如果存在未展开的子节点</returns>
    public bool existsUnexpandedChild()
    {
        return gameState.existsUnexpandedChild(getChildrenGameStates());
    }

    /// <summary>
    /// 返回当前所有子节点的游戏状态列表
    /// </summary>
    /// <returns>当前所有子节点的游戏状态列表</returns>
    public List<MCTSGameState> getChildrenGameStates()
    {
        List<MCTSGameState> childrenStates = new List<MCTSGameState>();
        foreach (var child in children)
        {
            childrenStates.Add(child.gameState);
        }

        return childrenStates;
    }

    // ========================================== 工具函数或读取器 ==========================================
    public void appendChild(Node child)
    {
        children.Add(child);
    }

    public LevelType getLevelType()
    {
        return type;
    }

    public bool isLeaf()
    {
        return children.Count == 0;
    }

    // ========================================== 闲置函数 ==========================================
    /// <summary>
    /// 从当前游戏状态出发，向前模拟一步，并将带有新状态的节点作为当前节点的子节点
    /// </summary>
    public void expand()
    {
        if (!existsUnexpandedChild()) return;
        MCTSGameState stateOfChild = gameState.expand(getChildrenGameStates());
        LevelType typeOfChild = type == LevelType.Computer ? LevelType.Player : LevelType.Computer;
        Node child = new Node(typeOfChild, stateOfChild, this);
        children.Add(child);
    }
}