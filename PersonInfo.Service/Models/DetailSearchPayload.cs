using PersonInfo.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Service.Models
{
    public class DetailSearchPayload
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PIN { get; set; }
        public Gender Gender { get; set; }
        public DateTime DoB { get; set; }
        public string? PhoneNumber { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
