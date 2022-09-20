namespace WebApplication1.Services.Interfaces;

public interface ICurrentAccountServices
{
            /// <summary>
            /// Get current user id
            /// </summary>
            Guid Id { get; }
    
            /// <summary>
            /// Get current user's tenant ids
            /// </summary>
            List<Guid> TenantId { get; }
            
    
            /// <summary>
            /// Check current user has the given permission or not
            /// </summary>
            /// <param name="perm"></param>
            /// <returns></returns>
            bool HasPerm(string perm);
    
            /// <summary>
            /// Get current jwt token which was provided by current HttpRequest
            /// </summary>
            /// <returns></returns>
            string GetJwtToken();
    
            /// <summary>
            /// Check current user has sys admin perm or not
            /// </summary>
            /// <returns></returns>
            public bool IsSysAdmin();
    
            /// <summary>
            /// Check current user has tenant admin perm or not
            /// </summary>
            /// <param name="tenantId"></param>
            /// <returns></returns>
            public bool IsTenantAdmin(Guid tenantId);
            /// <summary>
            /// Check current user has tenant perm or not
            /// </summary>
            /// <param name="tenantId"></param>
            /// <returns></returns>
            public bool IsTenantUser(Guid tenantId);
}