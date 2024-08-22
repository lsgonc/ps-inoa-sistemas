/*
    Autor: Lucas Sciarra Gonçalves

    Projeto desenvolvido para o processo seletivo da vaga de estágio da Inoa

   O aplicativo foi projetado para monitorar em tempo real a cotação de um ativo específico da B3, utilizando a API Alpha Vantage 
   para obter dados financeiros. Com base nos parâmetros definidos pelo usuário, o sistema analisa continuamente o preço 
   da ação. Se a cotação cair abaixo de um valor preestabelecido, o aplicativo gera um alerta via email para o usuário aconselhando a compra, sinalizando 
   uma oportunidade de investimento. Por outro lado, se a cotação subir acima de um valor de referência, o sistema sugere a venda, 
   ajudando o usuário a maximizar seus ganhos.


*/



using System.Net;
using System.Text.Json;
using System.Web.Script.Serialization;

namespace ps_inoa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=IBM&apikey=demo"
            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                JavaScriptSerializer js = new JavaScriptSerializer();
                dynamic json_data = js.Deserialize(client.DownloadString(queryUri), typeof(object));

                // -------------------------------------------------------------------------
                // if using .NET Core (System.Text.Json)
                // using .NET Core libraries to parse JSON is more complicated. For an informative blog post
                // https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/

                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));



            }
    }
}
