using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using FluentValidation.Results;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Controllers;
using Web_Api_Controllers.RequestModels;

namespace WebApi.Tests
{   public class SettingsControllerTests
    {
        private readonly Mock<IServiceFactory> _serviceMock = new Mock<IServiceFactory>();


        private SettingsController CreateService()
        {
            var controller = new SettingsController(
                _serviceMock.Object
            );

            return controller;
        }

        [Fact]
        public async void SetNewUserSettings_NotValid_BadRequest()
        {
            var expectedResult = new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } };

            _serviceMock.Setup(validator => validator.CreateSettingsValidator().ValidateAsync(It.IsAny<PutSettingsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var settingsController = CreateService();

            var result = await settingsController.SetNewUserSettings(new PutSettingsRequest() {});


            Assert.IsType<BadRequestResult>(result);
        }

    }
}
