function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
$(function () {
    

    try {
        $("#drpRentRange").hide();
        $('#AddressOwnerType').click(function () {

            if ($(this).val() == "2") {
                $("#drpRentRange").show();
            } else {
                $("#drpRentRange").hide();
            }

        });

        $.ajax({
            url: $('#incomeviewlist').data('request-url'),//'IncomeDeclaration/IncomeTypeList',
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                if (response.data.length > 0) {
                    $("#SourceOfIncomeID").empty();
                    $("#SourceOfIncomeID").append("<option value=''> --Select Income Source-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#SourceOfIncomeID").append("<option value='" + response.data[i].ItbID + "'>" +
                             response.data[i].SourceOfIncome + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
        });
        var $reqLogin = {
            url: $('#relationshipurl').data('request-url'), //'Family/RelationShipList',
            data: null,
            type: "Get",
            contentType: "application/json"
        };

        $.ajax({
            url: $('#rentPriceurl').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                if (response.data.length > 0) {
                    $("#RentRangeId").empty();
                    $("#RentRangeId").append("<option value=''> --Select Rent Price-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#RentRangeId").append("<option value='" + response.data[i].Id + "'>" +
                             response.data[i].RentRange + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
        });

        $.ajax({
            url: $('#relationshipurl').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                if (response.data.length > 0) {
                    $("#RelationshipType").empty();
                    $("#RelationshipType").append("<option value=''> --Select RelationShip-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#RelationshipType").append("<option value='" + response.data[i].ItbID + "'>" +
                             response.data[i].RelationshipName + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
        });
        $.ajax({
            url: $('#stateloader').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                if (response.data.length > 0) {
                    $("#StateCode").empty();
                    $("#StateCode").append("<option value=''> --Select State-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#StateCode").append("<option value='" + response.data[i].StateCode + "'>" +
                             response.data[i].StateName + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
        });

        $.ajax({
            url: $('#maritalloader').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;
                if (response.data.length > 0) {
                    $("#MaritalStatus").empty();
                    $("#MaritalStatus").append("<option value=''> --Select MaritalStatus-- </option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#MaritalStatus").append("<option value='" + response.data[i].Code + "'>" +
                             response.data[i].Description + "</option>");

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

$(document).on('change', '#StateCode', function (e) {
    e.preventDefault();

    $.ajax({
        url: $('#Cityloader').data('request-url'),
        data: {StateCode:  $(this).val()},
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            var exist = false;
            if (response.data.length > 0) {
                $("#LGACode").empty();
                $("#LGACode").append("<option value=''> --Select LGA-- </option>");
                for (var i = 0; i < response.data.length; i++) {
                    $("#LGACode").append("<option value='" + response.data[i].CityCode + "'>" +
                         response.data[i].CityName + "</option>");

                }
            }
        },
        failure: function (xhr, status, err) {
            return null;
        }
    });

});