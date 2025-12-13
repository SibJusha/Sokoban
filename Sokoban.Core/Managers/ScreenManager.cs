using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Screens;

namespace Sokoban.Core.Managers;

public class ScreenManager : DrawableGameComponent
{
    private GraphicsDeviceManager graphicsDeviceManager;
    private List<Screen> screens = [];
    private List<Screen> screensToUpdate = [];

    private Screen activeScreen;
    public SpriteBatch SpriteBatch { get; private set; }

    public Screen ActiveScreen => activeScreen;

    public Vector2 ScreenSize => 
        new(graphicsDeviceManager.PreferredBackBufferWidth,
            graphicsDeviceManager.PreferredBackBufferHeight);

    public SpriteFont Font { get; private set; }

    private readonly InputManager inputManager = new();

    public Texture2D WhitePixel { get; private set; }

    public ScreenManager(Game game, GraphicsDeviceManager graphicsDeviceManager) 
        : base(game)
    {
        this.graphicsDeviceManager = graphicsDeviceManager;
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

        WhitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
        WhitePixel.SetData([Color.White]);

        foreach (var screen in screens)
            screen.LoadContent();
    }

    public void ShowScreen(Screen screen)
    {
        if (activeScreen != null)
            activeScreen.IsActive = false;

        screens.Add(screen);
        activeScreen = screen;
        screen.ScreenManager = this;
        screen.IsActive = true;
        screen.Initialize();
        screen.LoadContent();

    }

    public void RemoveScreen(Screen screen)
    {
        screen.IsActive = false;
        screen.UnloadContent();
        screen.Dispose();
        screens.Remove(screen);

        if (activeScreen == screen && screens.Count > 0)
        {
            activeScreen = screens[^1];
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
        activeScreen?.UnloadContent();
        WhitePixel?.Dispose();
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

        if (screens.Count == 0)
            Game.Exit();
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