function BaseUrl() {
    // alert( $('base').attr('href'));
    return $('base').attr('href');
    // return '/CliqBEChannel';
}
$(document).ready(function () {
    var col;
    BindCombo();
    var url = $('#loader').data('request-url');
    console.log('**Family', url)
    var table2 = $('.familytable').DataTable({
        "processing": true,
        "serverSide": true,
        ajax: url,//"Family/FamilyList",
        "type": "Get",
        "datatype": "json",
        columns: [
            { data: "FullName" },
            { data: "RelationshipType" },
            { data: "Age" },
            { data: "Status" },

            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>';
                    return html;
                }
            }
        ]
    });

    //Getting User Details

        var col2;
      
        BindCombo();
       

        function addGridItem(model) {
            table2.row.add(model).draw();
        }
        function updateGridItem(model) {
            var rowIdx = table2
                .cell(col)
                .index().row;   

            var d = table2.row(rowIdx).data();
            d.FullName = model.FullName;
            d.RelationshipType = model.RelationshipType;
            d.Age = model.Age;
            d.Status = model.Status

            table2
                .row(rowIdx)
                .data(d)
                .draw();

        }

        $('a.editor_create').click(function () {

            //new Util().clearform('#formFamily');
            $('#formFamily')[0].reset();
            validator.resetForm();
            $('#divStatus').hide();
            $('#pnlAudit').css('display', 'none');
            $('#ItbID').val(0);
            $('#btnSave').val(1);
            $('#btnSave').html('<i class="fa fa-save"></i> Save');
            // $('#UserName').removeAttr('disabled', 'disabled');
            $('a.editor_reset').show();
            $('#myModal').modal({ backdrop: 'static', keyboard: false });
        });
        $("#example1").on("click", "a.editor_edit", editDetailServer);
        function editDetailServer() {
            //loaderSpin2(true);
            //  disableButton(true);
            var editLink = $(this).attr('data-key');
            col = $(this).parent();
            // alert(editLink);
            $('#ItbID').val(editLink);


            $.ajax({
                url: $('#createurl').data('request-url') + '/' + editLink, //'Family/ViewDetail/' + editLink,
                data: null,
                type: "Get",
                contentType: "application/json",
                success: function (response) {
                    if (response.RespCode === 0) {
                        $('#FullName').val(response.model.FullName);
                        $('#RelationshipType').val(response.model.RelationshipType);
                        $('#Age').val(response.model.Age);
                        //alert(response.model.ItbID);
                        $('#ItbID').val(response.model.ItbID);
                        $('#divStatus').show();
                        //  $('#UserName').attr('disabled','disabled');
                        $('#CreatedBy').text(response.model.CreatedBy);
                        $('#CreateDate').text(response.model.CreateDate);
                        $('#Status').val(response.model.Status);
                        $('#pnlAudit').css('display', 'block');
                        $('a.editor_reset').hide();
                        // $('').val(response.ExamName);
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
        function BindCombo() {
            try {
                //  Bind Level
                var $reqLogin = {
                    url: $('#relationshipurl').data('request-url'), //'Family/RelationShipList',
                    data: null,
                    type: "Get",
                    contentType: "application/json"
                };
                $.ajax({
                    url: $('#relationshipurl').data('request-url'),
                    data: null,
                    type: "Get",
                    contentType: "application/json",
                    success: function (response) {
                        var exist = false;
                        console.log('this res', response)
                        if (response.data.length > 0) {
                            $("#RelationshipType").empty();
                            $("#RelationshipType").append("<option value=''> --Select RelationShip-- </option>");
                            for (var i = 0; i < response.data.length; i++) {
                                $("#RelationshipType").append("<option value='" + response.data[i].ItbID + "'>" +
                                     response.data[i].RelationshipName + "</option>");

                            }
                        }
                    },
                    failure: function (xhr, status, err) {
                        return null;
                    }
                });

            }
            catch (err) {

            }


        }


        var validator = $("#formFamily").validate({
            rules: {
                FullName: {
                    required: true,
                },
                RelationshipType: {
                    required: true,
                },
                Age: {
                    required: true,
                },
                Status: {
                    required: true,
                }
            },
            messages: {
                FullName: {
                    required: "Please Enter FullName"
                },
                RelationshipType: {
                    required: "Please Select RelationshipType"
                },
                Age: {
                    required: "Please Enter Age"
                },
                Status: {
                    required: "Please select status"
                }


            },
            submitHandler: function () {
                var btn = $('#btnSave').val();
                var urlTemp;
                var postTemp;
                var event;
                if (btn === "1") {

                    urlTemp = $('#createfamilyurl').data('request-url'), //'Family/Create';
                    postTemp = 'post';
                    $('#ItbID').val(0);
                    event = 'new';
                }
                else {

                    urlTemp = $('#createfamilyurl').data('request-url');
                    //postTemp = 'put';
                    postTemp = 'post';
                    event = 'modify';
                };
                //  alert(urlTemp);
                var $reqLogin = {
                    url: urlTemp,

                    data: $('#formFamily').serialize(),
                    type: postTemp,
                    contentType: "application/x-www-form-urlencoded"
                };

                $.ajax({
                    url: urlTemp,
                    data: $('#formFamily').serialize(),
                    type: postTemp,
                    contentType: "application/x-www-form-urlencoded",
                    success: function (response) {
                        if (response.RespCode === 0) {
                            $('#formFamily')[0].reset();

                            if (event === 'new') {
                                addGridItem(response.data);
                                $('#myModal').modal('hide');
                                $('#ItbID').val(0);
                                swal(
                               'Successful!',
                               response.RespMessage,
                               'success'
                                   )
                                // alert('Record Created Successfully');
                                //displayDialogNoty('Notification', 'Record Created Successfully');
                            }
                            else {
                                var btn = $('#btnSave').html('<i class="fa fa-save"></i> Save');
                                updateGridItem(response.data);
                                $('#myModal').modal('hide');
                                $('#ItbID').val(0);
                                swal(
                               'Successful!',
                               response.RespMessage,
                               'success'
                                   )

                                // alert('Record Updated successfully');
                                //displayDialogNoty('Notification', 'Record Updated Successfully');

                            }

                        }
                        else {
                            swal(
                               'Error!',
                               response.RespMessage,
                               'error'
                                   )
                            //displayModalNoty('alert-warning', response.RespMessage, true);

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