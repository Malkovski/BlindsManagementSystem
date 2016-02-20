var ProductsViewModel;

ProductsViewModel = {
    galleryItem: '.galleryItem',
    imageShiftLeft: '.imageShiftLeft',
    imageShiftRight: '.imageShiftRight',

    init: function () {
        var self = ProductsViewModel;

        var count = $('div[data-action="picturesCount"]').attr('data-pictures-count');
        var container = $('#scroller');
        container.width = count * 120;
        self.scrollImages(count);

        $(self.galleryItem).on('click', function () {
            self.previewImage($(this).attr('src'));
        })
    },

    previewImage: function (src) {
        var $modalView = $('#previewImage'),
                $img = $modalView.find('img');

        $img.attr('src', src);
        $modalView.modal()
    },


    scrollImages: function (count) {
        var scroller = $('#scroller div.innerScrollArea');
        var scrollerContent = scroller.children('ul');
        scrollerContent.children().clone().appendTo(scrollerContent);
        var curX = 0;
        scrollerContent.children().each(function(){
            var $this = $(this);
            $this.css('left', curX);
            curX += $this.outerWidth(true);
        });
        var fullW = curX / 2;
        var viewportW = scroller.width();

        // Scrolling speed management
        var controller = {curSpeed:0, fullSpeed:1};
        var $controller = $(controller);
        var tweenToNewSpeed = function(newSpeed, duration)
        {
            if (duration === undefined)
                duration = 600;
            $controller.stop(true).animate({curSpeed:newSpeed}, duration);
        };

        // Pause on hover
        scroller.hover(function(){
            tweenToNewSpeed(0);
        }, function(){
            tweenToNewSpeed(controller.fullSpeed);
        });

        // Scrolling management; start the automatical scrolling
        var doScroll = function()
        {
            var curX = scroller.scrollLeft();
            var newX = curX + controller.curSpeed;
            if (newX > fullW*2 - viewportW)
                newX -= fullW;
            scroller.scrollLeft(newX);
        };
        setInterval(doScroll, 20);
        tweenToNewSpeed(controller.fullSpeed);
    }
};

$(window).load(function () {
    ProductsViewModel.init();
})