import $ from "jquery";
export var VisibilityHelpers;
(function (VisibilityHelpers) {
    var $loaderContainer = $("#loader-wrapper");
    var alertContainerId = "#alert-container";
    function alert(type, message, timeout) {
        $.fn.timeout = function (ms, callback) {
            var self = this;
            setTimeout(function () { callback.call(self); }, ms);
            return this;
        };
        var hbObj = { "type": type, "message": message };
        var alert = require("handlebarsTemplates/alert")(hbObj);
        if (timeout) {
            $(alert).appendTo(alertContainerId).timeout(10000, function () {
                hideAlert($(this));
            }).children(".close").on("click", function (e) {
                e.preventDefault();
                hideAlert($(this).parent());
            });
        }
        else {
            $(alert).appendTo(alertContainerId).children(".close").on("click", function (e) {
                e.preventDefault();
                hideAlert($(this).parent());
            });
        }
    }
    VisibilityHelpers.alert = alert;
    function hideAlert($alert) {
        $alert.removeClass("slide-in-right").addClass("slide-out-right").one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function () {
            $alert.remove();
        });
    }
    function loader(show) {
        if (show) {
            $loaderContainer.removeClass("hidden");
        }
        else {
            $loaderContainer.addClass("hidden");
        }
    }
    VisibilityHelpers.loader = loader;
})(VisibilityHelpers || (VisibilityHelpers = {}));
//# sourceMappingURL=visibility-helpers.js.map