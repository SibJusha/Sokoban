using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public class WallTile : Tile
{
    public WallTile(SpriteFont font, Vector2 gridPosition) : base(gridPosition)
    {
        Texture = font.Texture; 
        Glyph = font.GetGlyphs()['#'];
        IsPassable = false;
    }
}