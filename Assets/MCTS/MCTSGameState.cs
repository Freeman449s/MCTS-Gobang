using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: �ȴ�ʵ��
public interface MCTSGameState
{
    /// <summary>
    /// ��鵱ǰ��Ϸ״̬�Ƿ������δչ������״̬
    /// </summary>
    /// <param name="children">��չ������״̬</param>
    /// <returns>������δչ������״̬ʱ����true</returns>
    public bool existsUnexpandedChild(List<MCTSGameState> children);

    /// <summary>
    /// ����ؽ���Ϸ�ƽ�һ��
    /// </summary>
    /// <param name="existingChildren">�Ѿ����ڵ���״̬</param>
    /// <returns>�ƽ�һ�������Ϸ״̬</returns>
    public MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// �ӵ�ǰ��Ϸ״̬��ʼ��ʹ���������ģ��һ����������Ϸ����֪��Ϸ���
    /// </summary>
    /// <returns>�˴�ģ���У������Ƿ�ʤ��</returns>
    public bool simulate();
}