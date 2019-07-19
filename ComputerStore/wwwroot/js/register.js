$(document).ready(function () {
    $("#username-input").popover({
        trigger: 'hover',
        html: true,
        content: "<ul><li>At least 1 lowercase letter.</li><li>At least 1 digit.</li><li>Shouldn't contain nonalphanumeric values.</li></ul>",
        placement: 'right'
    });
    $("#password-input").popover({
        trigger: 'hover',
        html: true,
        content: "<ul><li>At least 1 lowercase letter.</li><li>At least 1 digit.</li><li>Shouldn't contain uppercase letters.</li><li>Shouldn't contain nonalphanumeric values.</li></ul>",
        placement: 'right'
    });
    $("#confirmPassword").on('input', function () {
        var password = $("#password-input").val();
        var confirmPassword = $("#confirmPassword").val();
        if (confirmPassword != password) {
            $("#password_error").text("Password doesn't match!");
        } else {
            $("#password_error").text("");
        }
    });
});