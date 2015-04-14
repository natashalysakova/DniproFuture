$(document).ready(function () {
    GetUnread();
    setInterval('GetUnread()', 5000);

    $('#myModal').on('show.bs.modal', function(event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var id = button.data('id');
        var name = button.data('name');

        var modal = $(this);
        modal.find('.modal-title').text(name);
        modal.find('#helpId').val(id);
    });
});

function OnSuccess(data) {
    if (data.Result == true) {
        var str = '#sum_' + data.Id;
        var field = $(str);
        field[0].innerHTML = data.NewSumm;
        $('#myModal').modal('hide');
    } else {
        alert("Введено неверное значение. Попробуйте снова");
    }
}


function ChangePanel(val) {

    var def, prime;
    if (val === 1) {
            def = $('#adding');
    prime = $('#replace');

    } else {
        prime = $('#adding');
        def = $('#replace');

    }

    def.removeClass('panel-default');
    def.addClass('panel-primary');
    prime.removeClass('panel-primary');
    prime.addClass('panel-default');

}

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

