using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public Publisher Publisher { get; set; }
    }
}
