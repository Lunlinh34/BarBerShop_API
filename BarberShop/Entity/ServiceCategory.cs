using System.ComponentModel.DataAnnotations;

namespace BarberShop.Entity
{
    public class ServiceCategory
    {
        [Key]
        public int serCateID { get; set; }
        public string? serCateName { get; set; }
        public string? description { get; set; }
        public string? ImageUrl { get; set; }


        #region QH
        public ICollection<Service> services { get; set; } = new HashSet<Service>();
        #endregion
    }
}
