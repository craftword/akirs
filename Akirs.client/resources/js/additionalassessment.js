
function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
$(document).ready(function () {

    getPayYear();
    var enrollid = $('#eid').val();
    let tableBody = '';


    function addGridItem(model) {
        table2.row.add(model).draw();
    }
    function updateGridItem(model) {
        var rowIdx = table2
            .cell(col)
            .index().row;

        var d = table2.row(rowIdx).data();
        d.FullName = model.FullName;
        d.RelationshipType = model.RelationshipType;
        d.Age = model.Age;
        d.Status = model.Status

        table2
            .row(rowIdx)
            .data(d)
            .draw();

    }

    $('#dropIPayYear').change(function () {
        if ($(this).val()) {
           // alert($(this).val())
            $('#ModifyPay').removeAttr('disabled');
            $.ajax({
                url: $('#modifyAssessmenturl').data('request-url'),//'Assessment/ModifyAssessmentDetails',
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


    //Getting Pay Year for Modify Assessment
    function getPayYear() {


        $.ajax({
            url: $('#getPayYearurl').data('request-url'),//'Assessment/ModifyAssessmentDetails',
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                console.log('direct years', response)
                if (response.RespCode == 0) {
                 

                    if (response.data) {

                        var dropdownValues = '<option value="">--select year--</option>';
                        $.each(response.data, function (index, value) {
                            dropdownValues += '<option value="' + value + '">' + value + '</option>'

                        })
                        $('#dropIPayYear').append(dropdownValues);
                      
                   
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

     

    var table2 = $('.familytable').DataTable({
        "processing": true,
        "serverSide": true,
        ajax: $('#modifyAssessmenturl').data('request-url'),//"PaidAssessmentList",
        "type": "GET",
        "datatype": "json",
        columns: [
            { data: "Source Of Income" },
            { data: "Amount" },
            { data: "Year" },
            { data: "Status" }            

        ]
    });

    //Posting Back to Income Source Table
    $('#ModifyPay').click(function () {
        //alert('Kindly Proceed to Assessment to Pay')
       // location.reload();
       // var e = document.getElementById("dropIPayYear");
        var result = $('#dropIPayYear option:selected').text();
        

        $.ajax({
            url: $('#incomeModifyurl').data('request-url'),//'Assessment/ModifyAssessmentDetails',
            data: { yearValue: result },
            type: "Post",
            contentType: "application/x-www-form-urlencoded",
            success: function (response) {
                
                if (response.RespCode == 0) {
                    // $('#familytable')[0].reset();

                    swal(
                      'Successful! Kindly go to Income Declaration to make changes ',
                      response.RespMessage,
                      'success'
                  )

                    setTimeout(function () { window.location.reload(true) }, 6000)
                    
                   
                } else {
                    // $('#directassessment_partial').css('display', 'none')
                    alert('No Record to Modify')
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

        //clear fields
        //alert('no')
        $('#directassessment_partial').css('display', 'none')
    });


    
});