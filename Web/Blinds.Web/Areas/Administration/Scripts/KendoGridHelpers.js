function sendAntiForgery() {
    return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() }
}

function sync_handler(e) {
    this.read();
}

function onUpload(e) {
}

function onSuccess(result) {
    if (result) {
        $('#HasImage').val(true);
    } else {
        $('#HasImage').val(false);
    }

    $('#HasImage').trigger('change');
}