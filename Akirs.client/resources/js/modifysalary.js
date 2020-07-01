$(document).ready(function () {

    function Translatemonthsindex(monthIndexValue) {
        var monthsValue = '';
        switch (monthIndexValue) {
            case 1:
                monthsValue = 'JANUARY';
                break;
            case 2:
                monthsValue = 'FEBUARY';
                break;
            case 3:
                monthsValue = 'MARCH';
                break;
            case 4:
                monthsValue = 'APRIL';
                break;
            case 5:
                monthsValue = 'MAY';
                break;
            case 6:
                monthsValue = 'JUNE';
                break;
            case 7:
                monthsValue = 'JULY';
                break;
            case 8:
                monthsValue = 'AUGUST';
                break;
            case 9:
                monthsValue = 'SEPTEMBER';
                break;
            case 10:
                monthsValue = 'OCTOBER';
                break;
            case 11:
                monthsValue = 'NOVEMBER';
                break;
            case 12:
                monthsValue = 'DECEMBER';
                break;
        }
        return monthsValue
    }

    $.ajax({
        url: $('#getsalarymonthsyear').data('request-url'),
        data: null,
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            console.log(response)
            var selectvalues = '<option value="0">--Select month and year--</option>';

            $.each(response.data, function (count, row) {
                selectvalues += '<option value="' + row.monthIndex + '-' + row.year + '">' + Translatemonthsindex(row.monthIndex) + ' - ' + row.year + '</option>'
            });
            $('#drpSalaryMonth').html(selectvalues)
        },
        failure: function (xhr, status, err) {
            $('.ajax-loader').css("visibility", "hidden");
        },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        }
    })
    $('#btnmodify').click(function () {
       // alert($('#drpSalaryMonth').val())
        $.ajax({
            url: $('#modifysalary').data('request-url'),
            type: "Post",
            data: JSON.stringify({ datemonth: $('#drpSalaryMonth').val() }),
            contentType: "application/json",
            success: function (response) {
                if (response.RespCode == 0) {
                   
                    swal({ title: 'Successfull', text: response.RespMessage, type: 'success' });
                    setTimeout(function () {  windows.location.reload(true)}, 4000)
                } else {
                    swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
                }
                //console.log(response)
                //var selectvalues = '<option value="0">--Select month and year--</option>';

                //$.each(response.data, function (count, row) {
                //    selectvalues += '<option value="' + row.monthIndex + '-' + row.year + '">' + Translatemonthsindex(row.monthIndex) + ' - ' + row.year + '</option>'
                //});
                //$('#drpSalaryMonth').html(selectvalues)
            },
            failure: function (xhr, status, err) {
                $('.ajax-loader').css("visibility", "hidden");
            },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            }
        })
    })

    $('#drpSalaryMonth').change(function () {
      $('#example123').DataTable({
          "processing": true,
          "destroy": true,
            "serverSide": true,
            "ajax": {
                "url": $('#load_salaryhistory').data('request-url'),
                "data": { datemonth :$(this).val()},
                "dataSrc": function (json) {
                    console.log(json)
                    if (json.data.length > 0)
                    {
                        $('#btnmodify').removeAttr('disabled')
                    } else {
                        $('#btnmodify').attr('disabled', 'disabled')
                    }
                    return json.data;
                }
            },//"SalList",
            "type": "GET",
            "datatype": "json",
            columns: [
                { data: "EmployeeID" },
                { data: "EmployeeName" },
                { data: "AnnualBasic" },
                { data: "AnnualHousing" },
                { data: "AnnualTransport" },
                 { data: "AnnualMeal" },
                 { data: "AnnualOthers" },
                 { data: "NHFContribution" },

                
            ]
        });
    })
});