using System;

namespace FeedbackAPI.Data.Domain.Interfaces
{
    public interface IFeedback
    {
        int FeedbackID { get; set; }
        int PlayerID { get; set; }
        int GameSessionID { get; set; }
        int FeedbackScore { get; set; }
        string FeedbackComment { get; set; }
        DateTime SubmissionDate { get; set; }
    }
}
