using System;
using System.Drawing;
using led.net.app;

namespace led.net.console
{
    /// <summary>
    /// Er noe funky medm åten ting fader på, virker som at hver farge fader opp i transitions. 
    /// I tillegg; 144 pikler er ca 20. 5 ga meg 2. 1000 ga meg hele stripa
    /// 
    /// Circus må sakkes ned
    /// 
    /// Trenger også inline fade der hver farge flytter seg piksel for piksel
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            int brightness = 255;
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
                }  else if (args[0].Equals("off"))
                {
                    Console.WriteLine("Turning off");
                    circus = false;
                    brightness = 0;
                } 
                else 
                {
                    circus = false;
                }
            }
            

            ILight light = new DotStarTestLight();

            LedStrip ls = new LedStrip(light, 50, 2, 5);
            
            if (!circus)
            {
                ls.SetAllPixelsToColor(brightness, defaultColor);

            } 
            else 
            {
                Console.WriteLine("Circus!");
                ls.Circus(255, LedStrip.Direction.CLOCKWISE, false);
            }
        }
    }
}
