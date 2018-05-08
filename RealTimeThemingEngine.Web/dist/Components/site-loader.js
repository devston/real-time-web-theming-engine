import $ from "jquery";
export var SiteLoader;
(function (SiteLoader) {
    function show(selector) {
        if ($("#site-loader-" + selector.substr(1)).length === 0) {
            $(selector).append("<div class=\"site-loader-backdrop load-in-container\" id=\"site-loader-" + selector.substr(1) + "\">" +
                "<div class=\"site-loader-container\">" +
                "<div class=\"site-loader\"></div>" +
                "</div>" +
                "</div>").addClass("overflow-hidden position-relative");
        }
    }
    SiteLoader.show = show;
    function remove(selector) {
        if ($("#site-loader-" + selector.substr(1)).length) {
            $("#site-loader-" + selector.substr(1)).remove();
        }
        $(selector).removeClass("overflow-hidden position-relative");
    }
    SiteLoader.remove = remove;
})(SiteLoader || (SiteLoader = {}));
//# sourceMappingURL=site-loader.js.map