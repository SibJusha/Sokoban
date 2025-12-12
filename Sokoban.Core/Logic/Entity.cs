using Microsoft.Xna.Framework;

namespace Sokoban.Core.Logic;

public abstract class Entity : GridObject
{
    public bool IsPushable { get; protected set; } = false;

    protected Entity(Vector2 gridPosition) : base(gridPosition)
    {
    }

    public abstract bool Move();

    public virtual bool CanPush(Direction dir, Level level) => true;
}