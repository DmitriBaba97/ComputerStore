$(document).ready(function () {
    var user = null;
    $(".secret-group-1").hide();
    $(".secret-group-2").hide();

    $("#input-username").on('input', function () {
        var username = $("#input-username").val();
        var myData = { username: username };
        $.ajax({
            type: 'Get',
            url: '/Account/GetUser',
            data: myData,
            success: function (result) {
                if (result == null) {
                    $("#username_error").text("User with this username doesn't exist!");
                    $(".secret-group-1").hide();
                    $(".secret-group-2").hide();
                } else {
                    user = result;
                    $("#username_error").text("");
                    $("#security_question").text(result.securityQuestion);
                    $(".secret-group-1").show();
                }
            }
        });
    });

    $("#input-answer").on('input', function () {
        var answer = $("#input-answer").val().toLowerCase();
        if (answer != user.securityQuestionAnswer) {
            $("#answer_error").text("Answer on security question is wrong.");
            $(".secret-group-2").hide();
        } else {
            $("#answer_error").text("");
            $(".secret-group-2").show();
        }
    });

    $("#confirmPassword").on('input', function () {
        var newPassword = $("#newPassword").val();
        var confirmPassword = $("#confirmPassword").val();
        if (confirmPassword != newPassword) {
            $("#newPassword_error").text("Password doesn't match!");
        } else {
            $("#newPassword_error").text("");
        }
    });

    $("#newPassword").popover({
        trigger: "focus",
        html: true,
        content: "<ul><li>At least 1 lowercase letter.</li><li>At least 1 digit.</li><li>Shouldn't contain uppercase letters.</li><li>Shouldn't contain nonalphanumeric values.</li></ul>",
        placement: 'right'
    });
});