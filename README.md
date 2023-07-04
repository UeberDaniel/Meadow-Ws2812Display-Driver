# Ws2812Display driver for Meadow

This driver has been created for the Meadow Feather V2 board, but should also be compatible with other Meadow boards.

With this driver it is possible to use WS2812 LED panels like a display, i.e. you can arrange several panels horizontally, vertically or in a field and have a panel that corresponds to the total pixel count of the individual panels.

When arranging the panels, make sure that the first pixel is always at the top left or (in the second row) at the bottom right, see:




TODO:

* Add support for rotating the display
* Add support for the WriteBuffer function