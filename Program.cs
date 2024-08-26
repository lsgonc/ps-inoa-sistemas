/*
    Autor: Lucas Sciarra Gonçalves

   Projeto desenvolvido para o processo seletivo da vaga de estágio da Inoa

   O aplicativo foi projetado para monitorar em tempo real a cotação de um ativo específico da B3, utilizando a API Alpha Vantage 
   para obter dados financeiros. Com base nos parâmetros definidos pelo usuário, o sistema analisa continuamente o preço 
   da ação. Se a cotação cair abaixo de um valor preestabelecido, o aplicativo gera um alerta via email para o usuário aconselhando a compra, sinalizando 
   uma oportunidade de investimento. Por outro lado, se a cotação subir acima de um valor de referência, o sistema sugere a venda, 
   ajudando o usuário a maximizar seus ganhos.

    Algumas das API's que foram analisadas para construção do app: Alpha Vantage, Yahoo Finance, B3    

*/

using System;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Program
    {
        private const double IntervaloEmMinutos = 30; //Chama a API de 30 em 30 minutos

        static async Task Main(string[] args)
        {
            Console.WriteLine("===== Bem-vindo ao monitorador de ativos INOA =====\n");

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

            //Garante que os valores minimo e máximo estão certo, mesmo que o usuário passa-los em uma ordem diferente
            double min = Math.Min(value1, value2);
            double max = Math.Max(value1, value2);

            double intervaloMilissegundos = IntervaloEmMinutos * 60000; // Convertendo minutos para milissegundos

            Monitoramento monitor = new Monitoramento();
            Alerta alertaEmail = new Alerta();

            while(true)
            { 
                await monitor.MonitoraAtivo(args[0], min, max);
                

                Console.WriteLine($"API call concluída. Aguardando {IntervaloEmMinutos} minutos para chamar a API novamente.");

                await Task.Delay((int)Math.Ceiling(intervaloMilissegundos)); // Atraso em milissegundos

            }
        }
    }
}
