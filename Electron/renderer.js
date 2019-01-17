'use strict';

window.$ = window.jQuery = require('jquery');
window.Tether = require('tether');
window.Bootstrap = require('bootstrap');

titlebar.on('minimize', (e) => {
    console.log(e);
    var win = remote.getCurrentWindow();
    win.minimize();
});
titlebar.on('maximize', (e) => {
    console.log(e);
    var win = remote.getCurrentWindow();
    if (!win.isMaximized()) {
        win.maximize();
    } else {
        win.unmaximize();
    }
});

titlebar.on('fullscreen', (e) => {
    console.log(e);
    var win = remote.getCurrentWindow();
    if (!win.isMaximized()) {
        win.maximize();
    } else {
        win.unmaximize();
    }
});

titlebar.on('close', (e) => {
    console.log(e);
    var win = remote.getCurrentWindow();
    win.close();
});

function fullscreen() {
    var win = remote.getCurrentWindow();
    win.setFullScreen(!win.isFullScreen());
}