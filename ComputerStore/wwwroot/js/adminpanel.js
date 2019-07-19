/*Change default jquery validators to allow invarian input of decimal value*/
$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}

/*Set new upload button*/
function setFileUploadControls(form) {
    //Replace default choose file control
    var fileInput = form.find("#uploadFile");
    var uploadBtn = form.find("#uploadFileBtn");
    uploadBtn.click(function () {
        fileInput.click()
    });
}

/*Set category options for select element*/
function setCategorySelectListItems(selectList) {
    $.ajax({
        type: 'GET',
        url: '/Category/GetCategoriesNames',
        success: function (categories) {
            if (categories != null) {
                for (var i = 0; i < categories.length; i++) {
                    selectList.append(new Option(categories[i], categories[i]));
                }
            }
        }
    });
}

/*Set subcategory options for appropriate category in select element*/
function setSubcategorySelectListItems(selectList, categoryName) {
    //Get subcategories names of category with specified name
    $.ajax({
        type: 'GET',
        url: '/Subcategory/GetSubcategoriesNames',
        data: { categoryName: categoryName },
        success: function (subcategories) {
            if (subcategories.length != 0) {
                //Clear select list
                selectList.empty();

                //Fill select list with options
                selectList.append(new Option("Select Subcategory", ""));
                subcategories.forEach(function (subcategory) {
                    selectList.append(new Option(subcategory, subcategory));
                });
            }
        }
    });
}

/*Put category id value in hidden field of form*/
function setCategoryId(input, categoryName) {
    //Get id of category with specified name
    $.ajax({
        type: 'GET',
        url: '/Category/GetCategoryId',
        data: { name: categoryName },
        success: function (id) {
            //Set value to hidden input
            input.val(id);
        }
    })
}

/*Reset input value*/
function resetInput(input) {
    input.val("");
}

/*Toggle readonly property of table name inputs*/
function toggleTableNameInputs(inputElems) {
    inputElems.on('focus', function () {
        $(this).prop('readonly', false);
    });
    inputElems.on('blur', function () {
        $(this).prop('readonly', true)
    });
}

/*Validate name input and append appropriate error message in error block*/
function validateNameInput(input, errorBlock) {
    input.on('input', function () {
        var subcategoryName = input.val();
        errorBlock.empty();

        if (subcategoryName == "") {
            errorBlock.append("<span id='no_value_error'>Name cannot be empty.</span>");
        }

        if (subcategoryName.length > 50) {
            errorBlock.append("<span id='length_error'>Name cannot contain more than 50 chars.</span>");
        }
    }); 
}

/*Prevent form from being submitted*/
function preventSubmit(form) {
    form.submit(function (e) {
        e.preventDefault();
    });
}

//Sets styles on selected and active menu item
function setStylesMenuItem() {
    var buttons = $(".menu-item button");
    /*-----------Sets styles on selected menu item---------*/
    buttons.click(function () {
        $(this).toggleClass("selected");
    });

    buttons.on('blur', function () {
        $(this).removeClass("selected");
    });
    /*-----------------------------------------------------*/

    /*------------Sets style on active menu item-------------*/
    var links = $(".menu-item .dropdown-menu a");
    links.each(function () {
        var currentBtn = $(this).parent().prev("button");
        if (window.location.pathname == $(this).attr("href")) {
            currentBtn.toggleClass("active");
        }
    });
    /*-------------------------------------------------------*/

    /*------------For menu items without dropdown menu--------*/
    var menuLinks = $(".menu-item .menu_link");
    menuLinks.each(function () {
        if (window.location.pathname == $(this).attr("href")) {
            $(this).toggleClass("active");
        }
    });
    /*-----------------------------------------------------*/

    /*------------Sets arrow rotation animation--------*/
    buttons.click(function () {
        var icon = $(this).children("div").children("i");
        icon.toggleClass("rotate");
        $(this).on('blur', function () {
            icon.removeClass("rotate");
        });
    });
    /*------------------------------------------------*/
}

//Creates content header according to the context
function createContentHeader() {
    var header = $("#content-header");
    var links = $(".menu-item .dropdown-menu a");
    links.each(function () {
        var currentBtn = $(this).parent().prev("button");
        if (window.location.pathname == $(this).attr('href')) {
            header.html(currentBtn.text() + " <small><i class='fas fa-angle-right fa-sm'></i> " + $(this).text() + "</small>");
        }
    });

    var menuLinks = $(".menu-item .menu_link");
    menuLinks.each(function () {
        if (window.location.pathname == $(this).attr('href')) {
            header.html($(this).text() + " <small><i class='fas fa-angle-right fa-sm'></i> Manage " + $(this).text() + "</small>");
        }
    });

    if (window.location.pathname.match("/User/EditUser/")) {
        header.html("Users <small><i class='fas fa-angle-right fa-sm'></i> Edit Profile</small>");
    }

    if (window.location.pathname.match("/User/ChangeRole/")) {
        header.html("Users <small><i class='fas fa-angle-right fa-sm'></i> Change Role</small>");
    }

    if (window.location.pathname.match("/Product/EditProduct")) {
        header.html("Products <small><i class='fas fa-angle-right fa-sm'></i> Edit Product</small>");
    }

    if (window.location.pathname == "/FilterOption/AddOption") {
        header.html("Subcategories <small><i class='fas fa-angle-right fa-sm'></i> Add Filter Options</small>");
    }

    if (window.location.pathname == "/FilterOption/AddOptionValue") {
        header.html("Filter Options <small><i class='fas fa-angle-right fa-sm'></i> Add Option Values</small>");
    }

    if (window.location.pathname == "/Admin") {
        header.html("Home <small><i class='fas fa-angle-right fa-sm'></i> Dashboard</small>");
    }


}

//Sets toggle button for side navbar
function toggleSidebarNav() {
    var btnOpen = $("#pushMenuBtn button");
    btnOpen.click(function () {
        $(".sidebar-nav").toggleClass("open");
    });

}

$(document).ready(function () {

    setStylesMenuItem();

    createContentHeader();

    toggleSidebarNav();
    
});