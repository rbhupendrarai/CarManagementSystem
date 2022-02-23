var datatable = $("#HomeTable").DataTable(
    {
        "dom": '<"top"l>rt<"buttom"ip><"clear">',
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ordering": true,
        "search.visible": false,
        "ajax": {
            "url": "/Home/GetDashboard",
            "type": "POST",
            "datatype": "json"
        },
       
        "columns": [
            { "data": "CR_Id", "name": "CR_Id", "visible": false },
            { "data": "CR_Name", "name": "CR_Name" },
            { "data": "MO_Name", "name": "MO_Name" },
            { "data": "SM_Name", "name": "SM_Name" }
        
        ]

 });
$("#txtcarName").keyup(function () {
        datatable.column(0).search($(this).val().toLowerCase(), 'CR_Name');
        datatable.draw();

    });
$("#txtmoName").keyup(function () {
    datatable.column(1).search($(this).val().toLowerCase(), 'MO_Name');
    datatable.draw();
});
$("#txtsmName").keyup(function () {
    datatable.column(2).search($(this).val().toLowerCase(), 'SM_Name');
    datatable.draw();
});