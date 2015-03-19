using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxorgRepository.Models
{
    [Table("Sessions")]
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

        public ICollection<SessionTaxType> SessionTaxTypes { get; set; }
    }
}