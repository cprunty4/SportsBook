$(document).ready(function() {
    var startDate = moment().toISOString().substr(0, 10);
    var endDate = moment().add(6, 'month').toISOString().substr(0, 10);
    $("#startDate").val(startDate);
    $("#endDate").val(endDate);
});