//#region Guide User
function FistSlideGuide() {
    document.getElementById("divFistSlideGuide").innerHTML = '';
    $("#divFistSlideGuide").load("/Guide/pageFirstGuideSlide" + "?ver=" + versionofWebApplication);
    $('#divFistSlideGuide').modal('show');
}
function DocumentGuideUser() {
    var win = window.open("/AuthoriseSetting/DocumentSetting/ReadDocument" + "?ver=" + versionofWebApplication, '_blank');
    win.focus();
}
function RunGuideEndUser(_guide) {
    _GuideForCurrentPage = (_guide != '') ? _guide : _GuideForCurrentPage;
    if (_GuideForCurrentPage == '[]') $('#btnGuideEndUser').hide();
    else {
        $('#btnGuideEndUser').show();
        let enjoyhint_instance = new EnjoyHint({
        });
        let enjoyhint_script_steps = '';
        let obj_nextButton = { className: "myNext", text: Outlang["Tiep_theo"] };
        let obj_skipButton = {className: "mySkip", text: Outlang["Xong"] };
        let guideTour = []; let _TextMessage = ""; let _NameControl = "";
        for (i = 0; i < _GuideForCurrentPage.length; i++) {
            _TextMessage = _GuideForCurrentPage[i].TextMessage;
            let element;
            if (i == _GuideForCurrentPage.length - 1) {
                _NameControl = 'skip #' + _GuideForCurrentPage[i].NameControl;
                element = {
                    'skipButton': obj_skipButton,
                    'showSkip': true
                };
            }
            else {

                _NameControl = 'next #' + _GuideForCurrentPage[i].NameControl;
                element = {
                    'nextButton': obj_nextButton,
                    'showSkip': false
                };
            }

            element[_NameControl] = _TextMessage;
            guideTour.push(element);
        }
        enjoyhint_script_steps = guideTour;

        enjoyhint_instance.set(enjoyhint_script_steps);
        enjoyhint_instance.run();

    }
    return false;
}
function PassDataGuideEndUser(_guide) {
    //_GuideForCurrentPage = _guide;
    //if (_GuideForCurrentPage == undefined || _GuideForCurrentPage == '' || _GuideForCurrentPage == '[]') $('#btnGuideEndUser').hide();
    //else $('#btnGuideEndUser').show();
}
function HideGUideEndUser() {
    $('#btnGuideEndUser').hide();
    _GuideForCurrentPage = '[]';
}

        // #endregion