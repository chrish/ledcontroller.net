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

            LedStrip ls = new LedStrip(light, 50, 0.2, 5);
            Console.WriteLine("Starting init");

            ls.SetAllPixelsToColor(50, Color.Red);

            Console.WriteLine("Done setting entire array, now for one pixel");
            
            ls.SetPixelToColor(3, 50, Color.Blue);
        }
    }
}
