using System;
using Microsoft.Xna.Framework;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Screens;

public abstract class Screen
{
    public SokobanGame Game { get; set; }
    public ScreenManager ScreenManager { get; set; }
    public bool IsActive { get; set; } = false;
    public bool UpdateWhenInactive { get; set; } = false;
    public bool DrawWhenInactive { get; set; } = false;
    public bool IsLoaded { get; private set; } = false;

    public Screen(SokobanGame game)
    {
        Game = game;
        ScreenManager = game.ScreenManager;
    }

    public virtual void Initialize() { }

    public virtual void LoadContent()
    {
        IsLoaded = true;
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
