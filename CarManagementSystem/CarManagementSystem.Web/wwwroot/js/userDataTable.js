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
           
             "columns": [
                 { "data": "Id", "name": "Id", "visible": false },
                 { "data": "UserName", "name": "UserName" },
                 { "data": "Email", "name": "Email" },
                
             ]
         


     });
});