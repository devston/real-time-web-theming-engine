/*----------------------------------------------------------------------------*\
    
    Theme management
    ----------------

    All js related to the theme management area is found here.

    Contents
    --------

    1. Initialisation functions
    2. Create, update and delete functions
    3. Helper functions
    4. Navigation functions
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";
import "datatables.net";
import "datatables.net-bs4";
import "rangeslider.js";
import "@claviska/jquery-minicolors";
import { Site } from "Scripts/Utilities/site-core";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";
import { SiteLoader } from "Scripts/Components/site-loader";

/**
 * A module containing all the logic for the theme management area.
 */
export namespace ThemeManagement {

    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/

    /**
     * Initialise theme management area.
     */
    export function init(pageToLoad: string) {
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

    /**
     * Initialise the index page.
     */
    function initIndex() {
        $("#theme-table").on("click", ".js-edit-btn", function (e) {
            e.preventDefault();
            const themeId = <string>$(this).attr("data-id");
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

    /**
     * Initialise the create theme page.
     */
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

    /**
     * Initialise the import theme page.
     */
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

    /**
     * Initialise the edit theme page.
     */
    function initEditTheme() {
        const themeId = <string>$("#ThemeId").val();
        const $form = $("#theme-form");

        // Go back to the theme list.
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });

        $form.on("submit", function (e) {
            e.preventDefault();
            editTheme();
        });

        // Fire the form submit if the save theme button is clicked.
        $("#save-theme-btn").on("click", function (e) {
            e.preventDefault();
            $form.submit();
        });

        // Export the theme to a file.
        $("#export-theme-btn").on("click", function (e) {
            e.preventDefault();
            exportTheme(themeId);
        });

        // Delete the theme.
        $("#confirm-delete-theme-btn").on("click", function (e) {
            e.preventDefault();
            openConfirmThemeDeletionModal(themeId);
        });

        // Navigate to the theme variables page.
        $("#edit-theme-variables-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toEditThemeVariables(<string>themeId);
        });
    }

    /**
     * Initialise the confirm theme deletion modal.
     * @param themeId
     */
    function initConfirmThemeDeletionModal(themeId: string | number) {
        $("#theme-deletion-form").on("submit", function (e) {
            e.preventDefault();
            deleteTheme(themeId);
        });
    }

    /**
     * Initialise the edit theme variables page.
     */
    function initEditVariables() {
        // Go back to the theme list.
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

    /**
     * Initialise the logo management page.
     */
    function initLogoManagement() {
        // Go back to the theme list.
        $("#back-to-theme-list-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });

        // Init delete logo btn events.
        $("#logo-table").on("click", ".js-delete-logo-btn", function (e) {
            e.preventDefault();
            const logoName = <string>$(this).attr("data-logo");
            openConfirmLogoDeletionModal(logoName);
        });
    }

    /**
     * Initialise the confirm logo deletion modal.
     * @param logoName
     */
    function initConfirmLogoDeletionModal(logoName: string) {
        $("#logo-deletion-form").on("submit", function (e) {
            e.preventDefault();
            deleteLogo(logoName);
        });
    }

    /*------------------------------------------------------------------------*\
        2. Create, update and delete functions
    \*------------------------------------------------------------------------*/

    /**
     * Create a theme.
     */
    function createTheme() {
        const formId = "#theme-form";
        const url = "/ThemeManagement/SaveNewTheme";

        saveTheme(url, formId);
    }

    /**
     * Save changes to a theme.
     */
    function editTheme() {
        const formId = "#theme-form";
        const url = "/ThemeManagement/SaveThemeChanges";

        saveTheme(url, formId);
    }

    /**
     * Save a theme via ajax.
     * @param url The url to send the theme too.
     * @param formId The form id to serialize the theme data from.
     */
    function saveTheme(url: string, formId: string) {
        // jQuery object is used multiple times so cache it in a variable.
        const $form = $(formId);

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

    /**
     * Import a theme from a file.
     */
    function importTheme() {
        // jQuery object is used multiple times so cache it in a variable.
        const formId = "#theme-form";
        const $form = $(formId);

        let formData = new FormData();
        const fileInput = <HTMLInputElement>$form.find("#FileToUse").get(0);

        // Check if any files have been uploaded.
        if (fileInput.files == null) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Select a theme to upload", true);
            return;
        }

        // Append the first file to FormData object.
        formData.append("FileToUse", fileInput.files[0]);

        // Append the theme name to FormData object.
        formData.append("Name", <string>$form.find("#Name").val());

        // Append the anti forgery token to FormData object.
        formData.append("__RequestVerificationToken", <string>$form.find("input[name=__RequestVerificationToken]").val());

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

    /**
     * Save theme variable changes.
     */
    function saveThemeVariables() {
        // jQuery object is used multiple times so cache it in a variable.
        const formId = "#theme-form";
        const $form = $(formId);

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

    /**
     * Delete a theme.
     */
    function deleteTheme(themeId: string | number) {
        const formId = "#theme-form";
        const token = <string>$("#theme-deletion-form").find("input[name=__RequestVerificationToken]").val();
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

    /**
     * Export the theme to a file.
     */
    function exportTheme(themeId: string | number) {
        const formId = "#theme-form";

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
            window.location.href = `/ThemeManagement/ExportTheme?id=${data.themeId}`;
        })
        .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }

    /**
     * Upload the logo to the server.
     * @param fileSelector
     */
    function uploadLogo(fileSelector: string, btnSelector: string) {
        var files = (<HTMLInputElement>$(fileSelector).get(0)).files;

        // Check if there is a file.
        if (files == null || files.length == 0) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Select an image.", true);
            return;
        }

        // Check logo is not larger than 1mb.
        else if (files[0].size > 1048576) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Image must be smaller than 1MB.", true);
            return;
        }

        // Create a fake form to put logo inside so ajax can serialise.
        var formData = new FormData();
        var totalFiles = files.length;

        for (var i = 0; i < totalFiles; i++) {
            var file = files[i];
            formData.append("upload-logo", file);
        }

        // Append the anti forgery token to FormData object.
        formData.append("__RequestVerificationToken", <string>$("#theme-form").find("input[name=__RequestVerificationToken]").val());

        // jQuery object is used multiple times so store it in a variable.
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
            .done(function (data: any) {
                // Inform the user the logo has uploaded successfully.
                if (data.warning) {
                    VisibilityHelpers.alert("warning", data.message, true);
                }
                else {
                    VisibilityHelpers.alert("success", data.message, true);
                }

                // Render logo in the necessary mark up.
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

    /**
     * Delete a logo.
     */
    function deleteLogo(logoName: string) {
        const containerId = "#logo-management-container";
        const token = <string>$("#logo-deletion-form").find("input[name=__RequestVerificationToken]").val();
        closeConfirmLogoDeletionModal();

        // jQuery object is used multiple times so store it in a variable.
        const $deleteBtns = $(containerId).find(".js-delete-logo-btn");

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

    /*------------------------------------------------------------------------*\
        3. Helper functions
    \*------------------------------------------------------------------------*/

    /**
     * Show the theme selection modal.
     * @param containerId The container id to place the theme selection modal inside.
     * @param themeIdInput The selected theme id input id.
     * @param themeNameInput The selected theme name input id.
     */
    function showThemeSelectionModal(containerId: string, themeIdInput: string, themeNameInput: string) {
        // Empty the container so there isn't multiple modals inside.
        $(containerId).empty();

        // Create the modal and table.
        initThemeTable(containerId);

        // Bind the click event.
        $(containerId).find("table > tbody").on("click", "tr", function (this: any) {
            var $this = $(this);
            $(themeIdInput).val(<string>$this.attr("data-id"));
            $(themeNameInput).val($this.children().first().text());
            $this.find(".modal").modal("hide");
        });
    }

    /**
     * Initialise the theme table.
     * @param containerId
     */
    function initThemeTable(containerId: string) {
        $(containerId).find("table").DataTable();
    }

    /**
     * Open the confirm theme deletion modal.
     * @param themeId
     */
    function openConfirmThemeDeletionModal(themeId: string | number) {
        if ($("#theme-deletion-modal-container").is(":empty")) {
            loadConfirmThemeDeletionModal(themeId);
        }
        else {
            $("#theme-deletion-modal").modal("show");
        }
    }

    /**
     * Close the confirm theme deletion modal.
     */
    function closeConfirmThemeDeletionModal() {
        $("#theme-deletion-modal").modal("hide");
    }

    /**
     * Load the confirm theme deletion modal.
     * @param themeId
     */
    function loadConfirmThemeDeletionModal(themeId: string | number) {
        $.ajax({
            beforeSend: function () {
                VisibilityHelpers.loader(true);
            },
            type: "GET",
            url: `/ThemeManagement/_ConfirmDeleteTheme/${themeId}`
        })
            .always(function () {
                VisibilityHelpers.loader(false);
            })
            .done(function (data: any) {
                $("#theme-deletion-modal-container").html(data);
                initConfirmThemeDeletionModal(themeId);
                openConfirmThemeDeletionModal(themeId);
            })
            .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
    }

    /**
     * Open the confirm logo deletion modal.
     * @param logoName
     */
    function openConfirmLogoDeletionModal(logoName: string) {
        // Empty the container if it is now empty.
        if (!$("#logo-deletion-modal-container").is(":empty")) {
            $("#logo-deletion-modal-container").empty();
        }

        loadConfirmLogoDeletionModal(logoName);
    }

    /**
     * Close the confirm logo deletion modal.
     */
    function closeConfirmLogoDeletionModal() {
        $("#logo-deletion-modal").modal("hide");
    }

    /**
     * Load the confirm logo deletion modal.
     * @param logoName
     */
    function loadConfirmLogoDeletionModal(logoName: string) {
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
        .done(function (data: any) {
            $("#logo-deletion-modal-container").html(data);
            initConfirmLogoDeletionModal(logoName);
            $("#logo-deletion-modal").modal("show");
        })
        .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }

    /*------------------------------------------------------------------------*\
        4. Theme control functions
    \*------------------------------------------------------------------------*/

    /**
     * Contains functionality for the theme variable controls.
     */
    namespace VariableControls {
        /**
         * Initialise the colour control.
         * @param selector Input selector.
         */
        export function initColour(selector: string) {
            $(selector).minicolors({
                "change": false,
                "theme": "bootstrap"
            });
        }

        /**
         * Initialise the logo control.
         * @param selector Selector for the logo list.
         */
        export function initLogo(selector: string) {
            $(selector).on("click", ".js-logo-item", function (e) {
                e.preventDefault();
                const $this = $(this);
                const logoName = <string>$this.attr("data-logo");
                $(selector).children(".js-logo-item").removeClass("active");
                $this.addClass("active");
                $("#selected-logo").val(logoName);

                // Get image width and update logo size.
                var img = $this.find("img").get(0);
                var width = img.clientWidth;
                $("#logo-size").val(width);
            });
        }

        /**
         * Initialise file upload input.
         * @param selector Selector for file upload.
         */
        export function initFileUpload(selector: string) {
            $(selector).on("change", function () {
                const $this = <JQuery<HTMLInputElement>>$(this);
                const files = $this.get(0).files;
                let fileName = "Choose file";

                if (files != null) {
                    fileName = files[0].name;
                }

                $this.next(".custom-file-label").html(fileName);
            });
        }

        /**
         * Initialise the logo upload control.
         * @param selector
         */
        export function initLogoUpload(fileSelector: string, btnSelector: string) {
            $(btnSelector).on("click", function (e) {
                e.preventDefault();
                uploadLogo(fileSelector, btnSelector);
            });
        }

        /**
         * Initialise the font control.
         * @param selector
         */
        export function initFont(selector: string) {
            $(selector).on("change", function (this: HTMLInputElement, _e) {
                const $this = $(this);
                const id = $this.attr("data-id");
                $this.parent().children(`.js-font-preview-${id}`).css("font-family", this.value);
            });
        }

        /**
         * Initialise the slider control.
         * @param selector
         */
        export function initSlider(selector: string) {
            $(selector).rangeslider({
                polyfill: false,
                onSlide: function (_position: any, value: number) {
                    const varId = this.$element.attr("data-id");
                    $(`#size-preview-${varId}`).val(value);
                    $(`#control-preview-${varId}`).css("border-radius", value);
                }
            });
        }
    }

    /*------------------------------------------------------------------------*\
        5. Navigation functions
    \*------------------------------------------------------------------------*/

    /**
     * Contains navigation based functionality for theme management.
     */
    namespace Navigate {
        /**
         * Navigate to the theme index page.
         */
        export function toIndex() {
            Site.loadPartial("/ThemeManagement/");
        }

        /**
         * Navigate to the create theme page.
         */
        export function toCreate() {
            Site.loadPartial("/ThemeManagement/CreateTheme/");
        }

        /**
         * Navigate to the import theme page.
         */
        export function toImport() {
            Site.loadPartial("/ThemeManagement/ImportTheme/");
        }

        /**
         * Navigate to the edit theme page.
         * @param themeId
         */
        export function toEditTheme(themeId: string) {
            Site.loadPartial(`/ThemeManagement/EditTheme/${themeId}`);
        }

        /**
         * Navigate to the edit theme variables page.
         * @param themeId
         */
        export function toEditThemeVariables(themeId: string) {
            Site.loadPartial(`/ThemeManagement/EditThemeVariables/${themeId}`);
        }

        /**
         * Navigate to the logo management page.
         */
        export function toLogoManagement() {
            Site.loadPartial("/ThemeManagement/LogoManagement");
        }
    }
}