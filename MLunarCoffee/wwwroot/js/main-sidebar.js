var sideBarIsHide = false;
var ManuelSideBarIsHide = false;
var ManuelSideBarIsState = false;

$(window).resize(function () {
    if (ManuelSideBarIsHide == false) {
        if ($(window).width() <= 767) {
            if (!sideBarIsHide) {
                resizeSidebar("1");
                sideBarIsHide = true;
                $(".colhidden").addClass("displaynone");
                $('.ui-cal-resourceline').addClass("fixed-left");
                Cookies.set("MLUECH_Menu_SideBarIsHide", true);
            }
            else {
                $('.ui-cal-resourceline').removeClass("fixed-left");
            }
        }
        else {
            resizeSidebar("0");
            sideBarIsHide = false;
            $(".colhidden").removeClass("displaynone");
            $('.ui-cal-resourceline').removeClass("fixed-left");
            Cookies.set("MLUECH_Menu_SideBarIsHide", false);
        }
    }
});

function resizeSidebar(op) {   
    if (op == "1") {

        $(".ui.sidebar.left").addClass("very thin icon");
        $(".navslide").addClass("marginlefting");
        $(".sidebar.left span").addClass("displaynone");
        $(".sidebar .accordion").addClass("displaynone");
        $(".ui.dropdown.item.displaynone").addClass("displayblock");
        $($(".logo img")[0]).addClass("displaynone");
        $($(".logo img")[1]).removeClass("displaynone");
        $(".hiddenCollapse").addClass("displaynone");
        $("#fitlerMenu").addClass("displaynone");
        $('.ui-cal-resourceline').addClass("fixed-left");
        $('#MainCustomer_Header_Info').addClass("nav_profile")
    } else {

        $(".ui.sidebar.left").removeClass("very thin icon");
        $(".navslide").removeClass("marginlefting");
        $(".sidebar.left span").removeClass("displaynone");
        $(".sidebar .accordion").removeClass("displaynone");
        $(".ui.dropdown.item.displaynone").removeClass("displayblock");
        $($(".logo img")[1]).addClass("displaynone");
        $($(".logo img")[0]).removeClass("displaynone");
        $(".hiddenCollapse").removeClass("displaynone");
        $("#fitlerMenu").removeClass("displaynone");
        $('.ui-cal-resourceline').removeClass("fixed-left");
        $('#MainCustomer_Header_Info').removeClass("nav_profile")
    }
}

function Change_SideBarLeft_ShowHide(NameCookie) {    
    if (Cookies.get(NameCookie) != undefined) {
        let IsSideBarIsHide = Cookies.get(NameCookie);
        if (IsSideBarIsHide == "true") {
            resizeSidebar("1");
            ManuelSideBarIsState = true;
            $("#MessengerArea_List").css("width", "299px");
            $('.ui-cal-resourceline').css("left", "198px");
        } else {
            resizeSidebar("0");
            ManuelSideBarIsState = false;
            $("#MessengerArea_List").css("width", "300px");
            $('.ui-cal-resourceline').css("left", "328px");
        }
    }
    $(".sidebar.vertical.left,#MasterContainer").show();
}


$("#Master_Menu_Top").on("click", ".button-toggle-sidebar", function () {   
    let NameCookie = "MLUECH_Menu_SideBarIsHide";
    if (Cookies.get(NameCookie) != undefined) {
        let IsHideMenu = Cookies.get(NameCookie);
        if (IsHideMenu == "true") {
            Cookies.set(NameCookie, false);
        }
        else {
            Cookies.set(NameCookie, true);
        }
    }
    else {
        Cookies.set(NameCookie, true);
    }
    Change_SideBarLeft_ShowHide(NameCookie);
});