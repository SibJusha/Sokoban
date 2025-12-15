using Sokoban.Core.Logic;

namespace Sokoban.Core.Screens;

public class LeaderboardScreen : MenuScreen
{
    private readonly Leaderboard leaderboard;
    private readonly Level level;

    public LeaderboardScreen(SokobanGame game, Level level)
        : base(game)
    {
        this.level = level;
        leaderboard = game.LevelsManager.GetLeaderboard(level);

        for (var i = 0; i < leaderboard.Entries.Count; ++i)
        {
            var entry = leaderboard.Entries[i];
            menuEntries.Add(new MenuEntry($"{i+1}. {entry.Steps} steps - {entry.Time:mm\\:ss\\.ff}"
                + $" - {entry.Date}"));
        }
    }
}