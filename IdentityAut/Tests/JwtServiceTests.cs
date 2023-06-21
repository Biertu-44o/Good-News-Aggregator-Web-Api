using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.ServiceModel.Syndication;
using System.Xml;
using Core.DTOs.Article;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Account;
using Services.Article;

namespace Tests
{
    public class JwtServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IConfiguration> _ñonfigurationMock = new Mock<IConfiguration>();


        private JwtService CreateService()
        {
            var jwtService = new JwtService(
                _ñonfigurationMock.Object,
                _mediatorMock.Object,
                _userServiceMock.Object
            );

            return jwtService;
        }

        public static IEnumerable<object[]> ClaimData()
        {
            yield return new object[] { new List<Claim>() };
        }

        [Theory]
        [MemberData(nameof(ClaimData))]
        [InlineData(null)]
        public async void GetJwtTokenString_WithNullOrEmptyClaims_ReturnCorrectError( List<Claim> claims)
        {

            var jwtService = CreateService();

            var result = async () => await jwtService
                .GetJwtTokenString(new List<Claim>());

            await Assert.ThrowsAnyAsync<InvalidOperationException>(result);
        }

        [Fact]
        public async void GetJwtTokenString_WithNullFromConfigurationFile_ReturnCorrectError()
        {
            _ñonfigurationMock.Setup(configurationFile => 
                    configurationFile["Jwt:SecurityKey"])
                .Returns("");

            var jwtService = CreateService();

            var result = async () => await jwtService
                .GetJwtTokenString(new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "ClaimStr"),
                });

            await Assert.ThrowsAnyAsync<InvalidOperationException>(result);
        }

        [Fact]
        public async void GetJwtTokenString_WithCorrectData_ReturnNotNullToken()
        {
            _ñonfigurationMock.Setup(configurationFile => 
                    configurationFile["Jwt:SecurityKey"])
                .Returns("ivoMHRcluL+Blg9VZ9XYHwWeZ+bHsKcV8zr0ciLuzac=");

            _ñonfigurationMock.Setup(configurationFile => 
                    configurationFile["Jwt:Issuer"])
                .Returns("AspNetSamples.WebAPI");
            
            _ñonfigurationMock.Setup(configurationFile => 
                    configurationFile["Jwt:Audience"])
                .Returns("https://localhost:7100/");

            _ñonfigurationMock.Setup(configurationFile => 
                    configurationFile["ExpireInMinutes"])
                .Returns("10");

            var jwtService = CreateService();

            var result = await jwtService
                .GetJwtTokenString(new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "ClaimStr"),
                });

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void AddRefreshTokenAsync_WithNullUserFromUserService_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(new GetUserByEmailQuery(), CancellationToken.None))!
                .ReturnsAsync(new User(){Name = "dsad", Email = "asdad"});

            _userServiceMock.Setup(userService => 
                    userService.GetUserByEmailAsync(It.IsAny<String>()))!
                .ReturnsAsync((User?)null);

            var jwtService = CreateService();

            var result = async () => await jwtService
                .AddRefreshTokenAsync(It.IsAny<String>(),It.IsAny<Guid>());

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }

        [Fact]
        public async void RefreshTokenAsync_WithEmptyUserClaims_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator=>mediator.Send(It.IsAny<GetUserByRefreshTokenQuery>(), CancellationToken.None))!
                .ReturnsAsync(new User(){Name = "Ferdinant"});

            _userServiceMock.Setup(userService => userService.GetUserClaimsAsync(
                    It.IsAny<String>()))
                .ReturnsAsync(new List<Claim>());
            
            var jwtService = CreateService();

            var result = async () => await jwtService
                .RefreshTokenAsync( It.IsAny<Guid>());

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }

        [Fact]
        public async void RefreshTokenAsync_WithNullUser_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByRefreshTokenQuery>(), CancellationToken.None))!
                .ReturnsAsync((User?)null);

            var jwtService = CreateService();

            var result = async () => await jwtService
                .RefreshTokenAsync(It.IsAny<Guid>());

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }

        [Fact]
        public async void RefreshTokenAsync_WithCorrectData_ReturnNotEmptyToken()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByRefreshTokenQuery>(), CancellationToken.None))!
                .ReturnsAsync(new User() { Email = "Ferdinant" });


            _userServiceMock.Setup(UserService => UserService.GetUserClaimsAsync(It.IsAny<String>()))
                .ReturnsAsync(new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "ClaimStr"),
                });

            _ñonfigurationMock.Setup(configurationFile =>
                    configurationFile["Jwt:SecurityKey"])
                .Returns("ivoMHRcluL+Blg9VZ9XYHwWeZ+bHsKcV8zr0ciLuzac=");

            _ñonfigurationMock.Setup(configurationFile =>
                    configurationFile["Jwt:Issuer"])
                .Returns("AspNetSamples.WebAPI");

            _ñonfigurationMock.Setup(configurationFile =>
                    configurationFile["Jwt:Audience"])
                .Returns("https://localhost:7100/");

            _ñonfigurationMock.Setup(configurationFile =>
                    configurationFile["ExpireInMinutes"])
                .Returns("10");
            
            _userServiceMock.Setup(userService =>
                    userService.GetUserByEmailAsync(It.IsAny<String>()))!
                .ReturnsAsync(new User(){Name = "Ferdinant"});

            var jwtService = CreateService();

            var result = await jwtService
                .RefreshTokenAsync(It.IsAny<Guid>());

            Assert.NotEmpty(result.RefreshToken);
            Assert.NotEmpty(result.JwtToken);
        }

    }
}