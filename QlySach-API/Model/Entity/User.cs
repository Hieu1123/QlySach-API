using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySach_API.Model.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
