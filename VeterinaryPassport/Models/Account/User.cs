using VeterinaryPassport.Models.Account;

namespace VeterinaryPassport.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public int? VetId { get; set; }
        public Vet? Vet { get; set; }
    }
}
