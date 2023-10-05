
var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();
connection.on("SendMessage", function (jsonstring) {
    let obj = JSON.parse(jsonstring);
    switch (obj.Type) {
        case "CustomerGate": {
            let e = {};
            e.Conver = obj.Conver;
            e.Time = obj.Time;
            e.User = obj.User;
            if (typeof Conver_UpdateUser !== 'undefined' && typeof Conver_UpdateUser === 'function') {
                Conver_UpdateUser(e);
            }
            break;
        }
    }
});
connection.start();