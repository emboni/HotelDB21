using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RazorPageHotelApp.Services
{
    public abstract class Connection
    {
        protected String connectionString;// = @"Data Source = mssql3.unoeuro.com; Initial Catalog = embo_zealand_dk_db_emil; User ID = embo_zealand_dk; Password=h69BryFf5dGD;Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        //protected String connectionString;
        public IConfiguration Configuration { get; }

        public Connection(string costring)
        {
            this.connectionString = costring;
        }

        public Connection(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

    }

}
