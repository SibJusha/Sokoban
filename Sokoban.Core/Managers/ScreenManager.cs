using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Screens;

namespace Sokoban.Core.Managers;

public class ScreenManager : DrawableGameComponent
{
    private List<Screen> screens = [];
    private List<Screen> screensToUpdate = [];

    private Screen activeScreen;
    public SpriteBatch SpriteBatch { get; private set; }

    public Screen ActiveScreen => activeScreen;

    private Vector2 screenSize = new(800, 600);
    public Vector2 ScreenSize
    {
        get => screenSize;
        set => screenSize = value;
    }

    public SpriteFont Font { get; private set; }

    private readonly InputManager inputManager = new();

    public ScreenManager(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        activeScreen?.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Font = Game.Content.Load<SpriteFont>("Fonts/Hud");

        foreach (var screen in screens)
            screen.LoadContent();
    }

    public void ShowScreen(Screen screen)
    {
        // ArgumentNullException.ThrowIfNull(screen);

        if (activeScreen != null)
            activeScreen.IsActive = false;

        screen.ScreenManager = this;
        screen.IsActive = true;
        screen.Initialize();
        // screen.LoadContent();

        screens.Add(screen);
        activeScreen = screen;
    }

    public void RemoveScreen(Screen screen)
    {
        screen.IsActive = false;
        screen.UnloadContent();
        screen.Dispose();
        screens.Remove(screen);

        if (activeScreen == screen && screens.Count > 0)
        {
            activeScreen = screens[screens.Count - 1];
            activeScreen.IsActive = true;
        }
    }

    public void CloseScreen()
    {
        if (screens.Count == 0)
            return;

        var screen = screens[screens.Count - 1];
        screens.RemoveAt(screens.Count - 1);
        screen.IsActive = false;
        screen.UnloadContent();
        screen.Dispose();

        if (screens.Count != 0)
        {
            activeScreen = screens[screens.Count - 1];
            activeScreen.IsActive = true;
        }
    }

    public void ReplaceScreen(Screen screen)
    {
        CloseScreen();
        ShowScreen(screen);
    }

    public void ClearScreens()
    {
        foreach (var screen in screens)
        {
            screen.IsActive = false; 
            screen.UnloadContent();
            screen.Dispose();
        }
        
        activeScreen = null;
        screens.Clear();
        screensToUpdate.Clear();
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();

        if (activeScreen != null)
        {
            activeScreen.UnloadContent();
        }
    }

    public override void Update(GameTime gameTime)
    {
        inputManager.Update(gameTime);
        screensToUpdate.Clear();
        screensToUpdate.AddRange(screens);

        foreach (var screen in screensToUpdate)
        {
            if (screen.IsActive || screen.UpdateWhenInactive)
            {
                screen.Update(gameTime);
                screen.HandleInput(gameTime, inputManager);
            }               
        }
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        foreach (var screen in screensToUpdate)
        {
            if (screen.IsActive || screen.DrawWhenInactive)
                screen.Draw(gameTime);
        }
        SpriteBatch.End();
    }    
}