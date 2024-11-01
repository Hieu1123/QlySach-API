using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySach_API.Model.Entity
{
    public class Role
    {
        public int Id { get; set; }
        public string nameRole { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
