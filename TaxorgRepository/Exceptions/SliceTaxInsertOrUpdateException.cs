using System;

namespace TaxorgRepository.Exceptions
{
    public class SliceTaxInsertOrUpdateException : Exception
    {
        public SliceTaxInsertOrUpdateException()
        {
        }

        public SliceTaxInsertOrUpdateException(string message) 
            : base(message)
        {
        }
    }
}