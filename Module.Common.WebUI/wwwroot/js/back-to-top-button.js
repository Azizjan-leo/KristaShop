$(function() {
    $("body").toTopButton({

        // path to icons
        imagePath: '/common/lib/floating-back-top-button/img/icons',

        // arrow, arrow-circle, caret, caret-circle, circle, circle-o, arrow-l, drop, rise, top
        // or your own SVG icon
        arrowType: 'arrow',

        // 'w' = white
        // 'b' = black
        iconColor: 'w',

        // icon shadow
        // from 1 to 16
        iconShadow: 0,
        
        // button shadow from 1-5
        btnShadow: 1,

        // auto hides when the screen size is smaller than this value
        mobileHide: 0,


        backgroundColor: '#000000'
    });
});
