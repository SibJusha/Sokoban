using System;
using System.IO;
using System.Linq;

namespace Sokoban.Core.Logic;

public class Level : IDisposable
{
    private TileType[,] tiles; 
    
    public string Name { get; set; }
    
    public string FilePath { get; private set; }

    public Level(string filePath)
    {
        FilePath = filePath;
        Name = Path.GetFileNameWithoutExtension(FilePath);
    }

    public bool LoadContent()
    {
        try {
            var rawLines = File.ReadAllLines(FilePath);
            
            var maxLength = rawLines.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            tiles = new TileType[rawLines.Length, maxLength];

            if (tiles.Length == 0)
                return false;

            for (var i = 0; i < rawLines.Length; ++i)
            {
                for (var j = 0; j < rawLines[i].Length; ++i)
                    tiles[i, j] = LoadTile(rawLines[i][j]);
                for (var j = rawLines.Length; j < maxLength; ++j)
                    tiles[i, j] = TileType.Wall; 
            }
        } 
        catch (SystemException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

        return true;
    }

    public void Dispose()
    {
    }

    private TileType LoadTile(char symbol) => symbol switch
    {
        'P' => TileType.Player,
        'C' => TileType.Crate,
        '*' => TileType.GoalCell,
        '#' => TileType.Wall,
        ' ' => TileType.Empty,
        _ => throw new ArgumentException($"Unsupported tile char in {FilePath} level: {symbol}.")
    };
}
