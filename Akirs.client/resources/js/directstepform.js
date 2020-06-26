function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}

var familytable;
$(document).ready(function () {

    $('#btnPayment').click(function () {
        
        $.ajax({
            url: $('#paymenturl').data('request-url'), 
             type: "Post",
            data: JSON.stringify( { Taxtype: $('#TaxtypeID').val() }),
            contentType: "application/json",
            success: function (response) {
                console.log('result*****', response);
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
    })


    function IncomeAssessment(EnrollID) {
        $.ajax({
            url: $('#assessmentload').data('request-url'),//'Assessment/AssessmentDetails',
            
            type: "Get",
            data:{EnrollID:EnrollID},
            contentType: "application/json",
            success: function (response) {
              //  alert("2")
                console.log('***fordirct', response)
                if (response.RespCode === 0) {
                    $('#txtEnrollId').val(response.data[0].enrollmentID);
                    $('#txtincome').val(response.data[0].Income);
                    $('#txtgrossincome').val(response.data[0].GrossIncome);
                    $('#txtconsolidateallowance').val(response.data[0].ConsolidateAllowance);
                    $('#txttotalpfa').val(response.data[0].TotalFPA);
                    $('#txttaxableincome').val(response.data[0].TaxableIncome);
                    $('#txtnettax').val(response.data[0].TaxableLiab);


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
    $(".add-family-row").click(function () {
        var FullName = $("#FullName").val();
        //var RelationshipType = $("#RelationshipType").val();
        var RelationshipType = $("#RelationshipType option:selected").text();
        var relatype = $("#RelationshipType").val();
        var Age = $("#Age").val();

        if (FullName.length === 0 || RelationshipType.length === 0 || Age.length === 0) {
            swal('Error!!', 'Fields cannot be empty', 'error');
            return;
        }
        var markup = "<tr><td>" + FullName + "</td><td>" + RelationshipType + "</td><td>" + Age + "</td><td><a class='btn btn-danger btn-xs delete-row'><i class='fa fa-thrash'></i>Delete</a></td></tr>";
        var markup1 = "<tr><td>" + FullName + "</td><td>" + relatype + "</td><td>" + Age + "</td><td><a class='btn btn-danger btn-xs delete-row'><i class='fa fa-thrash'></i>Delete</a></td></tr>";
        $("#example1_family_enroll tbody").append(markup);
        $("#example1_family_enroll2 tbody").append(markup1);
        familytable = $('#example1_family_enroll2').tableToJSON();
        console.log(familytable);
        //var table = $('#example1_family_enroll').tableToJSON();
        //alert(JSON.stringify(table));

        $("#FullName").val('');
        $("#RelationshipType").val('');
        $("#Age").val('');
    });

    // Find and remove selected table rows
    $("#example1_family_enroll").on("click", "a.delete-row", function () {
        $(this).parents("tr").remove();
    });


    const formatter1 = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 })


    $(".add-incomesource-row").click(function () {
        var SourceOfIncomeID = $("#SourceOfIncomeID option:selected").text();
        var Amount = formatter1.format($("#Amount").val());
       // var Year = formatter1.format($("#Year").val());
        var amt = $("#Amount").val();
        var srcc = $("#SourceOfIncomeID").val();
        var IncomeYear = $("#IncomeYear").val();
        //var amt = $("#Amount").val();

        if (SourceOfIncomeID.length === 0 || Amount.length === 0 || IncomeYear.length === 0) {
            swal('Error!!', 'Fields cannot be empty', 'error');
            return;
        }
        var markup = "<tr><td>" + SourceOfIncomeID + "</td><td>" + "&#8358; " + Amount + "</td><td>" + IncomeYear + "</td><td><a class='btn btn-danger btn-xs delete-row'><i class='fa fa-thrash'></i>Delete</a></td></tr>";
        var markup3 = "<tr><td>" + srcc + "</td><td>" + amt + "</td><td>" + IncomeYear + "</td><td><a class='btn btn-danger btn-xs delete-row'><i class='fa fa-thrash'></i>Delete</a></td></tr>";
        $("#example1_incomesource tbody").append(markup);
        $("#example1_incomesourcehide tbody").append(markup3);
       // example1_incomesourcehide
        console.log("Income table - "+markup)
        $("#SourceOfIncomeID").val('');
        $("#Amount").val('');
        $("#IncomeYear").val('');
    });

    // Find and remove selected table rows
    $("#example1_incomesource").on("click", "a.delete-row", function () {
        $(this).parents("tr").remove();
    });

   
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
            if (curStepBtn === 'step-3') {
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
                        
                        var Incometable = $('#example1_incomesourcehide').tableToJSON();
                       
                        console.log('**Income Json', familytable);
                        var datadetails = {
                            EnrollmentViewModel: {
                                FirstName: $('#FirstName').val(),
                                LastName: $('#LastName').val(),
                                Phonenumber: $('#Phonenumber').val(),
                                Gender: $('#Gender').val(),
                                Address: $('#Address').val(),
                                TaxtypeID: $('#TaxtypeID').val(),
                                MaritalStatus: $('#MaritalStatus').val(),
                                Numberofchildren: $('#Numberofchildren').val(),
                                StateCode: $('#StateCode').val(),
                                CityCode: $('#CityCode').val(),
                                Email: $('#Email').val(),
                               // IncomeYear : $('#IncomeYear').val()

                            },
                            FamilyModel: familytable,
                            IncomeSource: Incometable
                        }
                        console.log('***Josn', datadetails);
                        
                        $.ajax({
                            url: $('#postdirectAssessment').data('request-url'),//'IncomeDeclaration/IncomeTypeList',
                            data: JSON.stringify(datadetails),
                            type: "Post",
                            contentType: "application/json",
                            success: function (response) {
                                console.log('***response', response);
                                if (response.RespCode == 0) {
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
