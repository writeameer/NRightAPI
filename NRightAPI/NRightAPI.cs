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
