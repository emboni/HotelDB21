using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPageHotelApp.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) {
            
        }
    }
}
