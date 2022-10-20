
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaWeb.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }
        public string Describe { get; set; }
        

    }
}
