using FeedbackAPI.Data.Domain.Entities;
using FeedbackAPI.Data.Domain.Interfaces;
using FeedbackAPI.Models.Input;
using System;

namespace FeedbackAPI.Models
{
    public class ModelFactory
    {
        public IFeedback CreateFeedback(FeedbackViewModel feedbackViewModel)
        {
            return new Feedback
            {
                FeedbackComment = feedbackViewModel.FeedbackComment,
                FeedbackScore = feedbackViewModel.FeedbackScore,
                SubmissionDate = DateTime.Now,
                PlayerID = feedbackViewModel.PlayerID,
                GameSessionID = feedbackViewModel.GameSessionID
            };
        }
    }
}
