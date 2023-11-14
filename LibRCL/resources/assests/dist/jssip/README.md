## Thu vien tich hop webrtc
- jssip3.0.7
- libjs

## Document
1. Include file js
	<script type="text/javascript" src="jssip-3.0.7.min.js"></script>
	<script type="text/javascript" src="libcall.js"></script>
	
2. Add tag html
	<audio id="audio_callcenter" autoplay="autoplay" loop></audio>
	
3. Login extension
	loginExtend(outbound, port, domain, extension, password) //outbound, port, domain, extension, password provided by Callcenter system
	
4. Add media when call and get session call
	getInfoCall(callback); //callback is function require get session and return error message if extension cannot login
	var session = null;
	function callback(sessionCallback)
	{
		session = sessionCallback
	}
	
5. Click to call
	clickToCall(phoneNumber, callback) //callback is function not require. callback will return message error if phoneNumber empty or extension cannot login
	
6. Hangup
	hangUp(session) //session get from step 4