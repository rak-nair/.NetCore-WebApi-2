using FeedbackAPI.Data.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackAPI.Data.Domain.Entities
{
    public class Feedback : IFeedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }

        [Required]
        [ForeignKey("UserForeignKey")]
        public int PlayerID { get; set; }

        [Required]
        [ForeignKey("SessionForeignKey")]
        public int GameSessionID { get; set; }

        [Required]
        public int FeedbackScore { get; set; }

        [MaxLength(500)]
        public string FeedbackComment { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }
    }
}
