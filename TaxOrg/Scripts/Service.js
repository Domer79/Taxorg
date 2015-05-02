var query = {};

query.doQuery = function (url, options) {
    if (typeof (url) == "object") {
        options = url;
        url = options.url;
    }
    var async = options.async || true;
    var cache = options.cache || true;
    var contentType = options.contextType || "application/x-www-form-urlencoded; charset=UTF-8";
    var data = options.data || undefined;
    var method = options.method || options.type || "GET";
    var error = options.error;
    var success = options.success;
    var uri = url || options.url;
    var errorUrl = options.errorUrl || query.errorUrl;

    $.ajax(uri, {
        async: async,
        cache: cache,
        contentType: contentType,
        data: data,
        method: method,
        error: function(jqXhr, textStatus, errorThrown) {
            if (error == undefined || typeof (error) != "function") {
                showDialog(getErrorMessage(errorUrl, textStatus, errorThrown));
                return;
            }

            error(jqXhr, textStatus, errorThrown);
        },
        success: function(ajaxData, textStatus, jqXhr) {
            if (success != undefined && typeof (success) == "function") {
                success(ajaxData, textStatus, jqXhr);
            }
        }
    });
};

query.errorUrl = undefined;

var showDialog = function(message) {
    var messageDialog = $("#messageDialog");
    if (messageDialog.length == 0)
        return;

    $("#messageDialogTitle").text("Ошибка!");
    $("#messageDialogText").text(message);
    messageDialog.modal();
};

var getErrorMessage = function (url, textStatus, errorThrown) {
    var lastError = textStatus + ". " + errorThrown;
    $.ajax({
        url: url || "http://localhost/nourl/noquery",
        type: "POST",
        async: false,
        cache: false,
        success: function (data) {
            lastError = data;
        }
    });

    return lastError;
};