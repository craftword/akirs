
const formatter1 = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 })

let payment = {};

var nettaxx = 0;


$(document).ready(function () {

    var eID = $('#enrId').val();
    console.log(eID);

    


        $.ajax({
            url: $('#assessmentload').data('request-url'),//'Assessment/AssessmentDetails',
            data: null,
            type: "Get",
            data: { EnrollID: eID },
            contentType: "application/json",
            success: function (response) {
                console.log("im rkvboibrfjbfjk");
                console.log('***', response)
                
               
                if (response.RespCode === 0)
                {
                    

                  
                    $.each(response.data, function (count, row) {
                       // console.log('**index', row)
                        nettaxx += row.NetTax;
                        console.log(nettaxx)
                        // var amttt = forma

                        $('#txtnettax').val(formatter1.format(nettaxx));

                    })
                   // $('#txtamthid').val(nettaxx);

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
    

    $.ajax({
        url: $('#userdetails').data('request-url'),//'Assessment/AssessmentDetails',
        type: "GET",
        data: { EnrollID: eID },
        contentType: "application/json",
        success: function (response) {
          
            console.log(response)
            if (response.RespCode == 0) {
                payment.email = response.data.Email;
                payment.firstname = response.data.FirstName;
                payment.lastname = response.data.LastName;
                payment.phone = response.data.PhoneNo;
                payment.custId = response.data.EnrollmentId;


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

    $.ajax({
        type: "GET",
        url: $('#salaryverify').data('request-url'),
        contentType: 'json',
        data: null,
        success: function (response) {
            //loaderSpin2(false);
            if (response.RespCode == 0) {

                $('#divGrid').html(response.data_html);
                $('#btnValidate').removeAttr('disabled');

            }
            else {
                //displayDialogNoty('Notification', response.RespMessage);
            }
        },
        error: function (err) {
            console.log(err);
            //loaderSpin2(false);
        }
    });
    $(document).on('click', '#btnValidateSalary', function (e) {
        e.preventDefault();

        //return;
        $.ajax({
            type: "POST",
            url: $('#salaryverifypost').data('request-url'), //url + 'VerifyRecord',
            contentType: "application/json",
            data: null,
            success: function (response) {
                if (response.RespCode == 0) {

                    $('#divGrid').html(response.data_html);
                    $('#btnValidateSalary').attr('disabled','disabled');
                    swal({ title: 'Successfull', text: 'Record approved successfully', type: 'success' })

                }
                else {
                    swal({ title: 'Error!!', text: response.RespMessage, type: 'error' })
                    //displayDialogNoty('Notification', response.RespMessage);
                }
            },
            error: function (err) {
                console.log(err);
            }

        });

    });
    var table2 = $('#example123').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": $('#usersalarylist').data('request-url'),//"SalList",
        "type": "GET",
        "datatype": "json",
        columns: [
            { data: "EmployeeID" },
            { data: "EmployeeName" },
            { data: "AnnualBasic" },
            { data: "AnnualHousing" },
            { data: "AnnualTransport" },
            
            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    console.log('***', data)
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>';
                    return html;
                }
            },
            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    var html = '<a class="btn btn-primary btn-xs editor_delete" data-key="' + data.ItbID + '"><i class="fa fa-trash"></i></a>';
                    return html;
                }
            }
        ]
    });
    function updateGridItem(model) {
        var rowIdx = table2
            .cell(col)
            .index().row;

        var d = table2.row(rowIdx).data();
        d.EmployeeID = model.EmployeeID;
        d.EmployeeName = model.EmployeeName;
        d.AnnualBasic = model.AnnualBasic;
        d.AnnualHousing = model.AnnualHousing
        d.AnnualTransport = model.AnnualTransport

        table2
            .row(rowIdx)
            .data(d)
            .draw();

    }
    $("#example123").on("click", "a.editor_edit", editDetailServer);
    function editDetailServer() {
        var editLink = $(this).attr('data-key');
        col = $(this).parent();
        $('#ItbID').val(editLink);
        $.ajax({
            url: $('#usersalaryviewdetails').data('request-url') + '/' + editLink,//'ViewDetail/' + editLink,
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                if (response.RespCode === 0) {
                    $('#EmployeeID').val(response.model.EmployeeID);
                    $('#EmployeeName').val(response.model.EmployeeName);
                    $('#AnnualBasic').val(response.model.AnnualBasic);
                    $('#AnnualTransport').val(response.model.AnnualTransport);
                    $('#AnnualMeal').val(response.model.AnnualMeal);
                    $('#AnnualOthers').val(response.model.AnnualOthers);
                    $('#GrossPay').val(response.model.GrossPay);
                    $('#NHFContribution').val(response.model.NHFContribution);
                    $('#AnnualHousing').val(response.model.AnnualHousing);
                    $('#ItbID').val(response.model.ItbID);
                    //$('#divStatus').show();
                    $('#pnlAudit').css('display', 'block');
                    $('a.editor_reset').hide();
                    $('#btnSave').html('<i class="fa fa-edit"></i> Update');
                    $('#btnSave').val(2);
                    $('#myModal').modal({ backdrop: 'static', keyboard: false });
                }
                else {
                    alert(response.RespMessage + 'edit error');
                }
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


    }
    $("#example123").on("click", "a.editor_delete", deleteDetailServer);
    function deleteDetailServer() {
        var editLink = $(this).attr('data-key');
        col = $(this).parent();
        $('#ItbID').val(editLink);
        swal({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: $('#usersalaryviewdeletedetails').data('request-url') + '/' + editLink,//'DeleteDetail/' + editLink,
                    data: null,
                    type: "Get",
                    contentType: "application/json",
                    success: function (response) {
                        if (response.RespCode === 0) {
                            updateGridItem(response.model);
                            $('#myModal').modal('hide');
                            $('#ItbID').val(0);
                            swal(
                                  'Deleted!',
                                  'Your file has been deleted.',
                                  'success'
                                )
                        }
                        else {
                            swal(
                                  'Error!',
                                  response.RespMessage,
                                  'error'
                                )

                        }
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

            }
        })
    }
    var validator = $("#formFamily").validate({
        rules: {
            FullName: "required",
            RelationshipType: "required",
            Age: "required",
        },
        messages: {

            Name: {
                required: "Please Enter FullName"
            },


        },
        submitHandler: function () {
            var btn = $('#btnSave').val();
            var urlTemp;
            var postTemp;
            var event;
            if (btn === "1") {

                urlTemp =$('#usersalarycreate').data('request-url')// 'Create';
                postTemp = 'post';
                $('#ItbID').val(0);
                event = 'new';
            }
            else {

                urlTemp = $('#usersalarycreate').data('request-url')//'Create';
                //postTemp = 'put';
                postTemp = 'post';
                event = 'modify';
            };
            //  alert(urlTemp);
            var $reqLogin = {
                url: urlTemp,

                data: $('#formFamily').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded"
            };

            $.ajax({
                url: urlTemp,
                data: $('#formFamily').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded",
                success: function (response) {
                    if (response.RespCode === 0) {
                        $('#formFamily')[0].reset();

                        if (event === 'new') {
                            addGridItem(response.data);
                            $('#myModal').modal('hide');
                            $('#ItbID').val(0);
                        }
                        else {
                            var btn = $('#btnSave').html('<i class="fa fa-save"></i> Save');
                            updateGridItem(response.data);
                            $('#myModal').modal('hide');
                            $('#ItbID').val(0);

                            swal(
                                  'Success!',
                                  response.RespMessage,
                                  'success'
                                )

                        }

                    }
                    else {
                        swal(
                                   'Error!',
                                   response.RespMessage,
                                   'error'
                                 )

                    }
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

        }
    });
});

$(document).on('click', '#btnUpload', function (e) {

    console.log('uploading');
    $("#example1 tbody").empty();
    e.preventDefault();
    if (window.FormData !== undefined) {
        var fileUpload = $('#upldfile').get(0);
        var files = fileUpload.files;
        if (files.length <= 0) {
            alert("Please select the file you are to upload!");
            return;
        }
        $('#btnValidate').prop('disabled', true);

        var data = new FormData();
        for (var x = 0; x < files.length; x++) {
            data.append(files[x].name + x, files[x]);
        }

        $.ajax({
            type: "POST",
            url: $('#salaryupload').data('request-url'),//'UploadFiles',
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                if (response.RespCode === 0) {
                    $('#upldfile').val('');
                    swal({ title: 'Successfull', text: response.RespMessage, type: 'success' })
                    $('#divGrid').html(response.data_html);
                    $('#btnValidate').removeAttr('disabled');
                }
                else {
                    swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
                }
            },
            error: function (err) {
                console.log(err);
            }
        });

    }
    else {
        alert("This browser doesn't support HTML5 file uploads!");
        //loaderSpin2(false);
    }
});
$(document).on('click', '#btnValidate', function (e) {
    e.preventDefault();

    var enrollId = $('#EnrollmentID').val();
    //alert(enrollId);
    ////loaderSpin2(true);
    $('#hidEnrollId').val(enrollId);

    $.ajax({
        type: "POST",
        url: $('#salaryvalidate').data('request-url'),//'Validate',
        contentType: "application/json",
        data: JSON.stringify({ EnrollID: enrollId }),// JSON.stringify(data),
        success: function (response) {
            //loaderSpin2(false);
            if (response.RespCode === 0) {
                var msg = response.SucCount + ' Records Successfully Validated ';
                msg += response.FailCount + ' Records Failed Validation';
                //displayDialogNoty('Notification', msg);
                if (response.SucCount > 0) {
                    $('#btnSave').removeAttr('disabled');
                }
                else {
                    $('#btnSave').prop('disabled', 'disabled');
                }
                //$("#example1 tbody").empty();
                if (response.data_html) {
                    $('#divGrid').html(response.data_html);
                    swal({ title: 'Successfull', text: msg, type: 'success' })
                    return;
                }
            }
            else {
                swal(
                    'Error!',
                    response.RespMessage,
                    'error'
                     )
            }
        },
        error: function (err) {
            console.log(err);
        }
    });

});

$(document).on('click', '#SampleTemplatePayment', function () {
    $.ajax({

        cache: false,
        type: "POST",
        data: JSON.stringify({ Itbid: 1 }),
        url: $('#downloadtemp').data('request-url'),//'GetPaymentSampleTemplate',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            window.location = 'Salary/PaymentTemplateProcess?Itbid=' + data.Itbid
        },
        error: function () {

        },
        beforeSend: function () {

        },
        complete: function () {
            $('#loading').html('')
        }
    });



    
});





 
