using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public class CrateEntity : Entity
{
    public CrateEntity(SpriteFont font, Vector2 gridPosition) : base(gridPosition)
    {
        Texture = font.Texture;
        Glyph = font.GetGlyphs()['C'];
        IsPushable = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Texture == null)
            return;
        
        spriteBatch.Draw(Texture, GridPosition * Size, Glyph.BoundsInTexture, Color.Green);
    }
}