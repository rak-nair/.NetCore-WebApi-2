using FeedbackAPI.Data.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackAPI.Data.Domain.Entities
{
    public class Player : IPlayer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerID { get; set; }

        [Required]
        public string ScreenName { get; set; }
    }
}
