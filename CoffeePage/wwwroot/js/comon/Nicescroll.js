function DeclareNiceScroll(id) {
    $("#" + id).niceScroll({
        cursorwidth: 7,
        cursoropacitymin: 0.4,
        cursorcolor: '#6e8cb6',
        cursorborder: 'none',
        scrollspeed: 40,
        mousescrollstep: 55,
        cursorborderradius: 4,
        cursorheight: 7,
        autohidemode: true,
        horizrailenabled: false,
    });

}
function DeclareNiceScroll_Horizontal(id) {
    $("#" + id).niceScroll({
        //cursorwidth: 7,
        //cursorminheight: 7,
        //cursoropacitymin: 0.4,
        //cursorcolor: '#6e8cb6',
        //cursorborder: 'none',
        //scrollspeed: 40,
        //mousescrollstep: 55,
        //cursorborderradius: 4,
        //autohidemode: true,
        //horizrailenabled: true,
        //smoothscroll: true,
        cursorwidth: 7,
        cursoropacitymin: 0.4,
        cursorcolor: '#6e8cb6',
        cursorborder: 'none',
        scrollspeed: 40,
        mousescrollstep: 55,
        cursorborderradius: 4,
        cursorheight: 7,
        autohidemode: true,
        horizrailenabled: true,

    });

}
function RefreshNiceScroll() {
  $(".divScroll").getNiceScroll().resize();
}

