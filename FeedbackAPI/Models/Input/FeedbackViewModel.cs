using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAPI.Models.Input
{
    public class FeedbackViewModel
    {
        [Required]
        public int PlayerID { get; set; }

        [Required]
        public int GameSessionID { get; set; }

        [Required, Range(1,5)]
        public int FeedbackScore { get; set; }

        [MaxLength(500)]
        public string FeedbackComment { get; set; }
    }
}
