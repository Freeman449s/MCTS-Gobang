using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҳ㻹�ǵ��Բ�
/// ��Ҳ���ӽ����ѡ��UCB��С��һ�������Բ���ӽ����ѡ��UCB����һ��
/// �����Բ�ĵ÷�ȡ������ͳһѡ���ӽ����UCB����һ��
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
    readonly LevelType levelType; // readonly�ֶ�ֻ��������ʱ���캯���ڸ�ֵ��������Ƕ�����Ȼ�ܹ������������ṩ�ķ���
    internal readonly MCTSGameState gameState = null; // ��ǰ�ڵ��Ӧ����Ϸ״̬

    internal readonly Node parent = null;
    readonly List<Node> children = new List<Node>();

    public Node(LevelType levelType, MCTSGameState gameState, Node parent)
    {
        this.levelType = levelType;
        this.parent = parent;
        this.gameState = gameState;
    }

    /// <summary>
    /// �������һ��ģ���Ƿ�ʤ�������½ڵ�״̬
    /// </summary>
    /// <param name="won">���һ��ģ���Ƿ�ʤ��</param>
    public void updateStatus(bool won)
    {
        nSimulationTimes++;
        if (won)
        {
            if (levelType == LevelType.Player) nWinTimes++;
            else nWinTimes--;
        }

        if (parent != null) // ���ڵ������¼UCB
        {
            UCB = nWinTimes * 1.0f / nSimulationTimes +
                  Mathf.Sqrt(Mathf.Log(parent.nSimulationTimes) / nSimulationTimes);
        }
    }

    /// <summary>
    /// ���������С�㷨ѡ���ӽڵ�
    /// ���ڵ��Բ�ĵ÷�ȡ����ͳһѡ��UCB�����ӽڵ�
    /// </summary>
    /// <returns>����UCB�����ӽڵ㡣û���ӽڵ㽫����null</returns>
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
    /// ��鵱ǰ�ڵ��Ƿ񻹴���δչ�����ӽڵ�
    /// </summary>
    /// <returns>true, �������δչ�����ӽڵ�</returns>
    public bool existsUnexpandedChild()
    {
        return gameState.existsUnexpandedChild(getChildrenGameStates());
    }

    /// <summary>
    /// ���ص�ǰ�����ӽڵ����Ϸ״̬�б�
    /// </summary>
    /// <returns>��ǰ�����ӽڵ����Ϸ״̬�б�</returns>
    public List<MCTSGameState> getChildrenGameStates()
    {
        List<MCTSGameState> childrenStates = new List<MCTSGameState>();
        foreach (var child in children)
        {
            childrenStates.Add(child.gameState);
        }

        return childrenStates;
    }

    // ========================================== ���ߺ������ȡ�� ==========================================
    public void appendChild(Node child)
    {
        children.Add(child);
    }

    public LevelType getLevelType()
    {
        return levelType;
    }

    public bool isLeaf()
    {
        return children.Count == 0;
    }

    // ========================================== ���ú��� ==========================================
    /// <summary>
    /// �ӵ�ǰ��Ϸ״̬��������ǰģ��һ��������������״̬�Ľڵ���Ϊ��ǰ�ڵ���ӽڵ�
    /// </summary>
    public void expand()
    {
        if (!existsUnexpandedChild()) return;
        MCTSGameState stateOfChild = gameState.expand(getChildrenGameStates());
        LevelType typeOfChild = levelType == LevelType.Computer ? LevelType.Player : LevelType.Computer;
        Node child = new Node(typeOfChild, stateOfChild, this);
        children.Add(child);
    }
}