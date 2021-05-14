using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MCTSGameState {
    /// <summary>
    /// 检查当前游戏状态是否存在尚未展开的子状态
    /// </summary>
    /// <param name="gameState">当前游戏状态</param>
    /// <param name="children">已展开的子状态</param>
    /// <returns>存在尚未展开的子状态时返回true</returns>
    public bool existsUnexpandedChild(MCTSGameState gameState, List<MCTSGameState> children);
}
