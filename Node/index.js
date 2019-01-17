const BleedAPI = require('./BleedAPI');

let options = {
    sWidth: 0,
    sHeight: 0,
    sXNum: 0,
    sYNum: 0,
    bleed: 0
};

let i = 'in.png',o = 'out.png';
process.argv.forEach((val, index) => {
    if (index <= 0) return;
    const parts = val.split(':');
    switch (parts[0].toLowerCase()) {
        case 'i':
            i = parts[1] || "in.png";
            break;
        case 'o':
            o = parts[1] || "out.png"
            break;
        case 'sw':
            options.sWidth = parseInt(parts[1]) || 0;
            break;
        case 'sh':
            options.sHeight = parseInt(parts[1]) || 0;
            break;
        case 'xn':
            options.sXNum = parseInt(parts[1]) || 0;
            break;
        case 'yn':
            options.sYNum = parseInt(parts[1]) || 0;
            break;
        case 'b':
            options.bleed = parseInt(parts[1]) || 0;
        default:
            break;
    }
});


BleedAPI.ReadPNG(i, o, options);