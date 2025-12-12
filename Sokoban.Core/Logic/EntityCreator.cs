using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Core.Logic;

public static class EntityCreator
{
    public static SpriteFont Font { get; set; }

    public static Entity CreateEntity(string type, Vector2 gridPosition)
    {
        return type switch
        {
            "Player" => new PlayerEntity(Font, gridPosition),
            "Crate" => new CrateEntity(Font, gridPosition),
            _ => throw new ArgumentException($"Unsupported entity: {type}."),
        };
    }
}