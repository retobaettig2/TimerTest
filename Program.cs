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
            Console.WriteLine("Usage: TimerTest [objectcount] [objectsize]");
            Console.WriteLine("   Testing Dotnet Timer frequency and accuracy");
            Console.WriteLine("   if object count and size are specified, the program");
            Console.WriteLine("   is allocating and freeing objects of the given size as fast as possible");
            Console.WriteLine("   Configuration: ");
            Console.WriteLine($"       TimerDelay = {Config.timerDelayms}ms, UpdateTimeSeconds = {Config.updateTimeSeconds}");
            Console.WriteLine($"       objectcount = {Config.objectCount}, objectsize = {Config.objectSize}");
            Console.WriteLine("Press <ctrl>-<c> to abort.");

            new MainLoop().run();
        }

        public static void ParseArguments(string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out Config.objectCount))
            {
                Config.objectCount = 0;
            }
            if (args.Length < 2 || !int.TryParse(args[1], out Config.objectSize))
            {
                Config.objectSize = 0;
            }
        }

    }
}
