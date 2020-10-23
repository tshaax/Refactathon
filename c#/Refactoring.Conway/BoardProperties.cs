using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.Conway
{
    public static class BoardProperties
    {
        public static int width { get; set; }
        public static int height { get; set; }
        public static string inputHeight { get; set; }
        public static bool[,] board { get; set; }
        public static string inputWidth { get; set; }
        public static bool societyDied { get; set; } = true;
        public static int total { get; set; }
        public static int ratio { get; set; }
        public static int generations { get; set; }
        public static string inputGenerations { get; set; }

    }
}
