using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxorgRepository.Models
{
    [Table("Settings")]
    public class Settings : ModelBase
    {
        public const string TaxPeriodYear = "taxperiodyear";
        public const string TaxPeriodMonth = "taxperiodmonth";
        public const string IsNotSameTaxLoad = "isnotsametaxload";
        public const string TaxPrevPeriod = "taxprevperiod";
        public const string AppVersion = "appversion";
        public const string IsMaintenance = "ismaintenance";

        [Key]
        public int IdSettings { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
    }
}
