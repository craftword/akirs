$(document).ready(function () {
    getPayYear2();


    function getPayYear2() {
        //alert('fghj')

        $.ajax({
            url: $('#getPayYearurl2').data('request-url'),//'IncomePayment/GetincomePayYear',
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                console.log('years', response)
                if (response.RespCode == 0) {

                    if (response.data) {

                        var dropdownValues = '<option value="">--select year--</option>';
                        $.each(response.data, function (index, value) {
                            dropdownValues += '<option value="' + value + '">' + value + '</option>'

                        })
                        $('#dropIPayYear2').append(dropdownValues);


                    }
                } else {
                    console.log("error")

                }


            },
            failure: function (xhr, status, err) {
                $('.ajax-loader').css("visibility", "hidden");
                //$('#display-error').show();
            },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            }
        });
    }


    $('#dropIPayYear2').change(function () {
        if ($(this).val()) {
            // alert($(this).val())
            $('#ModifyPay').removeAttr('disabled');
            $.ajax({
                url: $('#paymentHistoryurl').data('request-url'),//'Assessment/ModifyAssessmentDetails',
                data: { yearValue: $(this).val() },
                type: "Get",
                contentType: "application/json",
                success: function (response) {

                    if (response.RespCode == 0) {
                        $('#directassessment_partial').css('display', '')
                        if (response.data) {
                            console.log(response.data)
                            let data1 = response.data;


                            for (var i = 0; i < data1.length; i++) {

                                tableBody += `<tr>
                                                    <td>${data1[i].SourceOfIncome}</td>
                                                    <td>${data1[i].Amount}</td>
                                                    <td>${data1[i].IncomeYear}</td>
                                                    <td>${data1[i].Status}</td>
                                             <\tr>`
                            }
                            $('#tableBody').html(tableBody);

                            $('#myModal').modal({ backdrop: 'static', keyboard: false });
                            //asign all fields you need
                        }
                    } else {
                        $('#directassessment_partial').css('display', 'none')
                        alert('Assessment Record not found')
                        $('#ModifyPay').setAttribute('disabled');
                        //Alert Assessment not found
                    }
                },
                failure: function (xhr, status, err) {
                    $('.ajax-loader').css("visibility", "hidden");
                    //$('#display-error').show();
                },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                }
            });
        } else {
            //clear fields
            //alert('no')
            $('#directassessment_partial').css('display', 'none')
        }
    })


    //var table2 = $('.familytable').DataTable({
    //    "processing": true,
    //    "serverSide": true,
    //    ajax: $('#modifyAssessmenturl').data('request-url'),//"PaidAssessmentList",
    //    "type": "GET",
    //    "datatype": "json",
    //    columns: [
    //        { data: "Source Of Income" },
    //        { data: "Amount" },
    //        { data: "Year" },
    //        { data: "Status" }

    //    ]
    //});

})