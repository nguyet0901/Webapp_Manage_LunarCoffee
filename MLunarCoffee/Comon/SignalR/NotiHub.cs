using System;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;

namespace MLunarCoffee.Comon.SignalR {

    public class NotiHub : Hub {
        public readonly IHubContext<NotiHub> hubContext;
        public NotiHub ( IHubContext<NotiHub> hubContext ) {
            this.hubContext = hubContext;
        }
        /// Gửi cho 1 user cụ thể, title và message
        //public async Task Send_Message_To_User ( string jsonString ) {
        //    await hubContext.Clients.All.SendAsync ("SendMessageClient_ToUser", jsonString);
        //}

        /// Gửi đến group user

        /// App khách hàng gọi controller, controller gọi hub này. Truyền 1 chuỗi json
        //public async Task Send_Message_From_App ( string jsonString ) {
        //    await hubContext.Clients.All.SendAsync ("SendMessageClient_From_App", jsonString);
        //}
        /// Nhận khi inbox từ facebook MLunarCoffee
        //public async Task Facebook_ReceiptMessageFromFace ( string data ) {
        //    await hubContext.Clients.All.SendAsync ("ReceiptMessageFromFace", data);
        //}
        /// Nhận khi comment từ facebook MLunarCoffee
        //public async Task Facebook_ReceiptCommentFromFace ( string data ) {
        //    await hubContext.Clients.All.SendAsync ("ReceiptCommentFromFace", data);
        //}
        /// Gửi cho Noti báo cho user
        //public async Task Send_Notier ( string jsonString ) {
        //    await hubContext.Clients.All.SendAsync ("Send_Notier_Client", jsonString);
        //}
        /// Send noti when change status appointment branchid
        public async Task Send_Message_To_Branch ( string jsonString ) {
            await hubContext.Clients.All.SendAsync ("Send_Message_To_Branch", jsonString);
        }
        /// Send noti when change status appointment roomid
        public async Task Send_Message_To_Room ( string jsonString ) {
            await hubContext.Clients.All.SendAsync ("Send_Message_To_Room", jsonString);
        }
        /// Send noti when change status appointment branchid to scheduler
        public async Task Send_Message_To_Scheduler ( string jsonString ) {
            await hubContext.Clients.All.SendAsync ("Send_Message_To_Scheduler", jsonString);
        }
        /// Gửi thông báo trong todo
        //public async Task Send_Todo_To_User ( string jsonString ) {
        //    await hubContext.Clients.All.SendAsync ("SendTodo_ToUser", jsonString);
        //}
        /// Gửi thông báo trong labo
        //public async Task Send_Labo_To_User ( string jsonString ) {
        //    await hubContext.Clients.All.SendAsync ("SendLabo_ToUser", jsonString);
        //}
        /// Gửi thông báo và logout user được chỉ định
        public async Task Send_Noti_And_LogOut(string jsonString)
        {
            await hubContext.Clients.All.SendAsync("Send_Noti_And_LogOut", jsonString);
        }

        /// Send noti when receipt patien
        public async Task Send_MonitorRoom(string jsonString)
        {
            await hubContext.Clients.All.SendAsync("Send_MonitorRoom" ,jsonString);
        }
    }
}
