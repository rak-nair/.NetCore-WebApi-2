using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Models.Input
{
    public class UserViewModel
    {
        [Required, MaxLength(30)]
        public string ScreenName { get; set; }
    }
}
