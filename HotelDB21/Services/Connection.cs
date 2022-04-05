using System;
using System.Collections.Generic;
using System.Text;

namespace HotelDBConsole21.Services
{
    public abstract class Connection
    {
        //indsæt din egen connectionstring
        //protected String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Hotel02032020;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //protected String connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Hotel250322; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        protected String connectionString = @"Data Source = mssql3.unoeuro.com; Initial Catalog = embo_zealand_dk_db_emil; User ID = embo_zealand_dk; Password=h69BryFf5dGD;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}

