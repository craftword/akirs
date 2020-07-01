$(document).ready(function () {
    function addDiv() {
        $('#assessment_container').append($('.payee_assessment'));
    }
    $.ajax({
        url: $('#assessmentload').data('request-url'),//'Assessment/AssessmentDetails',
        data: null,
        type: "Get",
        contentType: "application/json",
        success: function (response) {
            //console.log("im here");
            //console.log('***', response)
            //var firstoneview = '';
            //var seconedoneview = '';
            var container = $('#assessment_container');
            if (response.RespCode === 0) {
                container.append(response.html_div)
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
    $("#makepayment").click(function (e) {
      //  alert($('#netid').text())
        if (confirm('Are you sure you want to make this payment?')) {
            $.ajax({
                url: $('#makePaymenturl').data('request-url'),//'Assessment/AssessmentDetails',
                data: JSON.stringify({ netTax: $('#netid').text() }),
                type: "Post",
                contentType: "application/json",
                success: function (response) {
                    //var container = $('#assessment_container');
                    if (response.RespCode === 0) {
                        swal({ title: 'Successfull', text: response.RespMessage, type: 'success' });
                        $('#assessment_container').html('');
                        $("#makepayment").attr('disabled', 'disabled')
                    }
                },
                failure: function (xhr, status, err) {
                    $('.ajax-loader').css("visibility", "hidden");
                    //$('#display-error').show();
                },
                beforeSend: function () {
                    $("#makepayment").attr('disabled', 'disabled')
                    $('.ajax-loader').css("visibility", "visible");
                },
                complete: function () {
                    //$("#makepayment").removeAttr('disabled')
                    //$('.ajax-loader').css("visibility", "hidden");
                }
            })
        }
    });
})