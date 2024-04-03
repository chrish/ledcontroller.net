namespace led.net.app{

    /// <summary>
    /// Represents a single frame of output.
    /// </summary>
    public class Frame{

        public Pixel[] Pixels{get;set;}
        public byte[] PixelData {get;set;}

        public byte[] Render(){
            byte[] pixelData = new byte[Pixels.Length*4];

            for(int i=0; i<Pixels.Length; i=i+4){
                pixelData[i] = (byte)Pixels[i].Brightness;
                pixelData[i+1] = Pixels[i].Color.B;
                pixelData[i+2] = Pixels[i].Color.G;
                pixelData[i+3] = Pixels[i].Color.R;
            }

            return pixelData;
        }

        public Frame(Pixel[] pixels){
            Pixels = pixels;
        }
    }
}