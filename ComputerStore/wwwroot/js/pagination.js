(function ($) {
    $.fn.pagination = function (options) {
        return this.each(function () {
            var $this = $(this);
            var paginationContent = $this.children(".pagination-content");
            var firstElem = paginationContent.children("li").first();
            firstElem.addClass("currentPage");
            setPages($this, options.callback);
            setCurrentPage($this);
            setLeftControl($this, options.callback);
            setRightControl($this, options.callback);
            setScrollLeftControl($this, options.callback);
            setScrollRightControl($this, options.callback);
        });
    }
})(jQuery);

function setPages(pagination, changeTable) {
    var paginationContent = pagination.children(".pagination-content");
    var pageElems = paginationContent.children("li");
    pageElems.each(function () {
        var pageBtn = $(this).children("button");
        pageBtn.click(function () {
            var page = parseInt($(this).text());
            changeTable(page);
        });
    });

}

function setLeftControl(pagination, callback) {
    var leftControlElem = pagination.children(".arrow-left");
    var leftControl = leftControlElem.children("button");
    leftControl.click(function () {
        moveLeft(pagination, callback);
    });
}

function setRightControl(pagination, callback) {
    var rightControlElem = pagination.children(".arrow-right");
    var rightControl = rightControlElem.children("button");
    rightControl.click(function () {
        moveRight(pagination, callback);
    });
}

function setScrollLeftControl(pagination, callback) {
    var scrollLeftControlElem = pagination.children(".scroll-left");
    var scrollLeftControl = scrollLeftControlElem.children("button");
    scrollLeftControl.click(function () {
        scrollLeft(pagination, callback);
    });
}

function setScrollRightControl(pagination, callback) {
    var scrollRightControlElem = pagination.children(".scroll-right");
    var scrollRightControl = scrollRightControlElem.children("button");
    scrollRightControl.click(function () {
        scrollRight(pagination, callback);
    });
}

//Move current pagination element to the left
function moveLeft(pagination, changeTable) {
    var paginationContent = pagination.children(".pagination-content");
    //Get current page element
    var currentPaginationElem = paginationContent.children("li.currentPage");
    var currentPage = currentPaginationElem.children("button").text();
    //Get first element of pagination
    var firstPaginationElem = paginationContent.children("li").first();
    var firstElem = firstPaginationElem.children("button").text();
    if (currentPage == firstElem) {
        //Scroll pagination left
        scrollLeft(container, changeTable);
    } else if (currentPage != "1") {
        //Get new categories
        var page = parseInt(currentPage);
        page = page - 1;
        changeTable(page);
        //Set current page to the previous element
        currentPaginationElem.removeClass("currentPage");
        currentPaginationElem.prev("li").addClass("currentPage");
    }
}

//Scroll pagination to the left
function scrollLeft(pagination, changeTable) {
    var paginationContent = pagination.children(".pagination-content");
    var firstPaginationElem = paginationContent.children("li").first();
    var firstElem = firstPaginationElem.children("button").text();
    var startPage = parseInt(firstElem);
    if (startPage > 1) {
        //Get previous elements of pagination
        $.ajax({
            type: 'GET',
            url: '/Category/GetPreviousPages',
            data: { firstPage: startPage },
            success: function (pages) {
                if (pages.length != 0) {
                    //Fill pagination container
                    paginationContent.empty();
                    pages.forEach(function (page) {
                        paginationContent.append("<li><button type='button' class='pagination_btn'>" + page + "</button></li>");
                    });

                    //Set first element as current
                    var firstPaginationElem = paginationContent.children("li").first();
                    firstPaginationElem.addClass("currentPage");
                    //Change categories table
                    var firstElem = firstPaginationElem.children("button").text();
                    var page = parseInt(firstElem);
                    changeTable(page);
                    //Set current page tracking
                    setCurrentPage(container);
                }
            }
        });
    }
}

//Move to the right in pagination
function moveRight(pagination, changeTable) {
    var paginationContent = pagination.children(".pagination-content");
    //Get current page element
    var currentPaginationElem = paginationContent.children("li.currentPage");
    var currentPage = currentPaginationElem.children("button").text();
    //Get last pagination element
    var lastPaginationElem = paginationContent.children("li").last();
    var lastElem = lastPaginationElem.children("button").text();
    if (currentPage == lastElem) {
        //Scroll pagination right
        scrollRight(container, changeTable);
    } else {
        //Change table
        var page = parseInt(currentPage);
        page = page + 1;
        changeTable(page);
        //Set next element of pagination as current
        currentPaginationElem.removeClass("currentPage");
        currentPaginationElem.next("li").addClass("currentPage");
    }
}

function scrollRight(pagination, changeTable) {
    var paginationContent = pagination.children(".pagination-content");
    var lastPaginationElem = paginationContent.children("li").last();
    var lastElem = lastPaginationElem.children("button").text();
    var lastPage = parseInt(lastElem);
    //Get next elements for pagination
    $.ajax({
        type: 'GET',
        url: '/Category/GetNextPages',
        data: { lastPage: lastPage },
        success: function (result) {
            if (lastPage < result.totalPages || result.pages.length != 0) {
                //Fill pagination container
                paginationContent.empty();
                result.pages.forEach(function (page) {
                    paginationContent.append("<li><button type='button' class='pagination_btn'>" + page + "</button></li>");
                });
                //Set first element as current
                var firstPaginationElem = paginationContent.children("li").first();
                firstPaginationElem.addClass("currentPage");
                //Change table
                var firstElemStr = firstPaginationElem.text();
                var page = parseInt(firstElemStr);
                changeTable(page);
                //Set current page tracking
                setCurrentPage(pagination);
            }
        }
    });

}

/*Set styles for current page of pagination*/
function setCurrentPage(pagination) {
    var paginationContent = pagination.children(".pagination-content");
    var pages = paginationContent.children("li");
    pages.each(function () {
        $(this).click(function () {
            var currentPage = paginationContent.children("li.currentPage");
            currentPage.removeClass("currentPage");
            $(this).addClass("currentPage");
        });
    });
}