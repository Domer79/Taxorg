using System;
using TaxorgRepository.Models;

namespace EfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = TaxorgContext.Context;

            foreach (var org in context.Organizations)
            {
                Console.WriteLine(org);
            }

            Console.ReadKey();
        }
    }
}
