
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
         
            "columns":
            [
                { "data": "SM_Id", "name": "SM_Id", "visible": false },
                { "data": "SM_Name", "name": "SM_Name" },
                { "data": "SM_Discription", "name": "SM_Discription" },
                { "data": "SM_Feature", "name": "SM_Feature" },
                { "data": "SM_Price", "name": "SM_Price" },
                { "data": "MO_Name", "name": "MO_Name" },
                {
                    data: null,
                    render: function (data, type, row) {

                        return "<a href='#' class='btn btn-danger' onclick=UpdateSubModel('" + row.SM_Id + "');>Edit</a>";
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' onclick=ConfirmDelete('" + row.SM_Id + "');>Delete</a>";
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
        datatable.column(3).search($(this).val().toLowerCase(), 'SM_Feature');
        datatable.draw();

    });
    $("#txtsmPrice").keyup(function () {
        datatable.column(4).search($(this).val().toLowerCase(), 'SM_Price');
        datatable.draw();
    });
    $("#txtmodel").keyup(function () {
        datatable.column(5).search($(this).val().toLowerCase(), 'MO_Name');
        datatable.draw();
    });
});
function ConfirmDelete(SM_Id) {
    if (confirm("Are you sure you want to delete ...?")) {
        Delete(SM_Id);
    } else {
        return false;
    }
};

function Delete(SM_Id) {
    var url = '/SubModel/RemoveSubModel';
    $.post(url, { ID: SM_Id }, function (data) {
        if (data) {
            oTable = $('#SubModelTable').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });
};
function AddNewSubModel() {
    $("#form")[0].reset();
    $("#SM_Id").val(0);
    $("#ModalTitle").html("Add New Car");
    $("#addSubModel").modal();

}
function UpdateCar(SM_Id) {
    var url = "/SubModel/GetSMId?ID=" + SM_Id;

    $("#ModalTitle").html("Update Car ");
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
            $("#smid").val(obj.MO_Name);

        }
    });
}
$("#SaveSubModel").click(function () {
    var data = $("#SubmitForm").serialize();
    $.ajax({
        type: "Post",
        url: "/Car/AddOrEditSubModel",
        data: data,
        success: function (otput) {
            alert("Added Successfully");
            window.location.href = "/SubModel/SubModelDetail";
            $("#addCarModel").modal("hide");

        }
    });
});

 