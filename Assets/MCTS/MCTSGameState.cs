using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 等待实现

/// <summary>
/// 获胜状况
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
    /// 检查当前游戏状态是否存在尚未展开的子状态
    /// </summary>
    /// <param name="children">已展开的子状态</param>
    /// <returns>存在尚未展开的子状态时返回true</returns>
    public abstract bool existsUnexpandedChild(List<MCTSGameState> children);

    /// <summary>
    /// 随机地将游戏推进一步
    /// </summary>
    /// <param name="existingChildren">已经存在的子状态</param>
    /// <returns>推进一步后的游戏状态</returns>
    public abstract MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// 从当前游戏状态开始，使用随机方法模拟一次完整的游戏，告知游戏结果
    /// </summary>
    /// <returns>此次模拟中，电脑是否胜利</returns>
    public abstract bool simulate();

    /// <summary>
    /// 依据最后一步，评判胜利者
    /// </summary>
    /// <returns>如果电脑胜利则返回true</returns>
    public abstract GameResult judgeLastMove();
}