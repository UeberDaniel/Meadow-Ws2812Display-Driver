# Ws2812Display driver for Meadow

This driver has been created for the Meadow F7v2 Feather board, but should also be compatible with other Meadow boards.

With this driver it is possible to use WS2812 LED panels like a display, i.e. you can arrange several panels horizontally, vertically or in a field and have a panel that corresponds to the total pixel count of the individual panels.

When arranging the panels, make sure that the first pixel is always at the top left or (in the second row) at the bottom right, see:

![Ws2812panelLayout](https://github.com/UeberDaniel/Ws2812Display/assets/10797624/0857ad93-002d-4b16-87b7-0020e96bcf1d)

For the display test function (in the Task Run() function) at least one panel with 8x8 is required.


TODO:

* Add support for rotating the display
* Add support for the WriteBuffer function
