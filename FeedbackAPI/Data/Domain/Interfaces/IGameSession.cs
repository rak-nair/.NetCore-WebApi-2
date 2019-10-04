using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAPI.Data.Domain.Interfaces
{
    public interface IGameSession
    {
        int GameSessionID { get; set; }
        int GameID { get; set; }
        DateTime SessionStartTime { get; set; }
        DateTime SessionEndTime { get; set; }
    }
}
