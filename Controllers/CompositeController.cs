using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace StudentCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompositeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CompositeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("InternalAPI");
                var apiKey = _configuration["ApiKey"];

                var studentRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7135/api/v1/students");
                studentRequest.Headers.Add("X-API-KEY", apiKey);  

                var partnerRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7135/api/v1/students");
                partnerRequest.Headers.Add("X-API-KEY", apiKey);  

                
                var internalRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7135/api/internal/server-status");

                var results = new Dictionary<string, object>();

                // Handle student request
                try
                {
                    var studentResponse = await client.SendAsync(studentRequest);
                    if (studentResponse.IsSuccessStatusCode)
                    {
                        var content = await studentResponse.Content.ReadAsStringAsync();
                        results["students"] = JsonSerializer.Deserialize<object>(content);
                    }
                    else
                    {
                        results["students_error"] = $"Status: {(int)studentResponse.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    results["students_exception"] = ex.Message;
                }

                // Handle partner request
                try
                {
                    var partnerResponse = await client.SendAsync(partnerRequest);
                    if (partnerResponse.IsSuccessStatusCode)
                    {
                        var content = await partnerResponse.Content.ReadAsStringAsync();
                        results["partnerStudents"] = JsonSerializer.Deserialize<object>(content);
                    }
                    else
                    {
                        results["partnerStudents_error"] = $"Status: {(int)partnerResponse.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    results["partnerStudents_exception"] = ex.Message;
                }

                // Handle internal request
                try
                {
                    var internalResponse = await client.SendAsync(internalRequest);
                    if (internalResponse.IsSuccessStatusCode)
                    {
                        var content = await internalResponse.Content.ReadAsStringAsync();
                        results["internalStatus"] = JsonSerializer.Deserialize<object>(content);
                    }
                    else
                    {
                        results["internalStatus_error"] = $"Status: {(int)internalResponse.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    results["internalStatus_exception"] = ex.Message;
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}, Stack: {ex.StackTrace}");
            }
        }
    }
}