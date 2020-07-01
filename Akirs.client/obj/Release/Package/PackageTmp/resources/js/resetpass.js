const savePassword = () => {
    if ($('#newpass').val() !== $('#confirmpass').val())
    {
        alert("Passwords do not match")
    }
    else
    {
        $.ajax({
            url: $('#reset_url').data('request-url'),
            type: "Post",
            data: JSON.stringify({ email: $('#resetemail').val(), password: $('#newpass').val() }),
            contentType: "application/json",
            success: function (response) {
                console.log('result*****', response);
                if (response.RespCode == 0)
                {
                    alert("Password Successfully Changed")
                }

                else {
                    alert("An Error occured, pls try again.")
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
}