using System;
using System.Collections.Generic;
using System.Text;

namespace DirecAct.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Models;

    using Plugin.Connectivity;


    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Por favor enciende las configuraciones de internet.",
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Revisa tu conexión de internet.",
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok",
            };
        }

    }
}
