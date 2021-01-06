using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace FirstMicroService.Controllers
{
    [Authorize]
    [Route("TestAuth")]
    public class AuthorizationController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            using HttpClient client = new HttpClient();

            client.SetBearerToken(token);

            var result = await client.GetAsync("https://localhost:5011/WeatherForecast");

            var status = result.StatusCode;

            var content = await result.Content.ReadAsStringAsync();

            return View();
        }
        [Route("LogOut")]
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
