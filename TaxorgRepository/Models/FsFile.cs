using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataRepository;

namespace TaxorgRepository.Models
{

    [Table("FsFile")]
    public class FsFile : ModelBase
    {
        [Key]
        [Column("idFsFile")]
        public int IdFsFile { get; set; }

        [Column("idFileSystem")]
        public int IdFileSystem { get; set; }

        //TODO Реализоват сохранени данных через свойство Data
        [NotMapped]
        public byte[] Data { get; set; }

//        public virtual FileSystem FileSystem { get; set; }
        public override int GetHashCode()
        {
            return IdFileSystem.GetHashCode();
        }
    }
}
