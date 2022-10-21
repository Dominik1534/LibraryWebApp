using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using BibliotekaWeb.Models;
using LibraryWeb.Models;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Data;
using System.Net;

namespace LibraryWeb
{
    public class BooksSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        public BooksSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            //GetCSV("https://magazyn2022.blob.core.windows.net/newcontainer/books.csv");
            if (!_dbContext.Books.Any())
            {
                var recipes = GetBooksFromCSV();

                _dbContext.Books.AddRange(recipes);
                _dbContext.SaveChanges();
            }
        }
        public string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
        private IEnumerable<Book> GetBooksFromCSV()
        {
            using (var reader = new StreamReader("books.csv"))
            {
                List<string> badRecord = new List<string>();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    Mode = CsvMode.NoEscape,
                    BadDataFound = context => badRecord.Add(context.RawRecord)
                };
                using (var csv = new CsvReader(reader, config))
                {

                    var records = csv.GetRecords<BooksRaw>();
                 
                    List<Book> books = new List<Book>();
                    foreach (var record in records)
                    {
                        books.Add(new Book()
                        {
                            bookID = record.bookID,
                            Name = record.title,
                            Author = record.authors,
                            PublishDate = record.publication_date,
                            Describe = LoremNET.Lorem.Words(8, 15),

                        });
                    }
                   
                    return books;
                }
            }
        }
    }
}
