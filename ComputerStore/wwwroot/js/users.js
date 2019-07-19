function uploadPhoto() {
    //Get form files
    var userId = $("#editUserForm #Id").val();
    var formData = new FormData();
    var file = $("#photoForm input[type='file']")[0].files[0];
    formData.append('file', file);
    formData.append('userId', userId);

    $.ajax({
        //Send files to the server
        type: 'POST',
        url: '/Image/UploadUserPhoto',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            "XSRF-TOKEN": $("#photoForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function (imageUrl) {
            var editUserForm = $("#editUser_container #editUserForm");
            editUserForm.append("<input type='hidden' name='Photo' value='" + imageUrl + "'/>");

            var photoContainer = $("#photo_container #photo");
            photoContainer.empty();
            photoContainer.append("<img src='" + imageUrl + "' width='230' height='230'; class=/>");
            
            
        }
    });
}

$(document).ready(function () {
    if (window.location.pathname.match("/User/ChangeRole/")) {
        var form = $("#changeUserRole_container #changeRoleForm");
        preventSubmit(form);

        var roleSelect = $("#changeRoleForm #selectRole");
        var userId = $("#changeRoleForm input:hidden[name='Id']").val();
        roleSelect.change(function () {
            var roleName = roleSelect.children("option:selected").val();
            if (roleName != "") {
                $.ajax({
                    type: 'POST',
                    url: '/User/ChangeRole',
                    data: { userId: userId, role: roleName },
                    headers: {
                        "XSRF-TOKEN": $("#changeRoleForm input:hidden[name='__RequestVerificationToken']").val()
                    }
                });
            }
        });
    }

    if (window.location.pathname.match("/User/EditUser/")) {
        var form = $("#photo_container #photoForm");
        setFileUploadControls(form);
    }
});