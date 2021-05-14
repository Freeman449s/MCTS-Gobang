using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 等待实现
public interface MCTSGameState
{
    /// <summary>
    /// 检查当前游戏状态是否存在尚未展开的子状态
    /// </summary>
    /// <param name="children">已展开的子状态</param>
    /// <returns>存在尚未展开的子状态时返回true</returns>
    public bool existsUnexpandedChild(List<MCTSGameState> children);

    /// <summary>
    /// 随机地将游戏推进一步
    /// </summary>
    /// <param name="existingChildren">已经存在的子状态</param>
    /// <returns>推进一步后的游戏状态</returns>
    public MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// 从当前游戏状态开始，使用随机方法模拟一次完整的游戏，告知游戏结果
    /// </summary>
    /// <returns>此次模拟中，电脑是否胜利</returns>
    public bool simulate();
}