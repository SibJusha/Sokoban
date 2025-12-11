using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;
using Sokoban.Core.Logic;

namespace Sokoban.Core.Screens;

public class LevelScreen : Screen
{
    private readonly Level level;

    public LevelScreen(SokobanGame game, Level level) : base(game)
    {
        this.level = level;
    }

    public override void LoadContent()
    {
        base.LoadContent();
        if (!level.LoadContent())
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.ShowScreen(new MessageScreen(Game, $"Can't load {level.Name} level"));
        }
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);    
    }
}