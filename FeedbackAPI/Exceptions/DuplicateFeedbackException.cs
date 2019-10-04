using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackAPI.Services.ResponseMessages;

namespace FeedbackAPI.Exceptions
{
    public class DuplicateFeedbackException: Exception
    {
        public DuplicateFeedbackException(DateTime originalFeedbackSubmissionTime): 
            base($"{Responses.INVALID_FEEDBACK_ALREADY_ENTERED} {originalFeedbackSubmissionTime}")
        {
            
        }
    }
}
