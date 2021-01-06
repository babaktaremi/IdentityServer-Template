using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;

namespace FirstMicroService.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private string _accessToken;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (HttpClient client=new HttpClient())
            {
                client.SetBearerToken(await GetToken());

                var result = await client.GetAsync("https://localhost:5011/WeatherForecast");

               var status= result.StatusCode;

                var content = await result.Content.ReadAsStringAsync();

                return Ok(content);
            }
        }

        #region Get Token

        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
                return _accessToken;

            using HttpClient client=new HttpClient();

            var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5012");

            if(discoveryDoc.IsError)
                throw new Exception("Token Not Found");

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDoc.TokenEndpoint,
                ClientSecret = "790681d3-0078-48fd-a3fd-2a2eb66a52f3",
                ClientId = "APIID",
                Scope = "fullaccess"
            });

            if(tokenResponse.IsError)
                throw new Exception("Token Not Found");

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        #endregion
    }
}
