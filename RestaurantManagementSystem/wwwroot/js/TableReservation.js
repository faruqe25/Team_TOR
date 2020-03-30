////This method is to get the ToolTip Value from Database
function LoadData() {
    var result = " ";
    $.ajax('/Home/GetTableIfnoToolTip', {
        type: 'GET',  // http method
        data: { TableId: $(this).attr('alt') },  // data to submit
        dataType: 'json',
        async: false,
        success: function (data) {
            var AllowtoBook = "";
            if (data.bookedStatus == false) {
                AllowtoBook = "Table is free."
            }
            else {
                AllowtoBook = "This is already booked";
            }
            result = "<b>Table Name </b> : " + data.tableNumber + " <br><b>Capacity</b> : " + data.tableCapacity + " person <br><b>Reserve fee</b> : " + data.bookingPrice + " tk <br><b>Table Status</b> : " + AllowtoBook + " <br>";
        },

    });
    return result;
}
$("#TablesImage img").tooltip({
    placement: 'top',
    html: true,
    title: LoadData,


});

/// Done Method



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
     if (FromfocusOut == true
         && TofocusOut == true &&
         (hours > 0 || minutes > 0)) {
         $("#ConfirmTime").removeAttr("disabled");
     }
     else {
         $("#ConfirmTime").attr("disabled", true);
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
    else {
        $("#ConfirmTime").attr("disabled", true);
    }
});
$("#ConfirmTime").click(function () {
    var url = "/Customer/Home/TableReservationSet";
     $.getJSON(url, {From: fromTime, To: toTime, TableId: Id }, function (data) {

    });
});









    