using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LunarCoffee.Models.EntityFramework
{
    [Table("tb_Export")]
    public class Export: CommonAbstract
    {
        public Export()
        {
            this.ExportDetails = new HashSet<ExportDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string WareName { get; set; }
        public int Quantity { get; set; }
        public int IsExport { get; set; }
        public DateTime? DateExport { get; set; }

        public virtual ICollection<ExportDetail> ExportDetails { get; set; }
    }
}