window.session_call = {};
window.ua = null;
window.domain = null;

//login extend in server
window.loginExtend = function(outbound, port, domain, extension, password){
    window.domain = domain;
    var config = {
        media: {
            remote: {
                audio: document.getElementById('audio_callcenter'),
            },
        },
        ua: {
            uri: extension + '@' + domain,
            password: password,
            wsServers: ['wss://' + outbound + ':' + port],
            stunServers: ['stun:stun.l.google.com:19302'],
            log: {
                builtinEnabled: false,
                level: "error",
            }
        }
    };

    ua = new SIP.Web.Simple(config);
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
		ua.on('new', function(e){
            let type_of_call = e.request.from.uri.user == infocall['user_extend'] ? 'local' : 'remote'; //local : call out, remote: call in
            var numbercall = e.remoteIdentity.uri.user; //phone start call
			session_call[e.request.call_id] = {
                request: e.request,
                session: ua,
			}
			
			eventCall(session_call[e.request.call_id]);
			if (typeof callback == 'function') {
				return callback(session_call[e.request.call_id])
			}
		})
	}
}

window.eventCall = function(session) {
    
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
		ua.call(number + '@' + window.domain);
	}
}

window.hangUp = function(session) {
	session['session']['session'].terminate();
}

window.answerCall = function(session){
    session['session'].answer();
}