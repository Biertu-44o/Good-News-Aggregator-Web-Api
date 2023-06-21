using Entities_Context.Entities.UserNews;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using IServices;
using Core.DTOs.Account;
using Core.DTOs;
using System.Drawing;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Data.CQS.QueriesHandlers;
using IServices.Services;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Services.Account
{
    public class SettingsService : ISettingsService
    {
        private readonly IMediator _mediator;
        private readonly IUiThemeService _uiThemeService;

        public SettingsService(
            IUiThemeService uiThemeService, IMediator mediator)
        {
            _uiThemeService = uiThemeService ?? throw new ArgumentNullException(nameof(uiThemeService));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<userSettingsDTO> GetUserInformationAsync(String email)
        {
            if (email.IsNullOrEmpty())
            {
                Log.Warning("Failed attempt to change settings: null arguments");
                throw new ArgumentException(nameof(email));
            }

            try
            {

                userSettingsDTO? userModel = await _mediator.Send(new GetUserSettingsByEmailQuery()
                {
                    Email = email
                });

                userModel.AllThemes = await _uiThemeService.GetAllThemesAsync();

                return userModel;
                
            }
            catch(NullReferenceException)
            {
                Log.Warning("User with email {0} is not found", email);

                return null;
            }

        }


        public async Task<Boolean> UpdateUserSettingsAsync(userSettingsDTO userSettingsDto, String email)
        {
            if (email.IsNullOrEmpty())
            {
                Log.Information("Failed attempt to change settings : null arguments");
                throw new ArgumentException(nameof(userSettingsDto),nameof(email));
            }

            try
            {
                await _mediator.Send(new UpdateUserCommand()
                {
                    userSettingsDTO = userSettingsDto,
                    Email=email
                });

                Log.Information("User with email {0} changed the settings", email);
                return true;
            }
            catch (NullReferenceException)
            {
                Log.Warning("Failed attempt to change settings {0}: user is not found"
                    , email);
                return false;
            }

        }

        public async Task<Int32> GetUserArticleRateFilter(String email)
        {
            return await _mediator.Send(new GetUserArticleRateFilterByEmailQuery()
            {
                Email = email
            });
        }

    }
}
