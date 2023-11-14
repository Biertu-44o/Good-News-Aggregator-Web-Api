using Data.CQS.Commands;
using Data.CQS.Queries;
using IServices.Services;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Services.Account
{

    public class UiThemeService:IUiThemeService
    {

        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;


        public UiThemeService(IConfiguration configuration
            , IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }


        public async Task InitiateThemeAsync()
        {
            Log.Information("Attempt to create themes");

            String? themesFromConfigFile = _configuration["Themes:all"];
            
            if (String.IsNullOrEmpty(themesFromConfigFile))
            {
                throw new ArgumentException("No themes are defined in the configuration file");
            }

            await _mediator.Send(new InitiateThemesCommand()
            {
                Themes = themesFromConfigFile!.Split(" ")
        });
        }

        public async Task<Int32> GetIdDefaultThemeAsync()
        {
            String? defaultTheme = _configuration["Themes:default"];

            if (String.IsNullOrEmpty(defaultTheme))
            {
                throw new ArgumentException("No default theme is defined in the configuration file");
            }

            var themeId = await _mediator.Send(new GetDefaultThemeIdQuery()
            {
                Name= defaultTheme
            });

            if (themeId == 0)
            {
                await InitiateThemeAsync();

                themeId = await _mediator.Send(new GetDefaultThemeIdQuery()
                {
                    Name = defaultTheme
                });

                if (themeId==0)
                {
                    throw new InvalidOperationException("Can't initiate theme");
                }
            }

            return themeId;
        }

        public async Task<List<String>> GetAllThemesAsync()
        {
            List<String> themeList = await _mediator.Send(new GetAllThemesQuery());

            if (themeList.Count>0)
            {
                return themeList;
            }
            else
            {
                await InitiateThemeAsync();
                
                return await _mediator.Send(new GetAllThemesQuery());
            }
        }


    }
}
