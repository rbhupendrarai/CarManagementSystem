$(document).ready(function () {

        var datatable = $("#SubModelTable").DataTable(
        {
            "dom": '<"top"l>rt<"buttom"ip><"clear">',
            "processing": true,
            "serverSide": true,
            "filter": true,
            "ordering": true,
            "search.visible": false,
            "ajax": {
                "url": "/SubModel/GetSubModelDetail",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                    [{
                        targets: [6],
                        orderable: false
                    },
                    {
                        targets: [7],
                        orderable: false
            },],
            "columns":
            [
                { "data": "SM_Id", "name": "SM_Id", "visible": false },
                { "data": "SM_Name", "name": "SM_Name" },
                { "data": "SM_Discription", "name": "SM_Discription" },
                { "data": "SM_Feature", "name": "SM_Feature" },
                { "data": "MO_Name", "name": "MO_Name" },
                { "data": "SM_Price", "name": "SM_Price" },
             
                {
                    data: null,
                    render: function (data, type, row) {

                        return "<a href='#' class='btn btn-danger' onclick=UpdateSubModel('" + row.SM_Id + "');>Edit</a>";
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' onclick=DeleteSubModel('" + row.SM_Id + "');>Delete</a>";
                    }
                },
            ]

        });
    $("#txtsmName").keyup(function () {
        datatable.column(0).search($(this).val().toLowerCase(), 'SM_Name');
        datatable.draw();

    });
    $("#txtsmDescription").keyup(function () {
        datatable.column(1).search($(this).val().toLowerCase(), 'SM_Description');
        datatable.draw();
    });
    $("#txtsmFeature").keyup(function () {
        datatable.column(2).search($(this).val().toLowerCase(), 'SM_Feature');
        datatable.draw();

    });
 
    $("#txtModel").keyup(function () {
        datatable.column(3).search($(this).val().toLowerCase(), 'MO_Name');
        datatable.draw();
    });
});


function DeleteSubModel(SM_Id) {
    if (confirm("Are you sure you want to delete ...?")) {
        var url = '/SubModel/RemoveSubModel';
        $.post(url, { ID: SM_Id }, function (data) {
            if (data) {
                oTable = $('#SubModelTable').DataTable();
                oTable.draw();
            } else {
                alert("Something Went Wrong!");
            }
        });
    } else {
        return false;
    }
};
function AddNewSubModel() {
    $("#form")[0].reset();
    $("#SM_Id").val(0);
    $("#ModalTitle").html("Add New Sub Model");
    $("#addSubModel").modal();

}
function UpdateSubModel(SM_Id) {
    var url = "/SubModel/GetSMId?ID=" + SM_Id;

    $("#ModalTitle").html("Update Model");
    $("#addSubModel").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#smId").val(obj.SM_Id);
            $("#smName").val(obj.SM_Name);
            $("#smDiscription").val(obj.SM_Discription);
            $("#smFeature").val(obj.SM_Feature);
            $("#smPrice").val(obj.SM_Price);
            $("#mname").val(obj.MO_Name);
        }
    });
}

$("#SaveSubModel").click(function () {
    var data = $("#SubmitForm").serialize();
    $.ajax({
        type: "Post",
        url: "/SubModel/AddOrEditSubModel",
        data: data,
        success: function (otput) {
            alert("Added Successfully");
            window.location.href = "/SubModel/SubModelDetail";
            $("#addSubModel").modal("hide");

        }
    });
});

 