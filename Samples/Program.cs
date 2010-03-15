using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace RightClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new NRightApi();
            
            
            /*
            api.Login("user@host.com", "mypassword", "RightScaleAccountNumber");

            
            var scriptsResponse = api.Send(NRightApi.Get, "right_scripts.xml");

            Console.WriteLine(scriptsResponse.Content.ReadAsString());

            var deploymentResponse = api.Send(
                NRightApi.Post, "deployments",
                 "deployment[nickname]=Test Deployment",
                 "deployment[description]=Test Deployment Description"
                );


            Console.WriteLine(deploymentResponse.Headers["Location"]);
             
            */
        
        }
    }
}
