using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.ServiceModel.Syndication;
using System.Xml;
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
    public class UserRoleServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IConfiguration> _ñonfigurationMock = new Mock<IConfiguration>();


        private RoleService CreateService()
        {
            var RoleService = new RoleService(
                _ñonfigurationMock.Object,
                _mediatorMock.Object
            );

            return RoleService;
        }

        [Fact]
        public async void InitiateDefaultRolesAsync_WithNullFromConfigurationFile_ReturnCorrectError()
        {
            _ñonfigurationMock.Setup(configurationFile => configurationFile["Roles:all"])
                .Returns("");

            var roleService = CreateService();

            var result = async () => await roleService.InitiateDefaultRolesAsync();

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async void GetUserRolesByUserIdAsync_WithIncorrectId_CorrectError(Int32 id)
        {
            var UiThemeService = CreateService();

            var result = async () => await UiThemeService.GetUserRolesByUserIdAsync(id);

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void GetDefaultRoleAsync_WithNullFromGetDefaultUserRoleIdQuery_CorrectError()
        {
            _ñonfigurationMock.Setup(configurationFile => configurationFile["Roles:default"])
                .Returns("User");

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<String>(), CancellationToken.None))
                .ReturnsAsync(null);

            _ñonfigurationMock.Setup(configurationFile => configurationFile["Roles:all"])
                .Returns("User");

            var uiThemeService = CreateService();


            var result = async () => await uiThemeService.GetDefaultRoleAsync();

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }



        
    }
}