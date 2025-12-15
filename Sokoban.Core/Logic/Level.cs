using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Core.Managers;

namespace Sokoban.Core.Logic;

public class Level : IComparable<Level>
{
    private readonly LinkedList<Entity> entities = []; 
    private PlayerEntity player;
    private Tile[,] Grid { get; set; } 
    private readonly List<GoalTile> goalTiles = []; 

    public string Name { get; set; }
    public string FilePath { get; private set; }
    public Leaderboard Leaderboard { get; private set; }

    public int Width
    {
        get
        {
            if (Grid == null)
                throw new InvalidOperationException("Grid is null");
            return Grid.GetLength(0);
        }
    }

    public int Height
    {
        get
        {
            if (Grid == null)
                throw new InvalidOperationException("Grid is null");
            return Grid.GetLength(1);
        }
    }

    public Level(string filePath)
    {
        FilePath = filePath;
        var doc = XDocument.Load(FilePath);
        var root = doc.Root;
        ParseName(root);
    }

    public int CompareTo(Level other)
    {
        return Name.CompareTo(other.Name);    
    }

    public bool IsInBounds(Vector2 pos)
    {
        return pos.X >= 0 && pos.Y >= 0 && pos.X < Width && pos.Y < Height;
    }

    public Tile GetTile(Vector2 pos)
    {
        return Grid[(int)pos.X, (int)pos.Y];
    }

    public bool TryGetEntityAt(Vector2 pos, out Entity entity)
    {
        entity = entities.FirstOrDefault(e => e.GridPosition == pos);
        return entity != null;
    }

    public bool IsCompleted()
    {
        foreach (var goalTile in goalTiles)
        {
            if (!goalTile.IsCovered)
                return false;
        }

        return true;
    }

    // TODO: think about better caching
    public void LoadContent()
    {
        var stopwatch = Stopwatch.StartNew();
        var doc = XDocument.Load(FilePath);
        var root = doc.Root;

        // ParseName(root);
        ParseTiles(root);
        ParseEntities(root);

        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.Elapsed.TotalMilliseconds} ms");

        if (!AreGoalsAndCratesSame())
            throw new InvalidDataException(
                "Count of GoalTiles must be equal to Crates");

        LoadLeaderboard();
    }

    public void SaveLeaderbordToXml()
    {
        var doc = XDocument.Load(FilePath); 
        var root = doc.Root;

        root.Element("Leaderboard")?.Remove();
        root.Add(Leaderboard.ToXElement());

        doc.Save(FilePath);
    }

    public bool TryMovePlayer(GameTime gameTime, InputManager inputManager)
    {
        var direction = inputManager.GetDirection();
    
        if (direction == Direction.None)
            return false;
        
        return TryMoveThere(player, direction);
    }

    public void UnloadContent()
    {
        Grid = null;
        player = null;
        entities.Clear();
        goalTiles.Clear();
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = Grid[x, y];
                tile?.Draw(spriteBatch, pos);
            }
        }

        foreach (var entity in entities)
            entity?.Draw(spriteBatch, pos);
        
        player?.Draw(spriteBatch, pos);
    }

    public void LoadLeaderboard()
    {
        var doc = XDocument.Load(FilePath);
        var root = doc.Root;
        Leaderboard = Leaderboard.FromXElement(root.Element("Leaderboard"));
    }

    private void ParseName(XElement root)
    {
        Name = root.Attribute("name")?.Value?.Trim();
        if (string.IsNullOrEmpty(Name))
            Name = Path.GetFileNameWithoutExtension(FilePath);
    }

    private void ParseEntities(XElement root)
    {
        var entitiesElem = root.Element("Entities");
        if (entitiesElem == null)
            return;

        foreach (var entityElem in entitiesElem.Elements())
        {
            if (!int.TryParse(entityElem.Attribute("x")?.Value, out int x)
                || !int.TryParse(entityElem.Attribute("y")?.Value, out int y))
                continue;
            
            if (!IsInBounds(new(x, y)))
                throw new InvalidDataException("Entity is out of bounds.");

            var entity = EntityCreator.CreateEntity(entityElem.Name.LocalName,
                                                    new(x, y));
            if (entity is PlayerEntity entityPlayer)
            {
                player = entityPlayer;
                continue;
            }
            
            else if (entity is LabeledCrateEntity labeledCrateEntity) 
            {
                var ch = (entityElem.Attribute("label")?.Value[0]) 
                    ?? throw new InvalidDataException("Wrong label.");
                labeledCrateEntity.Label = ch;
            }

            entities.AddLast(entity);
            CollisionManager.TileActionOnEnter(GetTile(new(x, y)), entity);
        }
    }

    private void ParseTiles(XElement root)
    {
        var tileMapLines = root.Element("TileMap").Value
                               .Split(['\r', '\n'], StringSplitOptions.None)
                               .Select(l => l.Trim())
                               .Where(l => !string.IsNullOrWhiteSpace(l) 
                                    && !l.StartsWith("<!--"))
                               .ToArray() ?? [];
        var width  = root.Attribute("width")?.Value is { } wStr
                             && int.TryParse(wStr, out int w) ? w : 0;
        var height = root.Attribute("height")?.Value is { } hStr
                             && int.TryParse(hStr, out int h) ? h : 0;

        var linesCount = Math.Max(width,  tileMapLines.Max(l => l.Length));
        var colsCount = Math.Max(height, tileMapLines.Length);

        if (colsCount == 0 || linesCount == 0)
            throw new InvalidDataException("TileMap is empty");

        Grid = new Tile[linesCount, colsCount];

        for (var y = 0; y < Height; ++y)
        {
            for (var x = 0; y < tileMapLines.Length && x < tileMapLines[y].Length; ++x)
            {
                var symbol = tileMapLines[y][x];
                var tile = TileCreator.CreateTile(symbol, new Vector2(x, y));
                Grid[x, y] = tile;

                if (tile is GoalTile goalTile)
                    goalTiles.Add(goalTile);
            }

            var x2 = y < tileMapLines.Length ? tileMapLines[y].Length : 0;
            for (; x2 < Width; ++x2)
                Grid[x2, y] = new EmptyTile(new Vector2(x2, y));
        }
    }

    private bool AreGoalsAndCratesSame()
    {
        var crates = entities.Where(e => e is CrateEntity);
        if (crates.Count() != goalTiles.Count)
            return false;
        
        int labeledGoals = goalTiles.OfType<LabeledGoalTile>().Count();
        int labeledCrates = crates.OfType<LabeledCrateEntity>().Count();
        if (labeledGoals != labeledCrates)
            return false;
        
        return true;
    }

    private bool TryMoveThere(Entity mover, Direction dir)
    {
        var targetPos = mover.GridPosition + dir.ToVector2();

        if (!IsInBounds(targetPos)) 
            return false;

        var targetTile = GetTile(targetPos);
        if (!targetTile.IsPassable) 
            return false;

        if (TryGetEntityAt(targetPos, out var blocking))
        {
            if (!CollisionManager.CanPush(mover, blocking) || !TryMoveThere(blocking, dir))
                return false;
        }

        var prevTile = GetTile(mover.GridPosition);
        CollisionManager.TileActionOnLeave(prevTile, mover);
        CollisionManager.TileActionOnEnter(targetTile, mover);
        mover.GridPosition = targetPos;

        return true;
    }
}
