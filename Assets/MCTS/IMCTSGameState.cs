using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMCTSGameState {
    /// <summary>
    /// ��鵱ǰ��Ϸ״̬�Ƿ������δչ������״̬
    /// </summary>
    /// <param name="gameState">��ǰ��Ϸ״̬</param>
    /// <param name="children">��չ������״̬</param>
    /// <returns>������δչ������״̬ʱ����true</returns>
    public bool existsUnexpandedChild(IMCTSGameState gameState, List<IMCTSGameState> children);
}
