using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmallPDF.Services
{
    public static class HTTPService
    {
        public static async Task<HttpResponseMessage> GET_CallAsync(string uri)
        {
            // bypass SSL Check!
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var httpClient = new HttpClient(clientHandler))
            {
                return await httpClient.GetAsync(uri);
            }
        }

        public static async Task<HttpResponseMessage> POST_CallAsync(string uri, object data)
        {
            // bypass SSL Check!
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var httpClient = new HttpClient(clientHandler))
            {
                var response = await httpClient.PostAsync(
                    uri,
                     new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

                return response;
            }
        }

        public static async Task<HttpResponseMessage> PUT_CallAsync(string uri, object data)
        {
            // bypass SSL Check!
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var httpClient = new HttpClient(clientHandler))
            {
                var response = await httpClient.PutAsync(
                    uri,
                     new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

                return response;
            }
        }

        public static async Task<HttpResponseMessage> DELETE_CallAsync(string uri)
        {
            // bypass SSL Check!
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var httpClient = new HttpClient(clientHandler))
            {
                var response = await httpClient.DeleteAsync(uri);

                return response;
            }
        }
    }
}
