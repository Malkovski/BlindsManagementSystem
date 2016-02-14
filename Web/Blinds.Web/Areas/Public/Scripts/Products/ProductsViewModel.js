var ProductsViewModel;

ProductsViewModel = {
    galleryItem: '.galleryItem',

    init: function () {
        var self = ProductsViewModel;

        $(self.galleryItem).on('click', function () {
            self.previewImage($(this).attr('src'));
        })
    },

    previewImage: function (src) {
        var $modalView = $('#previewImage'),
                $img = $modalView.find('img');

        $img.attr('src', src);
        $modalView.modal()
    }
};

$(document).ready(function () {
    ProductsViewModel.init();
})