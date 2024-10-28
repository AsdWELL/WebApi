using System.ComponentModel.DataAnnotations;

namespace SportNewsWebApi.Requests
{
    public class CreateSportNewsRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }
    }
}
