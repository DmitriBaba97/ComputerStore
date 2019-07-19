/*Build roles table*/
function buildRolesTable() {
    var table = $("#roles_container #roles_table");
    var tableBody = table.children("tbody");
    var errorMessage = $("#roles_container #no_roles_message");

    //Get roles objects
    $.ajax({
        type: 'GET',
        url: '/Role/GetRoles',
        success: function (roles) {
            if (roles.length == 0) {
                //Show error message and hide table
                errorMessage.show();
                table.hide();
            } else {
                //Hide error message and show table
                errorMessage.hide();
                //Build table
                tableBody.empty();
                for (var i = 0; i < roles.length; i++) {
                    tableBody.append(roleRow(roles[i]));
                }
                table.show();
            }
        }
    });
}

/*Return row for roles table*/
function roleRow(role) {
    return "<tr class='text-center' data-rowId='" + role.id + "'>"
        + "<td>" + role.name + "</td>"
        + "<td>" + role.numOfUsers + "</td>"
        + "<td>"
        + "<button type='button' class='btn btn-outline-danger m-1' data-toggle='modal' data-target='#myRoleModal" + role.id + "'>Delete</button >"
        + "<div class='modal fade' id='myRoleModal" + role.id + "'>"
        + "<div class='modal-dialog modal-dialog-centered'>"
        + "<div class='modal-content'>"
        + "<div class='modal-header bg-danger text-white'>"
        + "<h4 class='modal-title'>Delete role</h4>"
        + "<button type='button' class='close' data-dismiss='modal'><i class='fas fa-window-close text-white'></i></button >"
        + "</div>"
        + "<div class='modal-body' style='font-size: 0.9rem; font-family: Numans, sans-serif;'>"
        + "<i class='fas fa-exclamation-triangle fa-lg text-danger'></i> Do you really want to delete role <b>" + role.name + "</b> ?"
        + "</div>"
        + "<div class='modal-footer'>"
        + "<button type='button' class='btn btn-outline-danger m-1' onclick=\"deleteRole('" + role.id + "')\" data-dismiss='modal'>Delete</button >"
        + "<button type='button' class='btn btn-outline-secondary m-1' data-dismiss='modal'>Close</button >"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</td>"
        + "</tr>";
}

/*Add new role with specified name*/
function addRole() {
    //Get value of input field
    var roleName = $("#roleForm #role_name").val();
    //Check validity of input field
    if (roleName != "" && roleName.length < 50) {
        //Add new role with specified name
        $.ajax({
            type: 'POST',
            url: '/Role/AddRole',
            data: { name: roleName },
            headers: {
                "XSRF-TOKEN": $("#roleForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function (role) {
                //Reset input
                var input = $("#roleForm #role_name");
                resetInput(input);
                //Add new row to the table
                var table = $("#roles_container #roles_table");
                var errorMessage = $("#roles_container #no_roles_message");
                var tableBody = table.children("tbody");
                var row = roleRow(role);
                tableBody.append(row);
                table.show();
                errorMessage.hide();
            }
        });
    }
}

/*Delete role with specified id*/
function deleteRole(id) {
    //Delete role with specified id from server
    $.ajax({
        type: 'POST',
        url: '/Role/DeleteRole',
        data: { id: id },
        headers: {
            "XSRF-TOKEN": $("#roleForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete row from table
            var row = $("#roles_table tr[data-rowId='" + id + "']");
            row.remove();
            var table = $("#roles_container #roles_table");
            var errorMessage = $("#roles_container #no_roles_message");
            var tableBody = table.children("tbody");
            if (tableBody.children("tr").length == 0) {
                //If all rows were deleted from table
                table.hide();
                errorMessage.show();
            }
        }
    });
}

$(document).ready(function () {
    if (window.location.pathname == "/Role") {

        //Cretae roles table
        buildRolesTable();

        //Prevent submitting form
        var form = $("#roles_container #roleForm");
        preventSubmit(form);

        //Validate role name input
        var roleInput = $("#roleForm #role_name");
        var errorBlock = $("#roleForm #role_name_error");
        validateNameInput(roleInput, errorBlock);

        //Add new role on click
        var addBtn = $("#roleForm #add_role_btn");
        addBtn.click(addRole);
    }
});