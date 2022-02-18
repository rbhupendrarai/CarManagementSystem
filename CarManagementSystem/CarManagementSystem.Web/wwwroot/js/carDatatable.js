
$(document).ready(function () {
   
        var datatable=$("#CarTable").DataTable(
        {
            "dom":'<"top"l>rt<"buttom"ip><"clear">',
            "processing": true,
            "serverSide": true,
            "filter": true,            
            "ordering": true,
            "search.visible": false,
            "ajax": {
                "url": "/Car/GetCarDetail",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                [{
                    targets: [3],                    
                    orderable: false
                },
                {
                   targets: [4],
                   orderable: false
                },],
            "columns": [
                { "data": "CR_Id", "name": "CR_Id", "visible": false },
                { "data": "CR_Name", "name": "CR_Name"},
                { "data": "CR_Discription", "name": "CR_Discription" },
        
                {
                    data: null,
                    render: function (data, type,row) {

                        return "<a href='#' class='btn btn-danger' onclick=UpdateCar('" + row.CR_Id + "');>Edit</a>";
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' onclick=ConfirmDelete('" + row.CR_Id + "');>Delete</a>";
                    }
                },
            ]

        });
    
    $("#txtCarName").keyup(function () {
        datatable.column(0).search($(this).val().toLowerCase(), 'CR_Name');
        datatable.draw();
        
    });
    $("#txtDescription").keyup(function () {
        datatable.column(1).search($(this).val().toLowerCase(), 'CR_Description');
        datatable.draw();
    });
  
});
    function ConfirmDelete(CR_Id) {
        if (confirm("Are you sure you want to delete ...?")) {
            Delete(CR_Id);
        } else {
            return false;
        }
    };

    function Delete(CR_Id) {
        var url = '/Car/RemoveCar';
        $.post(url, { ID: CR_Id }, function (data) {
            if (data) {
                oTable = $('#CarTable').DataTable();
                oTable.draw();
            } else {
                alert("Something Went Wrong!");
            }
        });
    };
function AddNewCar() {
    $("#form")[0].reset();
    $("#CR_Id").val(0);
    $("#ModalTitle").html("Add New Car");
    $("#addCarModel").modal();

}
function UpdateCar(CR_Id) {
    var url = "/Car/GetCarId?Id=" + CR_Id;
        
    $("#ModalTitle").html("Update Car ");
    $("#addCarModel").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);            
            $("#cid").val(obj.CR_Id);
            $("#carname").val(obj.CR_Name);
            $("#discription").val(obj.CR_Discription);
          

        }
    });
}
$("#SaveCar").click(function () {
    var data = $("#SubmitForm").serialize();
    $.ajax({
        type: "Post",
        url: "/Car/AddOrEditCar",
        data: data,
        success: function (otput) {
            alert("Added Successfully");
            window.location.href = "/Car/CarDetail";
            $("#addCarModel").modal("hide");

        }
    });
});
    
