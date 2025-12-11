using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Screens;

public class MenuEntry
{
    /// <summary>
    /// Tracks a fading selection effect on the entry.
    /// </summary>
    /// <remarks>
    /// The entries transition out of the selection effect when they are deselected.
    /// </remarks>
    // float selectionFade;

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
    // public event EventHandler<PlayerIndexEventArgs> Selected;

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
        // When the menu selection changes, entries gradually fade between
        // their selected and deselected appearance, rather than instantly
        // popping to the new state.
        // float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

        // if (isSelected)
            // selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        // else
            // selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
    }


    public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
        Color color;
        if (enabled)
            color = isSelected ? Color.Yellow : Color.White;
        else
            color = Color.Gray;

        // Pulsate the size of the selected menu entry.
        double time = gameTime.TotalGameTime.TotalSeconds;

        // float pulsate = (float)Math.Sin(time * 6) + 1;

        // float scale = 1 + pulsate * 0.05f * selectionFade;

        // Modify the alpha to fade text out during transitions.
        // color *= screen.TransitionAlpha;

        // Draw text, centered on the middle of each line.
        var screenManager = screen.ScreenManager;
        var spriteBatch = screenManager.SpriteBatch;
        var font = screen.Font;

        // Vector2 origin = new Vector2(0, font.LineSpacing / 2);

        spriteBatch.DrawString(font, text, position, color);
        // spriteBatch.DrawString(font, text, position, color, 0,
        //                        origin, 0, SpriteEffects.None, 0);
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