

console.log("here")

$(document).ready(function () {

var emailforpay = '';
var emailforpay2 = '';
var enrollid = $('#eid').val();
var compName = '';
var compPhone = '';
var userName = '';
var firstName = '';
var userPhoneNo = '';
var lastName = '';
var listData = '';

function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
getYear();
    getEEmaill();
    getEEmaill2();
    getUserData();



        const formatter1 = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2 })

    


   
    //ffor direct assessment
    $('#directassessment_partial').css('display','none')

$('#drpAssesmentYear').change(function () {
    if ($(this).val()) {
        $.ajax({
            url: $('#directAssessmentdetails').data('request-url'),//'Assessment/AssessmentDetails',
            data: { yearValue: $(this).val() },
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                console.log('res', response)
                if (response.RespCode == 0) {
                    $('#directassessment_partial').css('display', '')
                    if (response.data) {
                        console.log(response)
                        listData = Object.assign({}, response);
                        console.log(listData, "data returned")
                        $('#lblgrossincome').text(formatter1.format(response.data.GrossIncome));
                        $('#lblincome').text(formatter1.format(response.data.Income));
                        $('#lblEnrollId').text(response.data.enrollmentID);
                        $('#lblfullname').text(response.data.EmployeeName);
                        $('#lbltotalpfa').text(formatter1.format(response.data.TotalFPA));
                        $('#lbltaxableincome').text(formatter1.format(response.data.TaxableIncome));
                        $('#lblconsolidated_allowance').text(formatter1.format(response.data.ConsolidateAllowance));
                        $('#lbldirectnetid').text(formatter1.format(response.data.TaxableLiab));
                        $('#nid').val(formatter1.format(response.data.TaxableLiab));
                        //asign all fields you need
                    }
                } else {
                    $('#directassessment_partial').css('display', 'none')
                    alert('Assessment Record not found')
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


        function getYear() {
          

            $.ajax({
                url: $('#directAssessmentload').data('request-url'),//'Assessment/AssessmentDetails',
                data: null,
                type: "Get",
                contentType: "application/json",
                success: function (response) {
                    console.log('years',response)
                    if(response.RespCode==0)
                    {
                       
                        if(response.data)
                        {
                         
                            var dropdownValues = '<option value="">--select assessment year--</option>';
                            $.each(response.data, function (index, value) {
                                dropdownValues += '<option value="' + value + '">' + value + '</option>'
                            
                            })
                            $('#drpAssesmentYear').append(dropdownValues);
                        }
                    } else {
                        console.log("error")
                    }
                    //console.log('wonma')
                    //emailforpay = response.data;
                    //$('#emailhid').val(emailforpay);
                    //getUserData();

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


    


     function getEEmaill2() {

         $.ajax({
             url: $('#getEmail').data('request-url'),//'Assessment/AssessmentDetails',
             data: null,
             type: "Get",
             data: { EnrollID: enrollid },
             contentType: "application/json",
             success: function (response) {
                 console.log(response)
                 console.log('wonma')
                 emailforpay = response.data;
                 $('#emailhid').val(emailforpay);
                 getUserData();

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


     function getUserData() {
         console.log('ayiiiiiiii', enrollid)
         $.ajax({
             url: $('#getUser').data('request-url'),//'Assessment/AssessmentDetails',
             data: null,
             type: "Get",
             data: { EnrollId: enrollid },
             contentType: "application/json",
             success: function (response) {
                 console.log('user data', response)
                 firstName = response.data.FirstName;
                 userPhoneNo = response.data.MobileNo;
                 lastName = response.data.LastName;
                 Amount = response.data.NETTAX;

                 console.log(lastName, 'lllll');

                 $('#compSurname').val(lastName);

                 $('#compname').val(firstName);
                 $('#comphone').val(userPhoneNo);
                 $('#amount').val(Amount);
                
                 
                 console.log('heyyyyyyyyyyy',$('#comphone').val())
                 console.log('helooooooo', $('#compname').val())
                 console.log('helooooooo', $('#compSurname').val())

                 //console.log($('#compname').val())
                 console.log('amt',$('#amount').val())

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


    //To get netTax before making a modified payment
    function getNetTax() {

            $.ajax({
                url: $('#getNetTax').data('request-url'),//'Assessment/GetNetTax',
                data: null,
                type: "Get",
                data: null,
                contentType: "application/json",
                success: function (response) {
                    console.log(response);                    
                   
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



    $('#verify1').click(function (e) {
        e.preventDefault()
        if (listData.isNew == false) {
            console.log('modify', listData.isNew)
            $('#modifyAssessment').addClass('hideModal');
            $('#newAssesment').addClass('showM');
            $('#incomeDeclared').text(formatter1.format(listData.data.IncomePaid))
            $('#nIncomeDeclared').text(formatter1.format(listData.data.Income))
            $('#amount').text(formatter1.format(listData.data.IncomeDifference))
            $('#taxPayable').text(formatter1.format(listData.data.TaxableLiab))
        } else {

            console.log('new', listData.isNew)
            $('#modifyAssessment').addClass('showM');
            $('#newAssesment').addClass('hideModal');
            $('#amountDeclared').text(formatter1.format(listData.data.Income))
            $('#taxPayablee').text(formatter1.format(listData.data.TaxableLiab))
        }
        //alert('ff')
        $('#myModal').modal({ backdrop: 'static', keyboard: false });
        //console.log('modify', listData.isNew)


        //$.ajax({
        //    url: $('#getNetTax').data('request-url') + '/' + $('#drpAssesmentYear').val(),
        //        type: "Get",
        //        contentType: "application/json",
        //        success: function (response) {
        //            console.log(response);
        //            $('#old_amount').val(150000) //secondary table
        //            ('#new_amount').val(200000) //primary table
        //            //difference == 5000
        //            //show the calculated assessment on 50000
        //            $('#myModal').modal({ backdrop: 'static', keyboard: false });

        //        },
        //        failure: function (xhr, status, err) {
        //            $('.ajax-loader').css("visibility", "hidden");
        //            //$('#display-error').show();
        //        },
        //        beforeSend: function () {
        //            $('.ajax-loader').css("visibility", "visible");
        //        },
        //        complete: function () {
        //            $('.ajax-loader').css("visibility", "hidden");
        //        }
        //    });
        //console.log('hello world')
        //console.log('username')
        //let details = {

        //    firstName: document.getElementById('compname').value,
        //    lastName: document.getElementById('compSurname').value,
        //    phone: document.getElementById('comphone').value,
        //    amount: document.getElementById('nid').value,


        //};
        //console.log(details)
        //localStorage.setItem('details', JSON.stringify(details));
        //window.location = 'EnrollmentType/VerifyPay';

    });


    //$('#verify1').click(function (e) {
    //     e.preventDefault()
    //     if (listData.isNew == false) {
    //         console.log('modify', listData.isNew)
    //         $('#modifyAssessment').addClass('hideModal');
    //         $('#newAssesment').addClass('showM');
    //     } else {

    //         console.log('new', listData.isNew)
    //         $('#modifyAssessment').addClass('showM');
    //         $('#newAssesment').addClass('hideModal');
    //         $('#amountDeclared').text(formatter1.format(listData.data.Income))
    //         $('#taxPayablee').text(formatter1.format(listData.data.TaxableLiab))
    //     }
    ////alert('ff')
    //     $('#myModal').modal({ backdrop: 'static', keyboard: false });
    //     //console.log('modify', listData.isNew)

        
    //    //$.ajax({
    //    //    url: $('#getNetTax').data('request-url') + '/' + $('#drpAssesmentYear').val(),
    //    //        type: "Get",
    //    //        contentType: "application/json",
    //    //        success: function (response) {
    //    //            console.log(response);
    //    //            $('#old_amount').val(150000) //secondary table
    //    //            ('#new_amount').val(200000) //primary table
    //    //            //difference == 5000
    //    //            //show the calculated assessment on 50000
    //    //            $('#myModal').modal({ backdrop: 'static', keyboard: false });
                   
    //    //        },
    //    //        failure: function (xhr, status, err) {
    //    //            $('.ajax-loader').css("visibility", "hidden");
    //    //            //$('#display-error').show();
    //    //        },
    //    //        beforeSend: function () {
    //    //            $('.ajax-loader').css("visibility", "visible");
    //    //        },
    //    //        complete: function () {
    //    //            $('.ajax-loader').css("visibility", "hidden");
    //    //        }
    //    //    });
    //     //console.log('hello world')
    //     //console.log('username')
    //     //let details = {
             
    //     //    firstName: document.getElementById('compname').value,
    //     //    lastName: document.getElementById('compSurname').value,
    //     //    phone: document.getElementById('comphone').value,
    //     //    amount: document.getElementById('nid').value,


    //     //};
    //     //console.log(details)
    //     //localStorage.setItem('details', JSON.stringify(details));
    //     //window.location = 'EnrollmentType/VerifyPay';

    //});


    $('#verify').click(function () {
    console.log('hello world')
    console.log('username')
    let details = {

        firstName: document.getElementById('compname').value,
        lastName: document.getElementById('compSurname').value,
        phone: document.getElementById('comphone').value,
        amount: document.getElementById('nid').value,


    };
    console.log(details)
    localStorage.setItem('details', JSON.stringify(details));
    window.location = 'EnrollmentType/VerifyPay';

    });


    $('#verify2').click(function () {
        console.log('hello world')
        console.log('username')
        let details = {

            firstName: document.getElementById('compname').value,
            lastName: document.getElementById('compSurname').value,
            phone: document.getElementById('comphone').value,
            amount: document.getElementById('nid').value,


        };
        console.log(details)
        localStorage.setItem('details', JSON.stringify(details));
        window.location = 'EnrollmentType/VerifyPay';

    });

    
     
    
  //  function IncomeAssessment(EnrollID) {
        $.ajax({
            url: $('#assessmentload').data('request-url'),//'Assessment/AssessmentDetails',
            data: null,
            type: "Get",
            data: { EnrollID: enrollid },
            contentType: "application/json",
            success: function (response) {
                console.log("im here");
                console.log('***', response)
                var firstoneview = '';
                var seconedoneview = '';

                if (response.RespCode === 0) {
                    var nettaxx = 0;
                    console.log('**data', response.data);
                    $('#do').html(response.data[0].Income)
                    $.each(response.data, function (count, row) {
                        console.log('**index', row)
                        nettaxx += row.NetTax;
                        console.log(nettaxx)
                        // var amttt = formatter1.format(nettaxx);

                        //how to loop the view to be more than one
                       

         

                        seconedoneview +=
                        '<div class="col-lg-12">\
                <div class="panel panel-default">\
                    <div class="panel-heading text-center">\
                        <i class="fa fa-money"></i>Assessment Information\
                    </div>\
                    <div class="panel-body">\
                        <div class="form-horizontal row">\
                            <div class="form-group col-md-6">\
                                <div style="padding-left:15px; display:flex;">\
                                <label>EnrollmentID:</label>\
                                <div class="col-sm-10">\
                                    <span id="EnrollId">'+row.enrollmentID +'</span>\
                                </div>\
                                </div>\
                            </div>\
                            <div>\
                                <div class="form-group col-md-6" style="display:flex; justify-content:flex-end;">\
                                  <label>First Name:</label>\
                                <div class="col-sm-10">\
                                    <span id="EnrollId">' + row.EmployeeName + '</span>\
                                </div>\
                                </div>\
                                </div>\
                        </div>\
                    </div>\
                    <div class="panel panel-default">\
                        <div class="panel-heading" style="text-align:center !important">\
                            Assesment Details\
                        </div>\
                        <div class="panel-body">\
                            <div class="col-lg-6">\
                                <div role="form">\
                                    <div class="form-group">\
                                        <label>Total Income :</label>\
                                        <label id="txtincome">' + row.Income + '</label>\
                                    </div>\
                                    <div class="form-group">\
                                        <label>20% Gross Income: </label>\
                                        <label id="txtgrossincome">' + row.GrossIncome + '</label>\
                                    </div>\
                                </div>\
                            </div>\
                            <!-- /.col-lg-6 (nested) -->\
                            <div class="col-lg-6">\
                                <div role="form">\
                                    <div class="form-group">\
                                        <label>Total FPA: </label>\
                                        <label id="txttotalpfa">' + row.TotalFPA + '</label>\
                                    </div>\
                                    <div class="form-group">\
                                        <label>Consolidated Allowance: </label>\
                                       <label>' + row.ConsolidateAllowance + '</label>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="col-lg-6">\
                                <div class="form-group">\
                                    <label>Taxable Income: </label>\
                                    <label>' + row.TaxableIncome + '</label>\
                                </div>\
                             </div>\
                        </div>\
                    </div>\
                </div>\
            </div>\
                        ';
                    });
              
                    console.log("haha", nettaxx)
                    $('#netid').html(nettaxx);
                    $('#nid').val(nettaxx);
                }

               // console.log('total' + output);
               // $('#firstone').html(firstoneview);
                $('#secondone').html(seconedoneview);
               

                if (response.RespCode === 0) {
                    $('#EnrollId').val(response.data[0].enrollmentID);
                    $('#txtincome').val(response.data.Income);
                    $('#txtgrossincome').val(response.data.GrossIncome);
                    $('#txtconsolidateallowance').val(response.data.ConsolidateAllowance);

                    $('#txttotalpfa').val(response.data.TotalFPA);
                    $('#txttaxableincome').val(response.data.TaxableIncome);
                    $('#txtminimumtax').val(response.data.MinimumTax);

                    $('#taxableliabmonth').val(response.data.TaxableLiabMonth);
                    $('#nettaxx').val(response.data.NetTax);

                }
                else {
                    console.log("error again !!!")
                    //swal(
                    //    'Error!',
                    //    response.RespMessage,
                    //    'error'
                    //)
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





        function getEEmaill() {

            $.ajax({
                url: $('#getEmail').data('request-url'),//'Assessment/AssessmentDetails',
                data: null,
                type: "Get",
                data: { EnrollID: enrollid },
                contentType: "application/json",
                success: function (response) {
                    console.log(response);
                    emailforpay = response.data;
                    $('#emailhid').val(emailforpay);
                    getCompanyData();

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

        function getCompanyData() {
            var emailedd = $('#emailhid').val();
            console.log('ayiiiiiiii', emailedd)
            $.ajax({
                url: $('#getCompany').data('request-url'),//'Assessment/AssessmentDetails',
                data: null,
                type: "Get",
                data: { email: emailedd },
                contentType: "application/json",
                success: function (response) {
                    console.log('company data', response)

                    compName = response.data.CompanyName;
                                      

                    $('#compname').val(compName);
                    firstName: document.getElementById("compname").value,
                                     
                                     
                    compPhone = response.data.CompanyPhonenumber;

                   
                    console.log(response.CompanyName)

                    //console.log($('#compname').val())
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




        $('#verifyy').click(function () {
            console.log("hello")
            
            var details =
                {
                    
                    // amount: document.getElementById("txtnettax").value,
                   
                    firstName: document.getElementById("compname").value,
                    lastName: document.getElementById("compname").value,
                    amount: document.getElementById("nid").value,
                    //customerid: document.getElementById('eid').value,
                    phone: document.getElementById("comphone").value,
                   // emailno: document.getElementById('emailhid').value,
                   
                }
            console.log(details)
            localStorage.setItem('details', JSON.stringify(details));
           /* window.location = 'http://localhost:27035/EnrollmentType/VerifyPay';*/ // 'EnrollmentType / VerifyPay';
            window.location = 'http://62.173.32.98/revenuepoint/EnrollmentType/VerifyPay'; // 'EnrollmentType/VerifyPay';


        });

     



     


        //const verify2 = function() {

        //    var details =
        //        {
        //            // amount: document.getElementById("txtnettax").value,
        //            amount: document.getElementById("Amount").value,
        //            firstName: document.getElementById("FirstName").value,
        //            lastname: document.getElementById("LastName").value,
        //           // customerid: document.getElementById('eid').value,
        //            phone: document.getElementById('comphone').value,
        //          //  emailno: document.getElementById('emailhid').value,
                    
        //        }
        //    console.log(details, "****");
        //    localStorage.setItem('details', JSON.stringify(details));
        //    window.location = 'EnrollmentType/VerifyPay';
        //}

   // }
});



