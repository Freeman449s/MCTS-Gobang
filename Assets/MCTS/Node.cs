using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    /// <summary>
    /// ��Ҳ㻹�ǵ��Բ�
    /// ��Ҳ���ӽ����ѡ��UCB��С��һ�������Բ���ӽ����ѡ��UCB����һ��
    /// �����Բ�ĵ÷�ȡ������ͳһѡ���ӽ����UCB����һ��
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
        // TODO ����UCB
    }
}