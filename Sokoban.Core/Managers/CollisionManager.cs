using System;
using System.Collections.Generic;
using Sokoban.Core.Logic;

namespace Sokoban.Core.Managers;

public static class CollisionManager
{
    private static readonly HashSet<(Type ent1, Type ent2)> pushablePairs =
    [
        // left can push right
        (typeof(PlayerEntity), typeof(CrateEntity)),
        (typeof(PlayerEntity), typeof(PlayerEntity)),
        (typeof(PlayerEntity), typeof(LabeledCrateEntity)),
    ];
    
    public static void TileActionOnLeave(Tile tile, Entity leaving)
    {
        switch (tile, leaving) 
        {
            case (GoalTile gt and not ILabeled, CrateEntity crate and not ILabeled):
                gt.IsCovered = false;
                crate.IsOnGoal = false;
                break;
            case (LabeledGoalTile gt, LabeledCrateEntity crate) when gt.Label == crate.Label:
                gt.IsCovered = false;
                crate.IsOnGoal = false;
                break;
        }
    }

    public static void TileActionOnEnter(Tile tile, Entity entering)
    {
        switch (tile, entering)
        {
            case (GoalTile gt and not ILabeled, CrateEntity crate and not ILabeled):
                gt.IsCovered = true;
                crate.IsOnGoal = true;
                break;
            case (LabeledGoalTile gt, LabeledCrateEntity crate) when gt.Label == crate.Label:
                gt.IsCovered = true;
                crate.IsOnGoal = true;
                break;
        }
    }

    public static bool CanPush(Entity mover, Entity blocking)
    {
        return pushablePairs.Contains((mover.GetType(), blocking.GetType()));
    }
}