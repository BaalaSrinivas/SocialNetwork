using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.SignalR
{
    
    public class NotificationHub: Hub
    {
        IConfiguration _configuration;
        public NotificationHub(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public override Task OnConnectedAsync()
        {
            HttpContext context = Context.GetHttpContext();

            //Get User Information TODO:Not proper way of Handling need to fix
            HttpClient httpClient = new HttpClient();
            string token = context.Request.Query["access_token"];

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage httpResponse = httpClient.GetAsync($"{_configuration.GetValue<string>("IdentityAndAccessManagementBaseUrl")}{_configuration.GetValue<string>("UserInfoControllerPath")}").Result;

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);
                Groups.AddToGroupAsync(Context.ConnectionId, response.Value<string>("email"));
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            HttpContext context = Context.GetHttpContext();

            //Get User Information TODO:Not proper way of Handling need to fix
            HttpClient httpClient = new HttpClient();
            string token = context.Request.Query["access_token"];

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage httpResponse = httpClient.GetAsync($"{_configuration.GetValue<string>("IdentityAndAccessManagementBaseUrl")}{_configuration.GetValue<string>("UserInfoControllerPath")}").Result;

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, response.Value<string>("email"));
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
