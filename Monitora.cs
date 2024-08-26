using System;
using System.Collections.Generic;
using System.Globalization;
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

            var url = $"{BaseUrl}query?function=GLOBAL_QUOTE&symbol={ativo}&apikey={ApiKey}";

            try
            {
                var response = await httpClient.GetFromJsonAsync<dynamic>(new Uri(url));

                if (response.TryGetProperty("Global Quote", out JsonElement globalQuote))
                {
                    if (globalQuote.TryGetProperty("05. price", out JsonElement priceElement))
                    {
                        var priceString = priceElement.GetString(); // Pega o valor como
                        Console.WriteLine(priceString);                                    
                        
                        //Pega o decimal usando o "." como separador já que a API retorna os dados nesse formato
                        if (decimal.TryParse(priceString,CultureInfo.InvariantCulture , out decimal price)) // Tenta fazer o parse para decimal
                        {
                            Console.WriteLine("Puxando dados da API Alpha Vantage, a partir da URI: " + url);
                            Console.WriteLine("Preco da ação no momento: " + (double)price);
                            Console.WriteLine("Seus valores basais: " + min + " " + max);

                            if (price < (decimal)min)
                            {
                                Console.WriteLine("Disparando um email para comprar ação");
                            }
                            else if (price > (decimal)max)
                            {
                                Console.WriteLine("Disparando um email para vender ação!");
                            }

                            return price < (decimal)min ? "Comprar" :
                                   price > (decimal)max ? "Vender" : "Aguardar";
                        }
                        else
                        {
                            return "Não foi possível converter o preço para um decimal!";
                        }
                    }
                    else
                    {
                        return "Erro ao ler dados da API";
                    }

                } else
                {
                    return "Erro ao ler dados da API";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler cotação do ativo!", ex);
            }

        }
    }
}
