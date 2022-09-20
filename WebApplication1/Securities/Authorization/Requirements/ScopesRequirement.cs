using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Securities.Authorization.Requirements;

// ReSharper disable All

namespace WebApi.Securities.Authorization.Requirements
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class ScopesRequirement : IAuthorizationRequirement, IIdentifiable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="identifier"></param>
        public ScopesRequirement(string scopes, Guid identifier)
        {
            Scopes = scopes;
            Identifier = identifier;
        }

        /// <summary>
        /// Scope
        /// </summary>
        public string Scopes { get; }

        /// <summary>
        /// Scope GUID
        /// </summary>
        public Guid Identifier { get; set; }
    }
}