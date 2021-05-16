using System.Collections.Generic;

/// <summary>
/// ��ʤ״��
/// </summary>
public enum GameResult
{
    NoOutcome = 0,
    PlayerWon,
    ComputerWon,
    Draw
};

public interface MCTSGameState
{
    /// <summary>
    /// ��鵱ǰ��Ϸ״̬�Ƿ������δչ������״̬
    /// </summary>
    /// <param name="existingChildren">��չ������״̬</param>
    /// <returns>������δչ������״̬ʱ����true</returns>
    bool existsUnexpandedChild(List<MCTSGameState> existingChildren);

    /// <summary>
    /// ����ؽ���Ϸ�ƽ�һ��
    /// </summary>
    /// <param name="existingChildren">�Ѿ����ڵ���״̬</param>
    /// <returns>�ƽ�һ�������Ϸ״̬</returns>
    MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// �ӵ�ǰ��Ϸ״̬��ʼ��ʹ���������ģ��һ����������Ϸ����֪��Ϸ���
    /// </summary>
    /// <returns>�˴�ģ���У������Ƿ�ʤ��</returns>
    bool simulate();

    /// <summary>
    /// �������һ��������ʤ����
    /// </summary>
    /// <returns>�������ʤ���򷵻�true</returns>
    GameResult judgeLastMove();

    /// <summary>
    /// ���ص�ǰ��Ϸ״̬�����һ��
    /// </summary>
    /// <returns>��ǰ��Ϸ״̬�����һ��</returns>
    MCTSGameMove returnLastMove();
}