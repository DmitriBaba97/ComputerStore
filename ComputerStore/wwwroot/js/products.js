/*Set preview image of the product*/
function setPreviewProperty(imageId, productId, forPreview) {
    $.ajax({
        type: 'POST',
        url: '/Image/EditImage',
        data: { imageId: imageId, productId: productId, forPreview: forPreview },
        headers: {
            "XSRF-TOKEN": $("#editProductForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function (nonPreviewImagesId) {
            //Show selected image
            $("#editProductForm #preview_images_container div[data-imageid='" + imageId + "']").addClass("for_preview");
            //Hide style for not selected image
            if (nonPreviewImagesId.length != 0) {
                nonPreviewImagesId.forEach(function (imageId) {
                    var container = $("#preview_images_container div[data-imageid='" + imageId + "']");
                    container.removeClass('for_preview');
                }); 
            }
        }
    });
}

/*Validate subcategory select element on form submit*/
function validateSubcategorySelectOnSubmit(form) {
    //Validate required fields on form submit
    form.submit(function (e) {
        var subcategory = $("#addProductForm #selectSubcategory").children("option:selected").val();
        var subcategory_Error = $("#addProductForm #subcategory_name_error");

        subcategory_Error.empty();
        if (subcategory == "") {
            subcategory_Error.append("<span>Please select product subcategory.</span>");
            e.preventDefault();
        }
    });
}

/*Validate category select element on form submit*/
function validateCategorySelectOnSubmit(form) {
    //Validate required fields on form submit
    form.submit(function (e) {
        var category = $("#addProductForm #selectCategory").children("option:selected").val();
        var category_Error = $("#addProductForm #category_name_error");

        category_Error.empty();
        if (category == "") {
            category_Error.append("<span>Please select product category.</span>");
            e.preventDefault();
        }
    });
}

/*Delete image from product images list*/
function deleteImage(id) {
    //Delete image from server
    $.ajax({
        type: 'POST',
        url: '/Image/DeleteImage',
        data: { imageId: id },
        headers: {
            "XSRF-TOKEN": $("#editProductForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function () {
            //Delete image block from images container
            var imageContainer = $("#editProductForm .images_container div[data-imageid='" + id + "']");
            imageContainer.remove();
            var previewImagesContainer = $("#editProductForm #preview_images_container");
            var image = previewImagesContainer.children("div[data-imageid='" + id + "']");
            image.remove();
        }
    });
}

/*Upload file to the server using ajax request*/
function uploadImages(formID) {
    //Get form files
    var formData = new FormData();
    var files = $(formID + " input[type='file']")[0].files;
    for (var i = 0; i < files.length; i++) {
        formData.append('files', files[i]);
    }
    var productId = $("#editProductForm #Id").val();
    formData.append('productId', productId);

    $.ajax({
        //Set progress bar for upload contol
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener('progress', function (e) {
                if (e.lengthComputable) {
                    var percent = Math.round((e.loaded / e.total) * 100);
                    $(formID + " .progress").show();
                    $(formID + " #progressBar").attr('aria-valuenow', percent).css('width', percent + '%').text(percent + '%');
                }
            }, false);
            return xhr;
        },
        //Send files to the server
        type: 'POST',
        url: '/Image/UploadProductImages',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            "XSRF-TOKEN": $(formID + " input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function (images) {
            if (images.length != 0) {
                //Hide progress bar
                $(formID + " .progress").hide();
                var imagesContainer = $(formID + " .images_container");
                var previewImagesContainer = $("#preview_images_container");
                images.forEach(function (image) {
                    $.ajax({
                        type: 'GET',
                        url: '/Image/GetImageId',
                        data: { imageName: image.name },
                        success: function (imageId) {
                            
                            //Add image in images container with delete button
                            imagesContainer.append("<div data-imageid='" + imageId + "' class='float-left m-1 p-3 position-relative'>"
                                + "<button type='button' class='position-absolute' style='top: 0;right: 0;background-color: transparent;border: none;' onclick='deleteImage(" + imageId + ")'>"
                                + "<i class='fas fa-window-close text-danger'></i>"
                                + "</button > "
                                + "<img src= '\\images\\products\\" + image.name + "' alt = 'product_image' height = '100' width = '100' />"
                                + "</div> ");

                            //Add images in preview images container with ability to select images
                            var productId = $("#editProductForm #Id").val();
                            previewImagesContainer.append("<div data-imageid='" + imageId + "' class='float-left m-1 p-3'>"
                                + "<img src= '\\images\\products\\" + image.name + "' alt = 'product_image' height='100' width='100' onclick='setPreviewProperty(" + imageId + ", " + productId + ", " + true + ")' />"
                                + "</div> ");
                        }
                    });
                });
            }
        }
    });
}

/*Return string to append in properties container div*/
function propertySelectList(option, listOrder) {
    return "<div class='form-group d-flex flex-row'>"
        + "<input type='hidden' name='Properties[" + listOrder + "].Name' value='" + option.name + "'/>"
        + "<label class='font-weight-bold' style='width: 120px;'>" + option.name + ":</label>"
        + "<select id='select" + option.name.replace(/\s/g, "") + "' name='Properties[" + listOrder + "].Value' class='form-control' style='width: calc(100% - 120px);'>"
        + "<option value=''>Select " + option.name + "</option>"
        + "</select>"
        + "</div>";
}

//Set select lists with option values of appropriate subcategory
function setPropertiesSelectLists(subcategoryId, form, propertiesContainer) {
    //Get filter options of specified subcategory id
    $.ajax({
        type: 'GET',
        url: '/FilterOption/GetOptions',
        data: { subcategoryId: subcategoryId },
        success: function (options) {
            if (options.length != 0) {
                //Clear properties container
                propertiesContainer.empty();
                //Set order number in product.properties list
                var orderNum = 0;
                options.forEach(function (option) {
                    //Add container with select element for property values and hidden input for property name 
                    propertiesContainer.append(propertySelectList(option, orderNum));
                    orderNum++;
                    //Get values of option with specified id
                    $.ajax({
                        type: 'GET',
                        url: '/FilterOption/GetOptionValues',
                        data: { optionId: option.id },
                        success: function (values) {
                            //Fill select list of property with appropriate values
                            var selectElem = $(form + " #select" + option.name.replace(/\s/g, ""));
                            if (values.length != 0) {
                                values.forEach(function (value) {
                                    selectElem.append(new Option(value.value, value.value));
                                });
                            }
                        }
                    });
                });
            }
        }
    });
}

$(document).ready(function () {
    if (window.location.pathname.match("/Product/AddProduct")) {

        //Set new file input controls
        var form = $("#addProduct_container #addProductForm");

        setFileUploadControls(form);

        var category_Error = $("#addProductForm #category_name_error");
        var subcategory_Error = $("#addProductForm #subcategory_name_error");

        var categorySelectList = $("#addProductForm #selectCategory");
        categorySelectList.change(function () {
            //Get name of currently selected category
            var categoryName = $(this).children("option:selected").val();
            if (categoryName != "") {

                category_Error.empty();

                //Clear properties container
                var propertiesContainer = $("#addProductForm #productPropertiesContainer");
                propertiesContainer.empty();

                //Set value of category id hidden input
                var categoryIdInput = $("#addProductForm #CategoryID");
                setCategoryId(categoryIdInput, categoryName);

                //Fill subcategory select list
                var subcategorySelectList = $("#addProductForm #selectSubcategory");
                setSubcategorySelectListItems(subcategorySelectList, categoryName);

                subcategorySelectList.change(function () {
                    //Get name of currently selected subcategory
                    var subcategoryName = $(this).children("option:selected").val();
                    if (subcategoryName != "") {

                        subcategory_Error.empty();

                        //Get id of subcategory with specidfied name
                        $.ajax({
                            type: 'GET',
                            url: '/Subcategory/GetSubcategoryId',
                            data: { name: subcategoryName },
                            success: function (subcategoryId) {
                                //Set value of current selected subcategory id
                                var subcategoryIdInput = $("#addProductForm #SubcategoryID");
                                subcategoryIdInput.val(subcategoryId);

                                //Set product properties select lists
                                setPropertiesSelectLists(subcategoryId, '#addProductForm', propertiesContainer);
                            }
                        });
                    }
                });
            }
        });

        validateCategorySelectOnSubmit(form);

        validateSubcategorySelectOnSubmit(form);

    }

     if (window.location.pathname.match('/Product/EditProduct')) { 

         var form = $("#editProduct_container #editProductForm");
         //Replace default choose file control
         setFileUploadControls(form); 

         var propertiesContainer = $("#editProductForm #productProperties");

         //Select category > Select subcategory order
        var categorySelectList = $("#editProductForm #selectCategory");
        categorySelectList.change(function () {
            //Get name of currently selected category
            var categoryName = $(this).children("option:selected").val();
            if (categoryName != "") {

                //Clear properties container
                propertiesContainer.empty();

                //Set value of category id hidden input
                var categoryIdInput = $("#editProductForm #CategoryID");
                setCategoryId(categoryIdInput, categoryName);

                //Fill subcategory select list
                var subcategorySelectList = $("#editProductForm #selectSubcategory"); 
                setSubcategorySelectListItems(subcategorySelectList, categoryName);

                subcategorySelectList.change(function () {
                    //Get name of currently selected subcategory
                    var subcategoryName = $(this).children("option:selected").val();
                    if (subcategoryName != "") {
                        //Get id of subcategory with specidfied name
                        $.ajax({
                            type: 'GET',
                            url: '/Subcategory/GetSubcategoryId',
                            data: { name: subcategoryName },
                            success: function (subcategoryId) {
                                //Set value of current selected subcategory id
                                var subcategoryIdInput = $("#editProductForm #SubcategoryID");
                                subcategoryIdInput.val(subcategoryId);

                                //Create properties select lists
                                setPropertiesSelectLists(subcategoryId, '#editProductForm', propertiesContainer);
                            }
                        });
                    }
                });
            }
         });

         //If category has already been selected
         var subcategorySelectList = $("#editProductForm #selectSubcategory");
         subcategorySelectList.change(function () {
             //Get name of currently selected subcategory
             var subcategoryName = $(this).children("option:selected").val();
             if (subcategoryName != "") {
                 //Get id of subcategory with specidfied name
                 $.ajax({
                     type: 'GET',
                     url: '/Subcategory/GetSubcategoryId',
                     data: { name: subcategoryName },
                     success: function (subcategoryId) {
                         //Set value of current selected subcategory id
                         var subcategoryIdInput = $("#editProductForm #SubcategoryID");
                         subcategoryIdInput.val(subcategoryId);

                         //Create select elements for all product properties
                         setPropertiesSelectLists(subcategoryId, '#editProductForm', propertiesContainer);
                     }
                 });
             }
         });
    }

    if (window.location.pathname == "/Product") {
        var table = $(".products_container .products_table");
        table.DataTable();
    }
});