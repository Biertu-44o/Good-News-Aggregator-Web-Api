using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.ServiceModel.Syndication;
using System.Xml;
using AutoMapper;
using Core.DTOs.Account;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Services.Account;
using Services.Article;

namespace Tests
{
    public class UiThemeServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IConfiguration> _confMock = new Mock<IConfiguration>();
        private readonly Mock<IUiThemeService> _uiThemeMock = new Mock<IUiThemeService>();


        private UiThemeService CreateService()
        {
            var uiThemeService = new UiThemeService(
                _confMock.Object,
                _mediatorMock.Object
            );

            return uiThemeService;
        }

        [Fact]
        public async void InitiateThemeAsync_WithNullFromConfigurationFile_ReturnCorrectError()
        {
            _confMock.Setup(configurationFile => configurationFile["Themes:all"])
                .Returns("");

            var UiThemeService = CreateService();

            var result = async () => await UiThemeService.InitiateThemeAsync();

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void GetIdDefaultThemeAsync_WithNullFromConfigurationFile_CorrectError()
        {
            _confMock.Setup(configurationFile => configurationFile["Themes:all"])
                .Returns("");

            var UiThemeService = CreateService();

            var result = async () => await UiThemeService.InitiateThemeAsync();

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void GetIdDefaultThemeAsync_WithEmptyThemeId_CorrectError()
        {
            _confMock.Setup(configurationFile => configurationFile["Themes:default"])
                .Returns("User");
            
            _confMock.Setup(configurationFile => configurationFile["Themes:all"])
                .Returns("User Admin");

            _mediatorMock.Setup(mediator => mediator.Send(new GetDefaultThemeIdQuery(), CancellationToken.None))
                .ReturnsAsync(0);

            var uiThemeService = CreateService();


            var result = async () => await uiThemeService.GetIdDefaultThemeAsync();

            await Assert.ThrowsAnyAsync<InvalidOperationException>(result);
        }


    }
}
