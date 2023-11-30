using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ModelCustomerApp
{
    public class SMS
    {
        public string SMSThirdParty { get; set; }
        public string SMSUserName { get; set; }
        public string SMSPassword { get; set; }
        public string SMSBrandName { get; set; }
        public string SMSShareKey { get; set; }
        public string SMSLogin { get; set; }
        public string SMSSendSMS { get; set; }
        public string SMSBrandNameShow { get; set; }
    }
    public class SMSSending
    {
        public string SMSThirdParty { get; set; }
        public string SMSUserName { get; set; }
        public string SMSPassword { get; set; }
        public string SMSBrandName { get; set; }
        public string SMSShareKey { get; set; }
        public string SMSLogin { get; set; }
        public string SMSSendSMS { get; set; }
        public string SMSBrandNameShow { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
    }
    public class SMSNumInCurrent
    {
        public int Count { get; set; }
    }
    public class SMSSuccessDevice
    {
        public string DeviceID { get; set; }
    }
    public class PhoneRegister
    {
        public string Phone { get; set; }
    }
}