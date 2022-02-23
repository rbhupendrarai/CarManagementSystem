$(document).ready(function () {

    var datatable = $("#ImageTable").DataTable({
        "dom": '<"top"l>rt<"buttom"ip><"clear">',
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ordering": true,
        "search.visible": false,
        "ajax": {
            "url": "/Image/GetImageDetail",
            "type": "POST",
            "datatype": "json"
        },
  
        "columnDefs":
            [
                {
                    targets: [1],
                    orderable: false
                },
                {
                    targets: [3],
                    orderable: false
                },
                {
                    targets: [4],
                    orderable: false
                },
            ],
            "columns": [
                {
                    "data": "Img_Id", "name": "Img_Id", "visible": false
                },
               
                {
                    "data": "Img",
                    "render": function (data) {
                      var img = 'data:image/png;base64,' + data;
                        return '<img src="' + img + '" height="50px" width="50px" >';
                    }
                },
                {
                    "data": "MO_Name", "name": "MO_Name"
                },
                {
                    data: null,
                    render: function (data, type, row) {

                    return "<a href='#' class='btn btn-danger' onclick=UpdateImage('" + row.Img_Id + "');>Edit</a>";
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger' onclick=DeleteImage('" + row.Img_Id + "');>Delete</a>";
                }
            },]
    });
    $("#txtmoName").keyup(function () {
        datatable.column(2).search($(this).val().toLowerCase(), 'MO_Name');
        datatable.draw();

    });
});
function DeleteImage(Img_Id) {
    var url = '/Image/RemoveImage';
    $.post(url, { ID: Img_Id }, function (data) {
        if (data) {
            oTable = $('#ImageTable').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });
};
function AddNewImage() {
    $("#form")[0].reset();
    $("#Img_Id").val(0);
    $("#ModalTitle").html("Add New Image");
    $("#addImage").modal();

}
function UpdateImage(CR_Id) {
    var url = "/Image/GetImgId?Id=" + Img_Id;

    $("#ModalTitle").html("Update Image ");
    $("#addImage").modal();
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            var obj = JSON.parse(data);
            $("#imgId").val(obj.Img_Id);
            $("#moName").val(obj.MO_Name);
        }
    });
}
$("#SaveImage").click(function () {

  
    var data = $("#SubmitForm").serialize();
 
    $.ajax({
        type: "Post",
        url: "/Image/AddOrEditImage",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (otput) {
            alert("Added Successfully");
            window.location.href = "/Image/ImageDetail";
            $("#addImage").modal("hide");

        }
    });
});