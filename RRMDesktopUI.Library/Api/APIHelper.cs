using RRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient apiClient { get; set; }
        private ILoggedInUserModel LoggedInUser { get; set; }


        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();

            LoggedInUser = loggedInUser;
        }

        public HttpClient ApiClient
        {
            get { return apiClient; }
        }


        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

            using (var response = await apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    LoggedInUser.Token = token;
                    LoggedInUser.FirstName = result.FirstName;
                    LoggedInUser.LastName = result.LastName;
                    LoggedInUser.EmailAddress = result.EmailAddress;
                    LoggedInUser.CreateDate = result.CreateDate;
                    LoggedInUser.Id = result.Id;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
                
            }
        }
    }
}
