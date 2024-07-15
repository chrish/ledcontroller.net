using Newtonsoft.Json;

namespace led.net.app{

    /// <summary>
    /// Represents a single frame of output.
    /// </summary>
    public class Frame{

        public Pixel[] Pixels{get;set;}

        public byte[] GetPixelData (){
            byte[] pixelData = new byte[Pixels.Length*4];

            for(int i=0; i<Pixels.Length; i++){
                pixelData[i*4] = (byte)Pixels[i].Brightness;
                pixelData[i*4+1] = Pixels[i].Color.B;
                pixelData[i*4+2] = Pixels[i].Color.G;
                pixelData[i*4+3] = Pixels[i].Color.R;
            }

            return pixelData;
        }

        public Frame(Pixel[] pixels){
            Pixels = pixels;
        }

        public string Serialize(){
            return JsonConvert.SerializeObject(this);
        }
    }
}