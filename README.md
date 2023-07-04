# Ws2812Display driver for Meadow

This driver has been created for the Meadow F7v2 Feather board, but should also be compatible with other Meadow boards.

With this driver it is possible to use WS2812 LED panels like a display, i.e. you can arrange several panels horizontally, vertically or in a field and have a panel that corresponds to the total pixel count of the individual panels.

When arranging the panels, make sure that the first pixel is always at the top left or (in the second row) at the bottom right, see:

![Ws2812panelLayout](https://github.com/UeberDaniel/Ws2812Display/assets/10797624/9f29494c-bb41-466c-b7ae-f0bfbcaa9b17)

For the display test function (in the Task Run() function) at least one panel with 8x8 is required.

Tested with Panles from https://aliexpress.com/item/32944813367.html

With:
* 1 x 16x16 panel
* 4 x 8x8 panel in 1x4, 2x2 and 4x1 orientation
* 4 x 32x8 panel in 1x4, 2x2 and 4x1 orientation


TODO:
* Add support for rotating and flipping the display
* Add support for other paneltypes (orientation of the lines and rows of the LEDs inside a panel)
* Add support for the WriteBuffer function
