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

## IMPORTANT:
The conversion requires a lot of memory so this leads to a crash if no attention is paid to the following lines.
The memory used should be calculated beforehand.
When changing the image (with Micrographics api, or any other), the previously used image RAM should be released / destructed (depending on the display resolution / ram usage).
The required net LED COUNT RAM utilization is equal to the number of pixels in the LED grid * 8 and this sum * 3 since we have 3 color bytes per LED here.
There is no C#, IGraphicsDisplay (and no JPEG to binary conversion) or Mono overhead not included, just the used bytes for the buffers.

## HINT:
Since the Meadow Feather F7 board does not yet offer the possibility to run in resource utilization debug mode / display the RAM utilization at runtime, I could not determine or name the utilization.
With 32 16x16 panels -> effectively almost 200 Kbyte RAM usage (only buffers) I already had a crash when I had loaded an image and was about to load the next one without freeing the memory of the last image.
So always free up the memory that is no longer in use.

## TODO:
* Add support for rotating and flipping the display
* Add support for other paneltypes (orientation of the lines and rows of the LEDs inside a panel)
