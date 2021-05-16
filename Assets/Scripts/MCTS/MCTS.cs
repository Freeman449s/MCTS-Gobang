public class MCTS
{
    readonly Node root = null;
    readonly int N_SIMULATION_TIMES = 10000; // �������Ľڵ���

    public MCTS(MCTSGameState gameState, int nSimulationTimes)
    {
        root = new Node(LevelType.Computer, gameState, null);
        N_SIMULATION_TIMES = nSimulationTimes;
    }

    /// <summary>
    /// ������һ�����߷�
    /// </summary>
    /// <returns>��һ�����߷�</returns>
    public MCTSGameMove decideMove()
    {
        generateGameTree();
        Node selectedChild = root.selectChildWithMaxUCB();
        return selectedChild.gameState.returnLastMove();
    }

    /// <summary>
    /// ����ָ������������������Ϸ��
    /// </summary>
    void generateGameTree()
    {
        for (int i = 1; i <= N_SIMULATION_TIMES; i++) performOnePass();
    }

    /// <summary>
    /// ִ��һ����������
    /// </summary>
    void performOnePass()
    {
        Node selected = select(root);
        Node child = expand(selected); // selectedΪ�վֽڵ�ʱ����null
        bool computerWon;
        if (child != null) computerWon = simulate(child);
        else computerWon = (selected.gameState.judgeLastMove() == GameResult.ComputerWon); // �վֽڵ�
        backPropagate(child ?? selected, computerWon); // child != null ? child : selected
    }

    /// <summary>
    /// ���������С�㷨��ѡ���һ���д���δչ���ӽڵ�Ľڵ�
    /// </summary>
    /// <param name="root">���ڵ�</param>
    /// <returns>��һ���д���δչ���ڵ�Ľڵ㡣�����������Ľڵ�ʱ������һ��Ҷ�ڵ㡣</returns>
    static Node select(Node root)
    {
        return recurSelect(root);
    }

    /// <summary>
    /// ��չ����ڵ�
    /// </summary>
    /// <param name="node">����չ�ڵ�</param>
    /// <returns>��չ�ɹ������ӽڵ㣻��������δ��չ�ӽڵ㣬���Ѿ����վֽڵ㣬�򷵻�null</returns>
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
    /// �ӵ�ǰ��Ϸ״̬��ʼ��ʹ���������ģ��һ����������Ϸ����֪��Ϸ���
    /// ������µ�ǰ�ڵ��״̬
    /// </summary>
    /// <param name="node">���ĸ��ڵ�Ļ�����ģ��</param>
    /// <returns>�˴�ģ���У������Ƿ�ʤ��</returns>
    static bool simulate(Node node)
    {
        return node.gameState.simulate();
    }

    /// <summary>
    /// �ӵ�ǰ�ڵ㿪ʼ�����򴫲�ģ����
    /// �����´˽ڵ��״̬
    /// </summary>
    /// <param name="node">���򴫲����</param>
    /// <param name="won">�����Ƿ�ʤ��</param>
    static void backPropagate(Node node, bool won)
    {
        recurBackPropagate(node, won);
    }

    // ========================================== �ݹ鹦�ܺ��� ==========================================
    /// <summary>
    /// �ݹ�ѡ���ӽڵ�
    /// </summary>
    /// <param name="cur">��ǰ�ڵ�</param>
    /// <returns>���شӵ�ǰ�ڵ㿪ʼ�������ҵ��ĵ�һ���д���δչ���ӽڵ�Ľڵ㡣�п��ܷ����վֽڵ㡣</returns>
    static Node recurSelect(Node cur)
    {
        if (cur.existsUnexpandedChild()) return cur; // ����δ��չ�ӽڵ�
        else if (cur.isLeaf()) return cur; // �վֽڵ�
        else return recurSelect(cur.selectChildWithMaxUCB());
    }

    /// <summary>
    /// �ݹ鷴�򴫲�
    /// </summary>
    static void recurBackPropagate(Node cur, bool won)
    {
        cur.updateStatus(won);
        if (cur.parent != null) recurBackPropagate(cur.parent, won);
    }
}