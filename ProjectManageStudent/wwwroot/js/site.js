// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(Document).ready(function () {
    $('.bthEdit').click(function () {
        var SubjectId= $(this).parent().parent().find('td:eq(0)').text();
        var Id= $(this).parent().parent().find('td:eq(1)').text();
        var Practice= $(this).parent().parent().find('td:eq(2)').text();
        var Theory = $(this).parent().parent().find('td:eq(3)').text();
        var Assignment = $(this).parent().parent().find('td:eq(4)').text();
        $("#SubjectId").val(SubjectId);
        $("#Id").val(Id);
        $("#Practice").val(Practice);
        $("#Theory").val(Theory);
        $("#Assignment").val(Assignment);
        
    });
})

$(document).ready(function () {
    $("#bth_editFil").click(function () {
        if ($("#Id").val() != "") {


            $.ajax({
                url: "/Marks/Edit?id=" + $('#Id').val(),
                type: 'POST',

                data: $('form').serialize(),

                success: function (data) {
                    alert("Update success");
                    location.reload();
                },
                error: function (data) {
                    alert("Update fail");
                }
            });
        }
    });
})
$(document).ready(function () {
    $("#chu").click(function () {
        var likexx = $("#likessss").val();
        $("#same").val(likexx);
        if ($('#theo').val() == "") {
             $('#theo').val(-1);
        }
        if ($('#pra').val() == "") {
            $('#pra').val(-1);
        }
        if ($('#ass').val() == "") {
            $('#ass').val(-1);
        }
    });
})
$(document).ready(function () {
    $("#bth_editFil").click(function () {
        
        if ($('#Theory').val() == "") {
            $('#Theory').val(-1);
        }
        if ($('#Practice').val() == "") {
            $('#Practice').val(-1);
        }
        if ($('#Assignment').val() == "") {
            $('#Assignment').val(-1);
        }
    });
})
$(document).ready(function () {
    $("#ava").change(function () {
        var img = $('#ava').val();
        $('#imgs').attr('src', img);
    });
})
$(document).ready(function () {
    $("#ava1").change(function () {
        var img = $('#ava1').val();
        $('#imgs1').attr('src', img);
    });
})
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable1 tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
$(document).ready(function () {
    $('#example').DataTable();
});
