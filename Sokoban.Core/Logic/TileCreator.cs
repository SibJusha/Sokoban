using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public static class TileCreator
{
    public static SpriteFont Font { get; set; }

    public static Tile CreateTile(char symbol, Vector2 gridPosition)
    {
        return symbol switch
        {
            // '@': return new (TileType.Player);
            // 'C' => new Tile(TileType.Crate),
            '*' => new GoalTile(Font, gridPosition),
            '#' => new WallTile(Font, gridPosition),
            ' ' => new FloorTile(Font, gridPosition),
            '\0' => new EmptyTile(gridPosition),
            _ => throw new ArgumentException($"Unsupported tile char: {symbol}."),
        };
    }
}