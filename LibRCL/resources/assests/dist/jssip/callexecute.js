
session_call = {};
sessionCalling = null;
var phoneConnection = 1; // 0 : Tu Ngat May
phone = null;
var interval;
var flagCalling = 0;
var call_option = {
    'mediaConstraints': { 'audio': true, 'video': false },
    'pcConfig': {
        'iceServers': [
            { 'urls': ['stun:stun.l.google.com:19302'] }
        ]
       
    }
    ,'sessionTimersExpires': 120
    , 'iceTransportPolicy': 'relay'
};
//var call_option = {
//    'mediaConstraints': { 'audio': true, 'video': false },
//    'pcConfig': {
//        'iceServers': [
//            { 'urls': ['stun:stun.l.google.com:19302'] }
//        ]
//    },
//    'sessionTimersExpires': 120
//};
//login extend in server
function Call_loginExtend(outbound, port, domain, extension, password, timeout) {
    try {
        var socket = new JsSIP.WebSocketInterface('wss://' + outbound + ':' + port);
        var configuration = {
            sockets: [socket],
            uri: extension + '@' + domain,
            password: password,
            register_expires: timeout
        };

        phone = new JsSIP.UA(configuration);
        phone.on('connected', function (e) {
            // Your code here
            let oProgress = document.getElementById("StatusOfLineCenter");
            oProgress.innerHTML = "Connected " + extension;
            oProgress.style.color = "#00b5ad";
        });
        phone.on('disconnected', function (e) {
            // Your code here
            let oProgress = document.getElementById("StatusOfLineCenter");
            oProgress.innerHTML = "Not connected";
            oProgress.style.color = "#c68d8d";
       
        });
        phone.on('newSession', function (e) {
            // Your code here
            //console.log('newSession');
        });
        phone.on('newMessage', function (e) {
            // Your code here
           // console.log('newMessage');
        });
        phone.on('registered', function (e) {
           // console.log('registered');
        });
        phone.on('unregistered', function (e) {
            // Your code here
           //console.log('unregistered');
        });
        phone.on('registrationFailed', function (e) {
            // Your code here
            //console.log(e);
            let oProgress = document.getElementById("StatusOfLineCenter");
            oProgress.innerHTML = "Failed to login";
            oProgress.style.color = "#c68d8d";
     
        });
        phone.start();
    }
    catch (err) {
        //console.log(err);
    }
}
function Call_newRTCSession() {
    try {
        phone.on("newRTCSession", function (data) {
            //console.log(data.session)
  
            sessionCalling = data.session; // outgoing call session here
            if (sessionCalling.direction === "incoming") {
                //console.log('incoming');
                sessionCalling.on("accepted", function () {
                    CCF_BeginTime();
                    clearInterval(interval);
                    interval = null;
                    interval = setInterval(CCF_setTime, 1000);
                    var remoteAudio = window.document.createElement('audio');
                    window.document.body.appendChild(remoteAudio);
                    remoteAudio.src = window.URL.createObjectURL(
                        sessionCalling.connection.getRemoteStreams()[0]
                    );
                    remoteAudio.play();
                });
                sessionCalling.on("confirmed", function () {
                    //console.log('confirmed');
                    // this handler will be called for incoming calls too
                });
                sessionCalling.on("ended", function () {
                    let phonenumber = "";
                    if (typeof _phonenumber !== 'undefined' && typeof _isUnknow !== 'undefined') {
                        if (_phonenumber != "" && _isUnknow == 1)
                            phonenumber = _phonenumber;
                    }
                    //console.log('ended In Client');

                    CCF_CallFail(phonenumber);
                });                                                                                                                 
                sessionCalling.on("failed", function () {
                    //console.log('failed Incoming In');
                    CCF_CallFail();
                });
                //sessionCalling.on('addstream', function (e) {
                //    console.log('addstream');
                //    // set remote audio stream (to listen to remote audio)
                //    // remoteAudio is <audio> element on page
                //    remoteAudio.src = window.URL.createObjectURL(e.stream);
                //    remoteAudio.play();
                //});

                //session_call[data.request.call_id] = {
                //    session: data.session
                //};
                //eventCall(session_call[data.request.call_id]);
                // Answer call
                //sessionCalling.answer(call_option);

                var header = sessionCalling.request.getHeader('From');
                var headername = sessionCalling.remote_identity.display_name;
                sip_incomingcall(header, headername);


                // Reject call (or hang up it)
                // session.terminate();
            }
            else {
                // CAll OUT
                sessionCalling.on("confirmed", function () {
                    CCF_BeginTime();
                    clearInterval(interval);
                    interval = null;
                    interval = setInterval(CCF_setTime, 1000);
                });
                sessionCalling.on("ended", function () {
                    //console.log('ended Out');
                    CCF_CallFail();
                });
                sessionCalling.on("failed", function ( ) {
                    if (phoneConnection == 1) {
                       
                        // notiWarning("Failure Audio Headphone Or Not Register");
                    }

                    CCF_CallFail();
                });
                let self = this;
                sessionCalling.on('icecandidate', function (data) {
                    //console.log('icecandidate');
                    self.count_icecandidate++;
                    if (self.count_icecandidate > 2) {
                        //console.log('icecandidate10');
                        data.ready();
                    }

                });
                sessionCalling.on('peerconnection', function (e) {
                    //console.log('1');
                    e.peerconnection.onaddstream = function (d) {
                        audio_callcenter.srcObject = d.stream;
                        var localStream = sessionCalling.connection.getLocalStreams()[0];
                        //console.log(localStream.getAudioTracks()[0]);
                        //console.log(sessionCalling);
                        //console.log(sessionCalling.connection);

                        dtmfSender = sessionCalling.connection.createDTMFSender(localStream.getAudioTracks()[0])
                    }
                    //console.log('3');
                })
            }

        });
    }
    catch (err) {
        //console.log(err);
    }
   
}
function eventCall(session) {
    session['session'].on('peerconnection', function (e) {

        e.peerconnection.onaddstream = function (d) {
            audio_callcenter.srcObject = d.stream;
        }
    })
}
function sip_callexe(phonenumber,callback) {
    if (phone != undefined && phone != null) {
        try {
            //console.log(phonenumber);
            if (phonenumber != "") {
                phone.call(phonenumber, call_option);
                callback(phonenumber);
                flagCalling = 0;
            } else {
                console.log("phone null")
            }
        }
        catch (err) {
            //notiWarning("Device Not Found");
        }
    } else {
        notiWarning("User not connect extension");
    }
}
function sip_hangup() {
    phoneConnection = 0;
    if (sessionCalling)
        sessionCalling.terminate();
    phoneConnection = 1;
    flagCalling = 0;
}
function CallingAccept() {
    sessionCalling.answer(call_option);
}
function CallingMute() {
    sessionCalling.mute({ audio: true });
}
function CallingUnMute() {
    sessionCalling.unmute({ audio: true });
}
function CallingAnswer() {
    sessionCalling.answer(call_option);
}
