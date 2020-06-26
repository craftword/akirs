function passwordForgot()
{
    $.ajax({
        url: $('#forgot_url').data('request-url'), 
        type: "Post",
        data: JSON.stringify({ UserName: $('#email').val() }),
        contentType: "application/json",
        success: function (response) {
            console.log('result*****', response);
            if(response.RespCode == 0)
            {
                alert("kindly check your mail to reset password")
            }
            else
            {
                alert("email doesnt exist")
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