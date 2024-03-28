using AccountSeller.Application.Common.Helpers;
using AccountSeller.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AccountSeller.Application.Common.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userName;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserName => _userName ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Name =>  _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        public string RoleName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
        public string Method => _httpContextAccessor.HttpContext?.Request.Method;
        public string Url => _httpContextAccessor.HttpContext?.Request.Path;
        public DateTime? ClientTime => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, ClientTimeZone);

        public string JsonHeader => _httpContextAccessor.HttpContext == null
            ? string.Empty
            : (_httpContextAccessor.HttpContext.Request.Headers)
            .ConvertToString();

        private string Timezone => _httpContextAccessor.HttpContext?.Request.Headers["Timezone"];

        public TimeZoneInfo ClientTimeZone
        {
            get
            {
                try
                {
                    return Timezone != null
                        ? TimeZoneInfo.FindSystemTimeZoneById(Timezone)
                        : TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                }
                catch
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                }
            }
        }

        public void DeserializeUserId(string userSerialized)
        {
            if (userSerialized == null)
            {
                return;
            }
            var userByte = Convert.FromBase64String(userSerialized);
            using var mStream = new MemoryStream(userByte);
            using var bReader = new BinaryReader(mStream);
            var claims = new ClaimsPrincipal(bReader);
            _userName = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private ClaimsPrincipal _claimsPrincipal;

        private ClaimsPrincipal _claims
        {
            get
            {
                if (_claimsPrincipal == null)
                {
                    _claimsPrincipal = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
                }
                return _claimsPrincipal;
            }
            set
            {
                _claimsPrincipal = value;
            }
        }

        private static string Serialize(ClaimsPrincipal principal)
        {
            using var mStream = new MemoryStream();
            using (var bWriter = new BinaryWriter(mStream))
            {
                principal.WriteTo(bWriter);
            }
            mStream.Flush();
            return Convert.ToBase64String(mStream.GetBuffer());
        }
    }
}
