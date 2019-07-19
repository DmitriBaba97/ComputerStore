/*Set content of subcategories table*/
function buildSubcategoriesTable(categoryName) {
    var table = $("#subcategories_container #subcategory_table");
    var errorMessage = $("#subcategories_container #no_subcategories_message");
    var infoMessage = $("#subcategories_container #info_message");
    var tableBody = $("#subcategory_table tbody");

    //Show input and button controls
    var hiddenBlock = $("#subcategoryForm #subcategory_block");
    hiddenBlock.show();
   
    //Get subcategories of category with specified name
    $.ajax({
        type: 'GET',
        url: '/Subcategory/GetSubcategories',
        data: { categoryName: categoryName },
        success: function (subcategories) {
            if (subcategories.length == 0) {
                //Hide table and show error message
                table.hide();
                infoMessage.hide();
                errorMessage.show();
            } else {
                //Show table and hide error message
                errorMessage.hide();
                infoMessage.show();
                //Build subcategories table
                var id = 0;
                tableBody.empty();
                for (var i = 0; i < subcategories.length; i++) {
                    id++;
                    tableBody.append(subcategoryRow(subcategories[i], id));
                }
                table.show();
            }
            //Toggle inputs of table
            var inputElems = $("#subcategory_table tbody tr td input");
            toggleTableNameInputs(inputElems);

        }
    });
}

/*Return row to append in subcategories table*/
function subcategoryRow(subcategory, id) {
    return "<tr class='text-center' data-rowId='" + subcategory.id + "'>"
        + "<td>" + id + "</td>"
        + "<td>"
        + "<input type='text' style='border: none; background-color: transparent;text-align: center;' data-categoryId='" + subcategory.id + "' value='" + subcategory.name + "' readonly/>"
        + "</td>"
        + "<td>"
        + "<button class='btn btn-outline-warning m-1' onclick='editSubcategory(" + subcategory.id + ")'>Edit</button>"
        + "<button type='button' class='btn btn-outline-danger m-1' data-toggle='modal' data-target='#mySubcategoryModal" + id + "'>Delete</button >"
        + "<div class='modal fade' id='mySubcategoryModal" + id + "'>"
        + "<div class='modal-dialog modal-dialog-centered'>"
        + "<div class='modal-content'>"
        + "<div class='modal-header bg-danger text-white'>"
        + "<h4 class='modal-title'>Delete option value</h4>"
        + "<button type='button' class='close' data-dismiss='modal'><i class='fas fa-window-close text-white'></i></button >"
        + "</div>"
        + "<div class='modal-body' style='font-size: 0.9rem; font-family: Numans, sans-serif;'>"
        + "<i class='fas fa-exclamation-triangle fa-lg text-danger'></i> Do you really want to delete subcategory <b>" + subcategory.name + "</b> ?"
        + "</div>"
        + "<div class='modal-footer'>"
        + "<button type='button' class='btn btn-outline-danger m-1' onclick='deleteSubcategory(" + subcategory.id + ")' data-dismiss='modal'>Delete</button >"
        + "<button type='button' class='btn btn-outline-secondary m-1' data-dismiss='modal'>Close</button >"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "<a href='/FilterOption/AddOption?subcategoryId=" + subcategory.id + "' class='btn btn-outline-success m-1'>Add Filter Options</a>"
        + "</td > "
        + "</tr>"
}

/*Add new subcategory to specified category*/
function addSubcategory() {
    //Check validity of input field
    var subcategoryName = $("#subcategoryForm #subcategory_name").val();
    if (subcategoryName != "" && subcategoryName.length < 50) {
        //Get current selected category name
        var categoryName = $("#subcategoryForm #selectCategory").children("option:selected").val();
        if (categoryName != "") {
            //Get category id of category with specified name
            $.ajax({
                type: 'GET',
                url: '/Category/GetCategoryId',
                data: { name: categoryName },
                success: function (categoryId) {
                    //Add new subcategory to category with specified id
                    var myData = { name: subcategoryName, categoryId: categoryId };
                    $.ajax({
                        type: 'POST',
                        url: '/Subcategory/AddSubcategory',
                        data: myData,
                        headers: {
                            "XSRF-TOKEN": $("input:hidden[name='__RequestVerificationToken']").val()
                        },
                        success: function (subcategory) {
                            //Reset input field
                            var input = $("#subcategoryForm #subcategory_name");
                            resetInput(input);
                            //Add new row to the table
                            var table = $("#subcategories_container #subcategory_table");
                            var infoMessage = $("#subcategories_container #info_message");
                            var errorMessage = $("#subcategories_container #no_subcategories_message");
                            var tableBody = table.children("tbody");
                            if (tableBody.children("tr").length == 0) {
                                 //If table has no data
                                var row = subcategoryRow(subcategory, 1);
                                tableBody.append(row);
                                table.show();
                                infoMessage.show();
                                errorMessage.hide();

                            } else {
                                //If table already has at least one row
                                var lastRow = tableBody.children("tr").last();
                                var lastId = lastRow.children("td").first().text();
                                var id = parseInt(lastId) + 1;
                                var row = subcategoryRow(subcategory, id);
                                tableBody.append(row);
                            }
                            //Toggle name inputs in table
                            var inputElems = $("#subcategory_table tbody tr td input");
                            toggleTableNameInputs(inputElems);
                        }
                    });
                }
            });
        }

    }
}

/*Delete subcategory*/
function deleteSubcategory(id) {
    //Delete subcategory from the category of specified id
    $.ajax({
        type: 'POST',
        url: '/Subcategory/DeleteSubcategory',
        data: { id: id },
        headers: {
            "XSRF-TOKEN": $("input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete row from table
            var row = $("#subcategory_table tbody tr[data-rowId='" + id + "']");
            row.remove();
            //If were deleted all elements from table
            var errorMessage = $("#subcategories_container #no_subcategories_message");
            var infoMessage = $("#subcategories_container #info_message");
            var table = $("#subcategories_container #subcategory_table");
            var tableBody = table.children("tbody");
            if (tableBody.children("tr").length == 0) {
                table.hide();
                infoMessage.hide();
                errorMessage.show();
            } 
        }
    });
}

/*Edit subcategory*/
function editSubcategory(id) {
    //Get input of current row in table and its value
    var input = $("#subcategory_table tbody tr input[data-subcategoryId='" + id + "']");
    var subcategoryName = input.val();
    //Check validity of name input
    if (subcategoryName != "" && subcategoryName.length < 50) {
        //Edit name of subcategory with specified id
        $.ajax({
            type: 'POST',
            url: '/Subcategory/EditSubcategory',
            data: { name: subcategoryName, id: id },
            headers: {
                "XSRF-TOKEN": $("input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function () {
                //Edit value of input in table
                input.val(subcategoryName);
            }
        });
    }
}

$(document).ready(function () {

    $('.mdb-select').materialSelect();

    if (window.location.pathname == "/Subcategory") {



        //Create table if category already selected
        var categorySelectList = $("#subcategoryForm #selectCategory");
        var categoryName = categorySelectList.children("option:selected").val();
        if (categoryName != "") {
            buildSubcategoriesTable(categoryName);
        }

        //Create table of subcategories on select of category name
        categorySelectList.change(function () {
            var categoryName = $(this).children("option:selected").val();
            if (categoryName != "") {
                buildSubcategoriesTable(categoryName);
            }
        });

       

        //Prevent form submit
        var form = $("#subcategories_container #subcategoryForm");
        preventSubmit(form);

        //Validate name input
        var subcategoryInput = $("#subcategoryForm #subcategory_name");
        var errorBlock = $("#subcategoryForm #subcategory_name_error");
        validateNameInput(subcategoryInput, errorBlock);

        //Add new subcategory to selected category on click
        var addSubcategoryBtn = $("#subcategoryForm #add_subcategory_btn");
        addSubcategoryBtn.click(addSubcategory);
    }
});