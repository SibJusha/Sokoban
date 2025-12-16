using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Sokoban.Core.Logic;

namespace Sokoban.Core.Managers;

public class LevelsManager
{
    private readonly Game game;
    public readonly SortedSet<Level> Levels = [];

    public LevelsManager(Game game)
    {
        this.game = game;
    }

    public void PreloadLevels()
    {
        var levelFiles = Directory.EnumerateFiles(Path.Combine(game.Content.RootDirectory, "Levels"));
        foreach (var levelPath in levelFiles)
        {
            try
            {
                var level = new Level(levelPath);
                Levels.Add(level);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public Leaderboard GetLeaderboard(Level level)
    {
        if (Levels.Contains(level))
        {
            if (level.Leaderboard == null)
                level.LoadLeaderboard();
            return level.Leaderboard;
        }
        return new Leaderboard(); 
    }

    public void SaveLeaderboard(Level level)
    {
        if (Levels.Contains(level))
            level.SaveLeaderbordToXml();
    }
}