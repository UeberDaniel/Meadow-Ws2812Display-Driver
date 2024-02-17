using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Hardware;
using Meadow.Units;
using System;

namespace Ws2812Display
{
    public class Ws2812Display : IGraphicsDisplay, ISpiPeripheral
    {
        public enum DisplayBrightness { Level_0 = 7, Level_1 = 6, Level_2 = 5, Level_3 = 4, Level_4 = 3, Level_5 = 2, Level_6 = 1, Level_7 = 0 }
        /// <summary>
        /// Brightness level for the Display: Level_0(dark) - Level_7(bright)
        /// </summary>
        public DisplayBrightness Brightness { get; set; }
        public ColorMode ColorMode => imageBuffer.ColorMode;
        public ColorMode SupportedColorModes { get; }
        public int Width => imageBuffer.Width;
        public int Height => imageBuffer.Height;
        public IPixelBuffer PixelBuffer => imageBuffer;

        /// <summary>
        /// The offscreen image buffer with unmanipulated brightness
        /// </summary>
        protected IPixelBuffer imageBuffer;

        /// <summary>
        /// The LED outputStream with adjusted Brightness 
        /// </summary>
        private byte[] outputStream;

        /// <summary>
        /// The read buffer (we don't need)
        /// </summary>
        protected Memory<byte> readBuffer;

        /// <summary>
        /// The color-byte-to-led-byte converter array
        /// </summary>
        private static readonly byte[] ws2812Bytes = new byte[] { 0x44, 0x46, 0x64, 0x66 };

        /// <summary>
        /// SPI Communication bus used to communicate with the peripheral
        /// </summary>
        protected ISpiCommunications spiComms;

        /// <summary>
        /// The default SPI bus speed for the device
        /// </summary>
        public Frequency DefaultSpiBusSpeed => new Frequency(3.2, Frequency.UnitType.Megahertz);

        /// <summary>
        /// The SPI bus speed for the device
        /// </summary>
        public Frequency SpiBusSpeed
        {
            get => spiComms.BusSpeed;
            set => spiComms.BusSpeed = value;
        }

        /// <summary>
        /// The default SPI bus mode for the device
        /// </summary>
        public SpiClockConfiguration.Mode DefaultSpiBusMode => SpiClockConfiguration.Mode.Mode0;

        /// <summary>
        /// The SPI bus mode for the device
        /// </summary>
        public SpiClockConfiguration.Mode SpiBusMode
        {
            get => spiComms.BusMode;
            set => spiComms.BusMode = value;
        }

        /// <summary>
        /// Create a new Ws2812Display object.
        /// </summary>
        /// <param name="spiBus">The SPI bus we are using.</param>
        /// <param name="panelWidth">Width in LEDs of one panel.</param>
        /// <param name="panelHeight">Height in LEDs of one panel.</param>
        /// <param name="panelCountWidth">Count of panels on the X-axis.</param>
        /// <param name="panelCountHeight">Count of panels on the Y-axis.</param>
        /// <param name="brightness">Brightness of the display.</param>
        public Ws2812Display(ISpiBus spiBus, int panelWidth, int panelHeight, int panelCountWidth, int panelCountHeight, DisplayBrightness brightness = DisplayBrightness.Level_4)
        {
            spiComms = new SpiCommunications(spiBus, null, DefaultSpiBusSpeed, DefaultSpiBusMode);
            Brightness = brightness;
            imageBuffer = new Ws2812ScreenBufferGrb888(panelWidth, panelHeight, panelCountWidth, panelCountHeight);
            outputStream = new byte[panelWidth * panelHeight * panelCountWidth * panelCountHeight * 4 * 3];
        }

        public void Clear(bool updateDisplay = false)
        {
            imageBuffer.Clear();
            if (updateDisplay) Show();
        }

        public void DrawPixel(int x, int y, Color color)
        {
            imageBuffer.SetPixel(x, y, color);
        }

        public void DrawPixel(int x, int y, bool enabled)
        {
            if (enabled) imageBuffer.SetPixel(x, y, Color.White);
            else imageBuffer.SetPixel(x, y, Color.Black);
        }

        public void Fill(Color color, bool updateDisplay = false)
        {
            imageBuffer.Fill(color);
            if (updateDisplay)
            {
                Show();
            }
        }

        public void Fill(int x, int y, int width, int height, Color fillColor)
        {
            imageBuffer.Fill(x, y, width, height, fillColor);
        }

        public void InvertPixel(int x, int y)
        {
            imageBuffer.InvertPixel(x, y);
        }

        public void Show()
        {
            FormatOutputStream();
            spiComms.Write(outputStream);
        }

        public void Show(int left, int top, int right, int bottom)
        {
            Show();
        }

        public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
        {
            if (((displayBuffer.Width + x) <= Width) && ((displayBuffer.Height + y) <= Height)) WriteBuffer(x, y, displayBuffer);
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Translates the imageBuffer to the outputStream and adds the brightness.
        /// edited from https://github.com/bcr/Meadow.Ws2812/blob/main/Meadow.Ws2812/Ws2812.cs
        /// </summary>
        private void FormatOutputStream()
        {
            byte brightness = (byte)Brightness;
            int position;
            byte theByte;
            for (int index = 0; index < imageBuffer.Buffer.Length; index++)
            {
                position = index << 2;
                theByte = (byte)(imageBuffer.Buffer[index] >> brightness);

                for (int counter = 0; counter < 4; ++counter)
                {
                    outputStream[position++] = ws2812Bytes[(theByte & 0b1100_0000) >> 6];
                    theByte <<= 2;
                }
            }
        }
    }
}

















