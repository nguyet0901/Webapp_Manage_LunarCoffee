
var connection = new signalR.HubConnectionBuilder().withUrl("/onlineUsersHub").build();
connection.on("UpdateOnlineUsers", function (list,userid,userstatus) {
    //console.log(list)
    //console.log(userid)
    //console.log(userstatus)
   //document.getElementById("onlineUsersCount").innerText = count;
});
connection.start();