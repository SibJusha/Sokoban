using Microsoft.Xna.Framework;

namespace Sokoban.Core.Logic;

public enum Direction { None, Up, Down, Left, Right }

public static class DirectionExtensions
{
    public static Vector2 ToVector2(this Direction dir) => dir switch
    {
        Direction.Up => new Vector2(0, -1),
        Direction.Down => new Vector2(0, 1),
        Direction.Left => new Vector2(-1, 0),
        Direction.Right => new Vector2(1, 0),
        _ => Vector2.Zero
    };
}