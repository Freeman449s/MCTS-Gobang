using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҳ㻹�ǵ��Բ�
/// ��Ҳ���ӽ����ѡ��UCB��С��һ�������Բ���ӽ����ѡ��UCB����һ��
/// �����Բ�ĵ÷�ȡ������ͳһѡ���ӽ����UCB����һ��
/// </summary>
public enum LevelType { Player, Computer };

public class Node : MonoBehaviour {
    int nSimulationTimes = 0;
    int nWinTimes = 0;
    float UCB = 0;
    readonly LevelType type; // readonly�ֶ�ֻ��������ʱ���캯���ڸ�ֵ
    MCTSGameState gameState; // ��ǰ�ڵ��Ӧ����Ϸ״̬

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
    /// ���������С�㷨ѡ���ӽڵ�
    /// </summary>
    /// <returns></returns>
    public Node selectChild() {
        // TODO
        return null;
    }
}
