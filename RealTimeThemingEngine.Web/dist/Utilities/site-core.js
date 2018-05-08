import $ from "jquery";
import "jquery-pjax";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";
export var Site;
(function (Site) {
    var mainContainerId = "pjax-container";
    function init() {
        if ($.support.pjax) {
            $.pjax.defaults.timeout = 5000;
            $.pjax.defaults.maxCacheLength = 0;
            $(document).on("pjax:beforeSend", function () {
            });
            $(document).on("pjax:send", function () {
                VisibilityHelpers.loader(true);
            });
            $(document).on("pjax:timeout", function (event) {
                event.preventDefault();
                var loadDelayHtml = require("handlebarsTemplates/load-delay")({});
                $(loadDelayHtml).appendTo("#alert-container");
                $("#alert-container").find(".js-hard-refresh").on("click", function (e) {
                    e.preventDefault();
                    location.reload(true);
                });
            });
            $(document).on("pjax:error", function (_xhr, _textStatus, error, _options) {
                VisibilityHelpers.alert("danger", "<strong>Error</strong>: " + error, false);
            });
            $(document).on("pjax:success", function (data) {
                callPageInit(data);
            });
            $(document).on("pjax:complete", function (event) {
                $(event.target).removeClass("hidden");
                $("#delay-alert-container").remove();
                VisibilityHelpers.loader(false);
            });
            $(document).on("pjax:end", function () {
            });
            $(document).on("pjax:popstate", function () {
                location.reload(true);
            });
            callPageInit({ "target": { "id": mainContainerId } });
        }
        else {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Pjax failed to load.", false);
        }
    }
    Site.init = init;
    function loadPartial(url, data, containerSelector, callback, requestType) {
        if (!containerSelector || containerSelector.length === 0) {
            containerSelector = mainContainerId;
        }
        if (containerSelector.charAt(0) != "#" || containerSelector.charAt(0) != ".") {
            containerSelector = "#" + containerSelector;
        }
        if (!requestType || requestType.length === 0) {
            requestType = "GET";
        }
        $.pjax({
            "type": requestType,
            "url": url,
            "container": containerSelector,
            "data": data
        })
            .done(callback);
    }
    Site.loadPartial = loadPartial;
    function showJqXhrAsAlert(jqXHR) {
        var errorMsg = jqXHR.responseText;
        if (jqXHR.responseJSON) {
            errorMsg = jqXHR.responseJSON[0].ErrorMessage;
        }
        if (errorMsg.indexOf("DOCTYPE") !== -1) {
            errorMsg = $(errorMsg).find("h2").text();
        }
        VisibilityHelpers.alert("danger", "<strong>Error</strong>: " + errorMsg, true);
    }
    Site.showJqXhrAsAlert = showJqXhrAsAlert;
    function callPageInit(data) {
        var containerId = data.target.id;
        if (containerId === mainContainerId) {
            var hasRequire = $("#" + containerId).find("[page-require]");
            if (hasRequire.length > 0) {
                var ctrl = hasRequire.attr("page-require");
                var moduleName = hasRequire.attr("page-module-name");
                var moduleVariable = hasRequire.attr("page-module-variable");
                var moduleObject = require("Scripts/Pages/" + ctrl);
                if (moduleName == undefined) {
                    if (ctrl != undefined && moduleObject[ctrl] != undefined) {
                        moduleObject = moduleObject[ctrl];
                    }
                }
                else {
                    moduleObject = moduleObject[moduleName];
                }
                if (moduleVariable == undefined) {
                    moduleVariable = "index";
                }
                if (moduleObject.init != undefined) {
                    moduleObject.init(moduleVariable);
                }
                else {
                    var mo = new moduleObject();
                    if (mo.init != undefined) {
                        mo.init(moduleVariable);
                    }
                    else {
                        VisibilityHelpers.alert("danger", "Cannot find init method for " + ctrl);
                    }
                }
            }
        }
    }
})(Site || (Site = {}));
;
//# sourceMappingURL=site-core.js.map