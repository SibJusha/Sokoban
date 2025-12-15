using Microsoft.Xna.Framework;

namespace Sokoban.Core.Logic;

public abstract class Tile : GridObject
{
    // TODO: size could be changed in e.g. settings
    public bool IsPassable { get; protected set; } 

    protected Tile(Vector2 gridPosition) : base(gridPosition)
    {
    }

    public virtual void OnEnter() {}

    public virtual void OnLeave() {} 
}