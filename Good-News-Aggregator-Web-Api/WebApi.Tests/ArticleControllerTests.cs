using Core.DTOs.Article;
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

namespace WebApi.Tests
{
    public class ArticleControllerTests
    {
        private readonly Mock<IServiceFactory> _serviceMock = new Mock<IServiceFactory>();


        private ArticleController CreateService()
        {
            var controller = new ArticleController(
                _serviceMock.Object
            );

            return controller;
        }

        [Fact]
        public async void GetSelectedArticle_ServiceReturnNull_ReturnNoFound()
        {
            _serviceMock.Setup(services => services
                    .CreateArticlesService()
                    .GetFullArticleByIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync((FullArticleDto?)null);

            var articleController = CreateService();

            var request = 1;
            
            var result = await articleController.GetSelectedArticle(request) ;


            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteArticlesById_ServiceReturnNull_ReturnNoFound()
        {
            _serviceMock.Setup(services => services
                    .CreateArticlesService()
                    .DeleteArticleByIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(false);

            var articleController = CreateService();

            var request = 1;

            var result = await articleController.GetSelectedArticle(request);


            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async void DeleteArticlesById_IncorrectId_BadRequest(Int32 request)
        {
            var articleController = CreateService();

            var result = await articleController.GetSelectedArticle(request);


            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async void GetArticlesCount_IncorrectId_BadRequest(Int32 request)
        {
            var articleController = CreateService();

            var result = await articleController.GetSelectedArticle(request);


            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(12)]
        public async void GetArticlesCount_IncorrectRate_BadRequest(Int32 request)
        {
            //var articleController = CreateService();

            //var result = await articleController.GetArticlesCount(request);


            //Assert.IsType<BadRequestResult>(result);
        }

    }
    
}