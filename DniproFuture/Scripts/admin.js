$(document).ready(function () {
    GetUnread();
    setInterval('GetUnread()', 5000);
});

function GetUnread() {
    $.ajax({
        type: "POST",
        url: "/Home/GetUnread",
        success: function (response) {
            if (response > 0) {
                $("#unread").html(response);
                $("#unread").show();
            } else {
                $("#unread").hide();
            }

        }
    });
}

$(function () {
    $('#datetimepickerNews').datetimepicker({
        locale: 'ru',
        format: 'L'
    });
    $('#datetimepickerBirthday').datetimepicker({
        locale: 'ru',
        format: 'L'
    });
    $('#datetimepickerStartDate').datetimepicker({
        locale: 'ru',
        format: 'L'
    });
    $('#datetimepickerFinishDate').datetimepicker({
        locale: 'ru',
        format: 'L'
    });

    tinymce.init({
        selector: 'textarea',
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
    });

});

function HidePhoto(id) {
    $('#check_' + id).prop('checked', false);
    $('#check_' + id).prop('value', false);
    $('#oldPhoto_' + id).hide();
    if (id == 0) {
        $('#imgRemoveAlert').show();
    }
}

function ChekUploadedImageCount() {
    var inp = $('#Images')[0].files;
    if (inp.length > 0) {
        $('#imgRemoveAlert').hide();
    }
}