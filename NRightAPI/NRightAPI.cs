/*
Copyright 2010 Ameer Deen. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY <COPYRIGHT HOLDER> ``AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of <copyright holder>.
*/

using System;
using System.Net;
using System.Web;
using Microsoft.Http;
using Microsoft.Http.Headers;
using Cookie = Microsoft.Http.Headers.Cookie;




namespace RightClient
{
    public class NRightApi
    {


        public const HttpMethod Get = HttpMethod.GET;
        public const HttpMethod Post = HttpMethod.POST;
        public const HttpMethod Put = HttpMethod.PUT;
        public const HttpMethod Delete = HttpMethod.DELETE;

        public static string ApiVersion { get; set; }
        static string ApiUrl { get; set; }
        static Cookie SessionCookie { get; set; }
        public static string Account { get; set; }
        
        public static HttpClient HttpClient;





        public NRightApi(string account): this(account,"1.0")
        {
            // Set API version to default to 1.0
        }

        public NRightApi(string account, string version)
        {
            // Forcing SSLv3 after RightScale upgraded cert to V3 on April 14th 2010
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            // Set default values for public properties
            ApiVersion = version;
            ApiUrl = "https://my.rightscale.com/api/acct/";
            SessionCookie = new Cookie();

            // Initialize static HTTPClient
            Account = account;
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiUrl + Account + "/"),
                TransportSettings = { MaximumAutomaticRedirections = 0 }
            };

            // Add API version to HttpClient
            HttpClient.DefaultHeaders.Add("X-API-VERSION: " + ApiVersion);
        }

        public void Login(string username, string password)
        {

            var client = new HttpClient(); 

            // Add RightScale API version header
            client.DefaultHeaders.Add("X-API-VERSION: " + ApiVersion);

            // Add authentication header and execute login method
            client.DefaultHeaders.Authorization = Credential.CreateBasic(username, password);
            var response = client.Get(ApiUrl + Account + "/login");
            
            // Get RightScale session cookie from HTTP header
            var sessionId = response.Headers["Set-Cookie"].Split(';')[0].Split('=')[1];
            SessionCookie.Add("_session_id", sessionId);

            // Add session cookie to static HttpClient
            HttpClient.DefaultHeaders.Cookie.Add(SessionCookie);

            
        }

        public static void SetSessionId(string sessionId)
        {
            // Create Session Id cookie
            SessionCookie.Add("_session_id", sessionId);

            // Add Session Id cookie to static HttpClient
            HttpClient.DefaultHeaders.Cookie.Add(SessionCookie);
        }
        public HttpResponseMessage Send (HttpMethod httpMethod, string restMethod, params string[] parameters)
        {
            
            HttpResponseMessage response = null;

            // Return null if no RightScale session cookie was issued
            if (SessionCookie == null)
            {
               Console.WriteLine("No RightScale session cookie! Please use the Login method to receive one.");
               return  null;
            }


            // Check if parameters were passed
            if (parameters.Length==0)
                // Make REST request without parameters
                response = HttpClient.Send(httpMethod, restMethod);
            else
            {
                // Make REST request with parameters
                response = httpMethod == Post ? SendPostRequest(HttpClient, restMethod, parameters) : SendRequest(HttpClient, restMethod, parameters);
            }

            // Check Http response for errors
            CheckHttpResponseForErrors(response);

            return response;
        }

        public static void CheckHttpResponseForErrors (HttpResponseMessage response)
        {
            // Check for errors in the HTTP response status code and throw exception
            // if status code not in 200 - 2XX range
            
            return;
        }

        public static HttpResponseMessage SendRequest(HttpClient httpClient, string restMethod, string[] parameters)
        {
            var parameterString = "";

            // Construct parameter query string
            foreach (var p in parameters)
            {
                var delim = p.IndexOf("=");
                var key = p.Substring(0, delim);
                var value = p.Substring(delim + 1, p.Length - delim - 1);

                parameterString = parameterString + key + "=" + HttpUtility.UrlEncode(value) + "&";
            }

            // Make REST request with parameters
            return httpClient.Send(Get, restMethod + "?" + parameterString);
        }

        public static HttpResponseMessage SendPostRequest(HttpClient httpClient, string restMethod, string[] parameters)
        {
            HttpContent content = null;
            var form = new HttpUrlEncodedForm();

            // Create post data from parameters
            foreach (var p in parameters)
            {
                var delim = p.IndexOf("=");
                var key = p.Substring(0, delim);
                var value = p.Substring(delim + 1, p.Length - delim - 1);
                form.Add(key, value);
            }
            content = form.CreateHttpContent();

            // Make REST request with parameters
            return  httpClient.Send(Post, restMethod, content);
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
