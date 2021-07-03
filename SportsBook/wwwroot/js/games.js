$(document).ready(function() {
    var startDate = moment().subtract(11, 'month').toISOString().substr(0, 10);
    var endDate = moment().toISOString().substr(0, 10);
    $("#startDate").val(startDate);
    $("#endDate").val(endDate);
});