using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAPI.Services.ResponseMessages
{
    public class Responses
    {
        public const string INVALID_GAME_SESSION_ID = "Invalid Game Session ID supplied.";
        public const string INVALID_PLAYER_ID = "Invalid Player ID supplied.";
        public const string INVALID_FEEDBACK_ALREADY_ENTERED = "This combination of Player and Game Session previously submitted feedback at - ";
        public const string INVALID_FEEDBACK_INPUTS = "Inputs supplied when creating a new Feedback entry.";
    }
}
