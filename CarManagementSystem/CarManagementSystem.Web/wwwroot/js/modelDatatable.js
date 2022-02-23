$(document).ready(function () {

    var datatable = $("#ModelTable").DataTable(
        {
            "dom": '<"top"l>rt<"buttom"ip><"clear">',
            "processing": true,
            "serverSide": true,
            "filter": true,
            "ordering": true,
            "search.visible": false,
            "ajax": {
                "url": "/Model/GetModelDetail",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                [{
                    targets: [5],
                    orderable: false
                },
                {
                    targets: [6],
                    orderable: false
                },],
            "columns": [
                { "data": "MO_Id", "name": "MO_Id", "visible": false },
                { "data": "MO_Name", "name": "MO_Name" },
                { "data": "MO_Discription", "name": "MO_Discription" },
                { "data": "MO_Feature", "name": "MO_Feature" },
                { "data": "CR_Name", "name": "CR_Name" },
                {
                    data: null,
                    render: function (data, type, row) {

                        return "<a href='#' class='btn btn-danger' onclick=UpdateModel('" + row.MO_Id + "');>Edit</a>";
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' onclick=DeleteModel('" + row.MO_Id + "');>Delete</a>";
                    }
                },
            ]

        });

    $("#txtModelName").keyup(function () {
        datatable.column(0).search($(this).val().toLowerCase(), 'MO_Name');
        datatable.draw();

    });
    $("#txtModelDescription").keyup(function () {
        datatable.column(1).search($(this).val().toLowerCase(), 'MO_Description');
        datatable.draw();
    });
    $("#txtModelFeature").keyup(function () {
        datatable.column(2).search($(this).val().toLowerCase(), 'MO_Feature');
        datatable.draw();
    });
    $("#txtCarSearch").keyup(function () {
        datatable.column(3).search($(this).val().toLowerCase(), 'CR_Name');
        datatable.draw();
    });

});


function DeleteModel(MO_Id) {
    if (confirm("Are you sure you want to delete ...?")) {
        var url = 'RemoveModel';
        $.post(url, { ID: MO_Id }, function (data) {
            if (data) {
                oTable = $('#ModelTable').DataTable();
                oTable.draw();
            } else {
                alert("Something Went Wrong!");
            }
        });
    }
    else {
        return false;
    }
};
function AddNewModel() {
    $("#form")[0].reset();
    $("#MO_Id").val(0);
    $("#ModalTitle").html("Add New Model");
    $("#addModel").modal();

}
function UpdateModel(MO_Id) {
    var url = "/Model/GetModelId?ID=" + MO_Id;

    $("#ModalTitle").html("Update Model");
    $("#addModel").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#mid").val(obj.MO_Id);
            $("#mname").val(obj.MO_Name);
            $("#mdiscription").val(obj.MO_Discription);
            $("#mfeature").val(obj.MO_Feature);
            $("#cname").val(obj.CR_Name);


        }
    });
}
$("#SaveModel").click(function () {
    var data = $("#SubmitForm").serialize();
    $.ajax({
        type: "Post",
        url: "/Model/AddOrEditModel",
        data: data,
        success: function (otput) {
            alert("Added Successfully");
            window.location.href = "/Model/ModelDetail";
            $("#addModel").modal("hide");

        }
    });

})


