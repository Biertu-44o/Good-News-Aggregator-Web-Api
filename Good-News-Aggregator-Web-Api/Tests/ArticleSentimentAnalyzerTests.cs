using System.Collections.Concurrent;
using System.Net;
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
using Services.Article.ArticleRate;

namespace Tests
{
    public class ArticleSentimentAnalyzerTests
    {
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();


        private SentimentAnalyzerService CreateService()
        {
            var userService = new SentimentAnalyzerService(
                _configMock.Object
            );

            return userService;
        }

        [Fact]
        public async void TranslateText_WithEmptyText_ReturnNull()
        {

            var userService = CreateService();

            var result = await userService
                .TranslateText("");

            Assert.Null(result);
        }

        [Fact]
        public async void GetArticlesWithSentimentScore_WithEmptyConfigurationFile_ReturnCorrectError()
        {
            _configMock.Setup(configurationFile => configurationFile["ArticleSentimentAnalyzerMethods:DictionaryAnalyzer"])
                .Returns("");

            var userService = CreateService();

            var result = async ()=> await userService
                .GetArticlesWithSentimentScore(new List<FullArticleDto>());

            await Assert.ThrowsAnyAsync<ArgumentException>(result);
        }

    }
}