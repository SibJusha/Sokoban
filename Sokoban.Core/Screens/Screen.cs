using System;
using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public abstract class Screen : IDisposable
{
    public SokobanGame Game { get; set; }

    public ScreenManager ScreenManager { get; set; }

    public bool IsActive { get; set; }

    public bool UpdateWhenInactive { get; set; }

    public bool DrawWhenInactive { get; set; }

    private bool isLoaded = false;
    public bool IsLoaded => isLoaded;

    public Screen(SokobanGame game)
    {
        Game = game;
        ScreenManager = game.ScreenManager;
    }

    public virtual void Dispose() { }

    public virtual void Initialize() { }

    public virtual void LoadContent()
    {
        isLoaded = true;
    }

    public virtual void UnloadContent() { }

    public virtual void HandleInput(GameTime gameTime, InputManager inputManager) {}

    public virtual void Update(GameTime gameTime)
    {
        if (!IsLoaded)
            LoadContent();
    }

    public abstract void Draw(GameTime gameTime);

    public virtual void Exit()
    {
        ScreenManager.RemoveScreen(this);
    }
}
