using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS : MonoBehaviour
{
    readonly Node root = null;
    readonly int N_SIMULATION_TIMES = 10000;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public MCTS(MCTSGameState gameState, int nSimulationTimes)
    {
        root = new Node(LevelType.Computer, gameState, null);
        N_SIMULATION_TIMES = nSimulationTimes;
    }

    /// <summary>
    /// 产生下一步的走法
    /// </summary>
    /// <returns>下一步的走法</returns>
    public MCTSGameMove makeMove()
    {
        generateGameTree();
        Node selectedChild = root.selectChildWithMaxUCB();
        return selectedChild.gameState.lastMove;
    }

    /// <summary>
    /// 按照指定的搜索次数构建游戏树
    /// </summary>
    void generateGameTree()
    {
        for (int i = 1; i <= N_SIMULATION_TIMES; i++) performOnePass();
    }

    /// <summary>
    /// 执行一次搜索流程
    /// </summary>
    void performOnePass()
    {
        Node selected = select(root);
        Node child = expand(selected); // selected为终局节点时返回null
        bool computerWon;
        if (child != null) computerWon = simulate(child);
        else computerWon = (selected.gameState.judgeLastMove() == GameResult.ComputerWon); // 终局节点
        backPropagate(child ?? selected, computerWon); // child != null ? child : selected
    }

    /// <summary>
    /// 依据最大最小算法，选择第一个尚存在未展开子节点的节点
    /// </summary>
    /// <param name="root">根节点</param>
    /// <returns>第一个尚存在未展开节点的节点。不存在这样的节点时，返回一个叶节点。</returns>
    static Node select(Node root)
    {
        return recurSelect(root);
    }

    /// <summary>
    /// 扩展传入节点
    /// </summary>
    /// <param name="node">待扩展节点</param>
    /// <returns>扩展成功返回子节点；若不存在未扩展子节点，或已经是终局节点，则返回null</returns>
    static Node expand(Node node)
    {
        if (node.existsUnexpandedChild())
        {
            MCTSGameState stateOfChild = node.gameState.expand(node.getChildrenGameStates());
            LevelType typeOfChild = node.getLevelType() == LevelType.Computer ? LevelType.Player : LevelType.Computer;
            Node child = new Node(typeOfChild, stateOfChild, node);
            node.appendChild(child);
            return child;
        }
        else return null;
    }

    /// <summary>
    /// 从当前游戏状态开始，使用随机方法模拟一次完整的游戏，告知游戏结果
    /// 不会更新当前节点的状态
    /// </summary>
    /// <param name="node">在哪个节点的基础上模拟</param>
    /// <returns>此次模拟中，电脑是否胜利</returns>
    static bool simulate(Node node)
    {
        return node.gameState.simulate();
    }

    /// <summary>
    /// 从当前节点开始，反向传播模拟结果
    /// 将更新此节点的状态
    /// </summary>
    /// <param name="node">反向传播起点</param>
    /// <param name="won">电脑是否胜利</param>
    static void backPropagate(Node node, bool won)
    {
        recurBackPropagate(node, won);
    }

    // ============================== 递归功能函数 ==============================
    /// <summary>
    /// 递归选择子节点
    /// </summary>
    /// <param name="cur">当前节点</param>
    /// <returns>返回从当前节点开始，向下找到的第一个尚存在未展开子节点的节点。有可能返回终局节点。</returns>
    static Node recurSelect(Node cur)
    {
        if (cur.existsUnexpandedChild()) return cur; // 存在未扩展子节点
        else if (cur.isLeaf()) return cur; // 终局节点
        else return recurSelect(cur.selectChildWithMaxUCB());
    }

    /// <summary>
    /// 递归反向传播
    /// </summary>
    static void recurBackPropagate(Node cur, bool won)
    {
        cur.updateStatus(won);
        if (cur.parent != null) recurBackPropagate(cur.parent, won);
    }
}