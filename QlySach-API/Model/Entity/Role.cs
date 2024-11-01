using QlySach_API.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlySach_API.Model.Entity
{
    public enum Functionality 
    { 
        View,
        Add,
        Delete,
        Edit,
        getById,
        ViewPage
    }
    public class Role
    {
        public int Id { get; set; }
        public string nameRole { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public List<Functionality> functionalities { get; set; } = new List<Functionality>();

        public Role() { }

        public Role(string nameRole, List<Functionality> functionalities) 
        {
            this.nameRole = nameRole;
            this.functionalities = functionalities;
        }
    }
    public class RoleDefinitions
    {
        public static Role AdminRole = new Role( "admin", new List<Functionality>
        {
            Functionality.View,
            Functionality.Add,
            Functionality.Delete,
            Functionality.Edit,
            Functionality.getById,
            Functionality.ViewPage
        });
        public static Role UserRole = new Role("User", new List<Functionality>
        {
            Functionality.View,
            Functionality.ViewPage
        });
    }
}