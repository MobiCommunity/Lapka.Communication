using Microsoft.AspNetCore.Http;

namespace Lapka.Communication.Application.Services
{
    public interface IAuthenticator
    {
        public void Authenticate(HttpContext ctx);
    }
}