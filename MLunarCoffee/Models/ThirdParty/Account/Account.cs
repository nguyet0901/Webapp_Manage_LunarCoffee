using System;
using System.Data;

namespace MLunarCoffee.Models.ThirdParty.Account
{
    public class Account
    {

    }
    #region autho
    public class MisaAccountingAutho
    {
        public string app_id { get; set; }
        public string access_code { get; set; }
        public string org_company_code { get; set; } = "";
    }

    #endregion

    #region excuting
    public class AccountVoucher
    {
        public string guid { get; set; }
        public int currentID { get; set; }
        public int type { get; set; }
        public string action { get; set; } = "save";
    }

    public class AccountPara
    {
        public string para { get; set; }
        public string dataVoucher { get; set; }
        public string page { get; set; }
        public int branchid { get; set; }
        public int custid { get; set; }
        public string topic { get; set; }
        public int isupdate { get; set; } = 0;
        public string action { get; set; } = "save";

    }
    public class AccountVoucherMulti
    {
        public string data { get; set; }
        public string action { get; set; } = "save";
    }
    internal class dtAccountVoucher : DataTable
    {
        public dtAccountVoucher()
        {
            Columns.Add("ID" ,typeof(Int32));
            Columns.Add("Type" ,typeof(Int32));
            Columns.Add("MasterGuid" ,typeof(string));
        }
    }
    #endregion

    #region //callback

    public class CallbackPara
    {

    }

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

    public class CallbackResult
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
