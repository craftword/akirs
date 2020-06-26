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
                    $('#nettaxx').val(response.data);


                    //$('#nettaxx').val(response.data);

                  //var nettaxx = 0;
                    console.log('**data', response.data);

                    $.each(response.data, function (count, row) {
                        console.log('**index', row)
                        nettaxx += row.NetTax;
                        console.log(nettaxx)
                        // var amttt = formatter1.format(nettaxx);




                        //how to loop the view to be more than one
                        output +=
                            '<div>\
                        <div class="col-lg-6">\
                        <div role="form">\
                        <div class="form-group">\
                            <label>Enrollment ID</label>\
                            <input class="form-control" id="txtEnrollId" value="'+ row.enrollmentID + '" readonly />\
                        </div>\
                         <div class="form-group">\
                            <label>Employee Name</label>\
                            <input class="form-control" id="txtEmployeeName" value="' + row.EmployeeName + '" readonly />\
                        </div>\
                        <div class="form-group">\
                            <label>Gross Income</label>\
                            <input class="form-control" id="txtincome" value="' + row.Income + '" readonly />\
                       </div>\
                       <div class="form-group">\
                            <label>20% Of Gross Income</label>\
                           <input class="form-control" id="txtgrossincome" value="' + row.GrossIncome + '" readonly />\
                        </div>\
                        <div class="form-group">\
                            <label>Consolidated Allowance</label>\
                            <input class="form-control" id="txtconsolidateallowance" value="' + row.ConsolidateAllowance + '" readonly />\
                        </div>\
                        <br>\
                  <hr class="colorgraph">\
                    </div>\
                </div>\
                <!-- /.col-lg-6 (nested) -->\
                <div class="col-lg-6">\
                    <div role="form">\
                        <div class="form-group">\
                            <label>Total Tax Reliefs</label>\
                            <input class="form-control" id="txttotalpfa" value="' + row.TotalFPA + '" readonly />\
                       </div>\
                        <div class="form-group">\
                            <label>Taxable Income</label>\
                            <input class="form-control" id="txttaxableincome"  value="' + row.TaxableIncome + '" readonly />\
                        </div>\
                        <div class="form-group">\
                            <label>Non-Taxable Income</label>\
                            <input class="form-control" id="txtNonTaxableIncome"  value="' + row.NontaxableIncome + '" readonly />\
                        </div>\
                       <div class="form-group">\
                            <label>Minimum Tax</label>\
                            <input class="form-control" id="txtminimumtax"  value="' + row.MinimumTax + '"  readonly />\
                        </div>\
                       <div class="form-group">\
                           <label>Tax Liability Monthly</label>\
                            <input class="form-control" id="taxableliabmonth" value="' + row.TaxableLiabMonth + '" readonly />\
                        </div>\
                        </div>\
                        <br>\
                         <hr class="colorgraph">\
                        </div>';
                    });
                    $('#txtnettax').val(formatter1.format(nettaxx));
                    $('#txtamthid').val(nettaxx);

                }

                console.log('total' + output);
                $('#foritem').html(output);
                //if (response.RespCode === 0) {
                //    $('#txtEnrollId').val(response.data.enrollmentID);
                //    $('#txtincome').val(response.data.Income);
                //    $('#txtgrossincome').val(response.data.GrossIncome);
                //    $('#txtconsolidateallowance').val(response.data.ConsolidateAllowance);

                //    $('#txttotalpfa').val(response.data.TotalFPA);
                //    $('#txttaxableincome').val(response.data.TaxableIncome);
                //    $('#txtminimumtax').val(response.data.MinimumTax);

                //    $('#taxableliabmonth').val(response.data.TaxableLiabMonth);
                //    $('#txtnettax').val(response.data.NetTax);

                //}
                //else {
                //    swal(
                //        'Error!',
                //        response.RespMessage,
                //        'error'
                //    )
                //}
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


    function GetWHTNetTax(EnrollID) {
        $.ajax({
            url: $('#getWhtNatax').data('request-url'),//'Getting witholdingTax Net-Tax',
            type: "Get",
            data: { EnrollID: EnrollID },
            contentType: "application/json",
            success: function (response) {
                alert("2")
                console.log('***fordirct', response)
                console.log('***fordirct', data)
                if (response.RespCode === 0) {
                    alert('Ayiii')
                    //$('#txtEnrollId').val(response.data[0].enrollmentID);
                    //$('#txtincome').val(response.data[0].Income);
                    //$('#txtgrossincome').val(response.data[0].GrossIncome);
                    //$('#txtconsolidateallowance').val(response.data[0].ConsolidateAllowance);
                    //$('#txttotalpfa').val(response.data[0].TotalFPA);
                    //$('#txttaxableincome').val(response.data[0].TaxableIncome);
                    //$('#txtnettax').val(response.data[0].TaxableLiab);


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




    var navListItems = $('div.setup-panel div a'),
        allWells = $('.setup-content'),
        allNextBtn = $('.nextBtn'),
        allPrevBtn = $('.prevBtn');

    allWells.hide();

    navListItems.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this);

        if (!$item.hasClass('disabled')) {
            navListItems.removeClass('btn-primary').addClass('btn-default');
            $item.addClass('btn-primary');
            allWells.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    allPrevBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            prevStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().prev().children("a");

        prevStepWizard.removeAttr('disabled').trigger('click');
    });

    allNextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url']"),
            isValid = true;
        clickedContinue = true;
        if (curStepBtn === 'step-2') {
            swal({
                title: 'Are you sure you want to save this record?',
                text: "Save and continue!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes!'
            }, function (isConfirm) {
                if (isConfirm) {

                    var Witholding = $('#example1_wht').tableToJSON();
                    console.log('*** Witholding', Witholding)
                    //console.log('**Salary Json', salarytable);
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

                } else {
                    clickedContinue = false;
                }
            });
        }


        $(".form-group").removeClass("has-error");
        console.log('** curInputs', curInputs.length)
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid)
            nextStepWizard.removeAttr('disabled').trigger('click');
    });

    $('div.setup-panel div a.btn-primary').trigger('click');
});
