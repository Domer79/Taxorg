using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TaxorgRepository.Models
{
    [Table("Bug")]
    public class Bug : ModelBase
    {
        [Key]
        [Column("idBug")]
        public int IdBug { get; set; }

        [Column("data")]
        public string ErrorData { get; set; }

        [Column("cause")]
        public string Cause { get; set; }

        [Column("timeLoad")]
        public DateTime TimeLabel { get; set; }

        [Column("isNotLoaded")]
        public bool IsNotLoaded { get; set; }

        public override int GetHashCode()
        {
            return IdBug.GetHashCode();
        }
    }

    public abstract class ModelBase
    {
        private object _theKey;

        public override int GetHashCode()
        {
            return TheKey.GetHashCode();
        }

        public object TheKey
        {
            get { return _theKey ?? (_theKey = GetKey()); }
        }

        private object GetKey()
        {
            var pi = GetType().GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof (KeyAttribute)));
            if (pi == null)
                throw new KeyNotFoundException();

            return pi.GetValue(this);
        }
    }
}