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
        url: $('#loadbranchurl').data('request-url'),
        data: null,
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            var exist = false;

            if (response.data.length > 0) {
                $("#Description").empty();
                $("#Description").append("<option value=''>--Select description--</option>");
                for (var i = 0; i < response.data.length; i++) {
                    $("#Description").append("<option value='" + response.data[i].Description + "'>" +
                         response.data[i].Description + "</option>");

                }
            }
        },
        failure: function (xhr, status, err) {
            return null;
        }
    });

 $.ajax({
            url: $('#loadbranchurl').data('request-url'),
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                var exist = false;

                if (response.data.length > 0) {
                    $("#drpWthMonth").empty();
                    $("#drpWthMonth").append("<option value=''>--Select Month--</option>");
                    for (var i = 0; i < response.data.length; i++) {
                        $("#drpWthMonth").append("<option value='" + response.data[i].Description + "'>" +
                                response.data[i].drpWthMonth + "</option>");

                    }
                }
            },
            failure: function (xhr, status, err) {
                return null;
            }
});

 $('a.editor_create').click(function () {
     $('#formWht')[0].reset();
     validator.resetForm();
     $('#divStatus').hide();
     $('#pnlAudit').css('display', 'none');
     $('#ItbID').val(0);
     $('#btnSave').val(1);
     $('#btnSave').html('<i class="fa fa-save"></i> Save');
     
     //$('a.editor_reset').show();
     $('#myModal').modal({ backdrop: 'static', keyboard: false });
 });
    $(document).on('click', '#btnValidateWht', function (e) {
        e.preventDefault();

        //return;
        $.ajax({
            type: "POST",
            url: $('#whtverifypost').data('request-url'), //url + 'VerifyRecord',
            contentType: "application/json",
            data: null,
            success: function (response) {
                if (response.RespCode == 0) {

                    $('#divGrid').html(response.data_html);
                    $('#btnValidateWht').attr('disabled', 'disabled');
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
        "ajax": $('#whtlist').data('request-url'),//"WhtList",
        "type": "Get",
        "datatype": "json",
        columns: [
            { data: "VendorTINNO" },
            { data: "VendorName" },
            { data: "TaxAmount" },
            { data: "Description" },
            { data: "TaxRate" },
            { data: "WHTAmount" },
            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a> <a class="btn btn-danger btn-xs editor_delete" data-key="' + data.ItbID + '"><i class="fa fa-trash"></i></a>';
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
        d.VendorTINNO = model.VendorTINNO;
        d.VendorName = model.VendorName;
        d.TaxRate = model.TaxRate;
        d.TaxAmount = model.TaxAmount
        d.Description = model.Description
        d.WHTAmount = model.WHTAmount

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
            url: $('#whtdetails').data('request-url') + '/' + editLink,//'ViewDetail/' + editLink,
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                if (response.RespCode === 0) {
                    $('#VendorTINNO').val(response.model.VendorTINNO);
                    $('#VendorName').val(response.model.VendorName);
                    $('#TaxAmount').val(response.model.TaxAmount);
                    $('#TaxRate').val(response.model.TaxRate);
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
                    url: $('#whtdeletedetails').data('request-url') + '/' + editLink,// 'DeleteDetail/' + editLink,
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
            if (btn == "1") {

                urlTemp = $('#whtcreate').data('request-url')// 'Create';
                postTemp = 'post';
                $('#ItbID').val(0);
                event = 'new';
            }
            else {

                urlTemp = $('#whtcreate').data('request-url')//'Create';
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

                        if (event == 'new') {
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
                        //displayModalNoty('alert-warning', response.RespMessage, true);

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

$(document).on('click', '#SampleTemplatePayment', function () {
    $.ajax({
        cache: false,
        type: "POST",
        data: JSON.stringify({ Itbid: 1 }),
        url:$('#downloadtemp').data('request-url'), //'GetPaymentSampleTemplate',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            window.location = url + 'PaymentTemplateProcess?Itbid=' + data.Itbid
        },
        error: function () {

        },
        beforeSend: function () {

        },
        complete: function () {
            //$('#loading').html('')
        }
    });
})
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
        $('#btnValidate, #btnSave').prop('disabled', true);

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
                if (response.RespCode == 0) {
                    swal(
                           'Successful!',
                           response.RespMessage,
                           'success'
                               )
                    $('#upldfile').val('');
                    $('#divGrid').html(response.data_html);
                    $('#btnValidate').removeAttr('disabled');
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
        swal('Error!',"This browser doesn't support HTML5 file uploads!",'error');
    }
});

$(document).on('click', '#btnValidate', function (e) {
    e.preventDefault();

    var enrollId = $('#EnrollmentID').val();
    $('#hidEnrollId').val(enrollId);

    $.ajax({
        type: "POST",
        url: $('#whtvalidate').data('request-url'),//'wht/Validate',
        contentType: "application/json",
        data: JSON.stringify({ EnrollID: enrollId }),// JSON.stringify(data),
        success: function (response) {
            if (response.RespCode == 0) {
                var msg = response.SucCount + ' Records Successfully Validated ';
                msg += response.FailCount + ' Records Failed Validation';
                if (response.SucCount > 0) {
                    $('#btnSave').removeAttr('disabled');

                }
                else {
                    $('#btnSave').prop('disabled', 'disabled');
                }
                if (response.data_html) {
                    $('#divGrid').html(response.data_html);
                    swal({ title: 'Successfull', text: msg, type: 'success' })
                    return;
                }

            }
            else {
                swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
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

});


