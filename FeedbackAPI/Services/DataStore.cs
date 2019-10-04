using FeedbackAPI.Data;
using FeedbackAPI.Data.Domain.Entities;
using FeedbackAPI.Data.Domain.Interfaces;
using FeedbackAPI.Exceptions;
using FeedbackAPI.Helpers;
using FeedbackAPI.Models.Output;
using FeedbackAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAPI.Services
{
    public class DataStore : IDataStore
    {
        FeedbackDbContext _context;

        public DataStore(FeedbackDbContext context)
        {
            _context = context;
        }

        public async Task<FeedbackEntry> AddFeedback(IFeedback feedback)
        {
            var playerObject = await _context.Players.FindAsync(feedback.PlayerID);
            if (playerObject == null)
                throw new InvalidPlayerIDException();

            var gameSessionObject = await _context.GameSessions.FindAsync(feedback.GameSessionID);
            if (gameSessionObject == null)
                throw new InvalidGameSessionException();

            var priorFeedback = await _context.Feedback.Where(x =>
                x.GameSessionID == feedback.GameSessionID && x.PlayerID == feedback.PlayerID).FirstOrDefaultAsync();
            if (priorFeedback != null)
                throw new DuplicateFeedbackException(priorFeedback.SubmissionDate);

            var gameObject =
                await _context.Games.Where(x => x.GameID == gameSessionObject.GameID).FirstOrDefaultAsync();

            _context.Feedback.Add(feedback as Feedback);
            await _context.SaveChangesAsync();

            return new FeedbackEntry
            {
                Feedback = feedback as Feedback,
                Gamer = playerObject,
                GameSession = gameSessionObject,
                Game = gameObject
            };
        }

        public async Task <PagedList<FeedbackEntry>> ViewSubmittedFeedbackEntries(FeedbackViewResourceParameters parameters)
        {
            var responseEntries =
                await Task.FromResult(
                _context.Feedback.Where(x =>
                        parameters.FeedbackScore == null || x.FeedbackScore == parameters.FeedbackScore)
                    .OrderByDescending(x => x.SubmissionDate));
            return PagedList<FeedbackEntry>.Create<IFeedback>(responseEntries, parameters.PageNumber, parameters.PageSize,ConstructFeedbackEntry);
        }

        public async Task<FeedbackEntry> ViewSubmittedFeedbackEntryById(int feedbackId)
        {
            var feedback = await _context.Feedback.FindAsync(feedbackId);

            if (feedback != null)
            {
                return ConstructFeedbackEntry(feedback);
            }
            else
            {
                return null;
            }
        }

        FeedbackEntry ConstructFeedbackEntry(IFeedback feedback)
        {
            var feedbackEntry = new FeedbackEntry
            {
                Feedback = feedback as Feedback,
                GameSession = _context.GameSessions.Find(feedback.GameSessionID),
                Gamer = _context.Players.Find(feedback.PlayerID)
            };

            feedbackEntry.Game = _context.Games.Find(feedbackEntry.GameSession.GameID);

            return feedbackEntry;
        }
    }
}
