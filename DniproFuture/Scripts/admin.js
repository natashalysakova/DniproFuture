﻿$(document).ready(function () {
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