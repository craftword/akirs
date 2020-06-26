function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
$(document).ready(function () {
    $('.btnRegister').click(function (event) {
        event.preventDefault();
        var id = $(this).attr('data-val')
        $.ajax({
            url: $('#loader').data('request-url'),//'IncomeDeclaration/ViewDetail/' + editLink,
            data: { rt: id },
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                if (response.RespCode == 0) {
                    if (response.data_html) {
                        $('.modal-body').html('')
                        $('.modal-body').html(response.data_html);
                        $('#myModal').modal({ backdrop: 'static', keyboard: false });
                        return;
                    }
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

    });

    var validator = $("#formFamily").validate({
        rules: {
            LastName: {
                required: true,
            },
            FirstName: {
                required: true,
            },
            Address: {
                required: true,
            },
            Phonenumber: {
                required: true,
            },
            Email: {
                required: true,
            },
            Gender: {
                required: true,
            },
            StateCode: {
                required: true,
            },
        },
        messages: {
            LastName: {
                required: "Please Enter LastName"
            },
            FirstName: {
                required: "Please Enter FirstName"
            },
            Address: {
                required: "Please Enter Address"
            },
            Phonenumber: {
                required: "Please enter Phone number"
            },
            Email: {
                required: "Please Enter Email"
            },
            StateCode: {
                required: "Please select State"
            },
            Gender: {
                required: "Please select gender"
            }


        },
        submitHandler: function () {
            var btn = $('#btnSave').val();
            var urlTemp;
            var postTemp;
            var event;
           
           
            $.ajax({
                url: $('#addloader').data('request-url'),
                data: $('#formFamily').serialize(),
                type: 'Post',
                contentType: "application/x-www-form-urlencoded",
                success: function (response) {
                    if (response.RespCode === 0) {
                        $('#formFamily')[0].reset();

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
})