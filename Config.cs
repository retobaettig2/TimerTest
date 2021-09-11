/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/
using System;

namespace TimerTest
{

    // Simple compile time configuration instead of config file
    // ToDo: Move to config file
    public static class Config
    {
        public static int timerDelayms = 1;
        public static int updateTimeSeconds = 10;
        public static int objectCount = 0;
        public static int objectSize = 0;
        public static long[] binBoundaries = { 5, 10, 12, 14, 16, 18, 20, 30, 50, 100, 1000 };
    }

}
