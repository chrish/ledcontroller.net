using System.Drawing;

namespace led.net.app
{
    /// <summary>
    /// Represents a single light with color and brightness. 
    /// </summary>
    public class    Pixel{
        public int Brightness {get;set;}
        
        public Color Color{get;set;}

        /// <summary>
        /// Only used with special functions to flag the color currently being actively modified. 
        /// This is useful eg. in transitions, color fades etc. 
        /// </summary>
        public Color ActiveColor{get;set;}
        public int Stepping {get;set;}

        /// <summary>
        /// Pixel colors are 0-1, brightness 0-255. Total is color*brightness for final output. 
        /// </summary>
        public Pixel(int brightness, Color color):this(){
            Brightness = brightness;
            Color = color;
        }

        public Pixel(){
            Brightness =127;
            Color = Color.White;
            Stepping = 1;
            ActiveColor = Color.Red;
        }
    }
}