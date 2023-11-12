var Notier = Notier || {};
(function () {
    Notier.Second = 20;
    Notier.Interval = null;
    Notier.ElmID = '';
    Notier.BranchID = '';

    Notier.Defaults = {
        Title: "",
        Content: "",
        Link: '', //Link chuyển hướng khi bấm vào noti
        Time: null, //thời gian hiện noti -- 10:05:03
        Date: new Date(), //ngày hiện noti -- 10/04/2020
        Receivers: [], //[{Id:'',Name:''}] | null = All AND Send = 1
        Sender: '',
        Branchs: [] //[{Id:'',Name:''}] | null = All AND Send = 1
    }
    //nhận nhiệm vụ Kiểm tra
    function NotierSetup(options) {
        this.$options = null;
        var me = this;
        var _SendNoti = function () {
            if (me.$options.Receivers && me.$options.Receivers.length > 0) {
                //Gữi
                $.ajax({
                    url: "/Api/Notier",
                    dataType: "json",
                    type: "POST",
                    data: JSON.stringify(me.$options),
                    contentType: 'application/json; charset=utf-8',
                    async: true,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    },
                    success: function (result) {
                    }
                });
            }
        }
        //var _processInput = function (options) {
        //    if (options.Title) 
        //}
        this.$options = options;//_processInput(options);
        _SendNoti();
    }
    function NotierShow(options) {
        this.options = null;
        var me = this;
        var _CreateNotify = function () {
            Lobibox.notify("success", {
                size: "normal",
                rounded: false,
                delayIndicator: true,
                msg: me.$options.Content,
                //icon: null,
                closeOnClick: function () {
                    window.open(me.$options.Link, '_blank');
                },
                title: me.$options.Title,
                soundPath: '/assests/dist/plugins/lobibox/sounds/',   // The folder path where sounds are located
                soundExt: '.ogg',           // Default extension for all sounds
                sound: me.$options.Sound,
                showClass: "fadeInDown",
                hideClass: "zoomOut",
                delay: 5000

            });
        }

        this.$options = options;
        _CreateNotify();
    }
    function NotierCheckShow(options) {
        //type 1 kiểm tra từ cookie-> không insert 
        var optionsRe = [];

        let datenow = new Date();
        let Hours = datenow.getHours();
        let Minutes = datenow.getMinutes();
        for (i = 0; i < options.length; i++) {
            let option = options[i];

            let time = option.Time.split(':');
            let hr = Number(time[0]) - Number(Hours);
            let mt = Number(time[1]) - Number(Minutes);
            let numSecond = (hr * 3600) + (mt * 60);
            if (numSecond < 0) {
            } else {
                if (numSecond < Notier.Second) {
                    new NotierShow(option);
                } else {
                    optionsRe.push(option);
                }
            }
        }

        return optionsRe;
    }
    function NotierNew(option) {
        if (option.Time && option.Time != '') {
            let optionsCookie = JSON.parse(_getCookie('listNoti'));
            let options = [];
            if (optionsCookie.data.length > 0) {
                options = optionsCookie.data;
            }
             
            options.push(option);
            let d = { 'empID': Notier.ElmID, 'data': options };
            _setCookie('listNoti', JSON.stringify(d), 1);
        } else {
            new NotierShow(option);
        }
    }
    Notier.notify = function (type, option) {
        var _Notier_Registration_Show = function (option) {
            //kiểm tra Receivers// null show tất cả người nhận
            if (option.Receivers && option.Receivers.length > 0) {
                var _receiver = option.Receivers.filter(receiver => receiver.ID == Notier.ElmID);
                if (_receiver.length > 0) {
                    //kiểm tra Branchs// null show tất cả người nhận ở chi nhánh
                    if (option.Branchs && option.Branchs.length > 0) {
                        var _branch = option.Branchs.filter(branch => branch.ID == Notier.BranchID);
                        if (_branch.length > 0) {
                            NotierNew(option);
                        }
                    } else {
                        NotierNew(option);
                    }
                }
            } else {
                //kiểm tra Branchs// null show tất cả người nhận ở chi nhánh
                if (option.Branchs && option.Branchs.lenght > 0) {
                    var _branch = option.Branchs.filter(branch => branch.ID == this.BranchID);
                    if (_branch.length > 0) {
                        NotierNew(option);
                    }
                } else {
                    NotierNew(option);
                }
            }
        }
        if (type == 1) {
            new NotierSetup(option);
        } else {
            _Notier_Registration_Show(option);
        }
    }
    Notier.Init = function (ElmID, BranchID) {
        this.ElmID = ElmID;
        this.BranchID = BranchID;

        //nếu chưa có cookie thì load

        //reset 
        if (this.Interval) {
            clearInterval(this.Interval);
        }
        //get cookie để hiện noti
        this.Interval = setInterval(function () {
            LoadNoti();
        }, this.Second * 1000);
        LoadNoti();
    }
    var LoadNoti = function () {
        let d = null;
        if (_checkCookie('listNoti') && JSON.parse(_getCookie('listNoti')).empID == Notier.ElmID) {
            let listNotiSave = JSON.parse(_getCookie('listNoti')).data;
            //show những cái đến hạn-- trả lại những cái chưa đến hạng báo
            let options = new NotierCheckShow(listNotiSave);
            //thay thế cookie bằng những cái chưa báo
            d = { 'empID': Notier.ElmID, 'data': options };
            _setCookie('listNoti', JSON.stringify(d), 1);
        } else {
            $.ajax({
                url: "/Api/Notier/GetNoti?ID=" + Notier.ElmID,
                dataType: "json",
                type: "GET",
                // data: JSON.stringify(me.ElmID),
                contentType: 'application/json; charset=utf-8',
                async: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
                success: function (result) {
                    if (result != "") {
                        let data = result;
                        let options = new NotierCheckShow(JSON.parse(data));
                        d = { 'empID': Notier.ElmID, 'data': options }
                        _setCookie('listNoti', JSON.stringify(d), 1);
                    }
                }
            });
            //lấy data từ database
        }
    }
    var _checkCookie = function (name) {
        var Cookie = getCookie(name);
        if (Cookie != "" && Cookie != null) {
            return true;
        } else {
            return false;
        }
    }
    var _getCookie = function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
    var _setCookie = function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }



})();