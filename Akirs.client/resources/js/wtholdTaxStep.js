var salarytable;

const formatter1 = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 })

$(document).ready(function () {
    function IncomeAssessment(EnrollID) {
        $.ajax({
            url: $('#getWhtNatax').data('request-url'),//'Assessment/AssessmentDetails',
            data: null,
            type: "Get",
            data: { EnrollID: EnrollID },
            contentType: "application/json",
            success: function (response) {
                console.log("im here");
                console.log('***', response)
                var output = '';

                if (response.RespCode === 0) {
                    $('#NetTaxx').val(formatter1.format(response.data));


                    //$('#nettaxx').val(response.data);

                  //var nettaxx = 0;
                    console.log('**data', response.data);

                 
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
        })
    }

    $('#btnUploadWht').click(function (e) {

        console.log('uploading');
        $("#example1_wht tbody").empty();
        e.preventDefault();
        if (window.FormData !== undefined) {
            var fileUpload = $('#upldfileWht').get(0);
            var files = fileUpload.files;
            if (files.length <= 0) {
                alert("Please select the file you are to upload!");
                return;
            }
            //$('#btnValidate, #btnSave').prop('disabled', true);

            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append(files[x].name + x, files[x]);
            }
            $.ajax({
                type: "POST",
                url: $('#whtupload').data('request-url'),//'wht/UploadFiles',
                contentType: false,
                processData: false,
                data: data,
                success: function (response) {
                    if (response.RespCode === 0) {
                        swal(
                            'Successful!',
                            response.RespMessage,
                            'success'
                        )
                        $('#upldfileWht').val('');
                        $('#divGridwht').html(response.data_html);

                    }
                    else {
                        swal(
                            'Error!',
                            response.RespMessage,
                            'error'
                        )
                        //displayDialogNoty('Notification', response.RespMessage);
                    }
                },
                error: function (err) {
                    swal(
                        'Error!',
                        'A critical error occured',
                        'error'
                    )
                }
            });

        }
        else {
            swal('Error!', "This browser doesn't support HTML5 file uploads!", 'error');
        }
    });
    
      

                    //console.log('***Josn', datadetails);
      $(document).on('click', '#btnSaveDirectAccessment',
             function (e) {
                        e.preventDefault()
                        alert('gghh')
                        var datadetails = {
                            EnrollmentViewModel: {
                                Companyname: $('#Companyname').val(),
                                Email: $('#Email').val(),
                                Phonenumber: $('#Phonenumber').val(),
                                Address: $('#Address').val(),
                                TaxtypeID: "4"
                            }
                            // Salaryupload_temp: salarytable,
                            //WHTUPLOAD: Witholding
                        }
                        console.log('***Josn', datadetails);

                        $.ajax({
                            url: $('#postdirectAssessment').data('request-url'),//'IncomeDeclaration/IncomeTypeList',
                            data: JSON.stringify(datadetails),
                            type: "Post",
                            contentType: "application/json",
                            success: function (response) {
                                console.log('***response', response);
                                if (response.RespCode === 0) {
                                    swal(
                                        'Successful!',
                                        response.RespMessage,
                                        'success'

                                    )
                                    window.location.href = '/Home/Login';
                                    return false;
                                    $('#btnSaveDirectAccessment').attr('disabled', 'disabled');
                                    $('#directstepwizard').hide();
                                    $('#listAssessment_table').show();
                                    IncomeAssessment(response.EnrollID)

                                } else {
                                    swal(
                                        'Error!',
                                        response.RespMessage,
                                        'error'
                                    )
                                }

                            },
                            failure: function (xhr, status, err) {
                                return null;
                            }
                        });

                    })                   
         
    });

    

