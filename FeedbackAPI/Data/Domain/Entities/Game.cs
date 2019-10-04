using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackAPI.Data.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackAPI.Data.Domain.Entities
{
    public class Game : IGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }

        [Required]
        [MaxLength(50)]
        public string GameName { get; set; }

        [Required]
        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(50)]
        public string Publisher { get; set; }
    }
}
