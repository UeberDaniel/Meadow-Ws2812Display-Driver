using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using System;

namespace Ws2812Display
{
    /// <summary>
    /// Represents a 24bpp color pixel buffer, formated for WS2812 LED Panels
    /// </summary>
    public class Ws2812ScreenBufferGrb888 : PixelBufferBase
    {
        /// <summary>
        /// Color mode of the buffer.
        /// </summary>
        public override ColorMode ColorMode => ColorMode.Format24bppGrb888;

        /// <summary>
        /// Width in LEDs of one panel.
        /// </summary>
        private int spWidth;

        /// <summary>
        /// Height in LEDs of one panel.
        /// </summary>
        private int spHeight;

        /// <summary>
        /// Count of panels on the X-axis.
        /// </summary>
        private int pcWidth;

        /// <summary>
        /// Count of panels on the Y-axis.
        /// </summary>
        private int pcHeight;

        /// <summary>
        /// Count of LEDs in one Panel.
        /// </summary>
        private int spSize;

        /// <summary>
        /// Create a new Ws2812ScreenBufferGrb888 object.
        /// </summary>
        /// <param name="spWidth">Width in LEDs of one panel.</param>
        /// <param name="spHeight">Height in LEDs of one panel.</param>
        /// <param name="buffer">The LED buffer.</param>
        /// <param name="pcWidth">Count of panels on the X-axis.</param>
        /// <param name="pcHeight">Count of panels on the Y-axis.</param>
        public Ws2812ScreenBufferGrb888(int spWidth, int spHeight, byte[] buffer, int pcWidth = 1, int pcHeight = 1)
            : base(spWidth * pcWidth, spHeight * pcHeight, buffer)
        {
            this.spWidth = spWidth;
            this.spHeight = spHeight;
            this.pcWidth = pcWidth;
            this.pcHeight = pcHeight;
            spSize = spWidth * spHeight;
        }

        /// <summary>
        /// Create a new Ws2812ScreenBufferGrb888 object.
        /// </summary>
        /// <param name="spWidth">Width in LEDs of one panel.</param>
        /// <param name="spHeight">Height in LEDs of one panel.</param>
        /// <param name="pcWidth">Count of panels on the X-axis.</param>
        /// <param name="pcHeight">Count of panels on the Y-axis.</param>
        public Ws2812ScreenBufferGrb888(int spWidth, int spHeight, int pcWidth = 1, int pcHeight = 1)
            : base(spWidth * pcWidth, spHeight * pcHeight)
        {
            this.spWidth = spWidth;
            this.spHeight = spHeight;
            this.pcWidth = pcWidth;
            this.pcHeight = pcHeight;
            spSize = spWidth * spHeight;
        }

        /// <summary>
        /// Create a new Ws2812ScreenBufferGrb888 object.
        /// </summary>
        public Ws2812ScreenBufferGrb888() : base() { }

        /// <summary>
        /// Get the pixel color.
        /// </summary>
        /// <param name="x">The X pixel position.</param>
        /// <param name="y">The Y pixel position.</param>
        /// <returns>The pixel color.</returns>
        public override Color GetPixel(int x, int y)
        {
            int ledPos = GetPixelPos(x, y);
            byte green = Buffer[ledPos++];
            byte red = Buffer[ledPos++];
            byte blue = Buffer[ledPos];
            return new Color(red, green, blue);
        }

        /// <summary>
        /// Set the pixel color.
        /// </summary>
        /// <param name="x">The X pixel position.</param>
        /// <param name="y">The Y pixel position.</param>
        /// <param name="color">The pixel color.</param>
        public override void SetPixel(int x, int y, Color color)
        {
            int ledPos = GetPixelPos(x, y);
            Buffer[ledPos++] = color.G;
            Buffer[ledPos++] = color.R;
            Buffer[ledPos] = color.B;
        }

        /// <summary>
        /// Fill buffer with a color.
        /// </summary>
        /// <param name="color">The fill color.</param>
        public override void Fill(Color color)
        {
            {
                Buffer[0] = color.G;
                Buffer[1] = color.R;
                Buffer[2] = color.B;
                int num = Buffer.Length / 2;
                int num2;
                for (num2 = 3; num2 < num; num2 <<= 1)
                {
                    Array.Copy(Buffer, 0, Buffer, num2, num2);
                }
                Array.Copy(Buffer, 0, Buffer, num2, Buffer.Length - num2);
            }
        }

        /// <summary>
        /// Fill a region with a color.
        /// </summary>
        /// <param name="x">X start position in pixels.</param>
        /// <param name="y">Y start position in pixels.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="color">The fill color.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws an exception if fill area is beyond the buffer bounds.</exception>
        public override void Fill(int x, int y, int width, int height, Color color)
        {
            int xMax = x + width;
            int yMax = y + height;
            if (x < 0 || xMax > Width || y < 0 || yMax > Height)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int _x = x; _x < xMax; _x++)
            {
                for (int _y = y; _y < yMax; _y++)
                {
                    SetPixel(_x, _y, color);
                }
            }
        }

        /// <summary>
        /// Invert the pixel.
        /// </summary>
        /// <param name="x">The X pixel position.</param>
        /// <param name="y">The Y pixel position.</param>
        public override void InvertPixel(int x, int y)
        {
            int ledPos = GetPixelPos(x, y);
            Buffer[ledPos] = (byte)~Buffer[ledPos];
            ledPos++;
            Buffer[ledPos] = (byte)~Buffer[ledPos];
            ledPos++;
            Buffer[ledPos] = (byte)~Buffer[ledPos];
        }

        /// <summary>
        /// Write a buffer to specific location to the current buffer
        /// </summary>
        /// <param name="x">x origin</param>
        /// <param name="y">y origin</param>
        /// <param name="buffer">buffer to write</param>
        public override void WriteBuffer(int x, int y, IPixelBuffer buffer)
        {
            if (buffer.ColorMode == ColorMode.Format24bppRgb888)
            {
                for (int _x = 0; _x < buffer.Width; _x++)
                {
                    for (int _y = 0; _y < buffer.Height; _y++)
                    {
                        int posToWrite = GetPixelPos(x + _x, y + _y);
                        int posToRead = ((_y * buffer.Width) + _x) * 3;

                        Buffer[posToWrite++] = buffer.Buffer[++posToRead];
                        Buffer[posToWrite++] = buffer.Buffer[--posToRead];
                        posToRead += 2;
                        Buffer[posToWrite] = buffer.Buffer[posToRead];
                    }
                }
            }
            else base.WriteBuffer(x, y, buffer);
        }

        /// <summary>
        /// Calculates the LED position in the LED strip.
        /// See WS2812panelLayout.jpg
        /// </summary>
        /// <param name="x">The X pixel position.</param>
        /// <param name="y">The Y pixel position.</param>
        /// <returns>The pixel position in the LED strip.</returns>
        private int GetPixelPos(int x, int y)
        {
            int nativePanelX, nativePanelY, panelCountX, panelCountY, panelOffset, panelRowOffset;

            if (x >= spWidth)
            {
                nativePanelX = x % spWidth;
                panelCountX = x / spWidth;
            }
            else
            {
                nativePanelX = x;
                panelCountX = 0;
            }
            if (y >= spHeight)
            {
                nativePanelY = y % spHeight;
                panelCountY = y / spHeight;
            }
            else
            {
                nativePanelY = y;
                panelCountY = 0;
            }

            panelRowOffset = spHeight * nativePanelX;

            if (panelCountY % 2 == 0)   // Panel order: left to right        (0° rotated; 1st LED = top left)     
            {
                panelOffset = panelCountX * spSize + panelCountY * spSize * pcWidth;

                if (x % 2 == 0)         // positive colum: up -> down
                {
                    return 3 * (panelOffset + nativePanelY + panelRowOffset);
                }
                else                    // negative colum: down -> up 
                {
                    return 3 * (panelOffset + spHeight - nativePanelY - 1 + panelRowOffset);
                }
            }
            else                        // Panel order: right to left      (180° rotated; 1st LED = bottom right)
            {
                panelOffset = (pcWidth - 1 - panelCountX) * spSize + panelCountY * spSize * pcWidth + spSize;

                if (x % 2 == 0)         // negative colum: down -> up 
                {
                    return 3 * (panelOffset - spHeight + nativePanelY - panelRowOffset);
                }
                else                    // positive colum: up -> down
                {
                    return 3 * (panelOffset - nativePanelY - 1 - panelRowOffset);
                }
            }
        }
    }
}

