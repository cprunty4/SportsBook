$(document).ready(function () {

    $("#WagerAmount").change(function () {
        recalculate();
    });

});

function recalculate() {
    var wagerAmt = parseFloatNaN($("#WagerAmount").val());
    var profit = parseFloatNaN($("#WinAmount").val());
    var moneyline = parseIntNaN($("#Moneyline").val());
    var odds = 0;
    var payout = 0;
    if (moneyline < 0) {
        odds = (100 / Math.abs(moneyline)) + 1;
        payout = odds * wagerAmt;
        profit = payout - wagerAmt;
        winAmt = profit;
    } else if (moneyline > 0) {

    }
    $("#PayoutAmount").val(payout.toFixed(2));
    $("#WagerAmount").val(wagerAmt.toFixed(2));
    $("#WinAmount").val(winAmt.toFixed(2));
}

