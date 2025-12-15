using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Sokoban.Core.Logic;

public class Leaderboard
{
    public List<ScoreEntry> Entries { get; } = [];

    public void AddScore(int steps, TimeSpan time)
    {
        Entries.Add(new(steps, time));
        Entries.Sort(ScoreComparison);
        if (Entries.Count > 30)
            Entries.RemoveRange(30, Entries.Count - 30);
    }

    public XElement ToXElement()
    {
        var lbElement = new XElement("Leaderboard");
        foreach (var entry in Entries)
            lbElement.Add(entry.ToXElement());
        return lbElement;
    }

    public static Leaderboard FromXElement(XElement lbElement)
    {
        var lb = new Leaderboard();
        if (lbElement != null)
        {
            foreach (var entryElem in lbElement.Elements("Entry"))
                lb.Entries.Add(ScoreEntry.FromXElement(entryElem));
            lb.Entries.Sort(ScoreComparison); 
        }
        return lb;
    }

    private static readonly Comparison<ScoreEntry> ScoreComparison = (a, b) =>
    {
        int stepsCmp = a.Steps.CompareTo(b.Steps);
        if (stepsCmp != 0) 
            return stepsCmp;
        int timeCmp = a.Time.CompareTo(b.Time);
        return timeCmp != 0 ? timeCmp : b.Date.CompareTo(a.Date); 
    };
}