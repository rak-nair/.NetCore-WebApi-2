using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FeedbackAPI.Data.Domain.Interfaces;

namespace FeedbackAPI.Data.Domain.Entities
{
    public class GameSession : IGameSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameSessionID { get; set; }

        [ForeignKey("GameForeignKey")]
        public int GameID { get; set; }

        [Required]
        public DateTime SessionStartTime { get; set; }

        [Required]
        public DateTime SessionEndTime { get; set; }
    }
}
