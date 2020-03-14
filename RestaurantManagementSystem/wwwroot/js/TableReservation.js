var Id = 0;
$(document).ready(function () {
    $("#TablesImage img").each(function () {
        var status = $(this).data("poss");
        if (status == false)
        {
           $(this).off('click');
        }
    });
    $("#ReserveButton").attr("disabled", true);
});
$('#TablesImage img').click(function () {
   Id = $(this).attr('alt');
    if ($(this).attr('src') == "/images/table (3).svg")
    {
        $(this).attr('src', "/images/table (5).svg");
        $("#ReserveButton").attr("disabled", true);
    }
    else
    {
       $(this).attr('src', "/images/table (3).svg");
       $("#TablesImage img").each(function () {
           var status = $(this).data("conf");
            var statusPos = $(this).data("poss");
           if (status == false && statusPos == true
                && $(this).attr('alt') != Id &&
               $(this).attr('src') == "/images/table (3).svg"
           )
           {
            $(this).attr('src', "/images/table (5).svg");
            }
       });
     }

    if ($(this).attr('src') == "/images/table (3).svg")
    {
     $("#ReserveButton").removeAttr("disabled");
    }
});

var FromfocusOut = false;
var TofocusOut = false;
$("#ReserveButton").click(function ()
{
    var url = "/Customer/Home/GetTableName";
    $.getJSON(url, {TableId: Id }, function (data) {
            $("#TableName").html(data + " table");
       });
    $("#FromTime").val("");
    $("#ToTime").val("");
    FromfocusOut = false;
    TofocusOut = false;
    $("#ConfirmTime").attr("disabled", true);
       
});
var fromTime=0;
var toTime =0;
var hours = 0;
var minutes = 0;
 $("#FromTime").focusout(function () {
     FromfocusOut = true;
    fromTime = $("#FromTime").val();
    toTime = $("#ToTime").val();
    hours = toTime.split(':')[0] - fromTime.split(':')[0];
    minutes = toTime.split(':')[1] - fromTime.split(':')[1];
    if ( FromfocusOut == true
        && TofocusOut==true &&
        (hours > 0 || minutes > 0)) {
        $("#ConfirmTime").removeAttr("disabled");
     }
});
$("#ToTime").focusout(function () {
    fromTime = $("#FromTime").val();
    toTime = $("#ToTime").val();
    hours = toTime.split(':')[0] - fromTime.split(':')[0];
    minutes = toTime.split(':')[1] - fromTime.split(':')[1];
    TofocusOut = true;
    if (FromfocusOut == true
        && TofocusOut == true &&
        (hours > 0 || minutes > 0)) {
        $("#ConfirmTime").removeAttr("disabled");
    }
});
$("#ConfirmTime").click(function () {
    var url = "/Customer/Home/TableReservationSet";
     $.getJSON(url, {From: fromTime, To: toTime, TableId: Id }, function (data) {

    });
});









    