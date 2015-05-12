using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemTools.WebTools.Attributes;
using DataRepository;

namespace TaxorgRepository.Models
{
    [Table("Sessions")]
    [AuthorizeSkip]
    public class Session : ModelBase
    {
        public Session()
        {
            SessionTaxTypes = new HashSet<SessionTaxType>();
        }

        public Session(string sessionId)
            :this()
        {
            SessionId = sessionId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SessionId { get; set; }

//        [Required]
//        public DateTime Created { get; set; }
//
//        [Required]
//        public DateTime Expires { get; set; }
//
//        [Required]
//        public DateTime LockDate { get; set; }
//
//        [Required]
//        public int LockId { get; set; }
//
//        [Required]
//        public bool Locked { get; set; }
//
//        public byte[] ItemContent { get; set; }
//
//        [Required]
        public int UserId { get; set; }

        public ICollection<SessionTaxType> SessionTaxTypes { get; set; }
    }
}