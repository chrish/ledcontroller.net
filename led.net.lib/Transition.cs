using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace led.net.app
{
    /// <summary>
    /// A transition covers a change to one or more pixels. 
    /// Input should be a pixellist with current pixels, a pixellist
    /// of the to-be pixels and the rest should be calculated internally
    /// </summary>
    public class Transition{
        
        public uint FrameratePerSecond{get;set;}
        public double TransitionTimeInSeconds {get;set;}
        private Pixel[] _originalPixels;
        private Pixel[] _newPixels;

        public Transition(uint frameratePerSecond, double transitionTime, Pixel[] originalPixels, Pixel[] newPixels){
            FrameratePerSecond = frameratePerSecond;
            TransitionTimeInSeconds = transitionTime;
            _originalPixels = originalPixels;
            _newPixels = newPixels;
        }

        public Transition(Pixel[] originalPixels, Pixel[] newPixels){
            FrameratePerSecond = 20;
            TransitionTimeInSeconds = 0.05;
            _originalPixels = originalPixels;
            _newPixels = newPixels;
        }

        /// <summary>
        /// Calculates a transition using the original and new pixel arrays as a base, filling in 
        /// everything in between by averaging and subtracting the difference frame by frame. 
        /// </summary>
        /// <returns></returns>
        public Frame[] CalculateTransition(){
            int durationPerFrameMillisec = Convert.ToInt32(TransitionTimeInSeconds / FrameratePerSecond);
            double numFramesDbl = (double)TransitionTimeInSeconds * FrameratePerSecond;
            int numFrames = Convert.ToInt32(Math.Round(numFramesDbl, 0));

            Frame[] frames = new Frame[numFrames];
            (int Bright ,int R, int G, int B)[] diffs = new (int Bright, int R, int G, int B)[_originalPixels.Length];
            
            // Calculate the difference pixel by pixel as an average over the 
            // transition. Not 100% exact as we need to use ints instead of doubles:
            for(int i=0; i<_originalPixels.Length; i++){
                diffs[i].Bright = Convert.ToInt32(Math.Round((double)_originalPixels[i].Brightness-_newPixels[i].Brightness));
                diffs[i].R = Convert.ToInt32(Math.Round((double)_originalPixels[i].Color.R - _newPixels[i].Color.R,0));
                diffs[i].G = Convert.ToInt32(Math.Round((double)_originalPixels[i].Color.G - _newPixels[i].Color.G,0));
                diffs[i].B = Convert.ToInt32(Math.Round((double)_originalPixels[i].Color.B - _newPixels[i].Color.B,0));
            }

            // Construct the transition, by setting the start and end color, 
            // filling in everything inbetween by subtracting the avg difference 
            // from the previous color. 
            for(int i=0; i<numFrames; i++){
                Pixel[] framePixels = new Pixel[_originalPixels.Length];

                for(int j=0; j<_originalPixels.Length; j++){
                    Color newColor;
                    int newBrightness;

                    if (i==0){
                        newBrightness = _originalPixels[j].Brightness + ((_originalPixels[j].Brightness-diffs[j].Bright)/numFrames);
                        newColor = Color.FromArgb(
                            _originalPixels[j].Color.R-(diffs[j].R/numFrames),
                            _originalPixels[j].Color.G-(diffs[j].G/numFrames),
                            _originalPixels[j].Color.B-(diffs[j].B/numFrames)
                        );
                    }
                    else if (i == numFrames-1){
                        newBrightness = _newPixels[j].Brightness;
                        newColor = _newPixels[j].Color;
                    } else {
                        newBrightness = frames[i-1].Pixels[j].Brightness-diffs[j].Bright/numFrames;
                        newColor = Color.FromArgb(
                            frames[i-1].Pixels[j].Color.R-(diffs[j].R/numFrames),
                            frames[i-1].Pixels[j].Color.G-(diffs[j].G/numFrames),
                            frames[i-1].Pixels[j].Color.B-(diffs[j].B/numFrames)
                        );
                    }

                    if (newBrightness > 255) newBrightness = 255;
                    if (newBrightness < 0) newBrightness = 0;

                    framePixels[j] = new Pixel(newBrightness, newColor);
                }

                frames[i] = new Frame(framePixels);
            }

            return frames;
        }
    }
}