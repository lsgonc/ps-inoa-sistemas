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
using System.Globalization;
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
            double intervalMilisseconds = 56.7 * 60000;

            Monitora monitor = new Monitora();
            Alerter mailer = new Alerter();

            //Pega os valores minimo e maximo digitados pelo usuário como double
            double value1 = double.Parse(args[1]);
            double value2 = double.Parse(args[2]);

            //Pega o minimo e o máximo, independete da orgem que o usuário digitar na hora de rodar o app
            double min = Math.Min(value1, value2);
            double max = Math.Max(value1, value2);

            for (int i = 0; i < totalCalls; i++)
            {
                //Chama a função para monitorar o ativo escolhido
                dynamic item = await monitor.MonitoraAtivo(args[0], min, max);


                Console.WriteLine("Ação a ser tomada: " + item.ToLower() );

                Console.WriteLine($"API call {i + 1}/{totalCalls} completed. Waiting for {intervalMilisseconds} minutes.");


                if (i < totalCalls - 1)
                {
                    await Task.Delay((int)Math.Ceiling(intervalMilisseconds)); //valor milisegundos;
                }


            }


        }

    }
}
