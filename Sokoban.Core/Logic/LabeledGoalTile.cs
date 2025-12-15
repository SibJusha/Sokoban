using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public class LabeledGoalTile : GoalTile, ILabeled
{
    public int Label { get; set; }

    public LabeledGoalTile(SpriteFont font, Vector2 gridPosition, int number) 
        : base(font, gridPosition)
    {
        Label = number;    
        Glyph = font.GetGlyphs()[(char)number];
    }
}