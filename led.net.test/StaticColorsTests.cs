using System;
using System.Drawing;
using led.net.app;

namespace led.net.test;

[TestClass]
public class StaticColorsTests
{
    /// <summary>
    /// Creates a ledstrip of different configurations and checks that they are as expected
    /// </summary>
    [TestMethod]
    public void TestSetups()
    {
        ILight light = new DotStarTestLight();

        // Strip of 5 leds, 1 frame
        LedStrip ls = new LedStrip(light, 1, 1, 5);
        Transition t = ls.SetAllPixelsToColor(50, Color.Red);
        Frame[] frames = t.CalculateTransition();

        Assert.AreEqual(1, frames.Length);
        Assert.AreEqual(5, frames[0].Pixels.Length);

        // Strip of 150 leds, 100 frames
        ls = new LedStrip(light, 100, 1, 150);
        t = ls.SetAllPixelsToColor(50, Color.Red);
        frames = t.CalculateTransition();

        Assert.AreEqual(100, frames.Length);
        Assert.AreEqual(150, frames[0].Pixels.Length);
    }

    /// <summary>
    /// Sets static colors and checks that the result is as expected. 
    /// </summary>
    [TestMethod]
    public void TestStaticColors()
    {
        ILight light = new DotStarTestLight();

        LedStrip ls = new LedStrip(light, 1, 1, 5);
        Transition t = ls.SetAllPixelsToColor(50, Color.Red);
        Frame[] frames = t.CalculateTransition();

        Assert.AreEqual(1, frames.Length);
        Assert.AreEqual(5, frames[0].Pixels.Length);
        
        // Test that all pixels were set to red
        foreach (Frame f in frames)
        {
            foreach (Pixel p in f.Pixels)
            Assert.AreEqual(Color.Red.ToArgb(), p.Color.ToArgb());    
        }

        // Next color
        t = ls.SetAllPixelsToColor(50, Color.Green);
        frames = t.CalculateTransition();

        // Test that all pixels were set to blue
        foreach (Frame f in frames)
        {
            foreach (Pixel p in f.Pixels)
            Assert.AreEqual(Color.Green.ToArgb(), p.Color.ToArgb());    
        }

        // Next color
        t = ls.SetAllPixelsToColor(50, Color.Blue);
        frames = t.CalculateTransition();

        // Test that all pixels were set to blue
        foreach (Frame f in frames)
        {
            foreach (Pixel p in f.Pixels)
            Assert.AreEqual(Color.Blue.ToArgb(), p.Color.ToArgb());    
        }
    }

    /// <summary>
    /// Creates a ledstrip with a specific color, then applies a transition to a different color. 
    /// We should see a gradual fade to the new color over a number of frames.
    /// </summary>
    [TestMethod]
    public void TestTransition()
    {
        Color[] expectedColors = {ColorTranslator.FromHtml("#ffaaaa"), ColorTranslator.FromHtml("#ff5555"), ColorTranslator.FromHtml("#ff0000")};

        ILight light = new DotStarTestLight();

        // Strip of 5 leds, 3 frames
        LedStrip ls = new LedStrip(light, 3, 1, 5);
        
        // Set initial color to white
        ls.SetAllPixelsToColor(127, Color.White);
        
        Transition t = ls.SetAllPixelsToColor(127, Color.Red);
        Frame[] frames = t.CalculateTransition();

        int colorToTest = 0;

        // Each frame shold now be 1/3 of the difference between white and red.
        // Test all the pixels in all three frames:
        foreach (Frame f in frames){
            // Checking each pixel:
            foreach (Pixel p in f.Pixels){
                // Named colors doesn't work for comparison; Red and #FF0000 are treated as two different colors
                Color expected = expectedColors[colorToTest]; 
                Color actual = Color.FromArgb(p.Color.A, p.Color.R, p.Color.G, p.Color.B);

                Assert.AreEqual(expected, actual);
            }

            colorToTest++;
        }            
    }
}