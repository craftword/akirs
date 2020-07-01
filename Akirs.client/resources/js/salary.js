
const formatter1 = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 })

let payment = {};

var nettaxx = 0;


$(document).ready(function () {
    //$("#salaryuploadtable").DataTable().fnDestroy();
    var table2 = $('#salaryuploadtable').DataTable({
        "processing": true,
        "serverSide": true,
        "bDestroy": true,
        "ajax": {
            "url": $('#salarydefaultlist').data('request-url'),
            "dataSrc": function (json) {
                if (json.RespCode == 0) {
                    $('#btnUpload').attr('disabled', 'disabled');
                    $('#upldfile').attr('disabled', 'disabled');
                    $('#drpSalaryMonth').attr('disabled', 'disabled');
                    $('#divGrid').css('display', '');
                    $('#btnModifyRecord').css('display', '');
                   // $('#btnSkip').removeAttr('disabled');
                } else {
                    $('#drpSalaryMonth').removeAttr('disabled');
                    $('#upldfile').removeAttr('disabled');
                    $('#btnUpload').removeAttr('disabled');
                    $('#divGrid').css('display', 'none');
                    $('#btnModifyRecord').css('display', 'none');
                }
                return json.data;
            }
        },
        "type": "GET",
        "datatype": "json",
        columns: [
            { data: "EmployeeName" },
            { data: "AnnualBasic" },
            { data: "AnnualHousing" },
            { data: "AnnualTransport" },
            { data: "AnnualMeal" },
            { data: "AnnualOthers" },
            { data: "NHFContribution" },

            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>&emsp;&emsp; <a class ="btn btn-danger btn-xs editor_delete" data-key="' + data.ItbID  +'"><i class ="fa fa-trash"></i></a>'
                        
                    return html;
                }
            }
        ]
    });
    function Translatemonthsindex(monthIndexValue) {
        var monthsValue = '';
        switch(monthIndexValue){
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
        url: $('#salarymonths').data('request-url'),
        data: null,
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            var selectvalues = '<option value="0">--Select month--</option>';

            $.each(response.data, function (count, row) {
                //console.log('roww', row)
                selectvalues += '<option value="' + row + '">' + Translatemonthsindex(row) + '</option>'
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


        $.ajax({
            url: $('#assessmentload').data('request-url'),//'Assessment/AssessmentDetails',
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
          
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
    

 
    
    function updateGridItem(model) {
        var rowIdx = table2
            .cell(col)
            .index().row;

        var d = table2.row(rowIdx).data();
        d.EmployeeName = model.EmployeeName;
        d.AnnualBasic = model.AnnualBasic;
        d.AnnualHousing = model.AnnualHousing
        d.AnnualTransport = model.AnnualTransport
        d.AnnualMeal = model.AnnualMeal
        d.AnnualOthers = model.AnnualOthers
        d.NHFContribution = model.NHFContribution
        table2
            .row(rowIdx)
            .data(d)
            .draw();

    }
    
    $("#salaryuploadtable").on("click", "a.editor_edit", editDetailServer);
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
                    $('#EmployeeName').val(response.model.EmployeeName);
                    $('#AnnualBasic').val(response.model.AnnualBasic);
                    $('#AnnualTransport').val(response.model.AnnualTransport);
                    $('#AnnualMeal').val(response.model.AnnualMeal);
                    $('#AnnualOthers').val(response.model.AnnualOthers);
                    $('#GrossPay').val(response.model.GrossPay);
                    $('#NHFContribution').val(response.model.NHFContribution);
                    $('#AnnualHousing').val(response.model.AnnualHousing);
                    $('#ItbID').val(response.model.ItbID);
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
    $('#btnModifyRecord').click(function (e) {
        e.preventDefault();
        if (confirm('Are you sure you want to complete this modification?')) {
            $.ajax({
                url: $('#completemodification').data('request-url'),
                type: "Post",
                contentType: "application/json",
                success: function (response) {
                    if (response.RespCode == 0) {

                        swal({ title: 'Successfull', text: response.RespMessage, type: 'success' });
                        setTimeout(function () { window.location.reload(true) }, 4000)
                    } else {
                        swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
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
    $("#salaryuploadtable").on("click", "a.editor_delete", deleteDetailServer);
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
    var validator = $("#formsalary").validate({
        rules: {
            EmployeeName :{
                required:true
            },
            AnnualBasic :{
                required:true
            },
            AnnualHousing :{
                required:true
            },
            AnnualTransport :{
                required:true
            },
            AnnualMeal :{
                required:true
            },
            AnnualOthers :{
                required:true
            },
            NHFContribution :{
                required:true
            }
        },
        messages: {

            EmployeeName: {
                required: "Required"
            },
            AnnualBasic: {
                required: "Required"
            },
            AnnualHousing: {
                required: "Required"
            },
            AnnualTransport: {
                required: "Required"
            },
            AnnualMeal: {
                required: "Required"
            },
            AnnualOthers: {
                required: "Required"
            },
            NHFContribution: {
                required: "Required"
            }


        },
        submitHandler: function () {
            var btn = $('#btnEdit').val();
            var urlTemp;
            var postTemp;
            var event;
            urlTemp = $('#usersalarycreate').data('request-url')
            postTemp = 'post';
            event = 'modify';
            //  alert(urlTemp);
            var $reqLogin = {
                url: urlTemp,

                data: $('#formsalary').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded"
            };

            $.ajax({
                url: urlTemp,
                data: $('#formsalary').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded",
                success: function (response) {
                    if (response.RespCode === 0) {
                        $('#formsalary')[0].reset();
                        updateGridItem(response.data);
                        $('#myModal').modal('hide');
                        $('#ItbID').val(0);

                        swal(
                              'Success!',
                              response.RespMessage,
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
    });
});


$(document).on('click', '#btnUpload', function (e) {

    $("#example1 tbody").empty();
    e.preventDefault();
    if (window.FormData !== undefined) {
        var fileUpload = $('#upldfile').get(0);
        var files = fileUpload.files;
        if (files.length <= 0) {
            alert("Please select the file you are to upload!");
            return;
    }
    if ($('#drpSalaryMonth').val() ==='0') {
        alert("Please select a month!");
    return;
}
        $('#btnValidate').prop('disabled', true);

        var data = new FormData();
        for (var x = 0; x < files.length; x++) {
            data.append(files[x].name + x, files[x]);
            data.append("Month", $('#drpSalaryMonth').val())
        }

        //$("#salaryuploadtable2").DataTable().fnDestroy();
        //$('#salaryuploadtable2').DataTable({
        //    "processing": true,
        //    "serverSide": true,
        //    "destroy": true,
        //    "ajax": {
        //        "url": $('#salaryupload').data('request-url'),
        //        "data": data,
        //        "processData": false,
        //        "dataSrc": function (json) {
        //            if (json.RespCode == 0) {
        //                $('#upldfile').val('');
        //                $('#divGrid2').css('display', '');
        //                //$('#divGrid').css('display', 'none');
        //               swal({ title: 'Successfull', text: response.RespMessage, type: 'success' })
        //            } else {
        //                swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
        //                //$('#divGrid2').css('display', 'none');
        //            }
        //            return json.data;
        //        }
        //    },//"SalList",
        //    "type": "POST",
        //    "datatype": "json",
        //    columns: [
        //        { data: "EmployeeID" },
        //        { data: "EmployeeName" },
        //        { data: "AnnualBasic" },
        //        { data: "AnnualHousing" },
        //        { data: "AnnualTransport" },
        //        { data: "AnnualMeal" },
        //        { data: "AnnualOthers" },
        //        { data: "NHFContribution" },

        //        //{
        //        //    data: null,
        //        //    className: "center_column",
        //        //    render: function (data, type, row) {
        //        //        var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>';
        //        //        return html;
        //        //    }
        //        //}
        //    ]
        //});
      //  console.log('**salary', data.get("Month"))
        //return
        $.ajax({
            type: "POST",
            url: $('#salaryupload').data('request-url'),//'UploadFiles',
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                console.log('**', response)
                if (response.RespCode === 0) {
                    $('#upldfile').val('');
                    $('#drpSalaryMonth').val('0')

                    swal({ title: 'Successfull', text: response.RespMessage, type: 'success' })
                    
                    $('#divGrid2').html(response.data_html);
                    $('#btnSend_aroval').removeAttr('disabled');
                    setTimeout(function () { window.location.reload(true) }, 3000)
                }
                else {
                    swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
                }
            },
            error: function (err) {
                console.log(err);
            },
            beforeSend: function () {
                $('#divGrid2').css('display', 'none');
            },
            complete: function () {
                //$('.ajax-loader').css("visibility", "hidden");
                $('#divGrid2').css('display', '');
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





 
