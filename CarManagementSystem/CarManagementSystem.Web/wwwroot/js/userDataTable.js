$(document).ready(function (){
     $("#UserTable").DataTable(
     {
             "dom": '<"top"l>rt<"buttom"ip><"clear">',
             "processing": true,
             "serverSide": true,
             "filter": true,
             "ordering": true,
             "search.visible": false,
             "ajax": {
                 "url": "/Account/GetUserDetail",
                 "type": "POST",
                 "datatype": "json"
             },
             "columnDefs":
                 [
                     {
                         targets: [1],
                         orderable: false
                     },
                    

                 ],
             "columns": [
                 { "data": "Id", "name": "Id", "visible": false },
                 {
                     "data": "LockDate",
                     "render": function (data, type, row) {
                       
                         return '<input type="checkbox" onclick=UpdateUser("' + row.Id + '");>';
                     }
                 },
                 { "data": "UserName", "name": "UserName" },
                 { "data": "Email", "name": "Email" },
                 { "data": "Role", "name": "Role" },
                
             ]

     });
});
function UpdateUser(Id) {

    var url = '/Account/ActiveDeactiveUser';
   
    $.post(url, { ID: Id }, function (data) {
        if (data) {
            oTable = $('#UserTable').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });






};