

async function Call_Outsize (_linkClickToCall, _callextension, _callApi_Key, _calldomainApi, _phone, _line) {

    new Promise((resolove) => {
        let _url = `${_linkClickToCall}/from_number/${_callextension}/to_number/${_phone}/key/${_callApi_Key}/domain/${_calldomainApi}`;
        $.get(_url, function (data) {
            if (data != "Success") notiWarning("Error");
        });
    })
}