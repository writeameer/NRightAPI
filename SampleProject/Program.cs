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
using RightClient;

namespace RightClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new RightClient.NRightApi();
            
            // Log in to RightScale 
            api.Login("user@domain.com", "password", "rightscale account number");
           
            // Example: Get All Right Scipts
            var restResponse = api.Send(NRightApi.Get, "right_scripts.xml");
       
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
