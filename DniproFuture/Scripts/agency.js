/*!
 * Start Bootstrap - Agnecy Bootstrap Theme (http://startbootstrap.com)
 * Code licensed under the Apache License v2.0.
 * For details, see http://www.apache.org/licenses/LICENSE-2.0.
 */

// jQuery for page scrolling feature - requires jQuery Easing plugin
$(function () {
    $('a.page-scroll').bind('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });
});

// Highlight the top nav as scrolling occurs
$('body').scrollspy({
    target: '.navbar-fixed-top'
})

// Closes the Responsive Menu on Menu Item Click
$('.navbar-collapse ul li a').click(function () {
    $('.navbar-toggle:visible').click();
});

$(document).ready(function () {
    $('li img .mymodal').on('click', function () {
        var src = $(this).attr('src');
        var img = '<img src="' + src + '" class="img-responsive"/>';
        $('#myModal').modal();
        $('#myModal').on('shown.bs.modal', function () {
            $('#myModal .modal-body').html(img);
        });
        $('#myModal').on('hidden.bs.modal', function () {
            $('#myModal .modal-body').html('');
        });
    });

    $(document).delegate('*[data-toggle="lightbox"]', 'click', function (event) { event.preventDefault(); $(this).ekkoLightbox(); });

    jQuery(function ($) {
        $("#Phone").mask("+99 (999) 999-9999");
    });
});

function OnSuccessComment(data) {
    $('.fromModal').html(data.Name);
    $('.emailModal').html(data.Email);
    $('.phoneModal').html(data.Phone);
    $('.messageModal').html(data.Message);

    $('#DoneModal').modal('show');

}

