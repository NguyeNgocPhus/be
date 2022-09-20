using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591
namespace WebApi.Common.Constants
{
    /// <summary>
    /// System's Identity constant
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IdentityType
    {
        public const string Permission = "Permission";
        public const string Role = "Role";
        public const string Scope = "Scope";
    }
    public static class Permissions
    {
        // Structure area:module:perm
        public const string SysAdmin = "ROOT:ROOT:SYSADMIN";

        public const string TenantAdmin = "TENANT:ROOT:ADMIN";

        //public const string TenantUser = "TENANT:ROOT:USER";
        public const string UnlockAccount = "IDENTITY:ACCOUNT:UNLOCK";
        public const string LockAccount = "IDENTITY:ACCOUNT:LOCK";
    }

}