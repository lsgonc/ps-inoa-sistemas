/*
    Autor: Lucas Sciarra Gonçalves

   Projeto desenvolvido para o processo seletivo da vaga de estágio da Inoa

   O aplicativo foi projetado para monitorar em tempo real a cotação de um ativo específico da B3, utilizando a API Alpha Vantage 
   para obter dados financeiros. Com base nos parâmetros definidos pelo usuário, o sistema analisa continuamente o preço 
   da ação. Se a cotação cair abaixo de um valor preestabelecido, o aplicativo gera um alerta via email para o usuário aconselhando a compra, sinalizando 
   uma oportunidade de investimento. Por outro lado, se a cotação subir acima de um valor de referência, o sistema sugere a venda, 
   ajudando o usuário a maximizar seus ganhos.


*/

using System;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Program
    {
        private const int TotalCalls = 25; //Limite de chamadas diárias para a API Alpha Vantage
        private const double IntervaloEmMinutos = 56.7; //Distribuição espaçada das 25 chamadas da API em um dia

        static async Task Main(string[] args)
        {
            //Validando os argumentos do programa
            if (args.Length < 3)
            {
                Console.WriteLine("Uso: programa <ativo> <preco_venda> <preco_compra>");
                return;
            }

            if (!double.TryParse(args[1], out double value1) || !double.TryParse(args[2], out double value2))
            {
                Console.WriteLine("Valores de mínimo e máximo devem ser números válidos.");
                return;
            }

            //Garante que os valores de venda e compra estão corretos
            double min = Math.Min(value1, value2);
            double max = Math.Max(value1, value2);

            double intervaloMilissegundos = IntervaloEmMinutos * 60000; // Convertendo minutos para milissegundos

            Monitoramento monitor = new Monitoramento();
            Alerta alertaEmail = new Alerta();

            for (int i = 0; i < TotalCalls; i++)
            {
                var resultado = await monitor.MonitoraAtivo(args[0], min, max);
                Console.WriteLine("Ação a ser tomada: " + resultado.ToLower());

                Console.WriteLine($"API call {i + 1}/{TotalCalls} concluída. Aguardando {IntervaloEmMinutos} minutos para chamar a API novamente.");

                if (i < TotalCalls - 1)
                {
                    await Task.Delay((int)Math.Ceiling(intervaloMilissegundos)); // Atraso em milissegundos
                }
            }
        }
    }
}
