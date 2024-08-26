using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ps_inoa
{
    internal class Alerta
    {

        public void AlertaEmail(string configFile, string msg)
        {
            
            try
            {
                //Carrega as informações de config através de um arquivo Json
                dynamic configs = LoadConfig(configFile);


                //Tenta ler todas as propriedades necessarias para o envio do email
                configs.TryGetProperty("to", out JsonElement jsonTo);
                configs.TryGetProperty("host", out JsonElement jsonHost);
                configs.TryGetProperty("port", out JsonElement jsonPort);
                configs.TryGetProperty("credentials", out JsonElement jsonCredentials);
                jsonCredentials.TryGetProperty("username", out JsonElement jsonUsername);
                jsonCredentials.TryGetProperty("password", out JsonElement jsonPassword);

                MailMessage message = new MailMessage("monitoradorAcoes@inoa.com.br", jsonTo.ToString());


                message.Subject = "MONITORADOR DE ATIVOS - ALERTA";
                message.Body = msg;

                SmtpClient client = new SmtpClient();
                // Credentials are necessary if the server requires the client
                // to authenticate before it will send email on the client's behalf.
                client.Host = jsonHost.ToString();
                client.Port = int.Parse(jsonPort.ToString());
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(jsonUsername.ToString(), jsonPassword.ToString());

                client.Send(message);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



        }
        
        //Lê um arquivo JSON e retorna os dados em um objeto
        public dynamic LoadConfig(string configFile)
        {
           
            StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + "/" + configFile);

            string json = streamReader.ReadToEnd();

            dynamic items = JsonSerializer.Deserialize<object>(json);

            
            return items;

        }

    }
}
