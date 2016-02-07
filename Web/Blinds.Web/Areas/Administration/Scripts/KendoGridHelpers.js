function sendAntiForgery() {
    return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() }
}

function sync_handler(e) {
    this.read();
}