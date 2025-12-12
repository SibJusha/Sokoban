using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Logic;

public class FloorTile : Tile
{
    public FloorTile(SpriteFont font, Vector2 gridPosition) : base(gridPosition)
    {
        Texture = font.Texture;
        Glyph = font.GetGlyphs()[' '];
        IsPassable = true;
    }
}