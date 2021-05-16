using System.Collections.Generic;

/// <summary>
/// 获胜状况
/// </summary>
public enum GameResult
{
    PlayerWon,
    ComputerWon,
    NoOutcome,
    Draw
};

public interface MCTSGameState
{
    /// <summary>
    /// 检查当前游戏状态是否存在尚未展开的子状态
    /// </summary>
    /// <param name="existingChildren">已展开的子状态</param>
    /// <returns>存在尚未展开的子状态时返回true</returns>
    bool existsUnexpandedChild(List<MCTSGameState> existingChildren);

    /// <summary>
    /// 随机地将游戏推进一步
    /// </summary>
    /// <param name="existingChildren">已经存在的子状态</param>
    /// <returns>推进一步后的游戏状态</returns>
    MCTSGameState expand(List<MCTSGameState> existingChildren);

    /// <summary>
    /// 从当前游戏状态开始，使用随机方法模拟一次完整的游戏，告知游戏结果
    /// </summary>
    /// <returns>此次模拟中，电脑是否胜利</returns>
    bool simulate();

    /// <summary>
    /// 依据最后一步，评判胜利者
    /// </summary>
    /// <returns>如果电脑胜利则返回true</returns>
    GameResult judgeLastMove();

    /// <summary>
    /// 返回当前游戏状态的最后一步
    /// </summary>
    /// <returns>当前游戏状态的最后一步</returns>
    MCTSGameMove returnLastMove();
}