using Meadow;
using System.Threading;

namespace MeadowApplication
{
    class Program
    {
        static IApp? _app;
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--exitOnDebug") return;

            // instantiate and run new meadow app
            _app = new MeadowApp();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
