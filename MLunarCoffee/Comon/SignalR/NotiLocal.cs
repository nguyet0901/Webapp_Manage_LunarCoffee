using Microsoft.AspNetCore.Http;
using System.Data;
using MLunarCoffee.Comon.Session;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.SignalR;
using MLunarCoffee.Models.Model.SignalR;
using Newtonsoft.Json;
using System;

namespace MLunarCoffee.Comon.SignalR {
    public static class NotiLocal
    {


        #region // Notification
        //public static async Task Noti_Parti_Emp ( IHubContext<NotiHub> hubContext, int emp, string title, string message ) {
        //    try {
        //        SignalR_Parti_Employee obj = new SignalR_Parti_Employee () { empid = emp, title = title, message = message };
        //        NotiHub notiHub = new NotiHub (hubContext);
        //        await notiHub.Send_Message_To_User (JsonConvert.SerializeObject (obj));
        //    }
        //    catch (Exception) {
        //        return;
        //    }
        //}
        public static async Task Noti_Parti_Appointment_Scheduler ( IHubContext<NotiHub> hubContext, int istemp, int branch_id, int appointment_id, DateTime date_from, string type, int userexcept ) {
            try {
                if (Global.sys_ScheduleNoti == 1 && appointment_id != 0 && branch_id != 0) {
                    SignalR_Appointment_To_Scheduler obj = new SignalR_Appointment_To_Scheduler () {
                        branch_id = branch_id,
                        appointment_id = appointment_id,
                        date_from = date_from,
                        type = type,
                        istemp = istemp,
                        userexcept = userexcept
                    };
                    NotiHub notiHub = new NotiHub (hubContext);
                    await notiHub.Send_Message_To_Scheduler (JsonConvert.SerializeObject (obj));
                }
            }
            catch (Exception) {
                return;
            }
        }
        public static async Task Noti_Parti_Appointment_Branch ( IHubContext<NotiHub> hubContext, int branch_id, int appointment_id, int cust_id, string cust_code, string cust_name, int status, int doctor, string type) {
            try {
                if (Global.sys_ScheduleNoti == 1 && appointment_id != 0 && branch_id != 0 && cust_id != 0) {
                    SignalR_Appointment_To_Branch obj = new SignalR_Appointment_To_Branch () {
                        branch_id = branch_id,
                        appointment_id = appointment_id,
                        cust_id = cust_id,
                        cust_code = cust_code,
                        cust_name = cust_name,
                        status_id = status,
                        doctor_id = doctor,
                        type = type
                    };
                    NotiHub notiHub = new NotiHub (hubContext);
                    await notiHub.Send_Message_To_Branch (JsonConvert.SerializeObject (obj));
                }
            }
            catch (Exception) {
                return;
            }
        }
        public static async Task Noti_Parti_Appointment_Room ( IHubContext<NotiHub> hubContext, int current_doctor, int branch_id, int appointment_id, int cust_id, string cust_code
            , string cust_name, int oldroom_id, int newroom_id, string oldroom, string newroom
             ,int OldChairID,int NewChairID 
            , string type ) {
            try {
                if (Global.sys_RoomNoti == 1 && appointment_id != 0 && branch_id != 0 && cust_id != 0) {
                    SignalR_Appointment_Room obj = new SignalR_Appointment_Room () {
                        current_doctor = current_doctor,
                        branch_id = branch_id,
                        appointment_id = appointment_id,
                        cust_id = cust_id,
                        cust_code = cust_code,
                        cust_name = cust_name,
                        oldroom_id = oldroom_id,
                        newroom_id = newroom_id,
                        oldroom = oldroom,
                        newroom = newroom,
                        oldchair_id = OldChairID ,
                        newchair_id = NewChairID ,
                        type = type
                    };
                    NotiHub notiHub = new NotiHub (hubContext);
                    await notiHub.Send_Message_To_Room (JsonConvert.SerializeObject (obj));
                }
            }
            catch (Exception) {
                return;
            }
        }

        public static async Task Send_MonitorRoom(IHubContext<NotiHub> hubContext ,int roomid,string type  )
        {
            try
            {
                if (Global.sys_RoomNoti == 1 && roomid != 0)
                {
                    SignalR_MonitorRoom obj = new SignalR_MonitorRoom()
                    {
                        roomid = roomid ,
                        type = type
                    };
                    NotiHub notiHub = new NotiHub(hubContext);
                    await notiHub.Send_MonitorRoom(JsonConvert.SerializeObject(obj));
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        /// <summary>
        /// type 0: Insert, 1: Edit , 2:delete
        /// </summary>
        /// <param name="userfrom"></param>
        /// <param name="userto"></param>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        //public static async Task Noti_Todo_User ( IHubContext<NotiHub> hubContext, int userreceipt, int todoid, int messid, int type, string message ) {
        //    try {
        //        SignalR_Todo_User obj = new SignalR_Todo_User () { userreceipt = userreceipt, todoid = todoid, messid = messid, type = type, message = message };
        //        NotiHub notiHub = new NotiHub (hubContext);
        //        await notiHub.Send_Todo_To_User (JsonConvert.SerializeObject (obj));

        //    }
        //    catch (Exception ex) {
        //        return;
        //    }
        //}
        /// <summary>
        /// type 0: Insert, 1: Edit
        /// </summary>
        //public static async Task Noti_Labo_User ( IHubContext<NotiHub> hubContext, int userreceipt, int laboid, int messid, int type, int status, string message ) {
        //    try {
        //        SignalR_Labo_User obj = new SignalR_Labo_User () {
        //            userreceipt = userreceipt,
        //            laboid = laboid,
        //            messid = messid,
        //            type = type,
        //            status = status,
        //            message = message
        //        };
        //        NotiHub notiHub = new NotiHub (hubContext);
        //        await notiHub.Send_Labo_To_User (JsonConvert.SerializeObject (obj));

        //    }
        //    catch (Exception) {
        //        return;
        //    }
        //}

        //public static void Noti_Telesale_Execute(string telesale_id, string ticket_id, string cust_id, int type)
        //{
        //    try
        //    {
        //        if(Global.sys_ExecuteDataNoti == 1)
        //        {
        //            SignalR_Tele_Execute obj = new SignalR_Tele_Execute() { telesale_id = telesale_id, ticket_id = ticket_id, cust_id = cust_id, type = type };
        //            HubProgress.Hub.Send_Telesale_Execute(JsonConvert.SerializeObject(obj));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}

        /// <summary>
        /// userId: id của user được chỉ định logout
        /// </summary>
        public static async Task Noti_And_LogOut(IHubContext<NotiHub> hubContext, string userId, string title, string message)
        {
            try
            {
                SignalR_String_User obj = new SignalR_String_User() { userfrom = "0", userid = userId,  title = title, message = message };
                NotiHub notiHub = new NotiHub(hubContext);
                await notiHub.Send_Noti_And_LogOut(JsonConvert.SerializeObject(obj));
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion
    }
}
