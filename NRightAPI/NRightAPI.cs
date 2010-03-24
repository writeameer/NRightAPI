using System;
using System.Linq;
using Microsoft.Http;
using Microsoft.Http.Headers;


namespace RightClient
{
    public class NRightApi
    {
        public static string ApiVersion { get; set; }
        static string ApiUrl { get; set; }
        static string Account { get; set; }
        static Cookie SessionCookie { get; set; }

        public const HttpMethod Get = HttpMethod.GET;
        public const HttpMethod Post = HttpMethod.POST;
        public const HttpMethod Put = HttpMethod.PUT;
        public const HttpMethod Delete = HttpMethod.DELETE;



        public NRightApi ()
        {
            // Set default values for public properties
            ApiVersion = "1.0";
            ApiUrl = "https://my.rightscale.com/api/acct/";
            SessionCookie = new Cookie();
        }


        public void Login(string username, string password, string account)
        {
            var httpClient = new HttpClient(ApiUrl);
            
            Account = account;


            httpClient.DefaultHeaders.Add("X-API-VERSION: " + ApiVersion);
            httpClient.DefaultHeaders.Authorization = Credential.CreateBasic(username, password);

            var response = httpClient.Get(ApiUrl + account + "/login");

            var sessionId = response.Headers["Set-Cookie"].Split(';')[0].Split('=')[1];
            SessionCookie.Add("_session_id", sessionId);
        }


        public HttpResponseMessage Send (HttpMethod httpMethod, string apiString, params string[] parameters)
        {

            HttpResponseMessage response = null;

            // Return null if no RightScale session cookie was issued
            if (SessionCookie == null)
            {
               Console.WriteLine("No RightScale session cookie! Please use the Login method to receive one.");
               return  null;
            }

            // Create httpClient with session cookie
            var httpClient = new HttpClient
                                 {
                                     BaseAddress = new Uri(ApiUrl + Account + "/"),
                                     TransportSettings = {MaximumAutomaticRedirections = 0}
                                 };
            
            httpClient.DefaultHeaders.Cookie.Add(SessionCookie);
            httpClient.DefaultHeaders.Add("X-API-VERSION: " + ApiVersion);


            // Add parameters to HttpContent
            HttpContent content = null;
            var form = new HttpUrlEncodedForm();
            
            // Add parameters to request if any
            if (parameters != null && parameters.Length!=0) {
                foreach(var p in parameters)
                    form.Add(p.Split('=')[0],p.Split('=')[1]);
                content = form.CreateHttpContent();

                // Make REST request with parameters
                response = httpClient.Send(httpMethod, apiString, content);
            }
            else
            {
                // Make REST request without parameters
                response = httpClient.Send(httpMethod, apiString);
            }

            // Return response
            return response;
        }

        public static void DisplayRestResponse(HttpResponseMessage restResponse)
        {
            var content = restResponse.Content.ReadAsString();
            var statusCode = restResponse.StatusCode;


            Console.WriteLine("Status Code:" + statusCode);

            if (restResponse.Headers.ContainsKey("Location")) 
                Console.WriteLine("Location:" + restResponse.Headers["Location"]);

            if (!String.IsNullOrEmpty(content))
                Console.WriteLine(content);
        }


        public HttpResponseMessage GetRequest(string apiString, string[] parameters)
        {
            return parameters == null ? Send(Get, apiString) : Send(Get, apiString, parameters);
        }

        public HttpResponseMessage PostRequest(string apiString, string[] parameters)
        {
            return parameters == null ? Send(Post, apiString) : Send(Post, apiString, parameters);
        }

        public HttpResponseMessage PutRequest(string apiString, string[] parameters)
        {
            return parameters == null ? Send(Put, apiString) : Send(Put, apiString, parameters);
        }

        public HttpResponseMessage DeleteRequest(string apiString, string[] parameters)
        {
            return parameters == null ? Send(Delete, apiString) : Send(Delete, apiString, parameters);
        }


    }
}
