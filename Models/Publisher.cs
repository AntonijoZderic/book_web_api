using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
