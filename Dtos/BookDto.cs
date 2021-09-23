using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Dtos
{
    public class BookDtoRequest
    {
        [Required]
        public string Title { get; set; }
        public int? PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
    }
    public class BookDtoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public List<string> Authors { get; set; }
    }
}

