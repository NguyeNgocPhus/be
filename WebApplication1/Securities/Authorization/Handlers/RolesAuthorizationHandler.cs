using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Common.Constants;
using WebApi.Securities.Authorization;
using WebApi.Securities.Authorization.Requirements;
using WebApplication1.Common;

namespace WebApplication1.Securities.Authorization.Handlers
{
    /// <summary>
    /// Type of Role in table RolePermission must be "Role"
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesRequirement>
    {
        private readonly ILogger<RolesAuthorizationHandler> _logger;

        /// <inheritdoc />
        public RolesAuthorizationHandler(ILogger<RolesAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
        {
            try
            {
                if (context.User.Identity != null && !context.User.Identity.IsAuthenticated)
                {
                    return Task.CompletedTask;
                }

                if (context.HasSucceeded)
                {
                    return Task.CompletedTask;
                }

                if (context.User.IsInRole(Role.SuperAdmin))
                {
                    Utility.Succeed(context, requirement.Identifier);
                    return Task.CompletedTask;
                }

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (context.User == null || string.IsNullOrWhiteSpace(requirement.Roles))
                {
                    return Task.CompletedTask;
                }

                var requirementTokens = requirement.Roles.Split("|", StringSplitOptions.RemoveEmptyEntries);

                // ReSharper disable once ConstantConditionalAccessQualifier
                if (requirementTokens?.Any() != true)
                {
                    return Task.CompletedTask;
                }

                var expectedRequirements = requirementTokens.ToList();

                if (expectedRequirements.Count == 0)
                {
                    return Task.CompletedTask;
                }

                // ReSharper disable once ConstantConditionalAccessQualifier
                var userRoleClaims = context.User.Claims?.Where(c =>
                    string.Equals(c.Type, IdentityType.Role, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));

                // ReSharper disable once LoopCanBeConvertedToQuery
                // ReSharper disable once ConstantNullCoalescingCondition
                foreach (var claim in userRoleClaims ?? Enumerable.Empty<Claim>())
                {
                    var match = expectedRequirements
                        .Where(r => string.Equals(r, claim.Value, StringComparison.OrdinalIgnoreCase));

                    // ReSharper disable once InvertIf
                    if (match.Any())
                    {
                        Utility.Succeed(context, requirement.Identifier);
                        break;
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Role HandleRequirementAsync");
                throw;
            }
        }
    }
}