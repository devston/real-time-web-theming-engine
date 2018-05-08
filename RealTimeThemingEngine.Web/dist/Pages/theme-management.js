import $ from "jquery";
import "datatables.net";
import "datatables.net-bs4";
import "rangeslider.js";
import "@claviska/jquery-minicolors";
import { Site } from "Scripts/Utilities/site-core";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";
import { SiteLoader } from "Scripts/Components/site-loader";
export var ThemeManagement;
(function (ThemeManagement) {
    function init(pageToLoad) {
        switch (pageToLoad) {
            case "create":
                initCreateTheme();
                break;
            case "import":
                initImportTheme();
                break;
            case "editTheme":
                initEditTheme();
                break;
            case "editVariables":
                initEditVariables();
                break;
            case "logoManagement":
                initLogoManagement();
                break;
            default:
                initIndex();
                break;
        }
    }
    ThemeManagement.init = init;
    function initIndex() {
        $("#theme-table-container").find(".table").on("click", ".js-edit-btn", function (e) {
            e.preventDefault();
            var themeId = $(this).attr("data-id");
            Navigate.toEditTheme(themeId);
        });
        initThemeTable("#theme-table-container");
        $("#create-theme-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toCreate();
        });
        $("#import-theme-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toImport();
        });
        $("#logo-management-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toLogoManagement();
        });
    }
    function initCreateTheme() {
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
        $("#theme-form").on("submit", function (e) {
            e.preventDefault();
            createTheme();
        });
        $("#select-theme-btn").on("click", function (e) {
            e.preventDefault();
            showThemeSelectionModal("#theme-selection-modal-container", "#SelectedThemeId", "#SelectedThemeName");
        });
    }
    function initImportTheme() {
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
        $("#theme-form").on("submit", function (e) {
            e.preventDefault();
            importTheme();
        });
        VariableControls.initFileUpload("#FileToUse");
    }
    function initEditTheme() {
        var themeId = $("#ThemeId").val();
        var $form = $("#theme-form");
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
        $form.on("submit", function (e) {
            e.preventDefault();
            editTheme();
        });
        $("#save-theme-btn").on("click", function (e) {
            e.preventDefault();
            $form.submit();
        });
        $("#export-theme-btn").on("click", function (e) {
            e.preventDefault();
            exportTheme(themeId);
        });
        $("#confirm-delete-theme-btn").on("click", function (e) {
            e.preventDefault();
            openConfirmThemeDeletionModal(themeId);
        });
        $("#edit-theme-variables-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toEditThemeVariables(themeId);
        });
    }
    function initConfirmThemeDeletionModal(themeId) {
        $("#theme-deletion-form").on("submit", function (e) {
            e.preventDefault();
            deleteTheme(themeId);
        });
    }
    function initEditVariables() {
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
        $("#theme-form").on("submit", function (e) {
            e.preventDefault();
            saveThemeVariables();
        });
        VariableControls.initColour(".js-colour-control");
        VariableControls.initFont(".js-font-control");
        VariableControls.initLogo("#logo-list");
        VariableControls.initFileUpload("#upload-logo");
        VariableControls.initLogoUpload("#upload-logo", "#submit-logo");
        VariableControls.initSlider(".js-size-control");
    }
    function initLogoManagement() {
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
        $("#logo-table").on("click", ".js-delete-logo-btn", function (e) {
            e.preventDefault();
            var logoName = $(this).attr("data-logo");
            openConfirmLogoDeletionModal(logoName);
        });
    }
    function initConfirmLogoDeletionModal(logoName) {
        $("#logo-deletion-form").on("submit", function (e) {
            e.preventDefault();
            deleteLogo(logoName);
        });
    }
    function createTheme() {
        var formId = "#theme-form";
        var url = "/ThemeManagement/SaveNewTheme";
        saveTheme(url, formId);
    }
    function editTheme() {
        var formId = "#theme-form";
        var url = "/ThemeManagement/SaveThemeChanges";
        saveTheme(url, formId);
    }
    function saveTheme(url, formId) {
        var $form = $(formId);
        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    SiteLoader.show(formId);
                },
                url: url,
                type: "POST",
                data: $form.serialize()
            })
                .always(function () {
                SiteLoader.remove(formId);
            })
                .done(function (data) {
                VisibilityHelpers.alert("success", data.message, true);
                Navigate.toEditTheme(data.themeId);
            })
                .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }
    function importTheme() {
        var formId = "#theme-form";
        var $form = $(formId);
        var formData = new FormData();
        var fileInput = $form.find("#FileToUse").get(0);
        if (fileInput.files == null) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Select a theme to upload", true);
            return;
        }
        formData.append("FileToUse", fileInput.files[0]);
        formData.append("Name", $form.find("#Name").val());
        formData.append("__RequestVerificationToken", $form.find("input[name=__RequestVerificationToken]").val());
        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    SiteLoader.show(formId);
                },
                url: "/ThemeManagement/ImportThemeFromFile",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false
            })
                .always(function () {
                SiteLoader.remove(formId);
            })
                .done(function (data) {
                VisibilityHelpers.alert("success", data.message, true);
                Navigate.toEditTheme(data.themeId);
            })
                .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }
    function saveThemeVariables() {
        var formId = "#theme-form";
        var $form = $(formId);
        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    SiteLoader.show(formId);
                },
                url: "/ThemeManagement/SaveThemeVariables",
                type: "POST",
                data: $form.serialize()
            })
                .always(function () {
                SiteLoader.remove(formId);
            })
                .done(function (data) {
                VisibilityHelpers.alert("success", data.message, true);
                Navigate.toEditTheme(data.themeId);
            })
                .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }
    function deleteTheme(themeId) {
        var formId = "#theme-form";
        var token = $("#theme-deletion-form").find("input[name=__RequestVerificationToken]").val();
        closeConfirmThemeDeletionModal();
        $.ajax({
            beforeSend: function () {
                SiteLoader.show(formId);
            },
            url: "/ThemeManagement/DeleteTheme",
            type: "POST",
            data: {
                "id": themeId,
                "__RequestVerificationToken": token
            }
        })
            .always(function () {
            SiteLoader.remove(formId);
        })
            .done(function (data) {
            VisibilityHelpers.alert("success", data.message, true);
            Navigate.toIndex();
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    function exportTheme(themeId) {
        var formId = "#theme-form";
        $.ajax({
            beforeSend: function () {
                SiteLoader.show(formId);
            },
            url: "/ThemeManagement/ValidateExportTheme",
            type: "GET",
            data: {
                "id": themeId
            }
        })
            .always(function () {
            SiteLoader.remove(formId);
        })
            .done(function (data) {
            window.location.href = "/ThemeManagement/ExportTheme?id=" + data.themeId;
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    function uploadLogo(fileSelector, btnSelector) {
        var files = $(fileSelector).get(0).files;
        if (files == null || files.length == 0) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Select an image.", true);
            return;
        }
        else if (files[0].size > 1048576) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Image must be smaller than 1MB.", true);
            return;
        }
        var formData = new FormData();
        var totalFiles = files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = files[i];
            formData.append("upload-logo", file);
        }
        formData.append("__RequestVerificationToken", $("#theme-form").find("input[name=__RequestVerificationToken]").val());
        var $submitBtn = $(btnSelector);
        $.ajax({
            beforeSend: function () {
                VisibilityHelpers.loader(true);
                $submitBtn.attr("disabled", "");
            },
            type: "POST",
            url: "/ThemeManagement/UploadLogo",
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false
        })
            .always(function () {
            VisibilityHelpers.loader(false);
            $submitBtn.removeAttr("disabled");
        })
            .done(function (data) {
            if (data.warning) {
                VisibilityHelpers.alert("warning", data.message, true);
            }
            else {
                VisibilityHelpers.alert("success", data.message, true);
            }
            var hbObj = {
                "id": data.logoName.replace(".", "-"),
                "logoName": data.logoName
            };
            var logoTemplate = require("handlebarsTemplates/theme-logo-added-template")(hbObj);
            $("#logo-list").append(logoTemplate);
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    function deleteLogo(logoName) {
        var containerId = "#logo-management-container";
        var token = $("#logo-deletion-form").find("input[name=__RequestVerificationToken]").val();
        closeConfirmLogoDeletionModal();
        var $deleteBtns = $(containerId).find(".js-delete-logo-btn");
        $.ajax({
            beforeSend: function () {
                SiteLoader.show(containerId);
                $deleteBtns.attr("disabled", "");
            },
            url: "/ThemeManagement/DeleteLogo",
            type: "POST",
            data: {
                "logo": logoName,
                "__RequestVerificationToken": token
            }
        })
            .always(function () {
            SiteLoader.remove(containerId);
            $deleteBtns.removeAttr("disabled");
        })
            .done(function (data) {
            VisibilityHelpers.alert("success", data.message, true);
            $("#" + logoName.replace(".", "-")).remove();
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    function showThemeSelectionModal(containerId, themeIdInput, themeNameInput) {
        $(containerId).empty();
        initThemeTable(containerId);
        $(containerId).find("table > tbody").on("click", "tr", function () {
            var $this = $(this);
            $(themeIdInput).val($this.attr("data-id"));
            $(themeNameInput).val($this.children().first().text());
            $this.find(".modal").modal("hide");
        });
    }
    function initThemeTable(containerId) {
        $(containerId).find("table").DataTable();
    }
    function openConfirmThemeDeletionModal(themeId) {
        if ($("#theme-deletion-modal-container").is(":empty")) {
            loadConfirmThemeDeletionModal(themeId);
        }
        else {
            $("#theme-deletion-modal").modal("show");
        }
    }
    function closeConfirmThemeDeletionModal() {
        $("#theme-deletion-modal").modal("hide");
    }
    function loadConfirmThemeDeletionModal(themeId) {
        $.ajax({
            beforeSend: function () {
                VisibilityHelpers.loader(true);
            },
            type: "GET",
            url: "/ThemeManagement/_ConfirmDeleteTheme/" + themeId
        })
            .always(function () {
            VisibilityHelpers.loader(false);
        })
            .done(function (data) {
            $("#theme-deletion-modal-container").html(data);
            initConfirmThemeDeletionModal(themeId);
            openConfirmThemeDeletionModal(themeId);
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    function openConfirmLogoDeletionModal(logoName) {
        if (!$("#logo-deletion-modal-container").is(":empty")) {
            $("#logo-deletion-modal-container").empty();
        }
        loadConfirmLogoDeletionModal(logoName);
    }
    function closeConfirmLogoDeletionModal() {
        $("#logo-deletion-modal").modal("hide");
    }
    function loadConfirmLogoDeletionModal(logoName) {
        $.ajax({
            beforeSend: function () {
                VisibilityHelpers.loader(true);
            },
            type: "GET",
            url: "/ThemeManagement/_ConfirmDeleteLogo/"
        })
            .always(function () {
            VisibilityHelpers.loader(false);
        })
            .done(function (data) {
            $("#logo-deletion-modal-container").html(data);
            initConfirmLogoDeletionModal(logoName);
            $("#logo-deletion-modal").modal("show");
        })
            .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }
    var VariableControls;
    (function (VariableControls) {
        function initColour(selector) {
            $(selector).minicolors({
                "change": false,
                "theme": "bootstrap"
            });
        }
        VariableControls.initColour = initColour;
        function initLogo(selector) {
            $(selector).on("click", ".js-logo-item", function (e) {
                e.preventDefault();
                var $this = $(this);
                var logoName = $this.attr("data-logo");
                $(selector).children(".js-logo-item").removeClass("active");
                $this.addClass("active");
                $("#selected-logo").val(logoName);
                var img = $this.find("img").get(0);
                var width = img.clientWidth;
                $("#logo-size").val(width);
            });
        }
        VariableControls.initLogo = initLogo;
        function initFileUpload(selector) {
            $(selector).on("change", function () {
                var $this = $(this);
                var files = $this.get(0).files;
                var fileName = "Choose file";
                if (files != null) {
                    fileName = files[0].name;
                }
                $this.next(".custom-file-label").html(fileName);
            });
        }
        VariableControls.initFileUpload = initFileUpload;
        function initLogoUpload(fileSelector, btnSelector) {
            $(btnSelector).on("click", function (e) {
                e.preventDefault();
                uploadLogo(fileSelector, btnSelector);
            });
        }
        VariableControls.initLogoUpload = initLogoUpload;
        function initFont(selector) {
            $(selector).on("change", function (_e) {
                var $this = $(this);
                var id = $this.attr("data-id");
                $this.parent().children(".js-font-preview-" + id).css("font-family", this.value);
            });
        }
        VariableControls.initFont = initFont;
        function initSlider(selector) {
            $(selector).rangeslider({
                polyfill: false,
                onSlide: function (_position, value) {
                    var varId = this.$element.attr("data-id");
                    $("#size-preview-" + varId).val(value);
                    $("#control-preview-" + varId).css("border-radius", value);
                }
            });
        }
        VariableControls.initSlider = initSlider;
    })(VariableControls || (VariableControls = {}));
    var Navigate;
    (function (Navigate) {
        function toIndex() {
            Site.loadPartial("/ThemeManagement/");
        }
        Navigate.toIndex = toIndex;
        function toCreate() {
            Site.loadPartial("/ThemeManagement/CreateTheme/");
        }
        Navigate.toCreate = toCreate;
        function toImport() {
            Site.loadPartial("/ThemeManagement/ImportTheme/");
        }
        Navigate.toImport = toImport;
        function toEditTheme(themeId) {
            Site.loadPartial("/ThemeManagement/EditTheme/" + themeId);
        }
        Navigate.toEditTheme = toEditTheme;
        function toEditThemeVariables(themeId) {
            Site.loadPartial("/ThemeManagement/EditThemeVariables/" + themeId);
        }
        Navigate.toEditThemeVariables = toEditThemeVariables;
        function toLogoManagement() {
            Site.loadPartial("/ThemeManagement/LogoManagement");
        }
        Navigate.toLogoManagement = toLogoManagement;
    })(Navigate || (Navigate = {}));
})(ThemeManagement || (ThemeManagement = {}));
//# sourceMappingURL=theme-management.js.map