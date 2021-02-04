using System;
using Kevsoft.RTTTL;

namespace ConsoleBeep
{
    class Program
    {
        static void Main(string[] args)
        {
            var song = args[0];
            
            if (!Rtttl.TryParse(song, out var rtttl))
            {
                Console.WriteLine($"Failed to parse song: {song}");
                return;
            }
            
            Console.WriteLine($"Playing: {rtttl.Name}");
            
            rtttl.PlayWithConsoleBeep();
        }
    }
}