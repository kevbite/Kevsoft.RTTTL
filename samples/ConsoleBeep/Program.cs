using System;
using Kevsoft.RTTTL;

namespace ConsoleBeep
{
    class Program
    {
        static void Main(string[] args)
        {
            var song =
                "he Simpsons:d=4,o=5,b=160:c.6,e6,f#6,8a6,g.6,e6,c6,8a,8f#,8f#,8f#,2g,8p,8p,8f#,8f#,8f#,8g,a#.,8c6,8c6,8c6,c6";

            if (!Rtttl.TryParse(song, out var rtttl))
            {
                Console.WriteLine($"Failed to parse song: {song}");
                return;
            }
            rtttl.Play(new ConsoleBeepPlayer());
        }
    }
}