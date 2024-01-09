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
    public class PhoneNumberRepository : BaseRepository<PhoneNumber>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }

    public interface IPhoneNumberRepository : IBaseRepository<PhoneNumber>
    {

    }
}
