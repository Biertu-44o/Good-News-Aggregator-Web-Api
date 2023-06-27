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
    public class UserServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        private readonly Mock<IUiThemeService> _uiServiceMock = new Mock<IUiThemeService>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IRoleService> _roleServiceMock = new Mock<IRoleService>();


        private UserService CreateService()
        {
            var userService = new UserService(
                _mediatorMock.Object,
                _mapperMock.Object,
                _uiServiceMock.Object,
                _roleServiceMock.Object
            );

            return userService;
        }

        [Fact]
        public async void IsUserExistAsync_WithInCorrectEmail_ReturnCorrectError()
        {

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<IsUserExistByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(It.IsAny<Boolean>());

            var userService = CreateService();

            var result = async () => await userService
                .RegistrationAsync(new UserRegistrationDto() { Email = "" });

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }


        [Fact]
        public async void RegistrationAsync_WithCorrectUserDto_ReturnTrue()
        {

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<IsUserExistByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(false);

            _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns(new User(){Email = "email",Password = "password"});

            _uiServiceMock.Setup(_uiService => _uiService.GetIdDefaultThemeAsync())
                .ReturnsAsync(1);

            _roleServiceMock.Setup(roleService => roleService.GetDefaultRoleAsync())
                .ReturnsAsync(new UserRole() { Role = "User"});

            List<CreateUserWithRoleCommand> savedCommands = new List<CreateUserWithRoleCommand>();
            
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateUserWithRoleCommand>(), CancellationToken.None))
                .Callback<CreateUserWithRoleCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);

            var userService = CreateService();

            var result = await userService
                .RegistrationAsync( new UserRegistrationDto() { Email = "email", Password = "password" });

            Assert.True(result);
        }

        [Fact]
        public async void RegistrationAsync_WithExistentUser_ReturnFalse()
        {

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<IsUserExistByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(true);

            _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns(new User() { Email = "email", Password = "password" });

            _uiServiceMock.Setup(_uiService => _uiService.GetIdDefaultThemeAsync())
                .ReturnsAsync(1);

            _roleServiceMock.Setup(roleService => roleService.GetDefaultRoleAsync())
                .ReturnsAsync(new UserRole() { Role = "User" });

            List<CreateUserWithRoleCommand> savedCommands = new List<CreateUserWithRoleCommand>();

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateUserWithRoleCommand>(), CancellationToken.None))
                .Callback<CreateUserWithRoleCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);

            var userService = CreateService();

            var result = await userService
                .RegistrationAsync(new UserRegistrationDto() { Email = "email", Password = "password" });

            Assert.False(result);
        }

        [Fact]
        public async void RegistrationAsync_WithNullRoleFromRoleService_ReturnCorrectError()
        {

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<IsUserExistByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(false);

            _mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns(new User() { Email = "email", Password = "password" });

            _uiServiceMock.Setup(_uiService => _uiService.GetIdDefaultThemeAsync())
                .ReturnsAsync(1);

            _roleServiceMock.Setup(roleService => roleService.GetDefaultRoleAsync())
                .ReturnsAsync((UserRole?)null);

            List<CreateUserWithRoleCommand> savedCommands = new List<CreateUserWithRoleCommand>();

            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateUserWithRoleCommand>(), CancellationToken.None))
                .Callback<CreateUserWithRoleCommand, CancellationToken>((command, cancellationToken) =>
                {
                    savedCommands.Add(command);
                })
                .Returns(Task.CompletedTask);

            var userService = CreateService();

            var result = async () => await userService
                .RegistrationAsync(new UserRegistrationDto() { Email = "email", Password = "password" });

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

        [Fact]
        public async void LoginAsync_WithCorrectUserDto_ReturnTrue()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User() { Password = @"$2a$11$H0U4DXQ3LIrHvqEijRjTa.TpQOF0ldouMye9Hswu9lgf6b1CKbNNe"});

            var userService = CreateService();

            var result =  await userService
                .LoginAsync(new UserLoginDto() { Email = "email", Password = "Qwer123!!" });

            Assert.True(result); 
        }


        [Fact]
        public async void LoginAsync_WithNoneExistUser_ReturnFalse()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User() { Password = @"$2a$11$H0U4DXQ3LIrHvqEijRjTa.TpQOF0ldouMye9Hswu9lgf6b1CKbNNe" });

            var userService = CreateService();

            var result = await userService
                .LoginAsync(new UserLoginDto() { Email = "email", Password = "Qwer12!!" });

            Assert.False(result);
        }

        [Fact]
        public async void LoginAsync_WithIncorrectUser_ReturnFalse()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .Throws(new NullReferenceException()); 

            var userService = CreateService();

            var result = await userService
                .LoginAsync(new UserLoginDto() { Email = "email", Password = "Qwer123!!" });

            Assert.False(result);
        }


        [Fact]
        public async void GetUserClaimsAsync_WithCorrectUser_ReturnCorrectClaimList()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User(){Email = "mail"});

            _roleServiceMock.Setup(roleService => roleService.GetUserRolesByUserIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(new List<UserRole>() { new UserRole(){Role = "User"}, new UserRole(){Role = "Admin"} });

            var userService = CreateService();

            var result = await userService
                .GetUserClaimsAsync("email");

            Assert.Equal(3,result.Count);
        }

        [Fact]
        public async void GetUserClaimsAsync_WithNullEmail_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User());

            _roleServiceMock.Setup(roleService => roleService.GetUserRolesByUserIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(new List<UserRole>() { new UserRole() { Role = "User" }, new UserRole() { Role = "Admin" } });

            var userService = CreateService();

            var result = async ()=> await userService
                .GetUserClaimsAsync("");

            await Assert.ThrowsAnyAsync<ArgumentNullException>(result);
        }

        [Fact]
        public async void GetUserClaimsAsync_WithNoneExistUser_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .Throws(new NullReferenceException());

            _roleServiceMock.Setup(roleService => roleService.GetUserRolesByUserIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(new List<UserRole>() { new UserRole() { Role = "User" }, new UserRole() { Role = "Admin" } });

            var userService = CreateService();

            var result = async () => await userService
                .GetUserClaimsAsync("email");

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }

        [Fact]
        public async void GetUserClaimsAsync_WithEmptyUserRoles_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User(){Email = "email"});

            _roleServiceMock.Setup(roleService => roleService.GetUserRolesByUserIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(new List<UserRole>() { new UserRole()});

            var userService = CreateService();

            var result = async () => await userService
                .GetUserClaimsAsync("email");

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }

        [Fact]
        public async void GetUserClaimsAsync_WithNullUserRoles_ReturnCorrectError()
        {
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<GetUserByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(new User(){Email = "email"});

            _roleServiceMock.Setup(roleService => roleService.GetUserRolesByUserIdAsync(It.IsAny<Int32>()))!
                .ReturnsAsync((List<UserRole>?)null);

            var userService = CreateService();

            var result = async () => await userService
                .GetUserClaimsAsync("email");

            await Assert.ThrowsAnyAsync<NullReferenceException>(result);
        }



    }
}