# Bleed
Create bleed edges on sprite sheets

## Platforms
* [Unity](#Unity)
* [Node.js Module](#nodejs)
* [Packaged Command Line Interface](#packaged-cli)
* [Standalone Electron](#standalone-electron)

## Unity
Uses a Unity Editor Window.

On the menu bar, click Tools > Bleed

![Image Couldn't Load](https://i.imgur.com/9HHNIhF.png)

Built and tested on 2018.3.0f2.

#### Installation
Download .unitypackage [here](https://github.com/nofishleft/Bleed/releases)

Open with unity.

## Node.js
[npm](https://www.npmjs.com/package/bleed-node)

#### Installation
`npm i bleed-node`

See the [CLI](#packaged-cli) for an example on how to use bleed-node

## Packaged CLI
Uses a command line interface. [Source](https://github.com/nofishleft/Bleed/tree/master/Node)

#### Installation
Download packaged file [here](https://github.com/nofishleft/Bleed/releases), look for `bleed-cli-vX.X.X-platform-arch.`

#### Parameters
Parameter | Description
:---: | :---:
i | Input File
o | Output File
sw | Sprite Width
sh | Sprite Height
xn | X Sprite Count
yn | Y Sprite Count
b | Bleed Amount

#### Notes
While you can mix x and y parameters ``(i.e. Sprite Width and Y Sprite Count)``, using 2 different x's or 2 different y's will cause unexpected behaviour.

#### Usage
##### Unpackaged
`node . i:in.png o:out.png sw:4 sh:4 b:1`
##### Packaged
`./EXE_FILE_NAME i:in.png o:out.png sw:4 sh:4 b:1`

## Standalone Electron
Packaged Node.js version using an Electron interface.

![Image Couldn't Load](https://i.imgur.com/3T0uuzW.png)

![Image Couldn't Load](https://i.imgur.com/4wYaDME.png)

Download standalone [here](https://github.com/nofishleft/Bleed/releases), look for `bleed-electron-vX.X.X-platform-arch`.
