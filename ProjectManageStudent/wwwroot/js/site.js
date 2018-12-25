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
