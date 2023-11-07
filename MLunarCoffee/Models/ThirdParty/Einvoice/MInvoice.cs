using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Models.Invoice
{
    public class EInvoicesyntax
    {
        public int type { get; set; }
        [Required]
        public string token { get; set; }
    }
    public class EInvoice
    {

        public string typett { get; set; }
        public int editmode { get; set; }
        public int invoicenumber { get; set; }
        public string paymentid { get; set; }
        //public string token { get; set; }
        //public string invoiceseries { get; set; }
    }

    #region // TT32
    public class M_CancelData32
    {
        public int editmode { get; set; }
        public M_CancelInfo32[] data { get; set; }
    }
    public class M_CancelInfo32
    {
        public string inv_invoiceSeries { get; set; }
        public string inv_invoiceNumber { get; set; }
    }

    public class M_Data32
    {
        public int editmode { get; set; }
        public M_Info32[] data { get; set; }
    }

    public class M_Info32
    {

        public string inv_invoiceIssuedDate { get; set; }
        public string inv_invoiceSeries { get; set; }
        public int inv_invoiceNumber { get; set; }
        public string inv_currencyCode { get; set; }
        public string inv_paymentMethodName { get; set; }
        public string inv_buyerDisplayName { get; set; }
        public string ma_dt { get; set; }
        public string inv_buyerAddressLine { get; set; }
        public string inv_buyerEmail { get; set; }
        public decimal inv_TotalAmount { get; set; }
        public decimal inv_vatAmount { get; set; }
        public decimal inv_discountAmount { get; set; }
        public decimal inv_TotalAmountWithoutVat { get; set; }
        public M_DetailData32[] details { get; set; }
    }

    public class M_DetailData32
    {
        public M_Detail32[] data { get; set; }
    }
    public class M_Detail32
    {
        public int tchat { get; set; }
        public string stt_rec0 { get; set; }
        public string inv_itemCode { get; set; }
        public string inv_itemName { get; set; }
        //public string inv_unitCode { get; set; }
        public decimal inv_quantity { get; set; }
        public decimal inv_unitPrice { get; set; }
        public decimal inv_discountPercentage { get; set; }
        public decimal inv_discountAmount { get; set; }
        public decimal inv_TotalAmountWithoutVat { get; set; }
        public decimal ma_thue { get; set; }
        public decimal inv_vatAmount { get; set; }
        public decimal inv_TotalAmount { get; set; }
    }
    #endregion
    #region // TT78
    public class M_CancelData78
    {
        public int editmode { get; set; }
        public M_CancelInfo78[] data { get; set; }
    }
    public class M_CancelInfo78
    {
        public string khieu { get; set; }
        public string shdon { get; set; }
    }

    public class M_Data78
    {
        public int editmode { get; set; }
        public M_Info78[] data { get; set; }
    }

    public class M_Info78
    {

        public string tdlap { get; set; }
        public string khieu { get; set; }
        public int shdon { get; set; }
        public string dvtte { get; set; }
        public string htttoan { get; set; }
        public string tnmua { get; set; }
        public string mnmua { get; set; }
        public string dchi { get; set; }
        public string email { get; set; }
        public decimal tgtttbso { get; set; }
        public decimal tgtthue { get; set; }
        public decimal ttcktmai { get; set; }
        public decimal tgtcthue { get; set; }
        public M_DetailData78[] details { get; set; }
    }

    public class M_DetailData78
    {
        public M_Detail78[] data { get; set; }
    }
    public class M_Detail78
    {
        public int tchat { get; set; }
        public string stt { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public decimal sluong { get; set; }
        public decimal dgia { get; set; }
        public decimal tlckhau { get; set; }
        public decimal stckhau { get; set; }
        public decimal thtien { get; set; }
        public decimal tsuat { get; set; }
        public decimal tthue { get; set; }
        public decimal tgtien { get; set; }
    }
    #endregion
    public class MInvoiceauthor
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}