/*Create filter options table for specific subcategory*/
function buildOptionsTable() {
    var table = $("#filterOptions_container #filterOption_table");
    var tableBody = table.children("tbody");
    var infoMessage = $("#filterOptions_container #info_message");
    var errorMessage = $("#filterOptions_container #no_filterOption_message");

    //Get value of subcategory id
    var subcategoryIdInput = $("#filterOptionForm input:hidden[name='filterOption_subcategoryId']");
    var subcategoryId = subcategoryIdInput.val();

    //Get options
    $.ajax({
        type: 'GET',
        url: '/FilterOption/GetOptions',
        data: { subcategoryId: subcategoryId },
        success: function (options) {
            if (options.length == 0) {
                //Show error message and hide table
                errorMessage.show();
                infoMessage.hide();
                table.hide();
            } else {
                //Hide error message and show table
                errorMessage.hide();
                infoMessage.show();
                //Build table
                var id = 0;
                tableBody.empty();
                for (var i = 0; i < options.length; i++) {
                    id++;
                    tableBody.append(optionRow(options[i], id));
                }
                table.show();
            }
            //Toggle option name to be editable on focus
            var inputElems = $("#filterOption_table tbody tr td input");
            toggleTableNameInputs(inputElems);
        }
    });
}

/*Return row for options table*/
function optionRow(option, id) {
    return "<tr class='text-center' data-rowId='" + option.id + "'>"
        + "<td>" + id + "</td>"
        + "<td>"
        + "<input type='text' style='border: none; background-color: transparent;text-align: center;' data-optionId='" + option.id + "' value='" + option.name + "' readonly/>"
        + "</td>"
        + "<td>"
        + "<button type='button' class='btn btn-outline-warning m-1' onclick='editOption(" + option.id + ")'>Edit</button >"
        + "<button type='button' class='btn btn-outline-danger m-1' data-toggle='modal' data-target='#myFilterOptionModal" + id + "'>Delete</button >"
        + "<div class='modal fade' id='myFilterOptionModal" + id + "'>"
        + "<div class='modal-dialog modal-dialog-centered'>"
        + "<div class='modal-content'>"
        + "<div class='modal-header bg-danger text-white'>"
        + "<h4 class='modal-title'>Delete filter option</h4>"
        + "<button type='button' class='close' data-dismiss='modal'><i class='fas fa-window-close text-white'></i></button >"
        + "</div>"
        + "<div class='modal-body' style='font-size: 0.9rem; font-family: Numans, sans-serif;'>"
        + "<i class='fas fa-exclamation-triangle fa-lg text-danger'></i> Do you really want to delete filter option <b>" + option.name + "</b> ?"
        + "</div>"
        + "<div class='modal-footer'>"
        + "<button type='button' class='btn btn-outline-danger m-1' onclick='deleteOption(" + option.id + ")' data-dismiss='modal'>Delete</button >"
        + "<button type='button' class='btn btn-outline-secondary m-1' data-dismiss='modal'>Close</button >"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "<a href='/FilterOption/AddOptionValue?optionId=" + option.id + "' class='btn btn-outline-success m-1'>Add Values</a >"
        + "</td>"
        + "</tr>";
}

/*Set value for disabled input with subcategory name*/
function setSubcategoryName() {
    //Get input
    var subcategoryIdInput = $("#filterOptionForm input:hidden[name='filterOption_subcategoryId']");
    var subcategoryId = subcategoryIdInput.val();
    //Get subcategory name of specified id
    $.ajax({
        type: "GET",
        url: '/Subcategory/GetSubcategoryName',
        data: { id: subcategoryId },
        success: function (subcategory) {
            //Set value to input
            var subcategoryNameInput = $("#filterOptionForm #filterOption_subcategory_name");
            subcategoryNameInput.val(subcategory);
        }
    });
}

/*Add option*/
function addOption() {
    //Get value of input field
    var optionName = $("#filterOptionForm #filterOption_name").val();
    var subcategoryId = $("#filterOptionForm input[name='filterOption_subcategoryId']").val();
    //Check validity of input field
    if (optionName != "" && optionName.length < 50) {
        //Add new option with specified name
        $.ajax({
            type: 'POST',
            url: '/FilterOption/AddOption',
            data: { name: optionName, subcategoryId: subcategoryId },
            headers: {
                "XSRF-TOKEN": $("#filterOptionForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function (option) {
                //Reset input
                var input = $("#filterOptionForm #filterOption_name");
                resetInput(input);
                //Add new row to the table
                var table = $("#filterOptions_container #filterOption_table");
                var errorMessage = $("#filterOptions_container #no_filterOption_message");
                var infoMessage = $("#filterOptions_container #info_message");
                var tableBody = table.children("tbody");
                if (tableBody.children("tr").length == 0) {
                    //If table has no rows
                    var row = optionRow(option, 1);
                    tableBody.append(row);
                    infoMessage.show();
                    table.show();
                    errorMessage.hide();
                } else {
                    //If table has at least one row
                    var lastRow = tableBody.children("tr").last();
                    var lastId = lastRow.children("td").first().text();
                    var id = parseInt(lastId) + 1;
                    var row = optionRow(option, id);
                    tableBody.append(row);
                }
                //Toggle option name to be editable on focus
                var inputElems = $("#filterOption_table tbody tr td input");
                toggleTableNameInputs(inputElems);
            }
        });
    }
}

/*Delete option*/
function deleteOption(optionId) {

    //Delete coption with specified id from server
    $.ajax({
        type: 'POST',
        url: '/FilterOption/DeleteOption',
        data: { id: optionId },
        headers: {
            "XSRF-TOKEN": $("#filterOptionForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete row from table
            var row = $("#filterOption_table tr[data-rowId='" + optionId + "']");
            row.remove();
            var table = $("#filterOptions_container #filterOption_table");
            var errorMessage = $("#filterOptions_container #no_filterOption_message");
            var infoMessage = $("#filterOptions_container #info_message");
            var tableBody = table.children("tbody");
            if (tableBody.children("tr").length == 0) {
                //If all rows were deleted from table
                table.hide();
                infoMessage.hide();
                errorMessage.show();
            }
        }
    });
}

/*Edit option name*/
function editOption(id) {
    //Get input of current row
    var input = $("#filterOption_table tr td input[data-optionId='" + id + "']");
    var optionName = input.val();
    //Check validity of input field
    if (optionName != "" && optionName.length < 50) {
        //Edit name of option with specified id
        $.ajax({
            type: 'POST',
            url: '/FilterOption/EditOption',
            data: { name: optionName, id: id },
            headers: {
                "XSRF-TOKEN": $("#filterOptionForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function () {
                //Edit option name input in table
                input.val(optionName);
            }
        });
    }
}

/*Create table of values for specific filter option*/
function buildOptionValuesTable() {
    var table = $("#optionValues_container #optionValue_table");
    var tableBody = table.children("tbody");
    var errorMessage = $("#optionValues_container #no_optionValue_message");
    var infoMessage = $("#optionValues_container #info_message");

    //Get option id
    var optionIdInput = $("#optionValueForm input:hidden[name='optionValue_optionId']");
    var optionId = optionIdInput.val();

    //Get option values
    $.ajax({
        type: 'GET',
        url: '/FilterOption/GetOptionValues',
        data: { optionId: optionId },
        success: function (optValues) {
            if (optValues.length == 0) {
                //Show error message and hide table
                errorMessage.show();
                infoMessage.hide();
                table.hide();
            } else {
                //Hide error message and show table
                errorMessage.hide();
                infoMessage.show();
                //Build table
                var id = 0;
                tableBody.empty();
                for (var i = 0; i < optValues.length; i++) {
                    id++;
                    tableBody.append(optionValueRow(optValues[i], id));
                }
                table.show();
            }
            //Toggle option value to be editable on focus
            var inputElems = $("#optionValue_table tbody tr td input");
            toggleTableNameInputs(inputElems);
        }
    });
}

/*Set value of disabled input with filter option name*/
function setOptionName() {
    //Get input field
    var optionIdInput = $("#optionValueForm input:hidden[name='optionValue_optionId']");
    var optionId = optionIdInput.val();
    //Get option name
    $.ajax({
        type: "GET",
        url: '/FilterOption/GetOptionName',
        data: { id: optionId },
        success: function (option) {
            //Set value to input field
            var optionNameInput = $("#optionValueForm #optionValue_option_name");
            optionNameInput.val(option);
        }
    });
}

/*Return row for the option values table*/
function optionValueRow(value, id) {
    return "<tr class='text-center' data-rowId='" + value.id + "'>"
        + "<td>" + id + "</td>"
        + "<td>"
        + "<input type='text' style='border: none; background-color: transparent;text-align: center;' data-valueId='" + value.id + "' value='" + value.value + "' readonly/>"
        + "</td>"
        + "<td>"
        + "<button type='button' class='btn btn-outline-warning m-1' onclick='editValue(" + value.id + ")'>Edit</button >"
        + "<button type='button' class='btn btn-outline-danger m-1' data-toggle='modal' data-target='#myOptionValueModal" + value.id + "'>Delete</button >"
        + "<div class='modal fade' id='myOptionValueModal" + value.id + "'>"
        + "<div class='modal-dialog modal-dialog-centered'>"
        + "<div class='modal-content'>"
        + "<div class='modal-header bg-danger text-white'>"
        + "<h4 class='modal-title'>Delete option value</h4>"
        + "<button type='button' class='close' data-dismiss='modal'><i class='fas fa-window-close text-white'></i></button >"
        + "</div>"
        + "<div class='modal-body' style='font-size: 0.9rem; font-family: Numans, sans-serif;'>"
        + "<i class='fas fa-exclamation-triangle fa-lg text-danger'></i> Do you really want to delete option value <b>" + value.value + "</b> ?"
        + "</div>"
        + "<div class='modal-footer'>"
        + "<button type='button' class='btn btn-outline-danger m-1' onclick='deleteValue(" + value.id + ")' data-dismiss='modal'>Delete</button >"
        + "<button type='button' class='btn btn-outline-secondary m-1' data-dismiss='modal'>Close</button >"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</td>"
        + "</tr>";
}

/*Add new option value*/
function addValue() {
    //Get value of input field
    var optionValue = $("#optionValueForm #optionValue_value").val();
    var optionId = $("#optionValueForm input:hidden[name='optionValue_optionId']").val();
    //Check validity of input field
    if (optionValue != "" && optionValue.length < 50) {
        //Add new value to the specified option
        $.ajax({
            type: 'POST',
            url: '/FilterOption/AddOptionValue',
            data: { value: optionValue, optionId: optionId },
            headers: {
                "XSRF-TOKEN": $("#optionValueForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function (optValue) {
                //Reset input
                var input = $("#optionValueForm #optionValue_value");
                resetInput(input);
                //Add new row to the table
                var table = $("#optionValues_container #optionValue_table");
                var errorMessage = $("#optionValues_container #no_optionValue_message");
                var infoMessage = $("#optionValues_container #info_message");
                var tableBody = table.children("tbody");
                if (tableBody.children("tr").length == 0) {
                    //If table has no rows
                    var row = optionValueRow(optValue, 1);
                    tableBody.append(row);
                    infoMessage.show();
                    table.show();
                    errorMessage.hide();
                } else {
                    //If table has at least one row
                    var lastRow = tableBody.children("tr").last();
                    var lastId = lastRow.children("td").first().text();
                    var id = parseInt(lastId) + 1;
                    var row = optionValueRow(optValue, id);
                    tableBody.append(row);
                }
                //Toggle option value to be editable on focus
                var inputElems = $("#optionValue_table tbody tr td input");
                toggleTableNameInputs(inputElems);
            }
        });
    }
}

/*Delete option value */
function deleteValue(valueId) {
    //Delete value with specified id from server
    $.ajax({
        type: 'POST',
        url: '/FilterOption/DeleteOptionValue',
        data: { id: valueId },
        headers: {
            "XSRF-TOKEN": $("#optionValueForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete row from table
            var row = $("#optionValue_table tr[data-rowId='" + valueId + "']");
            row.remove();
            var table = $("#optionValues_container #optionValue_table");
            var errorMessage = $("#optionValues_container #no_optionValue_message");
            var infoMessage = $("#optionValues_container #info_message");
            var tableBody = table.children("tbody");
            if (tableBody.children("tr").length == 0) {
                //If all rows were deleted from table
                table.hide();
                infoMessage.hide();
                errorMessage.show();
            }
        }
    });
}

/*Edit option value*/
function editValue(id) {
    //Get input of current row
    var input = $("#optionValue_table tr td input[data-valueId='" + id + "']");
    var optionValue = input.val();
    //Check validity of input field
    if (optionValue != "" && optionValue.length < 50) {
        //Edit value of specified id
        $.ajax({
            type: 'POST',
            url: '/FilterOption/EditOptionValue',
            data: { value: optionValue, id: id },
            headers: {
                "XSRF-TOKEN": $("#optionValueForm input:hidden[name='__RequestVerificationToken']").val()
            },
            success: function () {
                //Edit opton value input in table
                input.val(optionValue);
            }
        });
    }
}

$(document).ready(function () {
    if (window.location.pathname == "/FilterOption/AddOption") {

        //Create table of options
        buildOptionsTable();

        //Set value of subcategory name field
        setSubcategoryName();

        //Prevent submitting form
        var form = $("#filterOptions_container #filterOptionForm");
        preventSubmit(form);

        //Add validation to option name input
        var optionNameInput = $("#filterOptionForm #filterOption_name");
        var errorBlock = $("#filterOptionForm #filterOption_name_error");
        validateNameInput(optionNameInput, errorBlock);

        //Add event listener to add button
        var addOptionBtn = $("#filterOptionForm #add_filterOptionName_btn");
        addOptionBtn.click(addOption);
    }

    if (window.location.pathname == "/FilterOption/AddOptionValue") {

        //Cretae option values table
        buildOptionValuesTable();

        //Set value of option name field
        setOptionName();

        //Prevent submitting of form
        var form = $("#optionValues_container #optionValueForm");
        preventSubmit(form);

        //Add validation to value input field
        var optionValueInput = $("#optionValueForm #optionValue_value");
        var errorBlock = $("#optionValueForm #optionValue_value_error");
        validateNameInput(optionValueInput, errorBlock);

        //Add event listener to add option value button
        var addOptionValueBtn = $("#optionValueForm #add_optionValue_value_btn");
        addOptionValueBtn.click(addValue);
    }
});