// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('.mode').on('click', function () {
        if (!$('.my-nav').hasClass('app')) {
            $('.my-nav').addClass("app");
            $('.my-nav').addClass("dark-mode");
            document.documentElement.classList.add('dark')
        }
        else {
            $('.my-nav').removeClass("app");
            $('.my-nav').removeClass("dark-mode");
            document.documentElement.classList.remove('dark')
        }
    });
});




