using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace ps_inoa
{
    internal class Alerta
    {
        public void EnviarAlertaEmail(string configFile, string mensagem)
        {
            try
            {
                SmtpConfig config = LoadConfig(configFile);

                MailMessage mensagemEmail = new MailMessage("alertaAtivos@inoa.com.br", config.To);

                mensagemEmail.Subject = "INOA SISTEMAS - ALERTA DE ATIVO";
                mensagemEmail.Body = mensagem;             

                using (var client = new SmtpClient(config.Host, config.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(config.Credentials.Username, config.Credentials.Password);
                    client.Send(mensagemEmail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail de alerta", ex);
            }
        }

        private SmtpConfig LoadConfig(string configFile)
        {
            var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), configFile);
            string json = File.ReadAllText(caminhoCompleto);
            return JsonSerializer.Deserialize<SmtpConfig>(json);
        }
    }

   
}
