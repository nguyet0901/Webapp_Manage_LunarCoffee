using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MLunarCoffee.Models.Invoice
{
    #region Info General
    public class MisaAccountingAutho
    {
        public string app_id { get; set; }
        public string access_code { get; set; }
        public string org_company_code { get; set; } = "";
    }

    public class MisaAccounting
    {
        //public string token { get; set; }
        public int currentID { get; set; }
        public int type { get; set; }
        public string page { get; set; }
    }
    public class MisaAccountingMulti
    {
        //public string token { get; set; }
        public string data { get; set; }
        public string page { get; set; }
    }

    internal class dtMisaAccounting : DataTable
    {
        public dtMisaAccounting()
        {
            Columns.Add("ID" ,typeof(Int32));
            Columns.Add("Type" ,typeof(Int32));
        }
    }
    #endregion

    #region Para Receipt
    public class MisaAccountingReceipt
    {
        [Required]
        public string app_id { get; set; }
        [Required]
        public string org_company_code { get; set; }
        [Required]
        public MisaAccountingVoucher[] voucher { get; set; }
        [Required]
        public MisaAccountingDictionary[] dictionary { get; set; }
    }

    public class MisaAccountingVoucher
    {
        [Required]
        public int voucher_type { get; set; }
        //GUID
        [Required]
        public string org_refid { get; set; }
        //GUID
        public string branch_id { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        //yyyy-mm-dd
        [Required]
        public string refdate { get; set; }
        [Required]
        public string posted_date { get; set; }
        [Required]
        public int org_reftype { get; set; }
        public string org_reftype_name { get; set; }
        [Required]
        //GUID
        public string org_refno { get; set; }
        [Required]
        public int reftype { get; set; }
        //GUID
        public string employee_id { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        //GUID Doi tuong thu chi
        public string account_object_id { get; set; }
        public string account_object_code { get; set; }
        public string account_object_name { get; set; }
        public string account_object_contact_name { get; set; }
        public string bank_account_id { get; set; }
        public string bank_account_number { get; set; }
        public string bank_name { get; set; }

        [Required]
        public string currency_id { get; set; } = "VND";
        public decimal total_amount_oc { get; set; }
        [Required]
        public decimal total_amount { get; set; }
        [Required]
        public int reason_type_id { get; set; }
        public string journal_memo { get; set; }
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string modified_date { get; set; }
        public string modified_by { get; set; }
        public MisaAccountingVoucherDetail[] detail { get; set; }
    }

    public class MisaAccountingVoucherDetail
    {
        //GUID
        public string account_object_id { get; set; }
        public string account_object_code { get; set; }
        public string account_object_name { get; set; }
        public string bank_account_id { get; set; }
        public string bank_account_number { get; set; }
        public string bank_name { get; set; }
        public int sort_order { get; set; } = 1;
        public decimal amount_oc { get; set; }
        [Required]
        public decimal amount { get; set; }
        public string description { get; set; }
        /// <summary>
        /// Tài khoản nợ: 1111 thu khác (tiền mặt), 1121 thu khác (Bank); 811 Chi khác
        /// </summary>
        [Required]
        public string debit_account { get; set; }
        /// <summary>
        /// Tài khoản có: 711 thu khác, 511 thu khách hàng, 1111 chi khác (tiền mặt), 1121 chi khác (Bank)
        /// </summary>
        [Required]
        public string credit_account { get; set; }
    }

    public class MisaAccountingDictionary
    {
        public int dictionary_type { get; set; }
        //GUID
        public string account_object_id { get; set; }
        public string account_object_code { get; set; }
        public string account_object_name { get; set; }
        //0 la to chuc, 1 là cá nhân
        public int account_object_type { get; set; }
        //Là NCC
        public bool is_vendor { get; set; }
        public bool is_customer { get; set; }
        public bool is_employee { get; set; }
        //Ngừng theo dõi chứng từ
        public bool inactive { get; set; } = false;
        public int state { get; set; } = 1;
        public string created_date { get; set; }
        public string created_by { get; set; }
        public string modified_date { get; set; }
        public string modified_by { get; set; }
    }

    public class MisaResponse
    {
        public string refguid { get; set; }
        public int vourcherType { get; set; }
        public string contentResult { get; set; }
    }
    #endregion

    #region //Delete

    public class MisaAccountingDelete
    {
        //public string token { get; set; }
        public int voucherType { get; set; }
        public string refid { get; set; }
    }
    #endregion

    #region //RESULT CALLBACK

    public class MisaCallBackPara
    {
        public bool success { get; set; }//Trạng thái: true thành công, false thất bại
        public string error_code { get; set; }//Mã lỗi
        public string error_message { get; set; } = string.Empty;//Thông tin chi tiết lỗi
        public string signature { get; set; }//chữ ký: = thông tin dùng hàm GeneratorSHA256HMAC(data , key là app_id do MISA cung cấp cho ứng dụng)
        public int data_type { get; set; }//Loại dữ liệu trả về
        public string data { get; set; }//dữ liệu chứa json kết quả trả về
        public string org_company_code { get; set; }//Mã công ty phía dữ liệu nguồn
        public string app_id { get; set; }//ID ứng dụng
    }

    public class MisaCallBackResponse
    {
        public bool Success { get; set; } = true;
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string Data { get; set; }
    }

    public class MisaCallBackParaDetail
    {
        public string org_refid { get; set; }// ID gốc của chứng từ
        public bool success { get; set; }//Trạng thái: true thành công, false thất bại
        public string error_code { get; set; }//Mã lỗi
        public string error_message { get; set; } = string.Empty;//Thông tin chi tiết lỗi
        public Guid? session_id { get; set; }//Lỗi của phiên làm việc nào
        public string error_call_back_message { get; set; }//Lỗi gọi hàm callback đến api callback của đối tác của lần gọi trước đó
        public int? voucher_type { get; set; }//Loại chứng từ
    }
    #endregion
}
