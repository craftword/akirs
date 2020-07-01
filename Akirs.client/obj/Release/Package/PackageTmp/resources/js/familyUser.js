$(document).ready(function () {

    var eID = $('#enrId').val();
    console.log(eID);




});




var userprofileTable = "";
//var historyTable = "";


$(document).ready(function () {
    //var d = document.getElementById("one-one")

    var url = $('#loader2').data('request-url');
    console.log('**userUser', url)

    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            console.log(data)
            console.log(data.data)

            let dataToShow = [];

            dataToShow.push([
                   data.data.LastName,
                   data.data.FirstName,
                   data.data.EnrollmentId,
                   data.data.PhoneNo,
                   data.data.Email,
                   data.data.Address,
                   data.data.Gender,
                   data.data.NoOfChildren,
                   data.data.Status,
                   data.data.MaritalStatus,
                   data.data.CreateDate,
                   data.data.City,
                   data.data.CompanyName
            ])
            $("#userLastName").val(data.data.LastName);
            $("#userFirstName").val(data.data.FirstName);
            $("#userEmail").val(data.data.Email);
            $("#userEnrollmentId").val(data.data.EnrollmentId);
            $("#userNum").val(data.data.PhoneNo);
            $("#userAddress").val(data.data.Address);
            $("#userGender").val(data.data.Gender);
            $("#userNoOfChild").val(data.data.NoOfChildren);
            $("#userStatus").val(data.data.Status);
            $("#userMStatus").val(data.data.MaritalStatus);
            $("#userCreateDate").val(data.data.CreateDate);
            $("#userCity").val(data.data.City)
            $("#companyNamee").val(data.data.CompanyName)
                          

        }
        
    })
    
})




//var userprofileTable = "";
////var historyTable = "";


//$(document).ready(function () {


//    var url = $('#loader2').data('request-url');
//    console.log('**userUser', url)   

//    $.ajax({
//        url: url,
//        type: "GET",
//        contentType:"application/json",
//        success: function (data) {
//            console.log(data)
//            console.log(data.data)
//            let dataToShow = [];

//            dataToShow.push([
//                   data.data.LastName,
//                   data.data.FirstName,
//                   data.data.EnrollmentId,
//                   data.data.PhoneNo,
//                   data.data.Email,
//                   data.data.Address,
//                   data.data.Gender,
//                   data.data.NoOfChildren,
//                   data.data.Status,
//                   data.data.MaritalStatus,
//                   data.data.CreateDate,
//                   data.data.City
//            ])
//            console.log('ewu',dataToShow)

//            if (userprofileTable != "") {
//                userprofileTable.destroy();

//            }
//            var userTab = "";
          
//            userTab = `<tr><td>${data.data.LastName}</td><td>${data.data.FirstName}</td><td>${data.data.EnrollmentId}</td><td>${data.data.PhoneNo}</td><td>${data.data.Email}</td></tr>`
           
//            $("#userTab").html(userTab)
            
//        }


//    })

     
   

//})
   
function GetPayHistory()
{ 
   var url = $('#loader3').data('request-url');
    console.log('**userUser', url)
   
    $.ajax({
        url: url,
        type: "GET",
        contentType:"application/json",
        success: function (data) {
            console.log(data)
            console.log(data.data)
            let dataToShow = [];

            dataToShow.push([
                   data.data.UserId,
                   data.data.AccountName,
                   data.data.Amount,
                   data.data.TransactionDate,
                   data.data.Status
            ])
            console.log('ewu',dataToShow)

            historyTable.destroy();       
                       
            if (response.RespCode === 0) {

                var historyTab = "";

                historyTab = `<tr><td>${data.data.UserId}</td><td>${data.data.AccountName}</td><td>${data.data.Amount}</td><td>${data.data.TransactionDate}</td><td>${data.data.Status}</td></tr>`

                $("#historyTab").html(historyTab)

            }
           
        }


    })
}