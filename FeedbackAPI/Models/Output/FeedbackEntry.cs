using FeedbackAPI.Data.Domain.Entities;

namespace FeedbackAPI.Models.Output
{
    public class FeedbackEntry
    {
        public Feedback Feedback { get; set; }
        public Player Gamer { get; set; }
        public GameSession GameSession { get; set; }
        public Game Game { get; set; }
    }
}
