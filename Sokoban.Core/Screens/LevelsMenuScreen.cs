using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class LevelsMenuScreen : MenuScreen
{
    private readonly LevelsManager levelsManager;

    public LevelsMenuScreen(SokobanGame game)
        : base(game)
    {
        this.levelsManager = game.LevelsManager;

        foreach (var levelName in levelsManager.LevelsMap) // optional sorting
        {
            menuEntries.Add(new MenuEntry(levelName.Key));
        }
    }

    protected override void OnSelectEntry()
    {
        var levelName = menuEntries[selectedEntry].Text;

        if (levelsManager.LevelsMap.TryGetValue(levelName, out var level))
        {
            // level.LoadContent();
            // exit this screen?
            ScreenManager.ShowScreen(new LevelScreen(Game, level));
        }
        else
            ScreenManager.ShowScreen(new MessageScreen(Game, "No such level"));
    }
}