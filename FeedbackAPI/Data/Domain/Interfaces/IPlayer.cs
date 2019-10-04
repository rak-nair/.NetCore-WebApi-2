using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAPI.Data.Domain.Interfaces
{
    public interface IPlayer
    {
        int PlayerID { get; set; }
        string ScreenName { get; set; }
    }
}
