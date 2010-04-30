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

namespace RightClient
{
    class Program
    {
        static void Main(string[] args)
        {
            

            // Username, passwords and account can be assigned directly as strings here
            // I'm using system variable to avoid accidentally checking in my RightScale credentials :)

            var username = Environment.GetEnvironmentVariable("RS_username", EnvironmentVariableTarget.Machine);
            var password = Environment.GetEnvironmentVariable("RS_password", EnvironmentVariableTarget.Machine);
            var account = Environment.GetEnvironmentVariable("RS_acct", EnvironmentVariableTarget.Machine);


            // Instantiate NRightAPI using RS account number
            var api = new NRightApi(account);

            // Log in to RightScale 
            api.Login(username, password);
            
            // Example: Get default deployments
            var parameters = new string[]
                                 {
                                     "filter=nickname=default"
                                 };
           
            var restResponse = api.Send(NRightApi.Get,"deployments.xml",parameters);
            NRightApi.DisplayRestResponse(restResponse);
            

        }
    }
}
