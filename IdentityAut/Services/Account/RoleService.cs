using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Services.Account
{
    public class RoleService :IRoleService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RoleService(IConfiguration configuration, IMediator mediator)
        {

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<UserRole>> GetUserRolesByUserIdAsync(Int32 id)
        {

            if (id < 1)
            {
                Log.Error("attempt to delete article with incorrect id: {id}");
                throw new ArgumentException();
            }

            List<UserRole>? userRoles = await _mediator.Send(new GetUserRolesByUserIdQuery()
            {
               UserId = id
            });

            return userRoles;
        }


        public async Task InitiateDefaultRolesAsync()
        {
            String? rolesFromConfig = _configuration["Roles:all"];

            if (rolesFromConfig.IsNullOrEmpty())
            {
                Log.Error("No roles are defined in the configuration file");
                throw new ArgumentException("No roles are defined in the configuration file");
            }

            await _mediator.Send(new InitiateDefaultRolesCommand()
            {
                Roles = rolesFromConfig!.Split(" ")
            });
        }

        public async Task<UserRole?> GetDefaultRoleAsync()
        {
            String? defaultRoleFromConfigFile = _configuration["Roles:default"];
            
            if (String.IsNullOrEmpty(defaultRoleFromConfigFile))
            {
                Log.Error("No default role is defined in the configuration file");
                throw new ArgumentException("No default role is defined in the configuration file");
            }

            UserRole? defaultRole = await _mediator.Send(new GetDefaultUserRoleQuery()
            {
                RoleName = defaultRoleFromConfigFile
            });
            
            if (defaultRole == null) 
            {
                Log.Warning("Default role is not found");
                
                await InitiateDefaultRolesAsync();

                defaultRole = await _mediator.Send(new GetDefaultUserRoleQuery()
                {
                    RoleName = defaultRoleFromConfigFile
                });
            }

            if (defaultRole == null)
            {
                Log.Error("Cant create default role");
                throw new NullReferenceException();
            }

            return defaultRole;
        }
    }
}
