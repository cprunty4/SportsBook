$(document).ready(function () {

    $("#WagerAmount").change(function () {
        recalculate();
    });

});

function recalculate() {
    var wagerAmt = parseFloatNaN($("#WagerAmount").val());
    var winAmt = parseFloatNaN($("#WinAmount").val());
    var moneyline = parseIntNaN($("#Moneyline").val());
    //alert('moneyline' + moneyline);
    $("#WagerAmount").val(wagerAmt.toFixed(2));
    $("#WinAmount").val(winAmt.toFixed(2));
}

