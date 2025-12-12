using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Screens;

public class MenuEntry
{
    string text;
    public string Text
    {
        get => text; 
        set { text = value; }
    }

    Vector2 position;
    public Vector2 Position
    {
        get => position; 
        set { position = value; }
    }

    private bool enabled;
    public bool Enabled
    {
        get => enabled; 
        set { enabled = value; }
    }

    public event EventHandler<EventArgs> Selected;

    protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
    {
        Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
    }

    public virtual void OnSelection()
    {
        Selected?.Invoke(this, null);
    }

    public MenuEntry(string text, bool enabled = true)
    {
        this.text = text;
        this.enabled = enabled;
    }

    public MenuEntry(string text, EventHandler<EventArgs> selectedEvent, bool enabled = true)
    {
        this.text = text;
        Selected += selectedEvent;
        this.enabled = enabled;
    }

    public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
    }


    public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
        Color color;
        if (enabled)
            color = isSelected ? Color.Yellow : Color.White;
        else
            color = Color.Gray;

        var screenManager = screen.ScreenManager;
        var spriteBatch = screenManager.SpriteBatch;
        var font = screen.Font;
        spriteBatch.DrawString(font, text, position, color);
    }

    public virtual int GetHeight(MenuScreen screen)
    {
        return screen.Font.LineSpacing;
    }

    public virtual int GetWidth(MenuScreen screen)
    {
        return (int)screen.Font.MeasureString(Text).X;
    }
}