namespace FeedbackAPI.Data.Domain.Interfaces
{
    public interface IGame
    {
        int GameID { get; set; }
        string GameName { get; set; }
        int ReleaseYear { get; set; }
        string Publisher { get; set; }
    }
}
