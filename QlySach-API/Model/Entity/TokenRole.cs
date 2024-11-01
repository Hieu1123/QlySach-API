namespace QlySach_API.Model.Entity
{
    public class TokenRole
    {
        public int Id { get; set; }
        public int roleId { get; set; }
        public string jwtToken  { get; set; }
        public virtual Role? Role { get; set; }
    }
}
