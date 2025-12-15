using System;
using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;
using Sokoban.Core.Logic;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Screens;

public class LevelScreen : Screen
{
    private readonly Level level;
    private TimeSpan timeTaken;
    private int stepsCount;

    public LevelScreen(SokobanGame game, Level level) : base(game)
    {
        this.level = level;
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
        level.Draw(ScreenManager.SpriteBatch, new(0, ScreenManager.Font.LineSpacing + 10));
        DrawHUD();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);    

        if (level.IsCompleted())
            OnCompletion();

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

        var font = ScreenManager.Font;
        var timePos = new Vector2(ScreenManager.ScreenSize.X - 20f 
            - font.MeasureString(timeToDraw).X, 10);

        ScreenManager.SpriteBatch.DrawStringWithShadow(
            font, timeToDraw, timePos, Color.White);    

        var stepsCountPos = timePos + new Vector2(0, font.LineSpacing);
        ScreenManager.SpriteBatch.DrawStringWithShadow(
            font, $"{stepsCount} steps", stepsCountPos, Color.White);

        var levelNamePos = new Vector2((ScreenManager.ScreenSize.X 
            - font.MeasureString(level.Name).X) / 2, 10);
        ScreenManager.SpriteBatch.DrawStringWithShadow(
            font, level.Name, levelNamePos, Color.White);
    }

    private void OnCompletion()
    {
        var leaderboard = Game.LevelsManager.GetLeaderboard(level);
        leaderboard.AddScore(stepsCount, timeTaken);
        Game.LevelsManager.SaveLeaderboard(level);

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
}