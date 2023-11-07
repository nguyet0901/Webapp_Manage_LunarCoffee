using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using MLunarCoffee.Comon.SignalR;

namespace MLunarCoffee.Pages.Chatting
{
    public class Message : PageModel
    {
       // private readonly IHubContext<MessageHub> hubContext;
        public string layout { get; set; }
        public string _CustID { get; set; }
        public Message()
        {
            //this.hubContext=hubContext;
        }
        public void OnGet()
        {
            string CustID = Request.Query["CustID"];
            if(CustID!=null)
            {
                _CustID=CustID.ToString();
            }
            else
            {
                _CustID="0";
            }

        }
        public async Task<IActionResult> OnPostGetInfo(string custid,string converid)
        {
            try
            {
                var user = Comon.Session.Session.GetUserSession(HttpContext);
                DataSet ds = new DataSet();

                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    ds=await confunc.ExecuteDataSet("YYY_Chatting_Cust_GetInfo",CommandType.StoredProcedure
                        ,"@CustID",SqlDbType.NVarChar,custid
                        ,"@ConverDetailID",SqlDbType.NVarChar,converid!=null ? converid : ""
                        );
                }

                if(ds!=null)
                {

                    if(ds.Tables[0].Rows.Count>0 &&ds.Tables[0].Rows[0]["ConverID"].ToString()!="")
                    {
                        string _converid = ds.Tables[0].Rows[0]["ConverID"].ToString();
                        using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                        {
                            await confunc.ExecuteDataTable("YYY_Chatting_UpdateRead",CommandType.StoredProcedure
                                ,"@ConverDetailID",SqlDbType.NVarChar,_converid
                                 ,"@UserID",SqlDbType.Int,user.sys_userid);
                        }
                    }

                    return Content(Comon.DataJson.Dataset(ds));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostCreateConver(string custid,string message)
        {
            try
            {
                DataTable dt = new DataTable();
                var user = Comon.Session.Session.GetUserSession(HttpContext);
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_Cust_CreateConver",CommandType.StoredProcedure
                        ,"@CustID",SqlDbType.Int,custid
                         ,"@UserID",SqlDbType.Int,user.sys_userid
                        ,"@Message",SqlDbType.NVarChar,message);
                }
                return Content(Comon.DataJson.Datatable(dt));
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadMessage(string converid,int limit,string time)
        {
            try
            {
                DataTable dt = new DataTable();

                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_MessageLoad",CommandType.StoredProcedure
                        ,"@ConverID",SqlDbType.NVarChar,converid
                        ,"@limit",SqlDbType.BigInt,limit
                        ,"@begin",SqlDbType.BigInt,time);
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostLoadMedia(string converid,string type)
        {
            try
            {
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_MediaLoad",CommandType.StoredProcedure
                        ,"@ConverID",SqlDbType.NVarChar,converid
                        ,"@type",SqlDbType.NVarChar,type);
                }
                if(dt!=null)
                {
                    return Content(Comon.DataJson.Datatable(dt));
                }
                else
                {
                    return Content("0");
                }
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
        public async Task<IActionResult> OnPostSendMessage(string customerid,string converid,string type,string message,string link,string linkfeature,string filename)
        {
            try
            {
                var user = Comon.Session.Session.GetUserSession(HttpContext);
                DataTable dt = new DataTable();
                using(Models.ExecuteDataBase confunc = new Models.ExecuteDataBase())
                {
                    dt=await confunc.ExecuteDataTable("YYY_Chatting_SendMessage",CommandType.StoredProcedure
                        ,"@ConverID",SqlDbType.NVarChar,converid
                        ,"@CustomerID",SqlDbType.Int,0
                        ,"@UserSend",SqlDbType.Int,user.sys_userid
                        ,"@Link",SqlDbType.NVarChar,link
                        ,"@LinkFeature",SqlDbType.NVarChar,linkfeature
                        ,"@Type",SqlDbType.NVarChar,type
                         ,"@Filename",SqlDbType.NVarChar,filename!=null ? filename : ""
                        ,"@Message",SqlDbType.NVarChar,message!=null ? message.Replace("'","") : "");
                }
               // MessageHub hub = new MessageHub(hubContext);
               // await hub.SendMessage( new {Type="CustomerGate", Conver = converid,User = user.sys_userid,Time = dt.Rows[0]["TIME"].ToString()  });

                return Content(dt.Rows[0]["TIME"].ToString());
            }
            catch(Exception ex)
            {
                return Content("0");
            }

        }
    }
}
