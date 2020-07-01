$(document).ready(function () {

    var eID = $('#enrId').val();
    console.log(eID);









});

$(document).ready(function () {

    var col;

    var url = '';
    BindCombo();
    //console.log(url, $("#EnrollmentID").val());
    var table2 = $('#example1').DataTable({
        ajax: $('#userviewlist').data('request-url'),//url + "UserList",
        columns: [
            { data: "LastName" },
            { data: "FirstName" },
            { data: "Email" },
            { data: "RoleName" },
            { data: "Status" },
            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbId + '"><i class="fa fa-edit"></i></a>';
                    return html;
                }
            }
        ]
    });

    var validator = $("#formUserProfile").validate({
        rules: {
            LastName: "required",
            FirstName: "required",
            Email: "required"
        },
        messages: {
            Name: {
                required: "Please Enter Email"
            },


        },
        submitHandler: function () {
            var btn = $('#btnSave').val();
            var urlTemp;
            var postTemp;
            var event;
            if (btn == "1") {

                urlTemp = $('#userviewcreate').data('request-url'), //url + 'Create';
                postTemp = 'post';
                $('#ItbID').val(0);
                event = 'new';
            }
            else {
                urlTemp = $('#userviewcreate').data('request-url'), //url + 'Create';
                postTemp = 'post';
                event = 'modify';
            };
            var $reqLogin = {
                url: urlTemp,

                data: $('#formUserProfile').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded"
            };
            console.log('**', $('#formUserProfile').serialize())
            //var ajax = new Ajax();
            $.ajax({
                url: urlTemp,
                data: $('#formUserProfile').serialize(),
                type: postTemp,
                contentType: "application/x-www-form-urlencoded",
                success: function (response) {
                    if (response.RespCode === 0) {
                        new Util().clearform('#formUserProfile');
                        console.log('orente')
                        if (event == 'new') {
                            addGridItem(response.data);
                            $('#myModal').modal('hide');
                            $('#ItbID').val(0);
                            //displayDialogNoty('Notification', 'Record Created Successfully');
                            swal({ title: 'Successfull', text: 'Record Created Successfully', type: 'success' })
                        }
                        else {
                            var btn = $('#btnSave').html('<i class="fa fa-save"></i> Save');
                            updateGridItem(response.data);
                            $('#myModal').modal('hide');
                            $('#ItbID').val(0);
                            //displayDialogNoty('Notification', 'Record Updated Successfully');
                            swal({ title: 'Notification', text: 'Record Updated Successfully', type: 'success' })

                        }

                    }
                    else {
                       // displayModalNoty('alert-warning', response.RespMessage, true);

                    }
                }, failure: function (xhr, status, err) {
                    return null;
                }
            })


        }
    });





    $("#example1").on("click", "a.editor_edit", editDetailServer);

    //$("#example1").on("click", ".current", setAsCurrent);

    function editDetailServer() {
        var editLink = $(this).attr('data-key');
        col = $(this).parent();
        $('#ItbID').val(editLink);

      
        $.ajax({
            url: $('#userviewdetails').data('request-url') + '/' + editLink,// url + 'ViewDetail/' + editLink,
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                console.log(response)
                if (response.RespCode === 0) {
                    $('#RoleId').attr('disabled', 'disabled');
                    $('#FirstName').attr('disabled', 'disabled');
                    $('#LastName').attr('disabled', 'disabled');
                    $('#Email').attr('disabled', 'disabled');
                    $('#btnSave').hide();

                    $('#RoleId').val(response.model.RoleId);
                    $('#FirstName').val(response.model.FirstName);
                    $('#LastName').val(response.model.LastName);
                    $('#Email').val(response.model.Email);
                    $('#ItbID').val(response.model.ItbId);
                    $('#divStatus').show();
                    $('#CreatedBy').text(response.model.CreatedBy);
                    $('#CreateDate').text(response.model.CreateDate);
                    $('#Status').val(response.model.Status);
                    $('#pnlAudit').css('display', 'block');
                    $('a.editor_reset').hide();
                    $('#btnSave').hide();
                    $('#btnSave').html('<i class="fa fa-edit"></i> Update');
                    $('#btnSave').val(2);
                    $('#myModal').modal({ backdrop: 'static', keyboard: false });
                }
                else {
                    swal(
                              'Error!',
                              response.RespMessage,
                              'error'
                              )
                    //alert(response.RespMessage + 'edit error');
                }
            }, failure: function (xhr, status, err) {
                return null;
            }
        });

       

    }

    function addGridItem(model) {
        table2.row.add(model).draw();
    }
    function updateGridItem(model) {
        var rowIdx = table2
            .cell(col)
            .index().row;

        var d = table2.row(rowIdx).data();
        d.SourceOfIncomeID = model.SourceOfIncomeID;
        d.Amount = model.Amount;
        d.Status = model.Status

        table2
            .row(rowIdx)
            .data(d)
            .draw();

    }

    $('a.editor_create').click(function () {

        validator.resetForm();
        $('#divStatus').hide();
        $('#pnlAudit').css('display', 'none');
        $('#ItbID').val(0);
        $('#btnSave').val(1);
        $('#btnSave').html('<i class="fa fa-save"></i> Save');
        $('a.editor_reset').show();
        $('#myModal').modal({ backdrop: 'static', keyboard: false });
    });
    $('a.editor_reset').click(function () {

        validator.resetForm();
        $('#pnlAudit').css('display', 'none');
        $('#ItbID').val(0);

    });
    function BindCombo() {
        
        try {
            $.ajax({
                type: "Get",
                url: $('#roleList').data('request-url'),//'RolesList',
                contentType: "application/json",
                data: null,// JSON.stringify(data),
                success: function (response) {
                    var exist = false;
                    if (response.data.length > 0) {
                        $("#RoleId").empty();
                        $("#RoleId").append("<option value=''> --Select Role-- </option>");
                        for (var i = 0; i < response.data.length; i++) {
                            $("#RoleId").append("<option value='" + response.data[i].Roleid + "'>" +
                                 response.data[i].RoleName + "</option>");

                        }
                    }
                }, failure: function (xhr, status, err) {
                    return null;
                }
            });

           
        }
        catch (err) {

        }


    }

});