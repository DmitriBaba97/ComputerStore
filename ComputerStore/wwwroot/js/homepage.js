function setProductSlider() {
    $("#newest_products .bxslider").bxSlider({
        maxSlides: 4,
        slideWidth: 400,
        slideMargin: 10,
        touchEnabled: false,
        responsive: true
    });
    //Remove z-index from slider arrow control
    $(".bx-wrapper .bx-controls-direction a").css('z-index', 0);
}

$(document).ready(function () {
    if (window.location.pathname == "/") {

        setProductSlider();

        
    }
});