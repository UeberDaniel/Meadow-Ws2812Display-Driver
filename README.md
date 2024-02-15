# Ws2812Display driver for Meadow

![Screenshot 2023-08-07 112907](https://github.com/UeberDaniel/Meadow-Ws2812Display-Driver/assets/10797624/b8f535aa-3b42-4ee1-afab-a1b46d67cf3a)![Screenshot 2023-08-07 113022](https://github.com/UeberDaniel/Meadow-Ws2812Display-Driver/assets/10797624/05bff9a8-2bd7-4799-98f2-fc7dfc162d9b)


## IMPORTANT! :
**Make sure that you have a sufficiently large external power source for many LEDs (min. 6 Amps per 256 LEDs).**

**!DO NOT USE THE 5 VOLTS FROM THE FEATHER BOARD!**

**Also, these panels (especially 16x16 / 32x8 panels) get very hot. Without sufficient cooling, there is a risk of fire when used at full brightness for a longer period of time!**

## USAGE :
This driver has been created for the Meadow F7v2 Feather board, but should also be compatible with other Meadow boards.

With this driver it is possible to use WS2812 LED panels like a display, i.e. you can arrange several panels horizontally, vertically or in a field and have a panel that corresponds to the total pixel count of the individual panels.

This driver uses the SPI-Bus, you have to connect the COPI/MOSI pin with the DIN line of the first Panel, also make sure to connect a GND pin with the first panel to have a stable data stream.

When arranging the panels, make sure that the first pixel is always at the top left or (in the second row) at the bottom right, see:

![Ws2812panelLayout](https://github.com/UeberDaniel/Ws2812Display/assets/10797624/9f29494c-bb41-466c-b7ae-f0bfbcaa9b17)

For the display test function (in the Task Run() function) at least one panel with 8x8 is required.

Tested with panles from [https://aliexpress.com/item/32944813367.html](https://de.aliexpress.com/item/32944813367.html)

With:
* 1 x 16x16 panel
* 4 x 8x8 panel in 1x4, 2x2 and 4x1 orientation
* 4 x 32x8 panel in 1x4, 2x2 and 4x1 orientation
* 32 x 16x16 panel in 8x4 orientation

## UPDATE:
This may have an effect when using images with an LED display (if it has a high number of pixels), the conversion requires a lot of memory and can lead to a crash in the worst case. The memory used should be calculated beforehand. When changing the image (with Micrographics), the previously used ram should be released again (depending on the display resolution, if there was a high RAM usage it ca lead to a crash). This means the required net RAM bytes utilization for this extension is equal to the number of pixels in the LED grid * 7 (for the output stream) * 3 (for the screen buffer) + ( 1 * LED grid (for the lookup table)) + 4 bytes for the byte to LED Strop mapping. Means 11 * the number of bytes of the RGB LED number + 4 bytes. IGraphicsDisplay & ISpiPeripheral & C# mono overhead not included.

## TODO:
* Add support for rotating and flipping the display
* Add support for other paneltypes (orientation of the lines and rows of the LEDs inside a panel)
