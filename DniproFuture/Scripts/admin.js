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
});