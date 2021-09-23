using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Dtos
{
    public class PublisherDtoRequest
    {
        [Required]
        public string Name { get; set; }
    }
    public class PublisherDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Books { get; set; }
    }
}
