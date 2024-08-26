using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Monitoramento
    {
        private const string ChaveApi = "CXY8MJYK89SY2EDL";
        private const string UrlBase = "https://www.alphavantage.co/";
        private readonly HttpClient httpClient = new HttpClient();
        private readonly Alerta alerta = new Alerta();

        public async Task MonitoraAtivo(string ativo, double minimo, double maximo)
        {
            var url = $"{UrlBase}query?function=GLOBAL_QUOTE&symbol={ativo}&apikey={ChaveApi}";
            Console.WriteLine($"Buscando dados da API: {url}\n");

            try
            {
                var resposta = await httpClient.GetFromJsonAsync<JsonElement>(new Uri(url));

                if (resposta.TryGetProperty("Global Quote", out JsonElement cotacaoGlobal) &&
                    cotacaoGlobal.TryGetProperty("05. price", out JsonElement elementoPreco))
                {
                    var precoString = elementoPreco.GetString();
                    Console.WriteLine($"Preço atual do ativo: {precoString}");

                    if (decimal.TryParse(precoString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal preco))
                    {
                        ProcessarPrecoAtivo(preco, minimo, maximo);
                    }

                }

                Console.WriteLine("Erro ao ler dados da API. A resposta foi a seguinte: ");
                Console.WriteLine(resposta.ToString() + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu uma exceção: {ex.Message}");
                throw new Exception("Erro ao recuperar o preço do ativo!", ex);
            }
        }

        private string ProcessarPrecoAtivo(decimal preco, double minimo, double maximo)
        {
            string decisao = preco < (decimal)minimo ? "Comprar" :
                             preco > (decimal)maximo ? "Vender" : "Aguardar";

            if (decisao == "Comprar")
            {
                Console.WriteLine("O preço do ativo está baixo, você deve comprá-lo!");
                alerta.EnviarAlertaEmail("smtp_configs.json", "O preço do ativo está baixo, você deve comprá-lo!");
            }
            else if (decisao == "Vender")
            {
                Console.WriteLine("O preço do ativo está alto, você deve vendê-lo!");
                alerta.EnviarAlertaEmail("smtp_configs.json", "O preço do ativo está alto, você deve vendê-lo!");
            }
            else
            {
                Console.WriteLine("O preço do ativo está entre os valores! Não faça nada");
            }

            return decisao;
        }
    }
}
