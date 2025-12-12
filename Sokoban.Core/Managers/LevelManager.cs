using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Sokoban.Core.Logic;

namespace Sokoban.Core.Managers;

public class LevelManager
{
    private readonly Game game;
    public readonly OrderedDictionary<string, Level> LevelsMap = [];

    public LevelManager(Game game)
    {
        this.game = game;
    }

    // public bool LoadLevel(string levelName)
    // {
    //     if (LevelsMap.TryGetValue(levelName, out var level))
    //         return level.LoadContent();

    //     return false;
    // }

    public void PreloadLevels()
    {
        var levelFiles = Directory.EnumerateFiles(Path.Combine(game.Content.RootDirectory, "Levels"));
        foreach (var level in levelFiles)
            LevelsMap.Add(Path.GetFileNameWithoutExtension(level), new Level(level));
    }
}