using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ws2812Display
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        private Random rand = new Random();
        private Ws2812Display display;
        private MicroGraphics graphics;
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            // For the display test function, at least one panel with 8x8 is required.
            // If the panel / grid is larger, the corresponding parameters of the Ws2812Display must be adjusted.
            display = new Ws2812Display(
                spiBus:             Device.CreateSpiBus(),
                panelWidth:         8, 
                panelHeight:        8, 
                panelCountWidth:    1, 
                panelCountHeight:   1,
                brightness:         Ws2812Display.DisplayBrightness.Level_3);

            graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true
            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            while(true) 
            { 
                // Ws2812 Display Test

                for (int i = 0; i < 8; i++)
                {
                    display.Brightness = (Ws2812Display.DisplayBrightness)i;
                    byte r = (byte)rand.Next(0, 255);
                    byte g = (byte)rand.Next(0, 255);
                    byte b = (byte)rand.Next(0, 255);
                    display.Fill(new Color(r, g, b));
                    display.Show();
                    await Task.Delay(250);
                    display.Clear();
                }

                await Task.Delay(250);
                display.Fill(Color.Red);
                display.Show();
                await Task.Delay(250);
                display.Fill(5, 5, 2, 2, Color.Green);
                display.Show();
                await Task.Delay(250);
                display.Clear();
                await Task.Delay(250);

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        display.DrawPixel(x, y, Color.OldLace);   // R=253, G=245, B=230
                        display.Show();
                        await Task.Delay(5);
                    }
                }
                Resolver.Log.Info("Current color: " + display.PixelBuffer.GetPixel(0, 0).ToString());
                await Task.Delay(250);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        display.InvertPixel(x, y);
                        display.Show();
                        await Task.Delay(5);
                    }
                }
                Resolver.Log.Info("Current color: " + display.PixelBuffer.GetPixel(0, 0).ToString());

                display.Brightness = Ws2812Display.DisplayBrightness.Level_2;


                // MicroGraphics Test

                graphics.Clear();
                graphics.DrawTriangle(1, 0, 7, 0, 7, 6, Color.HotPink, true);
                graphics.DrawTriangle(0, 1, 0, 7, 6, 7, Color.DarkSlateBlue, true);
                graphics.Show();
                await Task.Delay(250);
                graphics.Clear();
                await Task.Delay(250);

                graphics.DrawRectangle(3, 3, 2, 2, Color.Navy, true);
                graphics.Show();
                await Task.Delay(150);
                graphics.Clear();
                graphics.DrawRectangle(2, 2, 4, 4, Color.Navy, true);
                graphics.Show();
                await Task.Delay(150);
                graphics.Clear();
                graphics.DrawRectangle(1, 1, 6, 6, Color.Navy, false);
                graphics.Show();
                await Task.Delay(150);
                graphics.Clear();
                await Task.Delay(250);

                graphics.CurrentFont = new Font4x6();
                graphics.DrawText(0, 1, "HI", Color.Red);
                graphics.Show();
                await Task.Delay(2000);
                graphics.Clear();
                await Task.Delay(250);

                // Image Test

                display.Brightness = Ws2812Display.DisplayBrightness.Level_7;

                DrawImageFromFile(0, 0);
                await Task.Delay(2000);

                DrawImageFromResource(0, 0);
                await Task.Delay(2000);
            }
        }

        private void DrawImageFromFile(int x = 0, int y = 0)
        {
            Resolver.Log.Info("Showing file...");
            var filePath = Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, $"snake.bmp");
            var image = Image.LoadFromFile(filePath);
            graphics.Clear();
            graphics.DrawImage(x, y, image);
            graphics.Show();
        }

        private void DrawImageFromResource(int x = 0, int y = 0)
        {
            Resolver.Log.Info("Showing resource...");
            var image = Image.LoadFromResource($"zombie_res.bmp");
            graphics.Clear();
            graphics.DrawImage(x, y, image);
            graphics.Show();
        }
    }
}