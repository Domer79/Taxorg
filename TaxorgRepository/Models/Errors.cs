using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxorgRepository.Models
{
    [Table("Errors")]
    public class Error : ModelBase
    {
        [Key]
        [Column("idError")]
        public int IdError { get; set; }

        [Column("type")]
        public string TypeError { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("stackTrace")]
        public string StackTrace { get; set; }

        [Column("timeLabel")]
        public DateTime TimeLabel { get; set; }
    }
}
