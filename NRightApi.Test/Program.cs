using System;
using NUnit.Framework;

namespace NRightApi.Test
{
    class Program
    {
        public static string Username = Environment.GetEnvironmentVariable("RS_username", EnvironmentVariableTarget.Machine);
        public static string Password = Environment.GetEnvironmentVariable("RS_password", EnvironmentVariableTarget.Machine);
        public static string Account = Environment.GetEnvironmentVariable("RS_acct", EnvironmentVariableTarget.Machine);


        [Test]
        public void Rename_Deployment()
        {
            var api = new WriteAmeer.NRightApi(Account);

            api.Login(Username, Password);

            var parameters = new [] {"deployment[nickname]=AD_Testing_3"};


            var restResponse = api.Send(WriteAmeer.NRightApi.Put, "deployments/104171", parameters);
            Console.WriteLine(restResponse.Content.ReadAsString());

        }
    }
}
