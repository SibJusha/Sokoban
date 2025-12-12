using Microsoft.Xna.Framework;
using Sokoban.Core.Logic;

public class EmptyTile : Tile
{
    public EmptyTile(Vector2 gridPosition) : base(gridPosition)
    {
        IsPassable = false;
    }
}