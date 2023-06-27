using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.ServiceModel.Syndication;
using System.Xml;
using Core.DTOs.Account;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Data.CQS.Queries;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Account;
using Services.Article;

namespace Tests
{
    public class SettingsServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IUiThemeService> _uiServiceMock = new Mock<IUiThemeService>();


        private SettingsService CreateService()
        {
            var articleService = new SettingsService(
                _uiServiceMock.Object,
                _mediatorMock.Object
            );

            return articleService;
        }

        [Fact]
        public async void GetUserInformationAsync_WithIncorrectArguments_ReturnCorrectError()
        {
            var articleService = CreateService();

            var result = async () => await articleService
                .GetUserInformationAsync("");

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void GetUserInformationAsync_WithNullUser_ReturnNull()
        {
            
            _mediatorMock.Setup(mediator => mediator.Send(
                It.IsAny<GetUserSettingsByEmailQuery>(), CancellationToken.None))
                .Throws<NullReferenceException>();
            
            
            var articleService = CreateService();

            var result = await articleService
                .GetUserInformationAsync("StringArgument");

            Assert.Null(result);
        }

        public static object userSettingsDTO()
        {
            return new userSettingsDTO(){ Name = "Ferdinant"};
        }
        
        public static IEnumerable<object[]> EmptyArgumentData()
        {
            yield return new object[] { null, "email" };
            yield return new object[] { new userSettingsDTO(){Name = "Ferdinant"}, "" };
        }
        
        [Fact]
        public async void UpdateUserSettingsAsync_WithCorrectArguments_ReturnTrue()
        {

            List<UpdateUserCommand> savedCommands = new List<UpdateUserCommand>();
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateUserCommand>(), CancellationToken.None))
                .Callback<UpdateUserCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);


            var articleService = CreateService();

            var result = await articleService
                .UpdateUserSettingsAsync(new userSettingsDTO() { Name = "Ferdinant" }, "Mail");

            Assert.Single(savedCommands);

            Assert.NotNull(savedCommands[0].userSettingsDTO);

            Assert.NotNull(savedCommands[0].Email.FirstOrDefault());

            Assert.True(result);
        }

        [Fact]
        public async void UpdateUserSettingsAsync_WithNonExistUser_ReturnFalse()
        {

            List<UpdateUserCommand> savedCommands = new List<UpdateUserCommand>();
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<UpdateUserCommand>(), CancellationToken.None))
                .Callback<UpdateUserCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Throws<NullReferenceException>();


            var articleService = CreateService();

            var result = await articleService
                .UpdateUserSettingsAsync(new userSettingsDTO() { Name = "Ferdinant" }, "Mail");

            Assert.False(result);
        }




    }
}