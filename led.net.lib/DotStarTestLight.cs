namespace led.net.app
{
    public class DotStarTestLight : IDisposable, ILight
    {
        private string _logfile;
        public DotStarTestLight(){
            _logfile = "testoutput.txt";
        }

        public async void Activate(){
            
        }

        public bool Show(Frame[] transition, int waitBetweenFrames){
            try{
                int frameCounter = 0;
                int pixelCounter = 0;

                foreach(Frame f in transition){
                    Console.WriteLine(f.Serialize());

                    pixelCounter=0;
                    frameCounter++;
                    Thread.Sleep(waitBetweenFrames);
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
