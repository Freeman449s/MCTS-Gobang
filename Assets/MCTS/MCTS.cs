using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS : MonoBehaviour
{
    readonly Node root = null;
    readonly int N_SIMULATION_TIMES = 10000;

    // TODO: ����Ȩ���޸�
    
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

    public void makeMove()
    {
        // TODO
    }

    public void generateGameTree()
    {
        // TODO
    }

    /// <summary>
    /// ���������С�㷨��ѡ���һ���д���δչ���ӽڵ�Ľڵ�
    /// </summary>
    /// <returns>��һ���д���δչ���ڵ�Ľڵ㡣�����������Ľڵ�ʱ������һ��Ҷ�ڵ㡣</returns>
    public Node select()
    {
        return recurSelect(root);
    }

    /// <summary>
    /// ��չ����ڵ�
    /// </summary>
    /// <param name="node">����չ�ڵ�</param>
    /// <returns>��չ�ɹ�����true����������δ��չ�ӽڵ㣬���Ѿ���Ҷ�ڵ㣬�򷵻�false</returns>
    public static bool expand(Node node)
    {
        if (node.existsUnexpandedChild())
        {
            MCTSGameState stateOfChild = node.gameState.expand(node.getChildrenGameStates());
            LevelType typeOfChild = node.getLevelType() == LevelType.Computer ? LevelType.Player : LevelType.Computer;
            Node child = new Node(typeOfChild, stateOfChild, node);
            node.appendChild(child);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// �ӵ�ǰ��Ϸ״̬��ʼ��ʹ���������ģ��һ����������Ϸ����֪��Ϸ���
    /// ������µ�ǰ�ڵ��״̬
    /// </summary>
    /// <param name="node">���ĸ��ڵ�Ļ�����ģ��</param>
    /// <returns>�˴�ģ���У������Ƿ�ʤ��</returns>
    public bool simulate(Node node)
    {
        return node.gameState.simulate();
    }

    /// <summary>
    /// �ӵ�ǰ�ڵ㿪ʼ�����򴫲�ģ����
    /// �����´˽ڵ��״̬
    /// </summary>
    /// <param name="node">���򴫲����</param>
    /// <param name="won">�����Ƿ�ʤ��</param>
    public void backPropagate(Node node, bool won)
    {
        recurBackPropagate(node, won);
    }

    // ============================== �ݹ鹦�ܺ��� ==============================
    /// <summary>
    /// �ݹ�ѡ���ӽڵ�
    /// </summary>
    /// <param name="cur">��ǰ�ڵ�</param>
    /// <returns>���شӵ�ǰ�ڵ㿪ʼ�������ҵ��ĵ�һ���д���δչ���ӽڵ�Ľڵ㡣�����������Ľڵ�ʱ������һ��Ҷ�ڵ㡣</returns>
    Node recurSelect(Node cur)
    {
        if (cur.existsUnexpandedChild()) return cur;
        else return recurSelect(cur.selectChildWithMaxUCB());
    }

    /// <summary>
    /// �ݹ鷴�򴫲�
    /// </summary>
    void recurBackPropagate(Node cur, bool won)
    {
        cur.updateStatus(won);
        if (cur.parent != null) recurBackPropagate(cur.parent, won);
    }
}