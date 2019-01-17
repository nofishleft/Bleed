# Bleed
Create bleed edges on sprite sheets

## Platforms
* [Unity](#Unity)
* [Node.js](#Node.js)
* [Standalone Electron](#Standalone%20Electron)

### Unity
Uses a Unity Editor Window.

On the menu bar, click Tools > Bleed

### Node.js
Uses a command line interface.

##### Parameters
Parameter | Description
--- | ---
i | Input File
o | Output File
sw | Sprite Width
sh | Sprite Height
xn | X Sprite Count
yn | Y Sprite Count
b | Bleed Amount
##### Notes
While you can mix x and y paramters ``(i.e. Sprite Width and Y Sprite Count)``, using 2 different x's or 2 different y's will cause unexpected behaviour.

Example
`node . i:in.png o:out.png sw:4 sh:4 b:1`

### Standalone Electron
Packaged Node.js version using an Electron interface.
