using System;
using Sokoban.Core;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            using var game = new SokobanGame();
            game.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}