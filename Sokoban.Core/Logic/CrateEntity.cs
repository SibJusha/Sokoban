using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public class CrateEntity : Entity
{
    public bool IsOnGoal { get; set; } = false;

    public CrateEntity(SpriteFont font, Vector2 gridPosition) : base(gridPosition)
    {
        Texture = font.Texture;
        Glyph = font.GetGlyphs()['C'];
        IsPushable = true;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        if (Texture == null)
            return;
        
        var color = IsOnGoal ? Color.Green : Color.Yellow;
        spriteBatch.Draw(Texture, pos + GridPosition * Size, Glyph.BoundsInTexture, color);
    }
}