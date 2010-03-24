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
            
            // Log in to RightScale 
            api.Login("user@domain.com", "password", "rightscale account number");

            // Example: Get All Right Scipts
            var restResponse = api.GetRequest("right_scripts.xml", null);
       
            NRightApi.DisplayRestResponse(restResponse);

            
            // Example: Create a server template
             restResponse = api.Send(
                NRightApi.Post,"server_templates",
                "server_template[nickname]=template nickname",
                "server_template[description]=template description",
                "server_template[multi_cloud_image_href]=http://<multicloudhref>",
                "server_template[instance_type]=m1.small"
                );


            NRightApi.DisplayRestResponse(restResponse);

            // Example: Create Server
            restResponse = api.Send(
                NRightApi.Post, "servers",
                "server[cloud_id]=1",
                "server[nickname]=Server nickname",
                "server[server_template_href]=http://<server template href>",
                "server[ec2_ssh_key_href]=https://<ssh key href>",
                "server[ec2_security_groups_href]=https://<security gorup href>",
                "server[deployment_href]=<https://deployment href>",
                "server[instance_type]=m1.small"
                );

            NRightApi.DisplayRestResponse(restResponse);

        
        }
    }
}
