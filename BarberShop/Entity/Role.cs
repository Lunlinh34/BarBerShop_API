using System.ComponentModel.DataAnnotations;

namespace BarberShop.Entity
{
    public class Role
    {
        [Key]
        public int roleID { get; set; }
        public string? roleName { get; set; }

        #region QH

        // OM user
        public ICollection<User> users { get; set; } = new HashSet<User>();
        #endregion
    }
}
