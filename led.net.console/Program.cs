using System;
using System.Drawing;
using led.net.app;

namespace led.net.console
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILight light = new DotStarTestLight();

            LedStrip ls = new LedStrip(light, 50, 0.1, 5);
            Console.WriteLine("Starting init");

            ls.SetAllPixelsToColor(127, Color.Red);
        }
    }
}
