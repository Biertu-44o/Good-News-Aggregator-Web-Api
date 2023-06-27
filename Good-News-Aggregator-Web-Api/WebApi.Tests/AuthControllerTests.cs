
using System.Security.Claims;
using Core.DTOs.Account;
using FluentValidation.Results;
using IServices.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Account;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Controllers;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.Validators;

namespace WebApi.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IServiceFactory> _serviceMock = new Mock<IServiceFactory>();


        private AuthController CreateService()
        {
            var controller = new AuthController(
                _serviceMock.Object
            );

            return controller;
        }

        [Fact]
        public async void Login_NotValid_BadRequest()
        {
            var expectedResult = new ValidationResult(){Errors = new List<ValidationFailure>(){new ValidationFailure()}};

            _serviceMock.Setup(validator => validator.CreateLoginValidator().ValidateAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var articleController = CreateService();

            var result = await articleController.Login(new LoginRequest(){Email = "email"}) ;


            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Login_UserNotExist_BadRequest()
        {
            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() };

            _serviceMock.Setup(validator => validator.CreateLoginValidator().ValidateAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            _serviceMock.Setup(services => services.CreateMapperService().Map<UserLoginDto>(It.IsAny<LoginRequest>()))
                .Returns(It.IsAny<UserLoginDto>());


            _serviceMock.Setup(services => services.CreateIdentityService().LoginAsync(
                new UserLoginDto(){Email = "asd"}
            )).ReturnsAsync(false);

            var articleController = CreateService();

            var result = await articleController.Login(new LoginRequest() { Email = "email" });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Login_CorrectLoginPassword_Ok()
        {
            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() };

            _serviceMock.Setup(validator => validator.CreateLoginValidator().ValidateAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            _serviceMock.Setup(services => services.CreateMapperService().Map<UserLoginDto>(It.IsAny<LoginRequest>()))
                .Returns(It.IsAny<UserLoginDto>());

            _serviceMock.Setup(services => services.CreateJwtService().GetJwtTokenString(It.IsAny<List<Claim>>()))
                .ReturnsAsync("claims");

            _serviceMock.Setup(services => services.CreateIdentityService().LoginAsync(
                null
            )).ReturnsAsync(true);

            var articleController = CreateService();

            var result = await articleController.Login(new LoginRequest() { Email = "email" });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Registration_CorrectData_Ok()
        {
            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() };

            _serviceMock.Setup(validator => validator.CreateRegistrationValidator().ValidateAsync(It.IsAny<RegistrationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            _serviceMock.Setup(services => services.CreateMapperService().Map<UserRegistrationDto>(It.IsAny<UserRegistrationDto>()))
                .Returns(It.IsAny<UserRegistrationDto>());

            _serviceMock.Setup(services => services.CreateIdentityService().RegistrationAsync(
                null
            )).ReturnsAsync(true);

            var articleController = CreateService();

            var result = await articleController.Registration(new RegistrationRequest() { Email = "email" });

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Registration_NotValid_BadRequest()
        {
            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } };

            _serviceMock.Setup(validator => validator.CreateRegistrationValidator()
                    .ValidateAsync(It.IsAny<RegistrationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var articleController = CreateService();

            var result = await articleController.Registration(new RegistrationRequest() { Email = "email" });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Registration_UserNotExist_BadRequest()
        {

            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() };

            _serviceMock.Setup(validator => validator.CreateRegistrationValidator().ValidateAsync(It.IsAny<RegistrationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            _serviceMock.Setup(services => services.CreateMapperService().Map<UserRegistrationDto>(It.IsAny<UserRegistrationDto>()))
                .Returns(It.IsAny<UserRegistrationDto>());


            _serviceMock.Setup(services => services.CreateIdentityService().RegistrationAsync(
                new UserRegistrationDto() { Email = "asd" }
            )).ReturnsAsync(false);

            var articleController = CreateService();

            var result = await articleController.Registration(new RegistrationRequest() { Email = "email" });

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
    
}