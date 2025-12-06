using System.ComponentModel.DataAnnotations;

namespace BarberShop.Entity
{
    public class Notification
    {
        [Key]
        public int notiID { get; set; }
        public string? notiTitle { get; set; }
        public string? notiContent { get; set; }
        public DateTime lastUpdate { get; set; } = DateTime.Now;



        #region QH

        //OneMany CusNoti
        public ICollection<CustomerNotification> customerNotifications { get; set; } = new List<CustomerNotification>();

        #endregion
    }
}
