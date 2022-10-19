namespace LibraryWeb.Models
{
    public class BooksRaw
    {
        public int bookID { get; set; }
        public string title { get; set; }
        public string authors { get; set; }
        public float average_rating { get; set; }
        public string isbn { get; set; }
        public string isbn13 { get; set; }
        public string language_code { get; set; }
        public int num_pages { get; set; }
        public string ratings_count { get; set; }
        public string text_reviews_count { get; set; }
        public DateTime publication_date { get; set; }
        public string publisher { get; set; }
             
    }
}
