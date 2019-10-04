using FeedbackAPI.Exceptions;
using FeedbackAPI.Helpers;
using FeedbackAPI.Models;
using FeedbackAPI.Models.Input;
using FeedbackAPI.Models.Output;
using FeedbackAPI.Services.Interfaces;
using FeedbackAPI.Services.ResponseMessages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FeedbackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IDataStore _dataStore;
        private readonly ModelFactory _modelFactory;
        private readonly IUrlHelper _urlHelper;

        public FeedbackController(IDataStore dataStore, ModelFactory factory, IUrlHelper urlHelper)
        {
            _dataStore = dataStore;
            _modelFactory = factory;
            _urlHelper = urlHelper;
        }

        #region Endpoints
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Welcome to the Feedback service!");
        }

        [HttpPost("submit", Name = "SubmitFeedback")]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackViewModel feedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                var feedbackObject = _modelFactory.CreateFeedback(feedbackViewModel);

                try
                {
                    var feedbackEntry = await _dataStore.AddFeedback(feedbackObject);
                    return CreatedAtRoute("SubmitFeedback", feedbackEntry);
                }
                catch (InvalidGameSessionException)
                {
                    return BadRequest(Responses.INVALID_GAME_SESSION_ID);
                }
                catch (InvalidPlayerIDException)
                {
                    return BadRequest(Responses.INVALID_PLAYER_ID);
                }
                catch (DuplicateFeedbackException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(Responses.INVALID_FEEDBACK_INPUTS);
            }
        }

        [HttpGet("view", Name = "ViewFeedbackEntries")]
        public async Task<IActionResult> ViewFeedbackEntries([FromQuery]FeedbackViewResourceParameters parameters)
        {
            PagedList<FeedbackEntry> feedbackEntries = null;
            feedbackEntries = await _dataStore.ViewSubmittedFeedbackEntries(parameters);

            var paginationMetadata = CreatePaginationMetadata(feedbackEntries, parameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(feedbackEntries);
        }

        [HttpGet("view/{feedbackId}", Name = "ViewFeedbackEntry")]
        public async Task<IActionResult> ViewFeedbackEntryById(int feedbackId)
        {
            var feedbackEntry = await _dataStore.ViewSubmittedFeedbackEntryById(feedbackId);
            return Ok(feedbackEntry);
        }
        #endregion

        #region Private Methods
        object CreatePaginationMetadata(PagedList<FeedbackEntry> feedbackEntries, FeedbackViewResourceParameters parameters)
        {
            var nextPageLink = feedbackEntries.HasNextPage
                ? CreateFeedbackResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var previousPageLink = feedbackEntries.HasPreviousPage
                ? CreateFeedbackResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            return new
            {
                totalRecords = feedbackEntries.TotalRecordCount,
                currentPage = feedbackEntries.CurrentPage,
                pageSize = feedbackEntries.PageSize,
                totalPages = feedbackEntries.TotalPages,
                nextPageLink,
                previousPageLink
            };
        }

        string CreateFeedbackResourceUri(FeedbackViewResourceParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("ViewFeedbackEntries",
                        CreateRouteValues(parameters.PageNumber + 1, parameters.PageSize, parameters.FeedbackScore));
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("ViewFeedbackEntries",
                        CreateRouteValues(parameters.PageNumber - 1, parameters.PageSize, parameters.FeedbackScore));
                default:
                    return _urlHelper.Link("ViewFeedbackEntries",
                        CreateRouteValues(parameters.PageNumber, parameters.PageSize, parameters.FeedbackScore));
            }
        }

        object CreateRouteValues(int pageNumber, int pageSize, int? feedbackScore)
        {
            if (feedbackScore.HasValue)
            {
                return new
                {
                    FeedbackScore = feedbackScore.Value,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            else
            {
                return new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        #endregion
    }
}