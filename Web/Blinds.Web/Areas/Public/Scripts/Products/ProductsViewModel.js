var ProductsViewModel;

ProductsViewModel = {
    galleryItem: '.galleryItem',
    imageShiftLeft: '.imageShiftLeft',
    imageShiftRight: '.imageShiftRight',

    init: function () {
        var self = ProductsViewModel;

        self.scrollImages();

        $(self.galleryItem).on('click', function () {
            self.previewImage($(this).attr('src'));
        })
    },

    previewImage: function (src) {
        var $modalView = $('#previewImage'),
            $img = $modalView.find('img');

        $img.attr('src', src);
        $modalView.modal();
    },

    scrollImages: function () {
        var scroller,
            scrollerContent,
            curX,
            fullW,
            viewportW,
            controller,
            $controller,
            tweenToNewSpeed,
            doScroll,
            newX,
            currentWidth,
            width;
        
        scroller = $('#scroller div.innerScrollArea');
        scroller.removeClass('hidden');
        currentWidth = scroller.find('li').length * 120;
        scrollerContent = scroller.children('ul');
        scrollerContent.children().clone().appendTo(scrollerContent);

        curX = 0;
        scrollerContent.children().each(function(){
            var $this = $(this);
            $this.css('left', curX);
            curX += $this.outerWidth(true);
        });

        fullW = curX / 2;


        if (currentWidth < 1140) {
            scroller.width(currentWidth);
            scroller.css('left', (1140 - currentWidth) / 2)
        }

        viewportW = scroller.width();

        // Scrolling speed management
        controller = {
            curSpeed: 0,
            fullSpeed: 1
        };

        $controller = $(controller);

        tweenToNewSpeed = function(newSpeed, duration)
        {
            if (duration === undefined) {
                duration = 600;
            }
                
            $controller.stop(true).animate({curSpeed:newSpeed}, duration);
        };

        // Pause on hover
        scroller.hover(function(){
            tweenToNewSpeed(0);
        }, function(){
            tweenToNewSpeed(controller.fullSpeed);
        });

        // Scrolling management; start the automatical scrolling
        doScroll = function()
        {
            curX = scroller.scrollLeft();
            newX = curX + controller.curSpeed;
            if (newX > fullW * 2 - viewportW) {
                newX -= fullW;
            }
                
            scroller.scrollLeft(newX);
        };

        setInterval(doScroll, 20);
        tweenToNewSpeed(controller.fullSpeed);
    }
};

$(window).load(function () {
    ProductsViewModel.init();
})