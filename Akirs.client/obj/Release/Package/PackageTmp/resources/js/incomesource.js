$(document).ready(function () {
    var col;
    var datagotten;
    var result = [];
  
    console.log('Kapaichu'),
    BindCombo();
    var table2 = $('.familytable').DataTable({
        "processing": true,
        "serverSide": true,
        ajax: $('#incomedeclare').data('request-url'),//"IncomeDeclaration/IncomeSourceList",
        "type": "Get",
        "datatype": "json",
        columns: [
           
            { data: "SourceOfIncome" },
            { data: "Amount" },
            { data: "IncomeYear" },
            { data: "Status" },
            
            {
                data: null,
                className: "center_column",
                render: function (data, type, row) {
                    datagotten = Object.assign({}, data);
                   
                   
                    result.push(datagotten);
                   
                    

                  
                    var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>';
                    return html;                 
                }
                
            },

        {
        data: null,
        className: "center_column",
        render: function (data, type, row) {
                             
                    

                  
            var html = '<a class="btn btn-primary btn-xs editor_delete" data-key="' + data.ItbID + '"><i class="glyphicon glyphicon-trash" id="deleteRow"></i></a>';
            return html;                 
        }
                
    }

        ]
    });


    function addGridItem(model) {
        table2.row.add(model).draw();
    }
    function updateGridItem(model) {
        var rowIdx = table2
            .cell(col)
            .index().row;

        var d = table2.row(rowIdx).data();
        d.SourceOfIncome = model.SourceOfIncome;
        d.Amount = model.Amount;
        //d.IncomeYear = model.IncomeYear;
        d.Status = model.Status

        table2
            .row(rowIdx)
            .data(d)
            .draw();

    }


    $("#example1").on("click", "a.editor_delete", DeleteDetailS);

    function DeleteDetailS() {
       
        var deleteLink = $(this).attr('data-key');
        col = $(this).parent();
        //alert(deleteLink);
         $('#ItbID').val(deleteLink);


        $.ajax({
            url: $('#incomedeletedetail').data('request-url') + '/' + deleteLink,//'IncomeDeclaration/ViewDetail/' + deleteLink,

            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {

                if (response.RespCode === 0) {

                    // alert("testing")

                    //$('#SourceOfIncomeID').val(response.model.SourceOfIncomeID);
                    //$('#Amount').val(response.model.Amount);
                    //$('#ItbID').val(response.model.ItbID);
                    //$('#divStatus').show();
                    //$('#CreatedBy').text(response.model.CreatedBy);
                    //$('#CreateDate').text(response.model.CreateDate);
                    //$('#Status').val(response.model.Status === "P" ? "Pending" : "Closed");
                    //$('#IncomeYear').val(response.model.IncomeYear);
                    //$('#pnlAudit').css('display', 'block');
                    //$('a.editor_reset').hide();
                    //$('#btnSave').html('<i class="fa fa-edit"></i> Update');
                    //$('#btnSave').val(2);
                    //$('#myModal').modal({ backdrop: 'static', keyboard: false });
                }
                else {
                    //swal(
                    //       'Error!',
                    //       response.RespMessage,
                    //       'error'
                    //           )
                }
            },
            //failure: function (xhr, status, err) {
            //    $('.ajax-loader').css("visibility", "hidden");
            //},
            //beforeSend: function () {
            //    $('.ajax-loader').css("visibility", "visible");
            //},
            //complete: function () {
            //    $('.ajax-loader').css("visibility", "hidden");
            //}
            
        })

        location.reload();
    }

    

    function DeleteDetailServer() {
        //loaderSpin2(true);
        //  disableButton(true);
        var DeleteLink = $(this).attr('data-key');
        col = $(this).parent();
        // alert(editLink);
        $('#ItbID').val(DeleteLink);


        $.ajax({
            
            url: $('#deleteRow').data('request-url') + '/' + DeleteLink,//'IncomeDeclaration/ViewDetail/' + editLink,

            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {

                if (response.RespCode === 0) {

                     //alert("testing")

                    $('#SourceOfIncomeID').val(response.model.SourceOfIncomeID);
                    $('#Amount').val(response.model.Amount);
                    $('#ItbID').val(response.model.ItbID);
                    $('#divStatus').show();
                    $('#CreatedBy').text(response.model.CreatedBy);
                    $('#CreateDate').text(response.model.CreateDate);
                    $('#Status').val(response.model.Status === "P" ? "Pending" : "Closed");
                    $('#IncomeYear').val(response.model.IncomeYear);
                    $('#pnlAudit').css('display', 'block');
                    $('a.editor_reset').hide();
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
            url: $('#incomeviewdetails').data('request-url') + '/' + editLink,//'IncomeDeclaration/ViewDetail/' + editLink,
            data: null,
            type: "Get",
            contentType: "application/json",
            success: function (response) {
                
                if (response.RespCode === 0) {
                    
                   // alert("testing")
                   
                    $('#SourceOfIncomeID').val(response.model.SourceOfIncomeID);
                    $('#Amount').val(response.model.Amount);
                    $('#ItbID').val(response.model.ItbID);
                    $('#divStatus').show();
                    $('#CreatedBy').text(response.model.CreatedBy);
                    $('#CreateDate').text(response.model.CreateDate);
                    $('#Status').val(response.model.Status === "P" ? "Pending" : "Closed");
                    $('#IncomeYear').val(response.model.IncomeYear);
                    $('#pnlAudit').css('display', 'block');
                    $('a.editor_reset').hide();
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
    function BindCombo() {
        try {
           
            $.ajax({
                url: $('#incomeviewlist').data('request-url'),//'IncomeDeclaration/IncomeTypeList',
                data: null,
                type: "Get",
                contentType: "application/json",
                success: function (response) {
                    var exist = false;
                    if (response.data.length > 0) {
                        $("#SourceOfIncomeID").empty();
                        $("#SourceOfIncomeID").append("<option value=''> --Select Income Source-- </option>");
                        for (var i = 0; i < response.data.length; i++) {
                            $("#SourceOfIncomeID").append("<option value='" + response.data[i].ItbID + "'>" +
                                 response.data[i].SourceOfIncome + "</option>");

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
    console.log(result, "data");
    //$('select.i-source').change(function () {   
    //    var sourceOfIncome = $(this).children("option:selected").text();
    //    console.log(sourceOfIncome);
    //})

 


var validator = $("#formFamily").validate({
        rules: {
            FullName: "required",
            RelationshipType: "required",
            Age: "required",
        },
        messages: {

            Name: {
                required: "Please Enter FullName"
            },


        },
        submitHandler: function (){ 
            var btn = $('#btnSave').val();
            var incomeYear = $('#IncomeYear').val();
            var iSource = $("#SourceOfIncomeID option:selected").text();
            console.log(iSource, "INCOME SOURCE");

         
         
              
                var urlTemp;
                var postTemp;
                var event;
                if (btn == "1") {
                    if (result.some(data =>  data.IncomeYear === incomeYear && data.SourceOfIncome === iSource)) {
                        console.log("error")
                        swal(
                            'Error !',
                            'Source of Income cannot be declared more than once !!',
                            'error'
                        )
                    } else {
                        console.log("i'm fine")
                        urlTemp = $('#incomeviewcreate').data('request-url'), //'IncomeDeclaration/Create';
                        postTemp = 'post';
                        $('#ItbID').val(0);
                        event = 'new';
                    }

               
                }
                else {

                    urlTemp = $('#incomeviewcreate').data('request-url'), //'IncomeDeclaration/Create';
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

                            if (event == 'new') {
                                addGridItem(response.data);
                                $('#myModal').modal('hide');
                                $('#ItbID').val(0);
                                swal(
                              'Successful!',
                              response.RespMessage,
                              'success'
                                  )
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

                            }

                        }
                        else {
                           console.log("error")

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