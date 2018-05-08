﻿import $ from "jquery";

/**
 * A module containing all of our visibility helpers for the site.
 */
export namespace VisibilityHelpers {
    const $loaderContainer = $("#loader-wrapper");
    const alertContainerId = "#alert-container";

    /**
     * Create alerts inside the alert container.
     * @param type Alert type - "danger", "success", "info", "warning".
     * @param message Alert message.
     * @param timeout Whether to auto dismiss the alert after 10 seconds (Defaults to false if not passed).
     */
    export function alert(type: string, message: string, timeout?: boolean) {
        // Time out callback.
        $.fn.timeout = function (ms, callback) {
            var self = this;
            setTimeout(function () { callback.call(self); }, ms);
            return this;
        };

        var hbObj = { "type": type, "message": message };
        var alert = require("handlebarsTemplates/alert")(hbObj);

        if (timeout) {
            $(alert).appendTo(alertContainerId).timeout(10000, function (this: any) {
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

    /**
     * Helper function to hide alerts.
     * @param $alert The alert as a JQuery object.
     */
    function hideAlert($alert: JQuery) {
        $alert.removeClass("slide-in-right").addClass("slide-out-right").one("webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function () {
            $alert.remove();
        });
    }

    /**
     * Toggle visibility of the site loader.
     * @param show true = show the loader / false = hide the loader.
     */
    export function loader(show: boolean) {
        if (show) {
            $loaderContainer.removeClass("hidden");
        }
        else {
            $loaderContainer.addClass("hidden");
        }
    }
}