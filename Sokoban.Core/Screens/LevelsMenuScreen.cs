using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public class LevelsMenuScreen : MenuScreen
{
    public LevelsMenuScreen(SokobanGame game) : base(game)
    {
        foreach (var (levelName, _) in Game.LevelManager.LevelsMap)
            MenuEntries.Add(new MenuEntry(levelName));
    }

    // protected override void LoadContent()
    // {
    //     base.LoadContent();
    //     SpriteBatch = new SpriteBatch(GraphicsDevice); 

    //     if (activeScreen != null)
    //     {
    //         activeScreen.LoadContent();
    //     }
    // }

    /// <summary>
    /// Unloads content for the screen manager and the currently active screen if one exists.
    /// </summary>
}