using FeedbackAPI.Services.ResponseMessages;
using System;

namespace FeedbackAPI.Exceptions
{
    public class InvalidGameSessionException : Exception
    {
        public InvalidGameSessionException() : base(Responses.INVALID_GAME_SESSION_ID)
        {

        }
    }
}
