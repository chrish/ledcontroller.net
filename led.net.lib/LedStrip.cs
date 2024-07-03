using System;
using System.Threading.Tasks;
using System.Device.Spi;
using System.Device.Gpio;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace led.net.app
{
    public class LedStrip
    {
        public uint NumPixels { get; set; }
        
        private uint _framesPerSecond{get;set;}
        private double _transitionTimeInSeconds {get;set;}

        private Pixel[] _currentPixels {get;set;}

        private int _brightness {get; set;}

        private bool _circusIsRunning{get;set;}

        private ILight _light {get;set;}
        
        /// <summary>
        /// Init the led strip. 
        /// Set initial size and fps, then init the pixel array. 
        /// The pixels will default to 50% brightness and white. 
        /// </summary>
        /// <param name="light">The ILight device to use</param
        /// <param name="fps">Frames per second when rendering</param>
        /// <param name="transitionTimeInSeconds">Transition time in seconds for transitions</param>
        /// <param name="numPixels">Number of pixels in the strip</param>
        public LedStrip(ILight light, uint fps = 50, double transitionTimeInSeconds=0.5, uint numPixels = 144)
        {
            _framesPerSecond = fps;
            _transitionTimeInSeconds = transitionTimeInSeconds;
            _currentPixels = new Pixel[numPixels];

            for(int i=0; i<numPixels; i++){
                _currentPixels[i] = new Pixel();
                Console.WriteLine("Original pixels; brightness " + _currentPixels[i].Brightness + " color " + _currentPixels[i].Color);
            }


            _light = light;
            _light.Activate();

        }

        public void SetPixelToColor(uint pixelNumber, Color color){
            SetPixelToColor(pixelNumber, 127, color);
        }

        public void SetPixelToColor(uint pixelNumber, int brightness, Color color){
            Pixel[] newPixels = new Pixel[_currentPixels.Length];

            for(int i=0; i<newPixels.Length; i++)
            {
                if (i == pixelNumber)
                {
                    newPixels[i] = new Pixel(brightness, color);
                } 
                else 
                {
                    newPixels[i] = _currentPixels[i];
                }
            }

            Transition t = new Transition(_framesPerSecond, _transitionTimeInSeconds, _currentPixels, newPixels);
            _light.Show(t.CalculateTransition());

            // Updating the ledstrip so that the new pixels become the current. 
            _currentPixels = newPixels;
        }

        public Transition SetAllPixelsToColor(Color color)
        {    
            return SetAllPixelsToColor(127, color);
        }

        public Transition SetAllPixelsToColor(int brightness, Color color){
            
            Pixel[] newPixels = new Pixel[_currentPixels.Length];

            for(int i=0; i<newPixels.Length; i++)
            {
                newPixels[i] = new Pixel(brightness, color);
            }

            Transition t = new Transition(_framesPerSecond, _transitionTimeInSeconds, _currentPixels, newPixels);
            _light.Show(t.CalculateTransition());

            // Updating the ledstrip so that the new pixels become the current. 
            _currentPixels = newPixels;

            return t;
        }

        private int IndexerStart(Direction dir){
            if (dir == Direction.CLOCKWISE){
                return 0;
            } else {
                return _currentPixels.Length-1;
            }
        }

        private bool ContinueLoop(int indexer){
            return false;
        }

        public async void Circus(uint speed, Direction dir, bool showSameOnWholeArray=true){

            // Set speedrange to between 1 and 255
            if (speed > 255) speed = 255;
            if (speed < 1) speed = 1;
            uint maxWaitMs = 1000;
            int actualWait = (int)Math.Floor((double)maxWaitMs / speed);

            _circusIsRunning = true;

            while(_circusIsRunning){
                var oldPixels = _currentPixels;
                
                for(int i = IndexerStart(dir); i<_currentPixels.Length; i++){
                    var currentPixel = _currentPixels[i];
                    var prevPixel = _currentPixels[i];
                    if (i == 0){
                        prevPixel = _currentPixels[_currentPixels.Length-1];
                    } else {
                        prevPixel = _currentPixels[i-1];
                    }

                    if (currentPixel.ActiveColor == Color.Red){
                        if (currentPixel.Color.B >= 255 && currentPixel.Color.R > 0) {
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, Math.Max(currentPixel.Color.R - currentPixel.Stepping, 0), currentPixel.Color.G, currentPixel.Color.B);
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, Math.Max(prevPixel.Color.R - currentPixel.Stepping, 0), currentPixel.Color.G, currentPixel.Color.B);
                            }
                        }
                        else if (currentPixel.Color.G >= 255 && currentPixel.Color.R < 255){
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, Math.Min(currentPixel.Color.R + currentPixel.Stepping, 255), currentPixel.Color.G, currentPixel.Color.B);
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, Math.Min(prevPixel.Color.R + currentPixel.Stepping, 255), currentPixel.Color.G, currentPixel.Color.B);
                            }
                        }
                    }

                    if (currentPixel.ActiveColor == Color.Blue){
                        if (currentPixel.Color.G >= 255 && currentPixel.Color.B > 0){
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, currentPixel.Color.G, Math.Max(currentPixel.Color.B - currentPixel.Stepping, 0));
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, currentPixel.Color.G, Math.Max(prevPixel.Color.B - currentPixel.Stepping, 0));
                            }
                        }else if (currentPixel.Color.R >= 255 && currentPixel.Color.B < 255){
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, currentPixel.Color.G, Math.Min(currentPixel.Color.B + currentPixel.Stepping, 255));
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, currentPixel.Color.G, Math.Min(prevPixel.Color.B + currentPixel.Stepping, 255));
                            }
                        }
                    }

                    if (currentPixel.ActiveColor == Color.Green){
                        if (currentPixel.Color.R >= 255 && currentPixel.Color.G > 0){
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, Math.Max(currentPixel.Color.G - currentPixel.Stepping, 0), currentPixel.Color.B);
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, Math.Max(prevPixel.Color.G - currentPixel.Stepping, 0), currentPixel.Color.B);
                            }
                        }else if (currentPixel.Color.B >= 255 && currentPixel.Color.G < 255){
                            if (showSameOnWholeArray){
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, Math.Min(currentPixel.Color.G + currentPixel.Stepping, 255), currentPixel.Color.B);
                            } else {
                                currentPixel.Color = Color.FromArgb(currentPixel.Color.A, currentPixel.Color.R, Math.Min(prevPixel.Color.G + currentPixel.Stepping, 255), currentPixel.Color.B);
                            }
                        }
                    }

                    // Switch to new active color if constraints are met to keep track of which component to increase/decrease
                    if ((currentPixel.Color.R == 255 && currentPixel.Color.B == 0 && currentPixel.Color.G == 0) || (currentPixel.Color.R == 0 && currentPixel.Color.G == 255 && currentPixel.Color.B == 255)){
                        currentPixel.ActiveColor = Color.Blue;
                    }
                    else if ((currentPixel.Color.R == 255 && currentPixel.Color.B == 0 && currentPixel.Color.G == 255) || (currentPixel.Color.R == 0 && currentPixel.Color.G == 0 && currentPixel.Color.B == 255)){
                        currentPixel.ActiveColor = Color.Green;
                    }
                    else if ((currentPixel.Color.R == 255 && currentPixel.Color.B == 255 && currentPixel.Color.G == 0) || (currentPixel.Color.R == 0 && currentPixel.Color.G == 255 && currentPixel.Color.B == 0)){
                        currentPixel.ActiveColor = Color.Red;
                    }

                }

                Transition t = new Transition(oldPixels, _currentPixels);
                _light.Show(t.CalculateTransition());
                Thread.Sleep(actualWait);

            }
        }

        public enum Direction{
            CLOCKWISE, COUNTERCLOCKWISE
        }
    }
}