using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class User
    {
        [Key]
        public int userID { get; set; }
        public string? userName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        #region QH

        // MO role
        public int roleID { get; set; }
        [ForeignKey("roleID")]
        public Role Role { get; set; } = null!;
        // OO
        // customer
        public Customer? Customer { get; set; }
        // employee
        public Employee? Employee { get; set; }

        #endregion
    }
}
