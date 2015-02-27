using System;

namespace TaxorgRepository.Exceptions
{
    public class SaveTaxException : Exception
    {
        public SaveTaxException(string errorStr)
            : base(errorStr)
        {
            
        }
    }
}