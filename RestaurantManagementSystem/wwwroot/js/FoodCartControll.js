
$(document).ready(function () {
        $("[id^=Quantity]").each(function () {
            var C = $(this).closest("tr").find("#Quantity").val()
            if (C == 1) {
                $(this).closest("tr").find("#Minus").attr("disabled", true);
            }
        });
});

$("table").on("click", "#DeleteMe", function (event) {
    var FoodId = $(this).closest("tr").find('td:eq(5)').text();
    var url = "/Home/DeleteCart";
     $.getJSON(url, {id: FoodId }, function (data) {
       var FinalCost = data;
       $("#FinalTotal").html(FinalCost+" tk");
     });
    $(this).closest("tr").remove();
});
$("button").click(function () {
     var value = $(this).attr("value");
    if (value == "+")
    {
       var Q = $(this).closest("tr").find("#Quantity").val();
       var TotalQuantity = Number(Q) + Number(1);
       $(this).closest("tr").find("#Quantity").val(TotalQuantity);
        if (TotalQuantity > 1)
        {
            $(this).closest("tr").find('#Minus').removeAttr("disabled");
        }
       var FoodId = $(this).closest("tr").find('td:eq(5)').text();
        var url = "/Home/SetCartValueUpdated";
         $.getJSON(url, {id: FoodId, Quantity: TotalQuantity }, function (data) {
              var FinalCost = data;
            $("#FinalTotal").html(FinalCost+" tk");
         });
        var FoodPrice = $(this).closest("tr").find("#Ta").val();
        var CalculatedTotalPrice = Number(FoodPrice) * Number(TotalQuantity)
        $(this).closest("tr").find("#totalprice").html(CalculatedTotalPrice + " tk");

    }
     else if (value == "-") {
         var FoodId = $(this).closest("tr").find('td:eq(5)').text();
         var FoodPrice = $(this).closest("tr").find("#Ta").val();
         var Q = $(this).closest("tr").find("#Quantity").val();
         var TotalQuantity = Number(Q) - Number(1);
         $(this).closest("tr").find("#Quantity").val(TotalQuantity);
          $(this).closest("tr").find("#totalprice").html(Number(FoodPrice) * Number(TotalQuantity) + " tk");
        if (TotalQuantity == 1)
        {
            $(this).closest("tr").find("#Minus").attr("disabled", true);
        }


        var url = "/Home/SetCartValueUpdated";
         $.getJSON(url, {id: FoodId, Quantity: TotalQuantity }, function (data) {
           var FinalCost = data;
            $("#FinalTotal").html(FinalCost+" tk");

          });
     }
});

