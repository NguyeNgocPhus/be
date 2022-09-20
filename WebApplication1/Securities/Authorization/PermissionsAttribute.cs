using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Securities.Authorization
{
    /// <summary>
    /// Any user must be authenticated to reach out any protected resource.
    /// The context may be challenged against a set of service based permissions, roles or scopes.
    /// Permissions, roles or scopes may be defined as wild-cards.
    /// The context may be challenged with no expectations which means no permissions, roles or scopes at all or any are expected.
    /// All requirements should be combined as an OR relation. That is, if any of the requirements are satisfied, then the authorization should succeed.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PermissionsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Key for perm
        /// </summary>
        public const string PermissionsGroup = "Permissions";
        /// <summary>
        /// Key for role
        /// </summary>
        public const string RolesGroup = "Roles";
        /// <summary>
        /// Key for scope
        /// </summary>
        public const string ScopesGroup = "Scopes";

        private string[] _permissions;
        private string[] _scopes;
        private string[] _roles;

        private bool _isDefault = true;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///
        /// Sample
        /// 
        ///     [Permissions(
        ///                 Permissions = new[] { "weather:*:*"},
        ///                 Roles = new [] { "user", "super admin"},
        ///                 Scopes = new[] { "App.Demo" })
        ///     ]
        /// 
        /// Note:
        ///
        ///     If we leave it as blank ([Permissions]), it work like [Authorize] attribute
        /// </remarks>
        public PermissionsAttribute()
        {
            _permissions = Array.Empty<string>();
            _roles = Array.Empty<string>();
            _scopes = Array.Empty<string>();
        }

        /// <summary>
        /// Array of permissions to be evaluated by authorizer
        /// </summary>
        public string[] Permissions
        {
            get => _permissions;
            set => BuildPolicy(ref _permissions, value, PermissionsGroup);
        }

        /// <summary>
        /// Array of scopes
        /// </summary>
        public string[] Scopes
        {
            get => _scopes;
            set => BuildPolicy(ref _scopes, value, ScopesGroup);
        }

        /// <summary>
        /// Array of roles
        /// </summary>
        public new string[] Roles
        {
            get => _roles;
            set => BuildPolicy(ref _roles, value, RolesGroup);
        }

        /// <summary>
        /// To build list of policy
        /// </summary>
        /// <param name="target">Permission, Role or Policy</param>
        /// <param name="value">Come from constructor</param>
        /// <param name="group">Permission, Role or Policy</param>
        // ReSharper disable once RedundantAssignment
        private void BuildPolicy(ref string[] target, string[] value, string group)
        {
            target = value;

            if (_isDefault)
            {
                Policy = string.Empty;
                _isDefault = false;
            }
            var v = $"{group}${string.Join("|", target)}";
            Policy += $"{group}${string.Join("|", target)};";
        }
    }
}