using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Dtos
{
    public class PublisherRequestDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    public class PublisherResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Books { get; set; }
    }
}
