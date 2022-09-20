using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591
namespace WebApplication1.Common
{
    /// <summary>
    /// Role list in the system
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Role
    {
        public static string SuperAdmin = "Super Admin"; // Super Admin who have full power in the application
        public static string Anonymous = "Anonymous"; // Anonymous user
    }
}