using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataRepository;

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
