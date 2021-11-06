using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_web_api.Dtos
{
    public class AuthorRequestDto
    {
        public int? Id { get; set; }
        [Required]
        [RegularExpression("[a-zA-Z .]{2,}")]
        public string Name { get; set; }
    }
    public class AuthorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Books { get; set; }
    }
}
