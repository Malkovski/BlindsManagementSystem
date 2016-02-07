function sendAntiForgery() {
    return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() }
}

function sync_handler(e) {
    this.read();
}

function onUpload(e) {
}

function onError(error) {
    var popupNotification = $("#errorNotification").data("kendoNotification");
    popupNotification.show(error.errors, "error");
}


function onSuccess(result) {
    if (result) {
        $('#HasImage').val(true);
    } else {
        $('#HasImage').val(false);
    }

    $('#HasImage').trigger('change');
}