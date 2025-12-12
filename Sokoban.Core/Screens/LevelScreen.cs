using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;
using Sokoban.Core.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Screens;

public class LevelScreen : Screen
{
    private readonly Level level;
    private readonly SpriteFont tileFont;

    public LevelScreen(SokobanGame game, Level level) : base(game)
    {
        this.level = level;
        tileFont = Game.Content.Load<SpriteFont>("Fonts/Tiles");
    }

    public override void LoadContent()
    {
        base.LoadContent();

        try
        {
            level.LoadContent();
        }
        catch (System.Exception e)
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.ShowScreen(new MessageScreen(
                Game,
                $"Couldn't load {level.Name} level:\n{e.Message}"));
        }
    }

    public override void HandleInput(GameTime gameTime, InputManager inputManager)
    {
        level.HandleInput(gameTime, inputManager);
        if (inputManager.IsCanceled() || inputManager.IsSelected())
            Exit();
    }

    public override void Draw(GameTime gameTime)
    {
        level.Draw(ScreenManager.SpriteBatch);    
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);    
        level.Update(gameTime);
        if (level.IsCompleted())
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.ShowScreen(new MessageScreen(
                Game,
                $"YOU WON!\nLevel {level.Name} completed!"));
        }
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        level.UnloadContent();
    }
}