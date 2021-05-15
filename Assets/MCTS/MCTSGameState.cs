using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: �ȴ�ʵ��

/// <summary>
/// ��ʤ״��
/// </summary>
public enum GameResult
{
    PlayerWon,
    ComputerWon,
    Undefined
};

public abstract class MCTSGameState
{
    internal readonly MCTSGameMove lastMove;
    
    public MCTSGameState(MCTSGameMove lastMove)
    {
        this.lastMove = lastMove;
    }

    /// <summary>
    /// ��鵱ǰ��Ϸ״̬�Ƿ������δչ������״̬
    /// </summary>
    /// <param name="children">��չ������״̬</param>
    /// <returns>������δչ������״̬ʱ����true</returns>
    public abstract bool existsUnexpandedChild(List<MCTSGameState> children);

    /// <summary>
    /// ����ؽ���Ϸ�ƽ�һ��
    /// </summary>
    /// <param name="existingChildren">�Ѿ����ڵ���״̬</param>
    /// <returns>�ƽ�һ�������Ϸ״̬</returns>
    public abstract MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// �ӵ�ǰ��Ϸ״̬��ʼ��ʹ���������ģ��һ����������Ϸ����֪��Ϸ���
    /// </summary>
    /// <returns>�˴�ģ���У������Ƿ�ʤ��</returns>
    public abstract bool simulate();

    /// <summary>
    /// �������һ��������ʤ����
    /// </summary>
    /// <returns>�������ʤ���򷵻�true</returns>
    public abstract GameResult judgeLastMove();
}