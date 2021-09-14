/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/
using System;

namespace TimerTest
{

    class Program
    {
        public static void Main(string[] args)
        {

            ParseArguments(args);
            Console.WriteLine("Usage: TimerTest [\"timer\"|\"thread\"] [objectcount] [objectsize]");
            Console.WriteLine("   Testing Dotnet Timer frequency and accuracy");
            Console.WriteLine("   In default mode \"timer\", a dotnet Timer is used. In mode \"thread\"");
            Console.WriteLine("   a busy thread is started which waits until the defined time passed by");
            Console.WriteLine("   and then starts the OnTimer event.");
            Console.WriteLine("   if object count and size are specified, the program");
            Console.WriteLine("   is allocating and freeing objects of the given size as fast as possible");
            Console.WriteLine("   Configuration: ");
            Console.WriteLine($"       TimerMode: {(Config.threadMode?"thread":"timer")}");
            Console.WriteLine($"       TimerDelay = {Config.timerDelayms}ms, UpdateTimeSeconds = {Config.updateTimeSeconds}");
            Console.WriteLine($"       objectcount = {Config.objectCount}, objectsize = {Config.objectSize}");
            Console.WriteLine("Press <ctrl>-<c> to abort.");

            new MainLoop().run();
        }

        public static void ParseArguments(string[] args)
        {
            if (args.Length > 0) {
                if (args[0].ToLower() == "thread") {
                    Config.threadMode = true;
                }
            }
            if (args.Length < 2 || !int.TryParse(args[1], out Config.objectCount))
            {
                Config.objectCount = 0;
            }
            if (args.Length < 3 || !int.TryParse(args[2], out Config.objectSize))
            {
                Config.objectSize = 0;
            }
        }

    }
}
