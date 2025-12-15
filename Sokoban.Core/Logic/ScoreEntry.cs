using System;
using System.Xml.Linq;

namespace Sokoban.Core.Logic;

public class ScoreEntry
{
    // public string PlayerName { get; set; } = "Anonymous";
    public int Steps { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public ScoreEntry(int steps, TimeSpan time)
        : this(steps, time, DateTime.Now)
    {
    }

    public ScoreEntry(int steps, TimeSpan time, DateTime date)
    {
        Steps = steps;
        Time = time;
        Date = date;
    }

    public XElement ToXElement()
    {
        return new XElement("Entry",
            new XAttribute("Steps", Steps),
            new XAttribute("Time", Time.ToString(@"hh\:mm\:ss\.ff")),
            new XAttribute("Date", Date.ToString("o")));
    }

    public static ScoreEntry FromXElement(XElement element)
    {
        return new(
            (int?)element.Attribute("Steps") ?? 0,
            TimeSpan.Parse((string)element.Attribute("Time") ?? "00:00:00.00"),
            DateTime.Parse((string)element.Attribute("Date") ?? DateTime.Now.ToString("o"))
        );
    }
}
