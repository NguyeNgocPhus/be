using Microsoft.AspNetCore.Authorization;
namespace WebApi.Securities.Authorization;

public class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
    public const string PermissionsGroup = "Permissions";
    public MinimumAgeAuthorizeAttribute(int[] age) => Age = age;
    public int[] Age
    {
        get
        {
            return Age;
        }
        set
        {
            Policy = $"{PermissionsGroup}{value.ToString()}";
        }
    }
}