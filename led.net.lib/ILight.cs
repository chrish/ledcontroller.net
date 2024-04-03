namespace led.net.app
{
    public interface ILight
    {
        public void Activate();

        public bool Show(Frame[] frames);
    }
}