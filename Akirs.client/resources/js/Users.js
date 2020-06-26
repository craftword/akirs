
$(document).ready(function () {


    var url = $('#viewProfile_Url').data('request-url');
    console.log('**userUser', url)

    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            console.log(data)
            console.log(data.data)


            $("#userLastName").val(data.data.LastName);
            $("#userFirstName").val(data.data.FirstName);
            $("#userEmail").val(data.data.Email);
            $("#userEnrollmentId").val(data.data.EnrollmentId);
            $("#userNum").val(data.data.PhoneNo);
            $("#userGender").val(data.data.Gender);
            $("#userNOC").val(data.data.NoOfChildren);
            $("#userStatus").val(data.data.Status);
            $("#userMStatus").val(data.data.MaritalStatus);
            $("#userCity").val(data.data.City);
            
        }
    })


    $("#saveProfile").click(function (e) {
        e.preventDefault()

        let lName = $("#userLastName").val();
        let fName = $("#userFirstName").val();
        let em = $("#userEmail").val();
        let userEid = $("#userEnrollmentId").val();
        let uNum = $("#userNum").val();       
        let uGender = $("#userGender").val();
        let uNOC = $("#userNOC").val();
        let uStatus = $("#userStatus").val();
        let uMStatus = $("#userMStatus").val();
        let uCity = $("#userCity").val();
        console.log(lName, "GOTTTEN", uNum)

        $.ajax({
            url: $('#SaveUser_url').data('request-url'),
            type: 'POST',
            data: JSON.stringify( {
                LastName: lName,
                FirstName: fName,
                Email: em,
                EnrollmentID: userEid,
                PhoneNo: uNum,
                Gender: uGender,
                NoOfChildren: uNOC,
                Status: uStatus,
                MaritalStatus: uMStatus,
                City: uCity
                              
            }),
            contentType: 'application/json ; charset=utf-8',
            success: function (response) {
                alert("User Updated Successfully");
                console.log(lName, "Hello", PhoneNo)
                window.location.reload();               

              
            }

        });

    })
});



