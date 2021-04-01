// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function parseFloatNaN(arg) {
    var newVal = parseFloat(arg);
    if (isNaN(newVal)) newVal = 0;
    return newVal;
}

function parseIntNaN(arg) {
    var newVal = parseInt(arg);
    if (isNaN(newVal)) newVal = 0;
    return newVal;
}