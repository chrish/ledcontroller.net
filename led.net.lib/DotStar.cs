using System;
using System.Threading.Tasks;
using System.Device.Spi;
using System.Device.Gpio;
using System.Diagnostics;
using System.Drawing;

namespace led.net.app
{
    public class DotStar : IDisposable, ILight
    {
        private uint DataPin { get; set; }
        private uint ClockPin { get; set; }
        
        private SpiDevice _spiDevice { get; set; }

        public DotStar(uint dataPin=11, uint clockPin=12)
        {
            DataPin = dataPin;
            ClockPin = clockPin;

            Activate();
        }

        public async void Activate(){
            var spiConnectionSettings = new SpiConnectionSettings(0, 0) // Using bus 0 and chip select line 0
            {
                ClockFrequency = 2_400_000, // DotStar LEDs typically work well with 2.4MHz
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            _spiDevice = SpiDevice.Create(spiConnectionSettings);
        }

        public bool Show(Frame[] transition){
            try{
                foreach(Frame f in transition){
                    _spiDevice.Write(f.Render());
                }
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public void Dispose(){
            _spiDevice?.Dispose();
        }
    }
}