
/*Return row for categories table*/
function categoryRow(category, id) {
    return "<tr class='text-center' data-rowid='" + category.id + "'>"
        + "<td>" + id + "</td>"
        + "<td>"
        + "<input type='text' class='category_edit_input' onfocus='$(this).prop('readonly', false);' onblur='$(this).prop('readonly', true)' value='" + category.name + "' />"
        + "</td>"
        + "<td>"
        + "<form class='manageCategoryForm' method='post'>"
        + "<button type='button' class='btn btn-outline-warning m-1' onclick='editCategory(" + category.id + ")'>Edit</button >"
        + "<button type='button' class='btn btn-outline-danger m-1' data-toggle='modal' data-target='#categoryModal" + id + "'>Delete</button >"
        + "<div class='modal fade' id='categoryModal" + id + "'>"
        + "<div class='modal-dialog modal-dialog-centered modal-sm'>"
        + "<div class='modal-content text-center'>"
        + "<div class='modal-header d-flex justify-content-center bg-danger text-white'>"
        + "<h4 class='modal-title'>Delete option value</h4>"
        + "<button type='button' class='close' data-dismiss='modal'><i class='fas fa-window-close text-white'></i></button >"
        + "</div>"
        + "<div class='modal-body' style='font-size: 0.9rem; font-family: Numans, sans-serif;'>"
        + "<i class='fas fa-exclamation-triangle fa-lg text-danger'></i> Do you really want to delete category <b>" + category.name + "</b> ?"
        + "</div>"
        + "<div class='modal-footer'>"
        + "<button type='button' class='btn btn-outline-danger m-1' onclick='deleteCategory(" + category.id + "); $(this).next().click();'>Delete</button >"
        + "<button type='button' class='btn btn-outline-secondary m-1' data-dismiss='modal'>Close</button >"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</form>"
        + "</td>"
        + "</tr>";
}

/*Add new category*/
function addCategory() {
    //Get value of input field
    var categoryName = $(".categoryForm .category_name").val();
    //Check validity of input field
    if (categoryName != "" && categoryName.length < 50) {
        //Add new category with specified name
        $.ajax({
            type: 'POST',
            url: '/Category/AddCategory',
            data: { name: categoryName },
            headers: {
                "XSRF-TOKEN": $(".categoryForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function (result) {
                //Reset input
                var input = $(".categoryForm .category_name");
                resetInput(input);

                var table = $(".categories_container .category_table").DataTable();
                var newRow = categoryRow(result, 1);
                table.row.add($(newRow)).draw();
            }
        });
    }
}

/*Delete category with specified id*/
function deleteCategory(id) {
    //Delete category with specified id from server
    $.ajax({
        type: 'POST',
        url: '/Category/DeleteCategory',
        data: { id: id },
        headers: {
            "XSRF-TOKEN": $(".categoryForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete row from table
            var table = $(".categories_container .category_table").DataTable();
            table.row("tr[data-rowid='" + id + "']").remove().draw();
        }
    });
}

/*Edit specified category*/
function editCategory(input, id) {
    //Get input of current row
    var categoryName = input.val();
    var data = {
        Id: id,
        Name: categoryName
    };
    //Check validity of input field
    if (categoryName != "" && categoryName.length < 50) {
        //Edit name of category with specified id
        $.ajax({
            type: 'POST',
            url: '/Category/EditCategory',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            headers: {
                "XSRF-TOKEN": $(".categoryForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function () {
                //Edit category name input in table
                input.prop('readonly', true);
            }
        });
    } 
}

$(document).ready(function () {

    if (window.location.pathname.match(/admin\/categories/i)) {

        //Prevent form from being submitted
        var form = $(".categories_container .categoryForm");
        preventSubmit(form);

        var addCategoryBtn = $(".categoryForm .add_category_btn");
        addCategoryBtn.click(addCategory);

        //Validate name input
        var categoryInput = $(".categoryForm .category_name");
        var errorBlock = $(".categoryForm .category_name_error");
        validateNameInput(categoryInput, errorBlock);

        var table = $(".categories_container .category_table");
        table.DataTable({
            "serverSide": true,
            "processing": true,
            "order": [0, "desc"],
            "ajax": {
                "url": "/Category/GetCategories",
                "type": "GET",
                "datatype": "json"
            }
        });
    }
    

});