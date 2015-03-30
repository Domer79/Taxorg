using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataRepository;

namespace TaxorgRepository.Models
{
    [Table("FileSystem")]
    public class FileSystem : ModelBase
    {
        [Key]
        [Column("idFileSystem")]
        public int IdFileSystem { get; set; }

        [Required]
        [StringLength(200)]
        [Column("fileName")]
        public string FileName { get; set; }

        [StringLength(50)]
        [Column("remoteHostName")]
        public string RemoteHostName { get; set; }

        [StringLength(1000)]
        [Column("remoteHostFileName")]
        public string RemoteHostFileName { get; set; }

        [StringLength(20)]
        [Column("remoteHostIpv4")]
        public string RemoteHostIpv4 { get; set; }

        [StringLength(50)]
        [Column("remoteHostIpv6")]
        public string RemoteHostIpv6 { get; set; }

        [StringLength(50)]
        [Column("remoteHostMac")]
        public string RemoteHostMac { get; set; }

        [Column("isCompressed")]
        public bool IsCompressed { get; set; }

        [Column("contentType")]
        public string ContentType { get; set; }
     
        [NotMapped]
        public virtual FsFile FsFile { get; set; }
    }
}
