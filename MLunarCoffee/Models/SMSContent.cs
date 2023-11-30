using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MLunarCoffee.Models
{
    public class SMSContent
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Content { get; set; }
        public string Brandname { get; set; }
        public string znssms { get; set; }
        public string CallbackID { get; set; }
        public string CallbackUrl { get; set; }
    }

    public class SMSContentPending
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Content { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public string Amount { get; set; }
        public string BranchID { get; set; }
        public int IsZNS { get; set; }
    }
    public class SMSZNSContentEach
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Content { get; set; }

    }
    public class SMSZNSContentMulti
    {
        public SMSZNSContentEach[] SmsItem { get; set; }
        public string? CallbackMultiUrl { get; set; }

    }


    public class SMSPending
    {
        public string content { get; set; }
        public string phone { get; set; }
        public string amount { get; set; }
        public string customerID { get; set; }
        public string ticketID { get; set; }
        public string appID { get; set; }
        public string studentID { get; set; }
        public string typecare { get; set; }
        public string znssms { get; set; }

    }
    public class SMSSuccess
    {
        public string idsms { get; set; }
        public string status { get; set; }
    }

    public class SMSResultCallback
    {
        public string id { get; set; }
        public string status { get; set; }
        public string description { get; set; }
    }
    public class SMSResultCallbackMulti
    {
        public string phone { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string? iszns { get; set; }
    }

    public class SMSAfterPaid
    {
        public int customerid { get; set; }
        public int paymentid { get; set; }
        public int paymentcardid { get; set; }
        public int paymentmedid { get; set; }
        public int depositid { get; set; }
        public int tabid { get; set; }
        public int cardid { get; set; }
        public int medicineid { get; set; }
        public int stupaymentid { get; set; }
        public string templatetype { get; set; }
    }

    public class SMSAfterAppointment
    {
        public int customerid { get; set; }
        public int appid { get; set; }
        public string templatetype { get; set; }
    }

    public class SMSAfterChangeCard
    {
        public int customerid { get; set; }
        public string amount { get; set; }
        public int cardid { get; set; }
        public string templatetype { get; set; }
        public Dictionary<string ,string> dataSMSPayment { get; set; }
        public Dictionary<string ,string> KeyMessage { get; set; }
    }

    public class SMSTemplateResult
    {
        public string Result { get; set; }
        public string Content { get; set; }
        public string Phone { get; set; }
        public int IsZNS { get; set; } = 0;

    }
    public class SMSTemplateResultMulti
    {
        public SMSMareach[] SmsItem { get; set; }
        public int IsZNS { get; set; } = 0;

    }
    public class SMSMareach
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Content { get; set; }

    }
    public class SMSMarMulti
    {
        public SMSMareach[] SmsItem { get; set; }
        public string? CallbackMultiUrl { get; set; }
        public string? BrandName { get; set; }

    }

    public class SMSLog
    {
        public string? znssms { get; set; }
        public string? template_id { get; set; }
        public string? from_time { get; set; }
        public string? page { get; set; }
        public string? limit { get; set; }
    }


    public class SMSTemplate
    {
        public int template_id { get; set; }
        public string? template_type { get; set; }
        public int national_id { get; set; }
    }
    public class SMSContentTemplate
    {
        public string? Content { get; set; }
        public int IsZNS { get; set; }
        public int AllowSendPay { get; set; }
        public int AllowSendApp { get; set; }
        public int AllowSendTab { get; set; }
    }

    public class SMSKeyTemplate
    {
        public int cust_id { get; set; }
        public int custcard_id { get; set; }
        public int app_id { get; set; }
        public int custtreat_id { get; set; }
        public string custtreat_date { get; set; }
        public int branch_id { get; set; }
        public int payment_id { get; set; }
        public int paymentcard_id { get; set; }
        public int paymentmed_id { get; set; }
        public int deposit_id { get; set; }
        public int medicine_id { get; set; }
        public int tab_id { get; set; }
        public int stupayment_id { get; set; }
        public string? template_type { get; set; } = "";

    }


}
