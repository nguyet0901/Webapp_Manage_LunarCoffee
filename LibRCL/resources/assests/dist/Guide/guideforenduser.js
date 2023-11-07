
const urlGuide = [
    { "url": "/Appointment/AppointmentByDay", "script": "Doe" }
]
x = '[{"click #dtContentAppointmentByDayList": "Click vao day","nextButton": { className: "myNext", text: "tiep theo" }},{"next #ScheduleStatus": "day la loai lich hen","nextButton": { className: "myNext", text: "Tiep tuc" }},{"next #totalScheduler": "day la tong lich hen","skipButton": { className: "mySkip", text: "OK xong" }}]';
function RunGuideForEndUser(url) {

    switch (url) {
        case '/Appointment/AppointmentByDay':
            _executeFuntionGuide();
            break;
        default: break;
    }
}
function CheckShowHideGuide(url) {

}


function _executeFuntionGuide() {
    var enjoyhint_instance = new EnjoyHint({
    });
    var enjoyhint_script_steps = [
        {
            'click #dtContentAppointmentByDayList': 'Click vao day',
            'nextButton': { className: "myNext", text: "tiep theo" }
        },
        {
            'next #ScheduleStatus': 'day la loai lich hen',
            'nextButton': { className: "myNext", text: "Tiep tuc" }
        },
        {
            'next #totalScheduler': 'day la tong lich hen',
            'skipButton': { className: "mySkip", text: "OK xong" }
        }
    ];
    enjoyhint_instance.set(enjoyhint_script_steps);
    enjoyhint_instance.run();
}



//var enjoyhint_script_steps = [
//    {
//        'click #dtContentAppointmentByDayList': 'Click vao day',
//        'nextButton': { className: "myNext", text: "tiep theo" }
//        //'next #ScheduleStatus': 'day la loai lich hen'
//        //'showSkip': false,
//        // 'onBeforeStart': function () {
//        //     alert("aaaaa");
//        //}
//    },
//    {
//        /*hint code goes here*/
//        'next #ScheduleStatus': 'day la loai lich hen',
//        //'skipButton': { className: "mySkip", text: "Got It!" },
//        'nextButton': { className: "myNext", text: "Tiep tuc" }
//    },
//    {
//        /*hint code goes here*/
//        'next #totalScheduler': 'day la tong lich hen',
//        'skipButton': { className: "mySkip", text: "OK xong" }
//        //'nextButton': { className: "myNext", text: "Sure" },
//    },
//];