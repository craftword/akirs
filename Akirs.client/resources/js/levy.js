$(function () {
    try {
       
        $.ajax({
            url: $('#ministryloader').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                console.log(response)
                if (response.data.length > 0) {
                    $("#MinistryCode").empty();
                    $("#MinistryCode").append("<option value=''> --Select Ministry-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#MinistryCode").append("<option value='" + response.data[i].MinistryCode + "'>" +
                             response.data[i].MinistryDesc + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
        });

    }
    catch (err) {

    }
});

$(document).on('change', '#MinistryCode', function (e) {
    e.preventDefault();

    $.ajax({
        url: $('#revenueloader').data('request-url'),
        data: { MinistryCode: $(this).val() },
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            var exist = false;
            if (response.data.length > 0) {
                $("#RevenueCode").empty();
                $("#RevenueCode").append("<option value=''> --Select Revenue-- </option>");
                for (var i = 0; i < response.data.length; i++) {
                    $("#RevenueCode").append("<option value='" + response.data[i].RevenueCode + "'>" +
                        response.data[i].RevenueHead1 + "</option>");

                }
            }
        },
        failure: function (xhr, status, err) {
            return null;
        }
    });

  
});