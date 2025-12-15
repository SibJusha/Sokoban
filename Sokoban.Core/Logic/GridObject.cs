using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Microsoft.Xna.Framework.Graphics.SpriteFont;

namespace Sokoban.Core.Logic;

public abstract class GridObject
{
    public Glyph Glyph { get; protected set; }
    public Vector2 Size { get; } = new(42, 42);
    public Vector2 GridPosition { get; set; } 
    public Texture2D Texture { get; protected set; }

    protected GridObject(Vector2 gridPosition)
    {
        GridPosition = gridPosition;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (Texture == null)
            return;
        
        spriteBatch.Draw(Texture, GridPosition * Size, Glyph.BoundsInTexture, Color.Yellow);
    }
}
