const fs = require('fs');
const PNG = require('pngjs').PNG;

function CornerMap (outI, sx, sy, dx, dy, bleed)
{
    const fx = dx + bleed;
    const fy = dy + bleed;
    for (let x = dx; x < fx; ++x) {
        for (let y = dy; y < fy; ++y) {
            outI.bitblt(outI, sx, sy, 1, 1, x, y);
        }
    }
}

function ReadPNG(i_in, i_out, options) {
    fs.createReadStream(i_in)
        .pipe(new PNG({
            filterType: 4
        }))
        .on('parsed', function () {
            let bleed_amount = options.bleed;

            //Dimensions of texture
            let width = this.width;
            let height = this.height;

            //Dimensions of each sprite
            let sWidth = options.sWidth || width / sXNum;
            let sHeight = options.sHeight || width / sYNum;

            //Number of sprites in each dimension
            let sXNum = options.sXNum || width / sWidth;
            let sYNum = options.sYNum || height / sHeight;

            //Output texture dimensions
            let OUT_WIDTH = (sWidth + 2*bleed_amount) * sXNum;
            let OUT_HEIGHT = (sHeight + 2*bleed_amount) * sYNum;

            let outI = new PNG({
                filterType: 4,
                width: OUT_WIDTH,
                height: OUT_HEIGHT,
                bgColor: {
                    red: 255,
                    green: 255,
                    blue: 255,
                    alpha: 0
                }
            });

            for (let xSprite = 0; xSprite < sXNum; ++xSprite)
            {
                for (let ySprite = 0; ySprite < sYNum; ++ySprite)
                {
                    //Copy sprite over
                    let x = (2 * xSprite + 1) * bleed_amount + (xSprite * sWidth);
                    let y = (2 * ySprite + 1) * bleed_amount + (ySprite * sHeight);
                    this.bitblt(outI, xSprite * sWidth, ySprite * sHeight, sWidth, sHeight, x, y);

                    //Create Bleed Corners
                    CornerMap(outI, x, y,x - bleed_amount, y - bleed_amount, bleed_amount);
                    CornerMap(outI, x + sWidth - 1, y,x + sWidth, y - bleed_amount, bleed_amount);
                    CornerMap(outI, x, y + sHeight - 1,x - bleed_amount, y + sHeight, bleed_amount);
                    CornerMap(outI, x + sWidth - 1, y + sHeight - 1,x + sWidth, y + sHeight, bleed_amount);

                    //Create Bleed Lines
                    for (let i = 1; i <= bleed_amount; ++i) { //Top
                        outI.bitblt(outI, x,y,sWidth,1,x,y - i);
                    }
                    for (let i = 0; i < bleed_amount; ++i) { //Bottom
                        outI.bitblt(outI, x,y + sHeight - 1,sWidth,1,x,y + sHeight + i);
                    }
                    for (let i = 1; i <= bleed_amount; ++i) { //Left
                        outI.bitblt(outI, x,y,1,sHeight,x-i,y);
                    }
                    for (let i = 0; i < bleed_amount; ++i) { //Right
                        outI.bitblt(outI, x + sWidth - 1,y,1,sHeight,x + i +sWidth,y);
                    }
                }
            }

            let buffer = PNG.sync.write(outI, { colorType: 6 });
            fs.writeFileSync(i_out, buffer);

        });
}

let BleedAPI = {
    ReadPNG: ReadPNG
};

module.exports = BleedAPI;