using PersonInfo.Data.Infrastructure;
using PersonInfo.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }

    public interface IPersonRepository : IBaseRepository<Person>
    {

    }
}
