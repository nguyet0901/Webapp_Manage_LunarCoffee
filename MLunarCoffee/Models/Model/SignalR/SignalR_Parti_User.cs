using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.SignalR
{
    public class SignalR_Parti_Employee
    {
        public int empid { get; set; } 
        public string title { get; set; }
        public string message { get; set; }      
    }
    /// <summary>
    /// type : 'new,edit,movein,moveout,cancel,delete'
    /// </summary>
    public class SignalR_Appointment_To_Branch
    {
        public int branch_id { get; set; }
        public int appointment_id { get; set; }
        public int cust_id { get; set; }
        public string cust_code { get; set; }
        public string cust_name { get; set; }
        public int status_id { get; set; }
        public int doctor_id { get; set; }
        public string type { get; set; }

    }
    /// <summary>
    /// type:cancel,delete,insert,update,change_status
    /// </summary>
    public class SignalR_Appointment_To_Scheduler
    {
        public int branch_id { get; set; }
        public int appointment_id { get; set; }
        public DateTime date_from { get; set; }
        public int istemp { get; set; }
        public string type { get; set; }
        public int userexcept { get; set; }
    }

    public class SignalR_String_User
    {
        public string userfrom { get; set; }
        public string userid { get; set; }
        public string title { get; set; }
        public string message { get; set; }
    }
    //public class SignalR_Tele_Execute
    //{
    //    public string telesale_id { get; set; }                                   
    //    public string ticket_id { get; set; }                                  
    //    public string cust_id { get; set; }                               
    //    public int type { get; set; }
    //}
    public class SignalR_String_GroupUser
    {
        public string content { get; set; }
        public int branch { get; set; }
        public int usergroup { get; set; }
        public int custid { get; set; }
    }


    /// <summary>
    /// type : 'change,newbook,cancelbook'
    /// </summary>
    public class SignalR_Appointment_Room
    {
        public int current_doctor { get; set; }
        public int branch_id { get; set; }
        public int appointment_id { get; set; }
        public int cust_id { get; set; }
        public string cust_code { get; set; }
        public string cust_name { get; set; }
        public int oldroom_id { get; set; }
        public int newroom_id { get; set; }
        public string oldroom { get; set; }
        public string newroom { get; set; }
        public int oldchair_id { get; set; }
        public int newchair_id { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// type : 'receipt'
    /// </summary>
    public class SignalR_MonitorRoom
    {
        public int roomid { get; set; }
        public string type { get; set; }

    }


    /// type 0: Insert, 1: Edit , 2:delete
    public class SignalR_Todo_User
    {
        public int userreceipt { get; set; }
        public int todoid { get; set; }
        public int messid { get; set; }
        public int type { get; set; }
        public string message { get; set; }
    }

    /// type 0: Insert, 1: Edit  
    public class SignalR_Labo_User
    {
        public int userreceipt { get; set; }
        public int laboid { get; set; }
        public int messid { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
    public class SignlR_To_User_Cookie
    {
        public int current_doctor { get; set; }
        public int userid { get; set; }
        public string custid { get; set; }
        public string custname { get; set; }
        public string room { get; set; }

    }
}
