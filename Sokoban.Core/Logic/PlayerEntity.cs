using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Logic;

public class PlayerEntity : Entity
{
    public PlayerEntity(SpriteFont font, Vector2 gridPosition) : base(gridPosition)
    {
        Texture = font.Texture;
        Glyph = font.GetGlyphs()['@'];
        // IsMovable = true;
    }

    public override bool Move()
    {
        throw new System.NotImplementedException();
    }
}