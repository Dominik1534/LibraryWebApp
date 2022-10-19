using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models
{
    public class Reservation
    {
       
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        
    }
}
