using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class LevelsMenuScreen : MenuScreen
{
    private readonly LevelsManager levelsManager;

    public LevelsMenuScreen(SokobanGame game)
        : base(game)
    {
        levelsManager = game.LevelsManager;

        // add sorting by filename?
        foreach (var level in levelsManager.Levels) 
        {
            menuEntries.Add(new MenuEntry(level.Name));
        }
    }

    public override void HandleInput(GameTime gameTime, InputManager inputManager)
    {
        base.HandleInput(gameTime, inputManager);

        if (inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.L))
        {
            var level = levelsManager.Levels.ElementAtOrDefault(selectedEntry);
            if (level != null)
                ScreenManager.ShowScreen(new LeaderboardScreen(Game, level));
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        ScreenManager.SpriteBatch.DrawStringWithShadow(Font, "L: Leaderboard", 
            new(20, 20), Color.Red); 
    }

    protected override void OnSelectEntry()
    {
        var level = levelsManager.Levels.ElementAtOrDefault(selectedEntry);
        if (level != null)
            ScreenManager.ShowScreen(new LevelScreen(Game, level));
        else
            ScreenManager.ShowScreen(new MessageScreen(Game, "No such level"));
    }
}