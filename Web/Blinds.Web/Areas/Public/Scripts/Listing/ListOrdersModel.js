var ListOrdersModel = {
    init: function () {
        var self = ListOrdersModel;
        self.initSelectors();
        self.initEvents();
    },

    initSelectors: function () {
        var self = ListOrdersModel;

        self.page = $('#page');
        self.sortByNumber = $('#number');
        self.sortByColor = $('#color');
        self.sortByType = $('#blindType');
        self.sortByBlinds = $('#blindCount');
        self.ordersContent = $('#ordersContent');
    },

    initEvents: function () {
        var self = ListOrdersModel;

        self.sortByNumber.on('click', function () {
            var sortType = self.sortByNumber.attr('data-direction');
            var sortBy = self.sortByNumber.attr('data-name');
            var id = self.page.attr('data-id');

            if (self.sortType == 'desc') {
                self.sortType = 'asc'
            }
            else {
                self.sortType = 'desc'
            }

            $.get("SortOrders/", { sortBy: sortBy, sortType: sortType, id: id }, function (data) {
                $.each(data, function (index, element) {
                    var row = "<tr><td class='col-md-2'>" +
                        "<a href='Details/Index/'" + element.CurrentPage + ">" + element.OrderNumber.Split('_')[0] + "</a>"
                    "</td><td class='col-md-4'>" + "<span>" + element.BlindTypeName + "</span>" +
                    "</td><td class='col-md-3'>" + "<span>" + element.ColorName + "</span>" +
                    "</td><td class='col-md-2'>" + "<span>" + element.BlindsCount + "</span>" +
                    "</td><td class='col-md-1'>" + "<a class='btn btn-sm btn-default' href='Details/Index/'" + element.CurrentPage + ">" + Преглед + "</a>" +
                    "</td></tr>"
                    self.ordersContent.append(row);
                });
            })
        })
    },

}

$(document).ready(function () {
    ListOrdersModel.init();
});