window.session_call = {};
window.ua = null;

//login extend in server
window.loginExtend = function(outbound, port, domain, extension, password){
    var socket = new JsSIP.WebSocketInterface('wss://' + outbound + ':' + port);
    var configuration = {
        sockets: [socket],
        uri: extension + '@' + domain,
        password: password
    };

    ua = new JsSIP.UA(configuration);
    ua.on('connected', function (e) {
        // Your code here
        console.log('connected');
    });
    ua.on('disconnected', function (e) {
        // Your code here
        console.log('disconnected');
    });
    ua.on('newSession', function (e) {
        // Your code here
        console.log('newSession');
    });
    ua.on('newMessage', function (e) {
        // Your code here
        console.log('newMessage');
    });
    ua.on('registered', function (e) {
        // Your code here
        console.log('registered');
    });
    ua.on('unregistered', function (e) {
        // Your code here
        console.log('unregistered');
    });
    ua.on('registrationFailed', function (e) {
        // Your code here
        console.log('registrationFailed');
    });
    ua.start();
}


window.getInfoCall = function(callback) {
	if(!ua) {
		if (typeof callback == 'function') {
			return callback('Cannot login ua');
		} else {
			alert('Cannot login extension');
			return;
		}
	} else {
		ua.on('newRTCSession', function(data){
           

            console.log('1');
            var session = data.session; // outgoing call session here
            var dtmfSender;
            session.on("confirmed", function () {
                //the call has connected, and audio is playing
                console.log('confirmed');
                var localStream = session.connection.getLocalStreams()[0];
                dtmfSender = session.connection.createDTMFSender(localStream.getAudioTracks()[0])
            });
            session.on("ended", function () {
                //the call has ended
                console.log('ended');
            });
            session.on("failed", function () {
                // unable to establish the call
                console.log('failed');
            });
            session.on('addstream', function (e) {
                // set remote audio stream (to listen to remote audio)
                // remoteAudio is <audio> element on page
                remoteAudio.src = window.URL.createObjectURL(e.stream);
                remoteAudio.play();
            });
            session_call[data.request.call_id] = {
                session: data.session
            };

            eventCall(session_call[data.request.call_id]);
            if (typeof callback == 'function') {
                return callback(session_call[data.request.call_id])
            };

            ////play a DTMF tone (session has method `sendDTMF` too but it doesn't work with Catapult server right)
            //dtmfSender.insertDTMF("1");
            //dtmfSender.insertDTMF("#");

            //mute call
            session.mute({ audio: true });

            //unmute call
            session.unmute({ audio: true });

            //to hangup the call
            session.terminate();
		})
	}
}

window.eventCall = function(session) {
	session['session'].on('peerconnection', function(e){
        e.peerconnection.onaddstream = function(d){
            audio_callcenter.srcObject = d.stream;
        }
    })
}

window.clickToCall = function(number, callback) {
	if (!number) {
		if (typeof callback == 'function') {
			return callback('Phone number is not empty');
		} else {
			alert('Phone number is not empty');
			return;
		}
	}
	if(!ua) {
		if (typeof callback == 'function') {
			return callback('Cannot login extension');
		} else {
			alert('Cannot login extension');
			return;
		}
	} else {
		var options = {
			'mediaConstraints': {'audio': true, 'video': false},
			'pcConfig': {
				'iceServers': [
					{ 'urls': ['stun:stun.l.google.com:19302'] }
				]
			},
			'sessionTimersExpires': 120
		};
		ua.call(number, options);
	}
}

window.hangUp = function(session) {
	session['session'].terminate();
}
window.disconnect = function () {
    window.ua.stop();
}
var callOptions = {
    mediaConstraints: {
        audio: true, // only audio calls
        video: false
    }
};