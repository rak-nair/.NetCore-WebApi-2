using FeedbackAPI.Data.Domain.Interfaces;
using FeedbackAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackAPI.Helpers;
using FeedbackAPI.Data.Domain.Entities;

namespace FeedbackAPI.Services.Interfaces
{
    public interface IDataStore
    {
        Task<FeedbackEntry> AddFeedback(IFeedback feedback);
        Task<PagedList<FeedbackEntry>> ViewSubmittedFeedbackEntries(FeedbackViewResourceParameters parameters);
        Task<FeedbackEntry> ViewSubmittedFeedbackEntryById(int feedbackId);
    }
}
