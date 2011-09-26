function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}


String.prototype.format = function () {
    var s = this,
            i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};


$.fn.preload = function () {
    this.each(function () {
        $('<img/>')[0].src = this;
    });
}


$.fn.cleanWhitespace = function () {
    textNodes = this.contents().filter(
                function () { return (this.nodeType == 3 && !/\S/.test(this.nodeValue)); })
                .remove();
    return this;
}

$.fn.adjustTabs = function () {
    if ($(window).width() % 2 == 1) $(this).css("left", "-2px");
    else $(this).css("left", "-3px");
    return this;
}

$.fn.registerMouseOverAnimation = function () {
    $(this)
    .live('mouseenter', function (event) {
        $(this).addClass('with-background')
            .animate({
                left: '+=5',
                top: '-=5'
            }, 100);
    }).live('mouseleave', function (event) {
        $(this).removeClass('with-background')
            .animate({
                left: '-=5',
                top: '+=5'
            }, 100);
    }).live('click', function (event) {
        event.preventDefault();
        window.location.href = $(this).find(".item-target").attr("href");
    });
}
