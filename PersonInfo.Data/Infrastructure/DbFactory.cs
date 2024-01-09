using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        public PersonInfoContext Init()
        {
            return dbContext ?? (dbContext = new PersonInfoContext(options));
        }

        PersonInfoContext? dbContext;

        private readonly DbContextOptions<PersonInfoContext> options;

        public DbFactory(DbContextOptions<PersonInfoContext> options)
        {
            this.options = options;
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
