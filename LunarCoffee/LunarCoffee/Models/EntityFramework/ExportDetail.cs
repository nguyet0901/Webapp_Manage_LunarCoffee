using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LunarCoffee.Models.EntityFramework
{
    [Table("tb_ExportDetail")]
    public class ExportDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ExportId { get; set; }
        public int ProductId { get; set; }
        public int UnitID { get; set; }
        public int Quantity { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual Export Export { get; set; }
        public virtual Product Product { get; set; }

    }
}