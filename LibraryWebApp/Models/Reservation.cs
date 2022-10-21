using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryWeb.Models
{
    public class Reservation
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DisplayName("Book Id")]

        public int BookId { get; set; }
        [DisplayName("User ID")]
        public string UserId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        
    }
}
