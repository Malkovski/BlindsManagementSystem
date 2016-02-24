var OrdersViewModel, OrdersModel;

function showDetails(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    window.location.href = $('#detailsUrl').val() + "/" + dataItem.Id;
}

OrdersViewModel = {
    init: function (model) {
        var self = OrdersViewModel;

        self.model = model;
        self.initSelectors();
        self.initValidation();
        self.initEvents();
    },

    initSelectors: function () {
        var self = OrdersViewModel;

        self.form = $('#orderForm');
        self.addButton = $('#addBlindRow');
        self.removeButton = $('#removeBlindRow');
        self.saveButton = $('#saveBtn');
        self.sizesContainer = $('#sizesContainer');
        self.blindType = $('#BlindTypeId');
        self.railColors = $('#RailColor');
        self.fabricAndLamelColors = $('#FabricAndLamelColor');
        self.fabricAndLamelMaterials = $('#FabricAndLamelMaterial');
        self.newRowUrl = $('#newRowUrl');
        self.getRailColorsUrl = $('#getRailColors');
        self.getFabricAndLamelColorsUrl = $('#getFabricAndLamelColors');
        self.getFabricAndLamelMaterialsUrl = $('#getFabricAndLamelMaterials');
        self.detailsUrl = $('#detailsUrl');
        self.agreedTerms = $('#HasAgreedTerms');
    },

    initValidation: function () {
        var self = OrdersViewModel;

        jQuery.extend(jQuery.validator.messages, {
            required: "Полето е задължително!",
            min: jQuery.validator.format("Стойностт по-малка от {0} е невалидна!")
        });

        self.validator = $(self.form).validate({
            rules: {
                Number: { required: true },
                BlindTypeId: { required: true },
                RailColor: { required: true },
                FabricAndLamelColor: { required: true },
                FabricAndLamelMaterial: { required: true },
                InstalationType: { required: true },
                SizesRow: { required: true }
            },
            errorElement: 'div',
            errorClass: 'validation-error'
        });

        self.addCommonValidation();
    },

    addCommonValidation: function () {
        $('[name^=Control]').each(function () {
            $(this).rules('add', {
                required: true
            });
        });

        $('[name^=Width]').each(function () {
            $(this).rules('add', {
                required: true,
                min: 100
            });
        });

        $('[name^=Height]').each(function () {
            $(this).rules('add', {
                required: true,
                min: 100
            });
        });

        $('[name^=BlindsCount]').each(function () {
            $(this).rules('add', {
                required: true,
                min: 1
            });
        });

        $('#HasAgreedTerms').rules('add', {
            required: true
        });
    },

    initEvents: function () {
        var self = OrdersViewModel,
            url,
            params,
            onSuccess;

        self.addButton.on('click', self.addNewRow);

        self.removeButton.on('click', self.removeLastRow);

        self.blindType.on('change', function () {
            self.loadRailColors($(this).val());
            self.loadFabricAndLamelColors($(this).val());
        });

        self.fabricAndLamelColors.on('change', function () {
            self.loadFabricAndLamelsMaterials($(this).val(), self.blindType.val());;
        });

        self.saveButton.on('click', self.save);
    },

    addNewRow: function () {
        var self = OrdersViewModel;

        url = self.newRowUrl.val();

        params = {
            async: true,
            url: url
        };

        onSuccess = function (partialView) {
            self.sizesContainer.append(partialView);
            self.addCommonValidation();
        };

        self.model.addNewRow(params, onSuccess);
    },

    removeLastRow: function () {
        var self = OrdersViewModel;

        self.sizesContainer.children('.sizesRow').last().remove();
    },

    loadRailColors: function (blindTypeId) {
        var self = OrdersViewModel;

        url = self.getRailColorsUrl.val();

        if (blindTypeId) {
            self.model.get(url, {
                blindTypeId: blindTypeId
            }, function (response) {
                self.railColors.empty();
                $.each(response, function (index, element) {
                    self.railColors.append('<option value="' + element.Value + '">' + element.Text + '</option>');
                });
            });
        }
    },

    loadFabricAndLamelColors: function (blindTypeId) {
        var self = OrdersViewModel;

        url = self.getFabricAndLamelColorsUrl.val();

        if (blindTypeId) {
            self.model.get(url, {
                blindTypeId: blindTypeId
            }, function (response) {
                self.fabricAndLamelColors.empty();
                $.each(response, function (index, element) {
                    self.fabricAndLamelColors.append('<option value="' + element.Value + '">' + element.Text + '</option>');
                });

                self.fabricAndLamelColors.trigger('change');
            });
        }
    },

    loadFabricAndLamelsMaterials: function (colorId, blindTypeId) {
        var self = OrdersViewModel;

        url = self.getFabricAndLamelMaterialsUrl.val();

        if (blindTypeId && colorId) {
            self.model.get(url, {
                colorId: colorId, blindTypeId: blindTypeId
            }, function (response) {
                self.fabricAndLamelMaterials.empty();
                $.each(response, function (index, element) {
                    self.fabricAndLamelMaterials.append('<option value="' + element.Value + '">' + element.Text + '</option>');
                });
            });
        }
    },

    save: function () {
        var self = OrdersViewModel,
            url = self.form.data('save-url'),
            blind,
            blinds = [],
            widths,
            heights,
            controls,
            counts,
            onSuccess,
        onFail;

        if ($(self.form).valid()) {
            heights = _.map($('.heights'), function (item) {
                return $(item).val()
            });

            widths = _.map($('.widths'), function (item) {
                return $(item).val()
            });

            controls = _.map($('.controls'), function (item) {
                return $(item).val()
            });

            counts = _.map($('.counts'), function (item) {
                return $(item).val()
            });

            for (var i = 0; i < heights.length; i++) {
                blind = {
                    Width: widths[i],
                    Height: heights[i],
                    Control: controls[i],
                    Count: counts[i]
                }

                blinds.push(blind);
            }

            var params = {
                OrderNumber: $('#Number').val(),
                BlindTypeId: $('#BlindTypeId').val(),
                RailColorId: $('#RailColor').val(),
                FabricAndLamelColorId: $('#FabricAndLamelColor').val(),
                FabricAndLamelMaterialId: $('#FabricAndLamelMaterial').val(),
                InstalationTypeId: $('#InstalationType').val(),
                Blinds: blinds
            };

            onSuccess = function (result) {
                if (result && result.Id) {
                    window.location.href = self.detailsUrl.val() + '/' + result.Id;
                } else {
                    self.showError(result);
                }
            }

            self.model.save(url, params, onSuccess);
        }
    },

    showError: function (text) {
        var $modalView = $('#errorModal'),
            textMessage = $('#textMessage');

        textMessage.text(text);
        $modalView.modal();
    }
};

OrdersModel = {
    addNewRow: function (params, onSuccess) {
        $.ajax(params).success(onSuccess);
    },

    get: function (url, params, onSuccessFunc) {
        $.ajax({ url: url, type: 'GET', data: params })
            .done(onSuccessFunc)
            .fail(function () {
                alert('Грешка');
            });
    },

    save: function (url, params, onSuccessFunc) {
        $.ajax({ url: url, data: JSON.stringify(params), type: 'POST', contentType: 'application/json', dataType: 'json' })
            .done(onSuccessFunc)
            .fail(function () {
                alert('Грешка');
            });
    },
};

$(document).ready(function () {
    OrdersViewModel.init(OrdersModel);
});


