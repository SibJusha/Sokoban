using System;
using Microsoft.Xna.Framework;

namespace Sokoban.Core.Screens;

/// <summary>
/// Custom event argument which includes the index of the player who
/// triggered the event. This is used by the MenuEntry.Selected event.
/// </summary>
public class PlayerIndexEventArgs : EventArgs
{
    public PlayerIndex PlayerIndex { get; }

    public PlayerIndexEventArgs(PlayerIndex playerIndex)
    {
        PlayerIndex = playerIndex;
    }
}