using System.ComponentModel.DataAnnotations;

namespace FeedbackAPI.Models.Input
{
    public class GameSessionViewModel
    {
        [Required, MaxLength(50)]
        public string GameName { get; set; }
    }
}
