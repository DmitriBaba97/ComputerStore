function setImageGallery() {
    $("#product_preview_container #lightgallery").lightGallery({
        mode: 'lg-fade',
        cssEasing: 'cubic-bezier(0.25, 0, 0.25, 1)',
        thumbMargin: 10
    });
}

$(document).ready(function () {
    if (window.location.pathname.match("/Home/ProductPreview")) {
        
        setImageGallery();
    }
});