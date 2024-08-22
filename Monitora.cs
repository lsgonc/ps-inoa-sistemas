using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Monitora
    {

        private const string ApiKey = "CXY8MJYK89SY2EDL";
        private const string BaseUrl = "https://www.alphavantage.co/";
        private HttpClient httpClient = new HttpClient();


        public async Task<String> MonitoraAtivo(string ativo, double min, double max)
        {
            Console.WriteLine(ativo);
            Console.WriteLine(min);
            Console.WriteLine(max);

            var url = $"{BaseUrl}query?function=GLOBAL_QUOTE&symbol={ativo}&apikey={ApiKey}";
            Console.WriteLine(url);

            try
            {
                dynamic response = await httpClient.GetFromJsonAsync<dynamic>(new Uri(url));
                
                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler cotação do ativo!", ex);
            }

            
        }
    }
}
