using FeedbackAPI.Controllers;
using FeedbackAPI.Data.Domain.Entities;
using FeedbackAPI.Data.Domain.Interfaces;
using FeedbackAPI.Exceptions;
using FeedbackAPI.Helpers;
using FeedbackAPI.Models;
using FeedbackAPI.Models.Input;
using FeedbackAPI.Models.Output;
using FeedbackAPI.Services.Interfaces;
using FeedbackAPI.Services.ResponseMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FeedbackAPI.Tests
{
    public class FeedbackControllerShould
    {
        FeedbackController _sut;
        Mock<IDataStore> _mockDataStore;
        Mock<ModelFactory> _mockDataFactory;
        Mock<IUrlHelper> _mockUrlHelper;

        public FeedbackControllerShould()
        {
            _mockDataStore = new Mock<IDataStore>();
            _mockDataFactory = new Mock<ModelFactory>();
            _mockUrlHelper = new Mock<IUrlHelper>();
            _sut = new FeedbackController(_mockDataStore.Object, _mockDataFactory.Object, _mockUrlHelper.Object);
        }

        [Fact]
        public async void AddFeedback_ValidInput_Success()
        {
            _mockDataStore.Setup(x => x.AddFeedback(It.IsAny<IFeedback>()))
                .ReturnsAsync(new FeedbackEntry
                { Feedback = new Feedback() { FeedbackID = 1 } });

            var result = await _sut.AddFeedback(new FeedbackViewModel() { FeedbackScore = 5, GameSessionID = 1, PlayerID = 1 });

            var createdAtRouteResult = result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);
            Assert.IsAssignableFrom<FeedbackEntry>(createdAtRouteResult.Value);
            Assert.Equal(1, ((FeedbackEntry)createdAtRouteResult.Value).Feedback.FeedbackID);
        }

        [Fact]
        public async void AddFeedback_InvalidGameSessionID_BadRequest()
        {
            _mockDataStore.Setup(x => x.AddFeedback(It.Is((IFeedback y) => y.GameSessionID == 100)))
                .ThrowsAsync(new InvalidGameSessionException());

            var result =
                await _sut.AddFeedback(new FeedbackViewModel() { FeedbackScore = 5, GameSessionID = 100, PlayerID = 1 });

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(Responses.INVALID_GAME_SESSION_ID, ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async void AddFeedback_InvalidPlayerID_BadRequest()
        {
            _mockDataStore.Setup(x => x.AddFeedback(It.Is((IFeedback y) => y.PlayerID == 100)))
                .ThrowsAsync(new InvalidPlayerIDException());

            var result =
                await _sut.AddFeedback(new FeedbackViewModel() { FeedbackScore = 5, GameSessionID = 1, PlayerID = 100 });

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(Responses.INVALID_PLAYER_ID, ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async void AddFeedback_DuplicateFeedback_BadRequest()
        {
            var originalFeedbackSubmissionTime = DateTime.Now.AddDays(-50);
            _mockDataStore.Setup(x => x.AddFeedback(It.Is((IFeedback y) => y.PlayerID == 1 && y.GameSessionID == 1)))
                .ThrowsAsync(new DuplicateFeedbackException(originalFeedbackSubmissionTime));

            var result =
                await _sut.AddFeedback(new FeedbackViewModel() { FeedbackScore = 5, GameSessionID = 1, PlayerID = 1 });

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{Responses.INVALID_FEEDBACK_ALREADY_ENTERED} {originalFeedbackSubmissionTime}",
                ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async void ViewFeedback_NonFiltered_ReturnsAllFeedbackEntries()
        {
            FeedbackViewResourceParameters parameters = new FeedbackViewResourceParameters();
            int feedbackID = 1;

            _mockDataStore.Setup(x => x.ViewSubmittedFeedbackEntries(parameters)).ReturnsAsync(
                PagedList<FeedbackEntry>.Create(new List<FeedbackEntry>
                {
                    new FeedbackEntry
                    {
                        Feedback = new Feedback {FeedbackID = feedbackID},
                        Game = new Game{GameID = 1},
                        Gamer = new Player{PlayerID = 1},
                        GameSession = new GameSession{GameSessionID = 1}
                    }
                }.AsQueryable().OrderBy(x => x), 1, 1, x => x));

            SetUpControllerContext();

            var result = await _sut.ViewFeedbackEntries(parameters);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PagedList<FeedbackEntry>>(((OkObjectResult)result).Value);
            Assert.Single((List<FeedbackEntry>)((OkObjectResult)result).Value);

            var feedbackEntries = (List<FeedbackEntry>)((OkObjectResult)result).Value;
            Assert.Equal(feedbackID, feedbackEntries[0].Feedback.FeedbackID);
        }

        [Fact]
        public async void ViewFeedback_Filtered_ReturnsFilteredFeedbackEntries()
        {
            FeedbackViewResourceParameters parameters = new FeedbackViewResourceParameters { FeedbackScore = 4 };
            int feedbackID = 2;

            _mockDataStore.Setup(x => x.ViewSubmittedFeedbackEntries(parameters)).ReturnsAsync(
                PagedList<FeedbackEntry>.Create(new List<FeedbackEntry>
                {
                    new FeedbackEntry
                        {Feedback = new Feedback {FeedbackID = feedbackID, FeedbackScore = parameters.FeedbackScore.Value}}
                }.AsQueryable().OrderBy(x => x), 1, 1, x => x));

            SetUpControllerContext();

            var result = await _sut.ViewFeedbackEntries(parameters);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PagedList<FeedbackEntry>>(((OkObjectResult)result).Value);
            Assert.Single((List<FeedbackEntry>)((OkObjectResult)result).Value);

            var feedbackEntries = (List<FeedbackEntry>)((OkObjectResult)result).Value;
            Assert.Equal(feedbackID, feedbackEntries[0].Feedback.FeedbackID);
        }

        [Fact]
        public async void ViewFeedback_ByID_ReturnsFeedbackEntry()
        {
            int feedbackID = 1;

            _mockDataStore.Setup(x => x.ViewSubmittedFeedbackEntryById(feedbackID))
                .ReturnsAsync(new FeedbackEntry { Feedback = new Feedback { FeedbackID = feedbackID } });

            var result = await _sut.ViewFeedbackEntryById(feedbackID);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<FeedbackEntry>(((OkObjectResult)result).Value);

            var feedbackEntry = (FeedbackEntry)((OkObjectResult)result).Value;
            Assert.Equal(feedbackID, feedbackEntry.Feedback.FeedbackID);
        }

        void SetUpControllerContext()
        {
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();
        }
    }
}
