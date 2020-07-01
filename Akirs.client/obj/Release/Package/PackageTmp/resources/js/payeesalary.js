$(document).ready(function () {
     var table2 = $('#example123').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": $('#usersalarylist').data('request-url'),
            "dataSrc": function (json) {
                //Make your callback here.
                if (json.RespCode == 0)
                {
                    $('#btnApproveSalary').removeAttr('disabled');
                    $('#btnSkip').removeAttr('disabled');
                } else {
                    $('#btnApproveSalary').attr('disabled', 'disabled');
                    $('#btnSkip').attr('disabled', 'disabled');
                }
                return json.data;
            }
        },//"SalList",
        "type": "GET",
        "datatype": "json",
        columns: [
            { data: "EmployeeID" },
            { data: "EmployeeName" },
            { data: "AnnualBasic" },
            { data: "AnnualHousing" },
            { data: "AnnualTransport" },
             { data: "AnnualMeal" },
             { data: "AnnualOthers" },
             { data: "NHFContribution" },

            //{
            //    data: null,
            //    className: "center_column",
            //    render: function (data, type, row) {
            //        var html = '<a class="btn btn-primary btn-xs editor_edit" data-key="' + data.ItbID + '"><i class="fa fa-edit"></i></a>';
            //        return html;
            //    }
            //}
        ]
    });
     $('#btnSkip').click(function (e) {
         e.preventDefault();
        if (confirm('Are you sure you want to skip this record?')) {
            $.ajax({
                type: "POST",
                url: $('#skipayee').data('request-url'),//'UploadFiles',
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.RespCode === 0) {
                        $('#btnUploadSal').val('');

                        swal({ title: 'Successfull skipped', text: response.RespMessage, type: 'success' })

                    }
                    else {
                        swal({ title: 'Error!', text: response.RespMessage, type: 'error' })
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
    })
    $(document).on('click', '#btnApproveSalary', function (e) {
        e.preventDefault();

        //return;
        $.ajax({
            type: "POST",
            url: $('#salaryverify').data('request-url'), //url + 'VerifyRecord',
            contentType: "application/json",
            data: null,
            success: function (response) {
                if (response.RespCode == 0) {

                    $('#divGrid').html('');
                    $('#btnApproveSalary').attr('disabled', 'disabled');
                    $('#btnSkip').attr('disabled', 'disabled');
                    swal({ title: 'Successfull', text: 'Record approved successfully', type: 'success' })

                }
                else {
                    swal({ title: 'Error!!', text: response.RespMessage, type: 'error' })
                    //displayDialogNoty('Notification', response.RespMessage);
                }
            },
            error: function (err) {
                console.log(err);
            }

        });

    });
})