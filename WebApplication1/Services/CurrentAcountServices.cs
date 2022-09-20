using System.Security.Claims;
using Microsoft.VisualBasic;
using WebApplication1.Common;
using WebApplication1.Common.Exceptions;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class CurrentAccountService : ICurrentAccountServices
{ 
    
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentAccountService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public Guid Id
        {
            get
            {
                Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(JwtClaimTypes.UserId), out var userId);
                if (userId == Guid.Empty)
                {
                    throw new AccountNotFoundException("hi");
                }

                return userId;
            }
        }

        /// <inheritdoc />
        public List<Guid> TenantId
        {
            get
            {
                try
                {
                    return _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == JwtClaimTypes.TenantId).Select(x => Guid.Parse(x.Value)).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new AccountNotFoundException("hi");
                }
            }
        }


        /// <inheritdoc />
        public bool HasPerm(string perm)
        {
            return _httpContextAccessor.HttpContext?.User.Claims.Any(claim => claim.Type == JwtClaimTypes.Permission && claim.Value == perm) ?? false;
        }

        /// <inheritdoc />
        public string GetJwtToken()
        {
            var result = _httpContextAccessor.HttpContext?.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");
            return result?.Value.ToString().Replace("Bearer ", "");
        }

        /// <inheritdoc />
        public bool IsSysAdmin()
        {
            try
            {
                return HasPerm("hi");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AccountNotFoundException("hi");
            }
        }

        /// <inheritdoc />
        public bool IsTenantAdmin(Guid tenantId)
        {
            try
            {
                var hasPerm = HasPerm("abc");

                var belongToTenant = _httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == JwtClaimTypes.TenantId && c.Value == tenantId.ToString());
                if (belongToTenant == null)
                    return false;
                return hasPerm && (bool) belongToTenant;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AccountNotFoundException("hi");
            }
        }

        /// <inheritdoc />
        public bool IsTenantUser(Guid tenantId)
        {
            try
            {
                var belongToTenant = _httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == JwtClaimTypes.TenantId && c.Value == tenantId.ToString());
                if (belongToTenant == null)
                    return false;
                return (bool) belongToTenant;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AccountNotFoundException("hi");
            }
        }
}