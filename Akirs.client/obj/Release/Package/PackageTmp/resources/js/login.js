function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
$(document).ready(function () {

    $('#btnSave').click(function (e) {
        e.preventDefault();
        var btn = $('#btnSave').val();
        var urlTemp;
        var postTemp;

        console.log('** from login')
        urlTemp = $('#login_url').data('request-url');
        postTemp = 'post'; 
        var $reqLogin = {
            url: urlTemp,
            data: $('#formLogin').serialize(),
            type: postTemp,
            contentType: "application/x-www-form-urlencoded"
        };

        $btnSave = $('#btnSave');
        $loader = $('.loader');
        $btnSave.hide();
        $loader.show();
        var request = $.ajax({
            url: urlTemp,
            data: $('#formLogin').serialize(),
            type: postTemp,
            contentType: "application/x-www-form-urlencoded"
        });

        //console.log(request);
        request.done(function (response) {
           // console.log('***erorororo')
            $btnSave.show();
            $loader.hide();
            if (response.RespCode === 0) {
                console.log(response.RespCode);
               // alert($('#home_url').data('request-url'))
                window.location.href = $('#home_url').data('request-url') // BaseUrl + 'UserHome/Index';

            }
            else if (response.RespCode === 7) {
                $("#Itbid").val(response.Itbid);
                $('#myModal').modal({ backdrop: 'static', keyboard: false });
            }
            else {
                $('#Password').val('')
                 swal('Error',response.RespMessage,'error')
                //console.log(response.RespCode);
            }


        }).fail(function (xhr, status, err) {
            $btnSave.show();
            $loader.hide();
        });
    })


    var validator = $("#formFamilyChangepassword").validate({
        rules: {
            ConfirmPassword: {
                required: true,
                equalTo: "#ChangePassword"
            },
            ChangePassword: {
                required: true,
            }
        },
        messages: {
            ConfirmPassword: {
                required: "Please confirm your password"
            },
            ChangePassword: {
                required: "Please enter password"
            }


        },
        submitHandler: function () {
            var btn = $('#btnchange').val();
            var urlTemp;
            var postTemp;
            var event;
          
            $.ajax({
                url: $('#changpwd').data('request-url'),
                data: $('#formFamilyChangepassword').serialize(),
                type: 'POST',
                contentType: "application/x-www-form-urlencoded",
                success: function (response) {

                    if (response.RespCode === 0) {
                        $('#myModal').modal('hide');
                        swal({
                            title: 'Password change successful?',
                            text: "Continue To LogIn!",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Yes!'
                        }, function (isConfirm) {
                            if (isConfirm) {
                                var request = $.ajax({
                                    url: $('#login_url').data('request-url'),
                                    data: JSON.stringify({ UserName: $('[name=UserName]').val(), Password: $('#ChangePassword').val() }),
                                    type: 'POST',
                                    contentType: "application/json"
                                });

                                request.done(function (response) {
                                    console.log('**** login', response)
                                    if (response.RespCode === 0)
                                    {
                                        //alert("ii")
                                        location.reload(true);
                                        //window.location = 'Home/Login';
                                        //window.location.href = BaseUrl + 'Home/Login';btnchange
                                        return;

                                    }
                                    $('#formFamilyChangepassword')[0].reset();
                                    $('#formLogin')[0].reset();

                                    $('#ItbID').val(0);

                                }).fail(function (xhr, status, err) {

                                });
                            } else {
                                $('#formFamilyChangepassword')[0].reset();
                                $('#formLogin')[0].reset();
                            }
                        });



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
