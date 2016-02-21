var DetailsViewModel = {
    showBlinds: null,
    blindsTable: null,

    init: function () {
        var self = DetailsViewModel;
        self.initSelectors();
        self.initEvents();
    },

    initSelectors: function () {
        var self = DetailsViewModel;

        self.showBlindsBtn = $('#showBlinds');
        self.container = $('#blindsTable');
        self.emailBtn = $('#emailBtn');
    },

    initEvents: function () {
        var self = DetailsViewModel;

        self.showBlindsBtn.on('click', function () {
            self.container.slideToggle(function () {
                $(this).is(':visible') ? self.showBlindsBtn.text(JsConstants.OrdersDetailsHideBlindsButton) : self.showBlindsBtn.text(JsConstants.OrdersDetailsShowBlindsButton);
            });
        });

        self.emailBtn.on('click', function () {
            self.showMessage('Поръчката е изпратена на Вашият e-mail');
        })
    },

    showMessage: function (text) {
        var $modalView = $('#messageModal'),
            textMessage = $('#textMessage');

        textMessage.text(text);
        $modalView.modal();
    }
};

$(document).ready(function () {
    DetailsViewModel.init();
});