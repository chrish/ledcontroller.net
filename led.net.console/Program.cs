using System;
using System.Drawing;
using led.net.app;

namespace led.net.console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Color defaultColor = Color.White;
            bool circus = false;

            if(args.Length > 0)
            {
                Color newColor = Color.FromName(args[0]);
                if (newColor.IsKnownColor)
                {
                    Console.WriteLine("Found known color: " + newColor.Name);
                    defaultColor = newColor;
                }

                if (args[0].Equals("circus"))
                {
                    Console.WriteLine("Enabling circus");
                    circus = true;
                } 
                else 
                {
                    circus = false;
                }
            }
            

            ILight light = new DotStar();

            LedStrip ls = new LedStrip(light, 50, 2, 144);
            
            if (!circus)
            {
                ls.SetAllPixelsToColor(255, defaultColor);

            } 
            else 
            {
                Console.WriteLine("Circus!");
                ls.Circus(255, LedStrip.Direction.CLOCKWISE, false);
            }
        }
    }
}
