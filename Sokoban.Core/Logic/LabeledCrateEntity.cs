using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public class LabeledCrateEntity : CrateEntity, ILabeled
{
    private SpriteFont font;
    private int label;
    public int Label 
    { 
        get => label; 
        set 
        {
            label = value;
            Glyph = font.GetGlyphs()[(char)value];
        }
    }

    public LabeledCrateEntity(SpriteFont font, Vector2 gridPosition) 
        : base(font, gridPosition)
    {
        this.font = font;
    }

    public LabeledCrateEntity(SpriteFont font, Vector2 gridPosition, int number) 
        : base(font, gridPosition)
    {
        this.font = font;
        Label = number;
        Glyph = font.GetGlyphs()[(char)number];
    }
}