/*
    Autor: Lucas Sciarra Gonçalves

    Projeto desenvolvido para o processo seletivo da vaga de estágio da Inoa

   O aplicativo foi projetado para monitorar em tempo real a cotação de um ativo específico da B3, utilizando a API Alpha Vantage 
   para obter dados financeiros. Com base nos parâmetros definidos pelo usuário, o sistema analisa continuamente o preço 
   da ação. Se a cotação cair abaixo de um valor preestabelecido, o aplicativo gera um alerta via email para o usuário aconselhando a compra, sinalizando 
   uma oportunidade de investimento. Por outro lado, se a cotação subir acima de um valor de referência, o sistema sugere a venda, 
   ajudando o usuário a maximizar seus ganhos.


*/



using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ps_inoa
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Numero limite de chamadas da API por DIA
            int totalCalls = 25;

            //Intervalo calculado entre cada chamda da API => 24h*60min/25calls = X minutos = X*60000 millisegundos
            double intervalMilisseconds = 1 * 60000;

            Monitora monitor = new Monitora();

           
            for (int i = 0; i < totalCalls; i++)
            {
                dynamic item = await monitor.MonitoraAtivo(args[0], Double.Parse(args[1]), Double.Parse(args[2]));
                Console.WriteLine($"API call {i + 1}/{totalCalls} completed. Waiting for {intervalMilisseconds} minutes.");
                Console.WriteLine(item);

                if (i < totalCalls - 1)
                {
                    await Task.Delay((int)Math.Ceiling(intervalMilisseconds)); //valor milisegundos;
                }

               
            }


        }
            
    }
}
