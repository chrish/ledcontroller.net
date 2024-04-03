using System.Drawing;

namespace led.net.app
{
    /// <summary>
    /// Represents a single light with color and brightness. 
    /// </summary>
    public class Pixel{
        public int Brightness {get;set;}
        
        public Color Color{get;set;}

        /// <summary>
        /// Pixel colors are 0-1, brightness 0-255. Total is color*brightness for final output. 
        /// </summary>
        public Pixel(int brightness, Color color){
            Brightness = brightness;
            Color = color;
        }

        public Pixel(){
            Brightness =127;
            Color = Color.White;
        }
    }
}