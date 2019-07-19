function removeFromCart(productId) {
    $.ajax({
        type: 'POST',
        url: '/Cart/RemoveFromCart',
        data: { productId: productId },
        headers: {
            "XSRF-TOKEN": $("#removeFromCartForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function (result) {
            //Remove row from table
            var row = $(".cart_container table tr[data-rowid='" + productId + "']");
            row.remove();

            //Change total price of products
            var total = $(".cart_container .total_price");
            total.text("$" + result.totalPrice);

            //Change status label of the cart
            var cartStatusContainer = $("#top_navbar .lines_status");
            if (result.numOfLines == 0) {
                cartStatusContainer.text("");
            } else {
                cartStatusContainer.text(result.numOfLines);
            }
        }
    });
}

function addToCart(productId) {
    var productId = $("#product_preview_container #addToCartForm input:hidden[name='productId']").val();
    $.ajax({
        type: 'POST',
        url: '/Cart/AddToCart',
        data: { productId: productId },
        headers: {
            "XSRF-TOKEN": $("#addToCartForm input:hidden[name='__RequestVerificationToken']").val()
        },
        success: function (num) {
            var cartStatusContainer = $("#top_navbar .lines_status");
            if (num != 0) {
                cartStatusContainer.text(num);
            }
        }
    });
}
