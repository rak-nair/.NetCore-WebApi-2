using FeedbackAPI.Services.ResponseMessages;
using System;

namespace FeedbackAPI.Exceptions
{
    public class InvalidPlayerIDException : Exception
    {
        public InvalidPlayerIDException() : base(Responses.INVALID_PLAYER_ID)
        {

        }
    }
}
