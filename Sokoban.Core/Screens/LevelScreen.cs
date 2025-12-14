using System;
using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;
using Sokoban.Core.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Screens;

public class LevelScreen : Screen
{
    private readonly Level level;
    private readonly SpriteFont tileFont;
    private TimeSpan timeTaken;
    private int stepsCount;

    public LevelScreen(SokobanGame game, Level level) : base(game)
    {
        this.level = level;
        tileFont = Game.Content.Load<SpriteFont>("Fonts/Tiles");
    }

    public override void Initialize()
    {
        base.Initialize();
        timeTaken = TimeSpan.Zero;
    }

    public override void LoadContent()
    {
        base.LoadContent();

        try
        {
            level.LoadContent();
        }
        catch (Exception e)
        {
            ScreenManager.RemoveScreen(this);
            ScreenManager.ShowScreen(new MessageScreen(
                Game,
                $"Couldn't load {level.Name} level:\n{e.Message}"));
        }
    }

    public override void HandleInput(GameTime gameTime, InputManager inputManager)
    {
        stepsCount += level.TryMovePlayer(gameTime, inputManager) ? 1 : 0;
        if (inputManager.IsCanceled() || inputManager.IsSelected())
            Exit();
    }

    public override void Draw(GameTime gameTime)
    {
        level.Draw(ScreenManager.SpriteBatch);    
        DrawHUD();
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
                $"""
                YOU WON!
                Level {level.Name} completed!
                Time: {timeTaken:mm\:ss\.ff}
                Steps: {stepsCount} 
                """));
        }
        timeTaken += gameTime.ElapsedGameTime;
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        level.UnloadContent();
    }

    private void DrawHUD()
    {
        var timeToDraw = timeTaken.ToString(@"mm\:ss\.ff");

        var timePos = new Vector2(ScreenManager.ScreenSize.X - 20f 
            - ScreenManager.Font.MeasureString(timeToDraw).X, 10);

        ScreenManager.SpriteBatch.DrawStringWithShadow(
            ScreenManager.Font, timeToDraw, timePos, Color.White);    

        var stepsCountPos = timePos + new Vector2(0, ScreenManager.Font.LineSpacing);
        ScreenManager.SpriteBatch.DrawStringWithShadow(
            ScreenManager.Font, $"{stepsCount} steps", stepsCountPos, Color.White);
    }
}