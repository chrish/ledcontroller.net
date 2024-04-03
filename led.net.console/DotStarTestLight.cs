using System;
using System.Threading.Tasks;
using System.Device.Spi;
using System.Device.Gpio;
using System.Diagnostics;
using System.Drawing;
using led.net.app;

namespace led.net.console
{
    public class DotStarTestLight : IDisposable, ILight
    {

        private string _logfile;
        public DotStarTestLight(){
            _logfile = "testoutput.txt";
        }

        public async void Activate(){
            
        }

        public bool Show(Frame[] transition){
            try{
                int frameCounter = 0;
                int pixelCounter = 0;

                foreach(Frame f in transition){
                    foreach(Pixel p in f.Pixels){
                        Console.WriteLine("Frame " + frameCounter + " Pixel " + pixelCounter + " Brightness " + p.Brightness + " Color " + p.Color);

                        pixelCounter++;
                    }
                    
                    pixelCounter=0;
                    frameCounter++;
                }
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public void Dispose(){

        }
    }
}