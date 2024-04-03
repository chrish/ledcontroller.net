using System;
using System.Threading.Tasks;
using System.Device.Spi;
using System.Device.Gpio;
using System.Diagnostics;
using System.Drawing;

namespace led.net.app
{
    public class LedStrip
    {
        public uint NumPixels { get; set; }
        
        private uint _framesPerSecond{get;set;}
        private double _transitionTimeInSeconds {get;set;}

        private Pixel[] _currentPixels {get;set;}

        private int _brightness {get; set;}

        private ILight _light {get;set;}
        
        /// <summary>
        /// Init the led strip. 
        /// Set initial size and fps, then init the pixel array. 
        /// The pixels will default to 50% brightness and white. 
        /// </summary>
        /// <param name="fps"></param>
        /// <param name="numPixels"></param>
        public LedStrip(ILight light, uint fps = 50, double transitionTimeInSeconds=0.5, uint numPixels = 144)
        {
            _framesPerSecond = fps;
            _transitionTimeInSeconds = transitionTimeInSeconds;
            _currentPixels = new Pixel[numPixels];

            for(int i=0; i<numPixels; i++){
                _currentPixels[i] = new Pixel();
            }

            _light = light;
            _light.Activate();

        }

        public void SetPixelToColor(uint pixelNumber, Color color){
            SetPixelToColor(pixelNumber, color, 0);
        }

        public void SetPixelToColor(uint pixelNumber, Color color, uint transitionTime){
            
        }

        public void SetAllPixelsToColor(Color color){
            

            SetAllPixelsToColor(127, color);
        }
        public void SetAllPixelsToColor(int brightness, Color color){
            
            Pixel[] newPixels = new Pixel[_currentPixels.Length];

            for(int i=0; i<newPixels.Length; i++)
            {
                newPixels[i] = new Pixel(brightness, color);
            }

            Transition t = new Transition(_framesPerSecond, _transitionTimeInSeconds, _currentPixels, newPixels);
            _light.Show(t.CalculateTransition());

            // Updating the ledstrip so that the new pixels become the current. 
            _currentPixels = newPixels;
        }
    }
}