function showDetails(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    window.location.href = $('#detailsUrl').val() + "/" + dataItem.Id;
}